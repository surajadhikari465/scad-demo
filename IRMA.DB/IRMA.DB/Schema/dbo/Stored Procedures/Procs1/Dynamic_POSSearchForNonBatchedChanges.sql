
CREATE  PROCEDURE [dbo].[Dynamic_POSSearchForNonBatchedChanges]
  
-- INPUT PARAMETERS USED WITHIN THE SELECT STMT
@NewItemVal					bit = 0,		-- sets the return value for the new item flag
@ItemChangeVal				bit = 0,		-- sets the return value for the item change flag
@RemoveItemVal				bit = 0,		-- sets the return value for the remove item flag
@PIRUSHeaderActionVal		[varchar](2),	-- sets the return value for the PIRUS header action code
@Deletes					bit = 0,		-- TRUE if this is returning POS delete batches
 
-- INPUT PARAMETERS USED TO BUILD THE JOINS
@IsPOSPush					bit = 0,		-- TRUE if this is POS Push execution so Item Id adds are excluded from the result set
@IsScaleZoneData			bit = 0,		-- TRUE if the result set should return scale data
@POSDeAuthData				bit = 0,		-- TRUE if looking for the POS de-authorization results
@ScaleDeAuthData			bit = 0,		-- TRUE if looking for the Scale de-authorization results
@ScaleAuthData				bit = 0,		-- TRUE if looking for the Scale authorization results
@IdentifierAdds				bit = 0,		-- TRUE if looking for the item identifier add results
@IdentifierDeletes			bit = 0,		-- TRUE if looking for the item identifier delete results
 
-- FLAG USED TO ONLY PULL IN ITEMS MARKED FOR A REFRESH TO THE POS
@IdentifierRefreshes		bit	= 0, 
@Date						datetime,		-- Start/Effective date for the changes 
 
-- INPUT PARAMETERS USED TO BUILD THE WHERE CLAUSE
@AuditReport				bit = 0,		-- TRUE if the results are being used for the audit report/complete POS file process
@Store_No					int = NULL,		-- Store number if this is being used for the audit report/complete POS file process
 
-- FLAG USED TO PRINT QUERY IN DEBUG MODE
@Debug						bit = 0,
@IsItemNonBatchableChanges	bit = 0,
--When this flag is true then the data for all the stores is returned, else only the data for the legacy stored is returned
@LegacyStoresOnly bit = 1
AS

/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110125	1241	Merge Up FSA Changes - Had to replace table strings with aliases
							in order to keep from getting string errors.  Funny because the data
							type is Varchar(max).  If alterations are needed and the string 
							doesn't compile, try aliasing some more tables.  I prob padded a few
							more hundred character places but just so you know.

MU		20110126	 744	added ItemSurcharge
DBS		20110819	2672	Replace inside-out fn_GetScalePLU code w/fixed func call
BJL		20120430	5592	Add column Digi_LNU.
BJL		20120820	6577	Add column ItemScale.Nutrifact_ID
DN		20121012	6711	Update the condition with CASE..WHEN statement to return non-type-2 scale UPC.
KM		2013-01-05	9251	Check ItemOverride for new 4.8 override values (LabelType, Brand, 
							Product_Code, Unit_Price_Category, Recall_Flag, FSA_Eligible);
BAS		2013-01-07	8755	Updated Item.Discontinue_Item with SIV.DiscontinueItem due to schema change
KM		2013-01-22	9394	Check ItemScaleOverride for new 4.8 override values (ForceTare, Scale_Alternate_Tare_ID);
KM		2013-01-25	9382	More ItemScaleOverride columns: Scale_EatBy_ID, Scale_Grade_ID;
KM		2013-01-31	9382	Same as above, only this time with Nutrifact_ID;
DN		20130131	9995	Update the condition with CASE..WHEN statement to return type-2 and non-type-2 scale UPC
BS		20130205	10025	Replaced the function fn_GetScalePLU with the CASE statement that was
							inside this funtion for the ScalePLU field in the SELECT statement
BJL		20130430	11774	Added Item.GiftCard to the output.
BJL		20130514	11959	Modified the fn_GetScalePLU to accept the Identifier
DN		20140429	15003	Added a temp table #Identifiers to hold the validated scan codes. This temp table 
							replaces ItemIdentifiers in the dynamic SQL. 
DN		20140625	15220	Updated logic to handle new value dbo.fn_ReceiveUPCPLUUpdateFromIcon()
DN		20140731	15345	Updated LEFT JOIN to exclude un-validated scancode. 
DN		20140815	15371	Included Removed / Deleted Non-validated identifiers
DN		20141022	15467	Correct the order of the fields: NumPluDigitsSentToScale & Scale_Identifier
KM		2014-11-17	15534	Update the definition for scale PLU when populating the #Identifiers table.
BJL		2014-12-05	15565	Add Subteam.POSDept to the output to support Item-Subteam Alignment.
							This is TFS 2012 PBI 5493.
BJL		2014-01-13	15738	Sort by POSDept ASC if this is a posaudit file (build store pos file)
DN		2015-05-18	16152	Deprecate POSDept column in IRMA
DN		2015-07-05	16250	Concatenation of Ingredient, Allergen & Extra Text fields
DN		2015-07-07	16250	Reorder fields: Ingredients + Allergen + Extra Text
MZ      2015-10-23  16583(12203) Don't send nutrifacts to CAD stores if the alternate jurisdiction nutrifacts don't exist.
KM		2016-02-04	13984	Updates for 365 - includes join to ItemCustomerFacingScale and related WHERE condition; also includes
							new indexes on the identifiers temp table for performance.
DN		2016-04-21	19165	Add PriceBatchDetailID column in the returning result.
Jamali	2016-08-03	17576	Removed the invalid join from the ItemNonBatchChanges table, removed the nolock hint
Jamali  2016-09-28  18460	Added the @LegacyStoresOnly parameter
MZ      2017-04-13  23765(20859) Move Allergens before Ingredients in the concatenation. Correct the order of the Ingredients field
								 Allergens + Ingredients + ExtraText
***********************************************************************************************/

BEGIN

SET TRANSACTION ISOLATION LEVEL SNAPSHOT

--Synching with JDA?  
DECLARE @SyncJDA bit
SELECT @SyncJDA = (SELECT FlagValue FROM dbo.InstanceDataFlags  WHERE FlagKey='SyncJDA')

--Using the regional scale file?
DECLARE @UseRegionalScaleFile bit
SELECT @UseRegionalScaleFile = (SELECT FlagValue FROM dbo.InstanceDataFlags  WHERE FlagKey='UseRegionalScaleFile')
		
-- Check the Store Jurisdiction Flag
DECLARE @UseStoreJurisdictions int
SELECT @UseStoreJurisdictions = FlagValue FROM dbo.InstanceDataFlags  WHERE FlagKey = 'UseStoreJurisdictions'
 
--Exclude SKUs from the POS/Scale Push?  (TFS 3632)
DECLARE @ExcludeSKUIdentifiers bit
SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)

--Determine how region wants to send down data to scales
DECLARE @PluDigitsSentToScale varchar(20)
SELECT @PluDigitsSentToScale = PluDigitsSentToScale FROM dbo.InstanceData 

-- Set the values used for SmartX records
DECLARE @SmartX_DeletePendingName AS CHAR(16)
SELECT @SmartX_DeletePendingName = 'DELETE: ' + CONVERT(CHAR(8),@Date,10)
   
DECLARE @SmartX_MaintenanceDateTime AS CHAR(16)
SELECT @SmartX_MaintenanceDateTime = CONVERT(CHAR(8),@Date,10) + CONVERT(CHAR(8),@Date,8)

DECLARE @CurrDay smalldatetime
SELECT @CurrDay = CONVERT(smalldatetime, CONVERT(varchar(255), @Date, 101))

-- CFS Department prefix
DECLARE @CustomerFacingScaleDepartmentPrefix as nvarchar(1) = (
	select dbo.fn_GetAppConfigValue('CustomerFacingScaleDeptDigit', 'POS PUSH JOB'))

IF @CustomerFacingScaleDepartmentPrefix is null
	begin
		set @CustomerFacingScaleDepartmentPrefix = ''
	end

-- Create a temporary table to hold validated item identifiers (scan codes)
DECLARE @Status SMALLINT
IF OBJECT_ID('tempdb..#Identifiers') IS NOT NULL
BEGIN
	DROP TABLE #Identifiers
END 

IF OBJECT_ID('tempdb..#Stores') IS NOT NULL
BEGIN
	DROP TABLE #Stores
END 

CREATE TABLE #Stores
(
	Store_No INT
	, PLUMStoreNo INT
	, StoreJurisdictionID INT
	, Mega_Store BIT
	, WFM_Store BIT
)

CREATE TABLE #Identifiers
(
Identifier_ID			INT,
Item_Key				INT,
Identifier				VARCHAR(13),
Default_Identifier		TINYINT,
Deleted_Identifier		TINYINT,
Add_Identifier			TINYINT,
Remove_Identifier		TINYINT,
National_Identifier		TINYINT,
CheckDigit				CHAR(1),
IdentifierType			CHAR(1),
NumPluDigitsSentToScale	INT,
Scale_Identifier		BIT,
PRIMARY KEY CLUSTERED (Item_Key, Identifier)
)
	
CREATE NONCLUSTERED INDEX IX_NC_Identifiers_IdentifierFlags ON #Identifiers ([Add_Identifier],[Remove_Identifier],[Scale_Identifier])
	INCLUDE ([Identifier_ID],[Item_Key],[Identifier],[CheckDigit],[IdentifierType],[NumPluDigitsSentToScale])
	
SET @Status = dbo.fn_ReceiveUPCPLUUpdateFromIcon()

--get all the stores when the legacy stores flag is false
IF (@LegacyStoresOnly = 0)
BEGIN
	INSERT INTO #Stores
	SELECT s.Store_No, s.PLUMStoreNo, StoreJurisdictionID, Mega_Store, WFM_Store
	FROM Store s
	WHERE (Mega_Store = 1 OR WFM_Store = 1)
	AND (s.Internal = 1 AND s.BusinessUnit_ID IS NOT NULL)
END
ELSE
BEGIN
	--get all the Non R10/Legacy Stores
	INSERT INTO #Stores
	SELECT  s.Store_No, s.PLUMStoreNo, StoreJurisdictionID, Mega_Store, WFM_Store
	FROM Store s
	LEFT OUTER JOIN StorePOSConfig spc ON s.Store_No = spc.Store_No
	LEFT OUTER JOIN POSWriter pos ON spc.POSFileWriterKey = pos.POSFileWriterKey 
	WHERE (Mega_Store = 1 OR WFM_Store = 1)
		AND pos.POSFileWriterCode != 'R10'  --only the data from the non-R10 stores is needed
		AND (s.Internal = 1 AND s.BusinessUnit_ID IS NOT NULL)
END

IF @Status = 0 -- Validated UPC & PLU flags have not been turned on for the region.
	BEGIN
		INSERT INTO #Identifiers
		SELECT 
		Identifier_ID,
		Item_Key,
		Identifier,
		Default_Identifier,
		Deleted_Identifier,
		Add_Identifier,
		Remove_Identifier,
		National_Identifier,
		CheckDigit,
		IdentifierType,
		NumPluDigitsSentToScale,
		Scale_Identifier FROM ItemIdentifier II 
	END
ELSE
	IF @Status = 1 -- Only validated UPCs are passing from Icon to IRMA
		BEGIN
			INSERT INTO #Identifiers		
			SELECT 
			Identifier_ID,
			Item_Key,
			Identifier,
			Default_Identifier,
			Deleted_Identifier,
			Add_Identifier,
			Remove_Identifier,
			National_Identifier,
			CheckDigit,
			IdentifierType,
			NumPluDigitsSentToScale,
			Scale_Identifier
			FROM ItemIdentifier II  INNER JOIN ValidatedScanCode VSC 
			ON II.Identifier = VSC.ScanCode
			UNION
			SELECT 
			Identifier_ID,
			Item_Key,
			Identifier,
			Default_Identifier,
			Deleted_Identifier,
			Add_Identifier,
			Remove_Identifier,
			National_Identifier,
			CheckDigit,
			IdentifierType,
			NumPluDigitsSentToScale,
			Scale_Identifier
			FROM ItemIdentifier II 
			WHERE (LEN(Identifier) < 7 OR (LEN(Identifier) = 11 AND Identifier LIKE '2%00000'))
			UNION
			SELECT 
			Identifier_ID,
			I.Item_Key,
			Identifier,
			Default_Identifier,
			Deleted_Identifier,
			Add_Identifier,
			Remove_Identifier,
			National_Identifier,
			CheckDigit,
			IdentifierType,
			NumPluDigitsSentToScale,
			Scale_Identifier
			FROM Item I  INNER JOIN ItemIdentifier II 
			ON I.Item_Key = II.Item_Key
			WHERE I.Remove_Item = 1 OR II.Remove_Identifier = 1
		END
	ELSE
		IF @Status = 2 -- Only validated PLUs are passing from Icon to IRMA
				BEGIN
					INSERT INTO #Identifiers			
					SELECT 
					Identifier_ID,
					Item_Key,
					Identifier,
					Default_Identifier,
					Deleted_Identifier,
					Add_Identifier,
					Remove_Identifier,
					National_Identifier,
					CheckDigit,
					IdentifierType,
					NumPluDigitsSentToScale,
					Scale_Identifier
					FROM ItemIdentifier II  INNER JOIN ValidatedScanCode VSC 
					ON II.Identifier = VSC.ScanCode
					WHERE (LEN(Identifier) < 7 OR (LEN(Identifier) = 11 AND Identifier LIKE '2%00000'))
					UNION
					SELECT 
					Identifier_ID,
					Item_Key,
					Identifier,
					Default_Identifier,
					Deleted_Identifier,
					Add_Identifier,
					Remove_Identifier,
					National_Identifier,
					CheckDigit,
					IdentifierType,
					NumPluDigitsSentToScale,
					Scale_Identifier
					FROM ItemIdentifier II 
					WHERE NOT (LEN(Identifier) < 7 OR (LEN(Identifier) = 11 AND Identifier LIKE '2%00000'))
					UNION
					SELECT 
					Identifier_ID,
					I.Item_Key,
					Identifier,
					Default_Identifier,
					Deleted_Identifier,
					Add_Identifier,
					Remove_Identifier,
					National_Identifier,
					CheckDigit,
					IdentifierType,
					NumPluDigitsSentToScale,
					Scale_Identifier
					FROM Item I  INNER JOIN ItemIdentifier II 
					ON I.Item_Key = II.Item_Key
					WHERE I.Remove_Item = 1 OR II.Remove_Identifier = 1
				END
			ELSE 
				IF @Status = 3 -- Both Validated UPC & PLU are passing from Icon to IRMA
					BEGIN				
						INSERT INTO #Identifiers				
						SELECT 
						Identifier_ID,
						Item_Key,
						Identifier,
						Default_Identifier,
						Deleted_Identifier,
						Add_Identifier,
						Remove_Identifier,
						National_Identifier,
						CheckDigit,
						IdentifierType,
						NumPluDigitsSentToScale,
						Scale_Identifier
						FROM ItemIdentifier II  INNER JOIN ValidatedScanCode VSC 
						ON II.Identifier = VSC.ScanCode 
						UNION
						SELECT 
						Identifier_ID,
						I.Item_Key,
						Identifier,
						Default_Identifier,
						Deleted_Identifier,
						Add_Identifier,
						Remove_Identifier,
						National_Identifier,
						CheckDigit,
						IdentifierType,
						NumPluDigitsSentToScale,
						Scale_Identifier
						FROM Item I  INNER JOIN ItemIdentifier II 
						ON I.Item_Key = II.Item_Key
						WHERE I.Remove_Item = 1 OR II.Remove_Identifier = 1
					END

	UPDATE #Identifiers
	SET Scale_Identifier = 1
	WHERE Item_Key in 
		(
			SELECT Item_Key FROM dbo.ItemCustomerFacingScale icfs WHERE icfs.SendToScale = 1
		)

-- Select the unexpired Price changes from PriceBatchDetail
-- All references to the corresponding fields in Price should be wrapped with an ISNULL, same as ItemOverride,
-- to capture any existing price changes for this item already in progress
CREATE TABLE #PBDPrices (Item_Key int, Store_No int, Price money,	Multiple tinyint, Sale_Multiple tinyint, Sale_Price money,	StartDate smalldatetime, Sale_End_Date smalldatetime, PriceChgTypeId int, PricingMethod_ID int, Sale_Earned_Disc1 tinyint, Sale_Earned_Disc2 tinyint, Sale_Earned_Disc3 tinyint, POSPrice money, POSSale_Price money)

INSERT INTO #PBDPrices
SELECT TOP 1
		PBD.Item_Key,
		PBD.Store_No,
		PBD.Price,
		PBD.Multiple, 
		PBD.Sale_Multiple, 
		PBD.Sale_Price, 
		PBD.StartDate, 
		PBD.Sale_End_Date,
		PBD.PriceChgTypeId,
		PBD.PricingMethod_ID,
		PBD.Sale_Earned_Disc1,
		PBD.Sale_Earned_Disc2,
		PBD.Sale_Earned_Disc3,
		PBD.POSPrice,
		PBD.POSSale_Price 
	FROM dbo.PriceBatchDetail PBD 
		INNER JOIN dbo.PriceBatchHeader PBH  ON PBH.PriceBatchHeaderID = PBD.PriceBatchHeaderID
	WHERE PBH.PriceBatchStatusID = 5
		AND PBD.Offer_ID IS NULL
		AND ISNULL(PBD.PriceChgTypeID, 0) = 1
		AND PBH.StartDate <= @CurrDay
	ORDER BY PBH.StartDate
	
DECLARE @SQL NVARCHAR(MAX)
SET @SQL = '
SELECT I.Item_Key,
   II.Identifier, 
   II.Identifier_ID,
   CASE WHEN II.CheckDigit IS NOT NULL 
		THEN (II.Identifier + II.CheckDigit)
		ELSE II.Identifier
		END AS IdentifierWithCheckDigit, 
   CASE WHEN II.CheckDigit IS NOT NULL 
		THEN (II.Identifier + II.CheckDigit)
		ELSE II.Identifier + ''0''
		END AS RBX_IdentifierWithCheckDigit, 
   NULL AS PriceBatchHeaderID, 
   CASE WHEN COALESCE(RU_UOM_Override.Weight_Unit, RU_Override.Weight_Unit, RU.Weight_Unit, 0) = 1 THEN 1 
		ELSE 0 
		END As RetailUnit_WeightUnit,
   CASE WHEN COALESCE(RU_UOM_Override.Weight_Unit, RU_Override.Weight_Unit, RU.Weight_Unit, 0) = 1 
			AND dbo.fn_IsScaleItem(II.Identifier) = 0 THEN 1 
		ELSE 0 END AS Sold_By_Weight, 
   CASE WHEN COALESCE(RU_UOM_Override.Weight_Unit, RU_Override.Weight_Unit, RU.Weight_Unit, 0) = 1 
			AND dbo.fn_IsScaleItem(II.Identifier) = 0 THEN ''Y''
		ELSE ''N'' 
		END As PIRUS_Sold_By_Weight,
   P.Restricted_Hours, 
   P.LocalItem,
   ISNULL(IO.Quantity_Required, I.Quantity_Required) AS Quantity_Required, 
   ISNULL(IO.Price_Required, I.Price_Required) AS Price_Required, 
   I.Retail_Sale,        
   CASE I.Retail_Sale WHEN 1 THEN 0 ELSE 1 END AS NotRetailSale,
   P.Discountable, 
   CASE P.Discountable WHEN 1 THEN 0 ELSE 1 END AS NotDiscountable,
   CASE dbo.fn_isEmpDiscountException(P.Store_No, ISNULL(P.ExceptionSubTeam_No, I.SubTeam_No), P.Discountable) WHEN 1 THEN 0 ELSE 1 END as OHIO_Emp_Discount,'
   
   IF @IsItemNonBatchableChanges = 1
   BEGIN
			SET @SQL = @SQL + ' COALESCE(inbc.Food_Stamps, IO.Food_Stamps, I.Food_Stamps) AS Food_Stamps,' 
   END
   ELSE
   BEGIN
			SET @SQL = @SQL + ' ISNULL(IO.Food_Stamps, I.Food_Stamps) AS Food_Stamps,' 
   END

   SET @SQL = @SQL + '
   I.ItemType_ID,
   CASE WHEN I.ItemType_ID = 1 THEN ''Y''
	ELSE ''N'' 
	END As PIRUS_ItemTypeID,
   ISNULL(P.ExceptionSubTeam_No, I.SubTeam_No) As SubTeam_No,
   P.Store_No, 
   P.IBM_Discount, 
   CASE WHEN P.MixMatch IS NOT NULL THEN P.MixMatch ELSE 0 END AS MixMatch, 
   PCT.On_Sale As On_Sale,
   CONVERT(money, ROUND(dbo.fn_Price(ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID), ISNULL(PBD.Multiple, P.Multiple), ISNULL(PBD.Price, P.Price), ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID), ISNULL(PBD.Sale_Multiple, P.Sale_Multiple), ISNULL(PBD.Sale_Price, P.Sale_Price)) * ISNULL(SST_Exception.CasePriceDiscount,SST.CasePriceDiscount) * ISNULL(IO.Package_Desc1, I.Package_Desc1), 2)) As Case_Price,
   CONVERT(money, ROUND(dbo.fn_Price(ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID), ISNULL(PBD.Multiple, P.Multiple), ISNULL(PBD.POSPrice, P.POSPrice), ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID), ISNULL(PBD.Sale_Multiple, P.Sale_Multiple), ISNULL(PBD.POSSale_Price, P.POSSale_Price)) * ISNULL(SST_Exception.CasePriceDiscount,SST.CasePriceDiscount) * ISNULL(IO.Package_Desc1, I.Package_Desc1), 2)) As POSCase_Price,
   ISNULL(PBD.Sale_Earned_Disc1, P.Sale_Earned_Disc1) as Sale_Earned_Disc1, 
   ISNULL(PBD.Sale_Earned_Disc2, P.Sale_Earned_Disc2) as Sale_Earned_Disc2, 
   ISNULL(PBD.Sale_Earned_Disc3, P.Sale_Earned_Disc3) as Sale_Earned_Disc3, 
   ISNULL(P.MSRPMultiple, 1) As MSRPMultiple, 
   ROUND(ISNULL(P.MSRPPrice, 0), 2) As MSRPPrice,'
   
   IF @IsItemNonBatchableChanges = 1
   BEGIN
   SET @SQL = @SQL + '
   LEFT(REPLACE(COALESCE(inbc.POS_Description, IO.POS_Description, I.POS_Description),'','','' ''),18) AS Item_Desc, --truncates data to 18 chars
   COALESCE(inbc.POS_Description, IO.POS_Description, I.POS_Description) AS POS_Description, --untruncated version of POS_Description field'
   END
   ELSE
   BEGIN 
   SET @SQL = @SQL + '
   LEFT(REPLACE(ISNULL(IO.POS_Description, I.POS_Description),'','','' ''),18) AS Item_Desc, --truncates data to 18 chars
   ISNULL(IO.POS_Description, I.POS_Description) AS POS_Description, --untruncated version of POS_Description field'
   END

   SET @SQL = @SQL + '
   ISNULL(IO.Item_Description, I.Item_Description) AS Item_Description, 
   ISNULL(ISNULL(ISO.Scale_Description1, ItemScale.Scale_Description1), '''') AS ScaleDesc1, 
   ISNULL(ISNULL(ISO.Scale_Description2, ItemScale.Scale_Description2), '''') AS ScaleDesc2,
   ISNULL(ISNULL(ISO.Scale_Description3, ItemScale.Scale_Description3), '''') AS ScaleDesc3,
   ISNULL(ISNULL(ISO.Scale_Description4, ItemScale.Scale_Description4), '''') AS ScaleDesc4,
   ISNULL(ISNULL(Scale_ExtraText_Override.ExtraText, SUBSTRING(SA.Allergens+'' ''+ISNULL(SING.Ingredients, '''')+ISNULL(Scale_ExtraText.ExtraText, ''''),1,4200)), '''') As Ingredients,
   CASE WHEN ISNULL(ISNULL(Scale_ExtraText_Override.ExtraText, SUBSTRING(SA.Allergens+'' ''+ISNULL(SING.Ingredients, '''')+ISNULL(Scale_ExtraText.ExtraText, ''''),1,4200)), '''') <> '''' -- Scale Ingredients is not ''''
		THEN SUBSTRING(II.Identifier, 2, 5) 
		ELSE 0 
	END As IngredientNumber,		   
   COALESCE(RU_UOM_Override.Unit_Abbreviation, RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation, '''') As Retail_Unit_Abbr,			   
   CASE COALESCE(RU_UOM_Override.Unit_Abbreviation, RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation, '''')
		 		WHEN ''UNIT'' THEN ''FW''
				WHEN ''LB'' THEN ''LB''
				WHEN ''EA'' THEN ''BC''
			 END AS UnitOfMeasure,
   COALESCE(ScaleUOM_UOM_Override.Unit_Abbreviation, ScaleUOM_Override.Unit_Abbreviation, ScaleUOM.Unit_Abbreviation) AS ScaleUnitOfMeasure,
   COALESCE(ScaleUOM_UOM_Override.PlumUnitAbbr, ScaleUOM_Override.PlumUnitAbbr, ScaleUOM.PlumUnitAbbr) AS PlumUnitAbbr,
   CAST(ISNULL(Scale_Tare_Override.Zone1, Scale_Tare.Zone1) AS int) AS ScaleTare_Int,
   CAST(Alt_Scale_Tare.Zone1 AS int) AS AltScaleTare_Int,
   ISNULL(ISO.Scale_EatBy_ID, ItemScale.Scale_EatBy_ID) AS UseBy_ID,		   
   ISNULL(ISO.ForceTare, ItemScale.ForceTare) AS ForceTare,
   CASE WHEN ISNULL(ISO.ForceTare, ItemScale.ForceTare) = 1 
			THEN ''Y''
			ELSE ''N''
		END AS ScaleForcedTare,
   ISNULL(ISO.ShelfLife_Length, ItemScale.ShelfLife_Length) AS ShelfLife_Length,
   COALESCE(IUO.Scale_FixedWeight, ISO.Scale_FixedWeight, ItemScale.Scale_FixedWeight) AS Scale_FixedWeight,
   COALESCE(IUO.Scale_ByCount, ISO.Scale_ByCount, ItemScale.Scale_ByCount) AS Scale_ByCount,
   Scale_Grade.Zone1 AS Grade,
   ISNULL(IO.Package_Desc1, I.Package_Desc1) AS Package_Desc1, 
   ISNULL(IO.Package_Desc2, I.Package_Desc2) AS Package_Desc2, 
   ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '''') As Package_Unit_Abbr,       
   CONVERT(varchar, CONVERT(real, I.Package_Desc2)) + '''' + ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '''') As PackSize, --removes trailing zeros from package_desc2; ex: 1.500g resolves to 1.5g OR 1.000lb = 1lb
   @NewItemVal AS New_Item,
   CASE WHEN ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID) IS NOT NULL THEN 1 ELSE 0 END AS Price_Change, 
   @ItemChangeVal AS Item_Change, 
   @RemoveItemVal AS Remove_Item, 
   @PIRUSHeaderActionVal AS PIRUS_HeaderAction,  --UK/PIRUS ONLY FIELD	
   CASE WHEN dbo.fn_IsScaleItem(II.Identifier) = 1 THEN 1 ELSE 0 END AS IsScaleItem,
   ISNULL(PBD.Multiple, P.Multiple) As Multiple,
   ISNULL(PBD.Sale_Multiple, P.Sale_Multiple) As Sale_Multiple , 
	dbo.fn_PricingMethodInt(ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID), 
							ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID), 
							ISNULL(PBD.Multiple, P.Multiple), 
							ISNULL(PBD.Sale_Multiple, P.Sale_Multiple)) AS CurrMultiple,
   ROUND(ISNULL(PBD.Price, P.Price), 2) AS Price,  -- this value will be POSPrice unless item is on sale it will be POSSale_Price
   ROUND(ISNULL(PBD.Sale_Price, P.Sale_Price), 2) AS Sale_Price, 
	dbo.fn_PricingMethodMoney(ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID), 
							ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID), 
							ISNULL(PBD.Price, P.Price), 
							ISNULL(PBD.Sale_Price, P.Sale_Price)) AS CurrPrice,
   ROUND(ISNULL(PBD.POSPrice, P.POSPrice), 2) AS POSPrice,
   ROUND(ISNULL(PBD.POSSale_Price, P.POSSale_Price), 2) AS POSSale_Price,
   ROUND(CASE WHEN PCT.On_Sale = 1 
			  THEN CASE ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID) WHEN 0 THEN ISNULL(PBD.POSSale_Price, P.POSSale_Price) 
										 WHEN 1 THEN ISNULL(PBD.POSSale_Price, P.POSSale_Price)
										 WHEN 2 THEN ISNULL(PBD.POSPrice, P.POSPrice)
										 WHEN 4 THEN ISNULL(PBD.POSPrice, P.POSPrice) END
			  ELSE ISNULL(PBD.POSPrice, P.POSPrice) END, 2) AS POSCurrPrice,
	dbo.fn_PricingMethodMoney(ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID), 
							ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID), 
							ISNULL(PBD.POSPrice, P.POSPrice), 
							ISNULL(PBD.POSSale_Price, P.POSSale_Price)) AS POSCurrPrice,
   CONVERT(varchar(3),ISNULL(PBD.Multiple, P.Multiple)) + ''/'' + CONVERT(varchar(10),ISNULL(PBD.POSPrice, P.POSPrice)) AS  MultipleWithPOSPrice, -- always the Base Price for the Item
	CONVERT(varchar(3),dbo.fn_PricingMethodInt(ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID), 
							ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID), 
							ISNULL(PBD.Multiple, P.Multiple), 
							ISNULL(PBD.Sale_Multiple, P.Sale_Multiple)))
	+ ''/'' +
	CONVERT(varchar(10),dbo.fn_PricingMethodMoney(ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID), 
							ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID), 
							ISNULL(PBD.POSPrice, P.POSPrice), 
							ISNULL(PBD.POSSale_Price, P.POSSale_Price)))
		As SaleMultipleWithPOSSalePrice,  -- the current Price for the Item 
   ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID) As PricingMethod_ID, 
   ISNULL(PBD.StartDate, P.Sale_Start_Date) As StartDate,
   CONVERT(varchar, ISNULL(PBD.StartDate, P.Sale_Start_Date), 1) As RBX_Sale_Start_Date,
   ISNULL(PBD.Sale_End_Date, P.Sale_End_Date) As Sale_End_Date, 
   CONVERT(varchar, P.Sale_End_Date, 1) As RBX_Sale_End_Date,
   I.Category_ID, 
   VCA.UnitCost,
   CASE WHEN ISNULL(VCA.Package_Desc1, 0) = 0 THEN VCA.UnitCost
		WHEN dbo.fn_IsCaseItemUnit(VCA.CostUnit_ID) = 1 THEN VCA.UnitCost / VCA.Package_Desc1
		ELSE VCA.UnitCost
		END As RBX_UnitCost,    
   ISNULL(ST_Exception.Target_Margin,ST.Target_Margin) as Target_Margin,
   V.Vendor_Key, 
   VI.Item_ID AS Vendor_Item_ID,
   UPPER(ISNULL(VI.Item_ID, II.Identifier)) AS RBX_Vendor_Item_ID,
   CASE WHEN ISNULL(IO.Price_Required, I.Price_Required) = 1 THEN ''Y''
		WHEN ISNULL(IO.Price_Required, I.Price_Required) = 0 THEN ''N''
		END As Compulsory_Price_Input, 
   CASE WHEN ISNULL(IO.Price_Required, I.Price_Required) = 1 THEN ''Y''
		WHEN ISNULL(IO.Price_Required, I.Price_Required) = 0 THEN ''N''
		END As Calculated_Cost_Item,
   CASE WHEN I.Deleted_Item = 1 THEN ''X''
		WHEN SIV.DiscontinueItem = 1 THEN ''D''
		ELSE ''A''
		END As Availability_Flag, --THIS MIGHT BE A UK/PIRUS ONLY FIELD		   
   dbo.fn_GetTorexJulianDate(@Date) AS PIRUS_StartDate,
   dbo.fn_GetTorexJulianDate(I.Insert_Date) As PIRUS_InsertDate,
   dbo.fn_GetTorexJulianDate(GetDate()) As PIRUS_CurrentDate,
   CASE WHEN @Deletes = 1 THEN dbo.fn_GetTorexJulianDate(GetDate())
		ELSE NULL 
		END As PIRUS_DeleteDate, --UK/PIRUS ONLY
   CASE WHEN II.IdentifierType = ''B''
			AND LEN(II.IdentifierType) < 6 THEN ''P''
		WHEN II.IdentifierType = ''B''
			AND LEN(II.Identifier) = 11
			AND SUBSTRING(II.Identifier,1,1) = ''2'' THEN ''S''
		WHEN II.IdentifierType = ''B''
			AND LEN(II.Identifier) = 12 THEN ''3''
		WHEN II.IdentifierType = ''B''
			AND LEN(II.Identifier) = 7 THEN ''8''
		WHEN II.IdentifierType = ''B''
			AND LEN(II.Identifier) = 11 THEN ''A''
		WHEN II.IdentifierType = ''B''
			AND LEN(II.Identifier) = 6 THEN ''E''
		END As Barcode_Type,
   LT.LabelTypeDesc,
   CASE WHEN P.NotAuthorizedForSale = 1 THEN ''Y''
		ELSE ''N''
		END As NotAuthorizedForSale,
   CASE WHEN P.NotAuthorizedForSale = 1 THEN ''09''
		ELSE CONVERT(varchar, P.AgeCode)
		END As NCR_RestrictedCode, -- NCR SPECIFIC (FL and SP regions)
   CASE WHEN P.NotAuthorizedForSale = 1 THEN ''09''
		ELSE CASE WHEN P.AgeCode = 2 THEN ''01''
				ELSE CONVERT(varchar, P.AgeCode)
				END
		END As NCR_NENA_RestrictedCode,
   CASE WHEN PCT.On_Sale = 1 THEN ''Y''
		ELSE ''N''
		END AS PIRUS_OnSale,
   CASE WHEN PCT.On_Sale = 1 THEN dbo.fn_GetTorexJulianDate(P.Sale_End_Date)
		ELSE ''000000''
		END AS PIRUS_SaleEndDate,
	ISNULL(ST_Exception.Dept_No, ST.Dept_No) as Dept_No,  
	-- DaveStacey - 20070926 - added new IBM-specific field for 3 and 4 digit dept mixes
	IBM_Dept_No = CASE WHEN LEN(isnull(ST_Exception.Dept_No, ST.Dept_No)) = 4 then isnull(ST_Exception.Dept_No, ST.Dept_No) ELSE LEFT(isnull(ST_Exception.Dept_No, ST.Dept_No), 2) END, 
	IBM_Dept_No_3Chrs = ST.Subteam_No/10,
	IB.Brand_Name, 
	PCT.PriceChgTypeDesc As RBX_PriceType,
	VCA.Package_Desc1 As CaseSize,
	CASE WHEN dbo.fn_IsCaseItemUnit(VCA.CostUnit_ID) = 1 THEN VCA.UnitCost 
		 ELSE VCA.UnitCost * VCA.Package_Desc1
		 END As CaseCost,	     
	VCA.StartDate As ChangeDate,
	CONVERT(varchar, ISNULL(VCA.StartDate,@Date), 1) As RBX_ChangeDate,
	CASE WHEN ItemType_ID = 7 THEN ''Y''
		 ELSE ''N'' 
		 END As RBX_Coupon,
	Case I.ItemType_ID When 1 Then ''Y'' ELSE ''N'' END AS FX_DepositItem,
	Case I.ItemType_ID When 2 Then ''Y'' ELSE ''N'' END AS FX_RefundItem,
	Case I.ItemType_ID When 3 Then ''Y'' ELSE ''N'' END AS FX_DepositReturn,
	Case I.ItemType_ID When 6 Then ''Y'' ELSE ''N'' END AS FX_MfgCoupon,
	Case I.ItemType_ID When 7 Then ''Y'' ELSE ''N'' END AS FX_StoreCoupon,
	Case I.ItemType_ID When 4 Then ''Y'' ELSE ''N'' END As FX_MiscSale,
	Case I.ItemType_ID When 5 Then ''Y'' ELSE ''N'' END As FX_MiscRefund,	
	Case I.ItemType_ID When 2 Then ''Y'' When 3 Then ''Y'' When 6 Then ''Y'' When 8 Then ''Y'' ELSE ''N'' END AS FX_Retalix_NegativeItem,	
	Case I.ItemType_Id When 8 Then ''Y'' ELSE ''N'' END AS FX_NCR_NegativeItem,				 				 
	CASE WHEN ISNULL(ISNULL(IO.QtyProhibit, I.QtyProhibit), 0) = 1 THEN ''Y'' ELSE ''N'' END AS QtyProhibit,
	ISNULL(ISNULL(IO.QtyProhibit, I.QtyProhibit), 0) AS QtyProhibit_Boolean,
	CASE WHEN ISNULL(P.GrillPrint, 0) = 1 THEN ''Y'' ELSE ''N'' END AS GrillPrint,
	CASE WHEN ISNULL(P.SrCitizenDiscount, 0) = 1 THEN ''Y'' ELSE ''N'' END AS SrCitizenDiscount,
	CASE WHEN ISNULL(P.VisualVerify, 0) = 1 THEN ''Y'' ELSE ''N'' END AS VisualVerify,
	ISNULL(IO.GroupList, I.GroupList) AS GroupList,
	P.PosTare,
	LII.Identifier AS LinkCode_ItemIdentifier,
	CASE WHEN LEN(LII.Identifier) <= 4 THEN LII.Identifier
		 ELSE ''0'' 
		 END AS LinkCode_ItemIdentifier_MA,
	LP.POSLinkCode AS LinkCode_Value,
	P.AgeCode,
	case 
		when icfs.CustomerFacingScaleDepartment = 1 then cast(@CustomerFacingScaleDepartmentPrefix + cast(ST.ScaleDept as varchar) as int)
		else ST.ScaleDept
	end as ScaleDept, 
	dbo.fn_GetScalePLU(II.Identifier, II.NumPluDigitsSentToScale, @PluDigitsSentToScale, icfs.SendToScale) AS ScalePLU,
	CASE
		WHEN SUBSTRING(II.Identifier, 1, 1) = ''2''
			AND RIGHT(II.Identifier,5) = ''00000''
			AND LEN(RTRIM(II.Identifier)) = 11
			THEN SUBSTRING(II.Identifier, 2, 5)
		WHEN SUBSTRING(II.Identifier,1,1) != ''2''
			OR (SUBSTRING(II.Identifier,1,1) = ''2''
			AND (RIGHT(II.Identifier,5) != ''00000''
			OR LEN(RTRIM(II.Identifier)) != 11))
			THEN RIGHT(''0000000000000'' + II.Identifier, 13)
		ELSE SUBSTRING(II.Identifier, 2, 5)
	END	AS ScaleUPC,
	Store.PLUMStoreNo,		
	CASE WHEN @Deletes = 1 THEN ''D''
		 WHEN CASE WHEN PCT.On_Sale = 1 
			  THEN CASE ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID) WHEN 0 THEN ISNULL(PBD.POSSale_Price, P.POSSale_Price) 
										 WHEN 1 THEN ISNULL(PBD.POSSale_Price, P.POSSale_Price)
										 WHEN 2 THEN ISNULL(PBD.POSPrice, P.POSPrice)
										 WHEN 4 THEN ISNULL(PBD.POSPrice, P.POSPrice) END 
			  ELSE ISNULL(PBD.POSPrice, P.POSPrice) END > 0 THEN ''Y''
		 ELSE ''N''
		 END AS PLUM_ItemStatus, 
	[IBM_NoPrice_NotScaleItem] =
		CASE WHEN dbo.fn_IsScaleItem(II.Identifier) = 1 THEN 0	
			WHEN ISNULL(IO.Price_Required, I.Price_Required) = 1 THEN 1
			WHEN ISNULL(PBD.POSPrice, P.POSPrice) = 0 THEN 1
			ELSE 0
		END,
	[IBM_Offset09_Length1] =
		CASE WHEN PCT.On_Sale = 0 THEN (CONVERT(varchar, I.ItemType_ID) + ''0'')
			ELSE (CONVERT(varchar, I.ItemType_ID) + CONVERT(varchar, ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID)))	
		END,
	[IBM_Offset09_Length1_MA] =
		CASE WHEN ISNULL(IO.Ice_Tare, I.Ice_Tare) > 0 THEN ''03''
		ELSE							
			CASE WHEN PCT.On_Sale = 0 THEN (CONVERT(varchar, I.ItemType_ID) + ''0'')
				WHEN ISNULL(PBD.Sale_Multiple, P.Sale_Multiple) > 1 Then (CONVERT(varchar, I.ItemType_ID) + ''2'')
				ELSE (CONVERT(varchar, I.ItemType_ID) + CONVERT(varchar, ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID)))	
			END
		END,			
	[IBM_Offset15_Length1] =
		CASE WHEN PCT.On_Sale = 0 THEN 0
			ELSE CASE ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID)
				WHEN 0 THEN 0
				WHEN 1 THEN 99
				WHEN 2 THEN 0
				WHEN 4 THEN 0
			END
		END,
	[IBM_Offset15_Length1_MA] =
		CASE WHEN PCT.On_Sale = 0 THEN 0
			ELSE CASE 
				WHEN ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID) = 2 OR ISNULL(PBD.Sale_Multiple, P.Sale_Multiple) > 1 THEN 99
				ELSE 0
			END
		END,			
	[IBM_Offset16_Length1] =
		CASE WHEN PCT.On_Sale = 0 THEN ISNULL(PBD.Multiple, P.Multiple)
			ELSE CASE ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID)
				WHEN 0 THEN ISNULL(PBD.Sale_Multiple, P.Sale_Multiple)
				WHEN 1 THEN ISNULL(PBD.Sale_Multiple, P.Sale_Multiple)
				WHEN 2 THEN (ISNULL(PBD.Sale_Earned_Disc1, P.Sale_Earned_Disc1) + ISNULL(PBD.Sale_Earned_Disc2, P.Sale_Earned_Disc2))
				WHEN 4 THEN ISNULL(PBD.Sale_Earned_Disc3, P.Sale_Earned_Disc3)
			END
		END,
	[IBM_Offset16_Length1_MA] =
		CASE WHEN PCT.On_Sale = 0 THEN ISNULL(PBD.Multiple, P.Multiple)
			 ELSE ISNULL(PBD.Sale_Multiple, P.Sale_Multiple)
			 END,			
	[IBM_Offset17_Length5] =
		CASE WHEN PCT.On_Sale = 0 THEN ''00'' + RIGHT(''00000000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSPrice, P.POSPrice) AS MONEY) * 100))), 8)
			ELSE CASE ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID)
				WHEN 0 THEN ''00'' + RIGHT(''00000000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSSale_Price, P.POSSale_Price) AS MONEY) * 100))), 8)
				WHEN 1 THEN ''00'' + RIGHT(''00000000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSSale_Price, P.POSSale_Price) AS MONEY) * 100))), 8)
				WHEN 2 THEN RIGHT(''00000'' + CONVERT(varchar(8),CONVERT(int, ((CAST(ISNULL(PBD.POSPrice, P.POSPrice) AS MONEY)/ISNULL(PBD.Multiple, P.Multiple) * ISNULL(PBD.Sale_Earned_Disc1, P.Sale_Earned_Disc1) + CAST(ISNULL(PBD.POSSale_Price, P.POSSale_Price) AS MONEY)) * 100))), 5) 
							+ RIGHT(''00000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSPrice, P.POSPrice) AS MONEY)/ISNULL(PBD.Multiple, P.Multiple) * 100))), 5)
				WHEN 4 THEN RIGHT(''00000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSPrice, P.POSPrice) AS MONEY)/ISNULL(PBD.Multiple, P.Multiple) * 100))), 5) 
							+ RIGHT(''00000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSSale_Price, P.POSSale_Price) AS MONEY) * 100))), 5)
			END
		END,
	[IBM_Offset17_Length5_MA] =
		CASE WHEN ISNULL(IO.Ice_Tare, I.Ice_Tare) > 0 THEN
			--USE APPROPRIATE PRICE FIELD BASED ON ON_SALE FLAG
			CASE WHEN PCT.On_Sale = 0 THEN
				''2'' + RIGHT(''0000'' + CAST(ISNULL(IO.Ice_Tare, I.Ice_Tare) as varchar(4)), 4) + RIGHT(''00000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSPrice, P.POSPrice) AS MONEY) * 100))), 5)	
			ELSE
				''2'' + RIGHT(''0000'' + CAST(ISNULL(IO.Ice_Tare, I.Ice_Tare) as varchar(4)), 4) + RIGHT(''00000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSSale_Price, P.POSSale_Price) AS MONEY) * 100))), 5)
			END
		ELSE
			CASE WHEN PCT.On_Sale = 0 THEN ''00'' + RIGHT(''00000000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSPrice, P.POSPrice) AS MONEY) * 100))), 8)
				ELSE CASE 
					WHEN (ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID) = 2 OR ISNULL(PBD.Sale_Multiple, P.Sale_Multiple) >1)THEN RIGHT(''00000'' + CONVERT(varchar(8),CONVERT(int, ((CAST(ISNULL(PBD.POSPrice, P.POSPrice) AS MONEY)/ISNULL(PBD.Multiple, P.Multiple) * ISNULL(PBD.Sale_Earned_Disc1, P.Sale_Earned_Disc1) + CAST(ISNULL(PBD.POSSale_Price, P.POSSale_Price) AS MONEY)) * 100))), 5) 
								+ RIGHT(''00000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSPrice, P.POSPrice) AS MONEY)/ISNULL(PBD.Multiple, P.Multiple) * 100))), 5)
					WHEN ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID) = 0 THEN ''00'' + RIGHT(''00000000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSSale_Price, P.POSSale_Price) AS MONEY) * 100))), 8)						
					WHEN ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID) = 4 THEN RIGHT(''00000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSPrice, P.POSPrice) AS MONEY)/ISNULL(PBD.Multiple, P.Multiple) * 100))), 5) 
								+ RIGHT(''00000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSSale_Price, P.POSSale_Price) AS MONEY) * 100))), 5)
					WHEN ISNULL(PBD.PricingMethod_ID, P.PricingMethod_ID) = 5 THEN ''00'' + RIGHT(''00000000'' + CONVERT(varchar(8),CONVERT(int, (CAST(ISNULL(PBD.POSSale_Price, P.POSSale_Price) AS MONEY) * 100))), 8)
				END
			END
		END,		
	ISNULL(ISNULL(IO.Case_Discount, I.Case_Discount), 0) AS Case_Discount,
	CASE ISNULL(ISNULL(IO.Coupon_Multiplier, I.Coupon_Multiplier), 0) WHEN 0 THEN 1 ELSE 0 END AS Coupon_Multiplier,
	CASE ISNULL(ISNULL(IO.FSA_Eligible, I.FSA_Eligible), 0) WHEN 0 THEN 1 ELSE 0 END AS FSA_Eligible,
	ISNULL(ISNULL(IO.Misc_Transaction_Sale, I.Misc_Transaction_Sale), 0) AS Misc_Transaction_Sale,
	ISNULL(ISNULL(IO.Misc_Transaction_Refund, I.Misc_Transaction_Refund), 0) AS Misc_Transaction_Refund,
    (RIGHT(''000''+ CAST(COALESCE(IO.Misc_Transaction_Sale, I.Misc_Transaction_Sale, 0) AS varchar(3)),3) + RIGHT(''000''+ CAST(COALESCE(IO.Misc_Transaction_Refund, I.Misc_Transaction_Refund, 0) AS varchar(3)),3) ) AS MiscTransactionSaleAndRefund,
	0 AS MA_CasePrice,
	ISNULL(ISNULL(IO.Recall_Flag, I.Recall_Flag), 0) AS Recall_Flag,
	ISNULL(P.Age_Restrict, 0) AS Age_Restrict,	
	ISNULL(P.Routing_Priority, 1) AS Routing_Priority, --DEFAULT TO 1; RANGE = 1-99
	CASE WHEN ISNULL(P.Consolidate_Price_To_Prev_Item, 0) = 1 THEN ''Y'' ELSE ''N'' END AS Consolidate_Price_To_Prev_Item,
	CASE WHEN ISNULL(P.Print_Condiment_On_Receipt, 0) = 1 THEN ''Y'' ELSE ''N'' END AS Print_Condiment_On_Receipt,'

	IF @SyncJDA = 1
	BEGIN
	SET @SQL = @SQL + 
	' (SELECT (ISNULL(JDA_Dept, 100) - 100)
	 FROM JDA_HierarchyMapping  
	 WHERE ProdHierarchyLevel4_ID = I.ProdHierarchyLevel4_ID) AS JDA_Dept, '
	END

select @SQL = @SQL + ' 
	 (SELECT ISNULL(Value,'''') FROM KitchenRoute  WHERE KitchenRoute_ID = P.KitchenRoute_ID) AS KitchenRouteValue,
	 
	 --Price ''Savings'' = Sale Price - Reg Price
	 CASE WHEN PCT.On_Sale = 1 THEN ISNULL(PBD.POSPrice, P.POSPrice) - ISNULL(PBD.POSSale_Price, P.POSSale_Price)                                       
		  ELSE 0
		  END AS SavingsAmount,
	 CASE WHEN I.ItemType_ID IN (6,7) 
		  THEN ISNULL(I.PurchaseThresholdCouponAmount, 0.00)
		  ELSE 0.00
		  END AS PurchaseThresholdCouponAmount,
	CASE WHEN I.ItemType_ID IN (6,7) 
		  THEN [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, ISNULL(I.PurchaseThresholdCouponAmount,0) * 100), 1)
		  ELSE ''0.00''
		  END AS PurchaseThresholdCouponAmount_ReversedHex,
	 CASE WHEN ST.PurchaseThresholdCouponAvailable = 1 
			AND I.PurchaseThresholdCouponSubTeam = 1 
			AND I.PurchaseThresholdCouponAmount > 0
			AND I.ItemType_ID in (6,7)
		  THEN ISNULL(I.PurchaseThresholdCouponSubTeam , 0) 
		  ELSE 0 
		  END AS PurchaseThresholdCouponSubTeam,
	 @SmartX_DeletePendingName AS SmartX_DeletePendingName, 
	 @SmartX_MaintenanceDateTime AS SmartX_MaintenanceDateTime,
	 CONVERT(CHAR(8),@Date,10) As SmartX_EffectiveDate,
	 SI.StoreItemAuthorizationID,
	 ISNULL(IO.Product_Code, I.Product_Code) AS Product_Code,
	 ISNULL(IO.Unit_Price_Category, I.Unit_Price_Category) AS Unit_Price_Category,
	 [POSPrice_AsHex] = [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, ROUND((CAST(PBD.POSPrice AS money) * 100)/PBD.Multiple,0)), 1) ,	 
	CASE WHEN ISNULL(P.KitchenRoute_ID,0)<>0 
								THEN [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, P.KitchenRoute_ID ), 1)
								ELSE 
									CASE WHEN I.ItemType_ID IN (6,7) 
									THEN [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, ISNULL(I.PurchaseThresholdCouponAmount,0) * 100), 1) 
									ELSE ''0''
									END 
								END AS PurchaseThresholdCouponAmountReversedHex_GrillPrint_FileWriterElement,
		CASE WHEN PM.POS_Code = ''RBX_Promo'' THEN 1 ELSE 0 END As [RBX_Promo],
		CASE WHEN PM.POS_Code = ''RBX_BasePlusOne'' THEN 1 ELSE 0 END As [RBX_BasePlusOne],
		CASE WHEN PM.POS_Code = ''RBX_GroupThreshold'' THEN 1 ELSE 0 END As [RBX_GroupThreshold],
		CASE WHEN PM.POS_Code = ''RBX_GroupAdjusted'' THEN 1 ELSE 0 END As [RBX_GroupAdjusted],
		CASE WHEN PM.POS_Code = ''RBX_UnitAdjusted'' THEN 1 ELSE 0 END As [RBX_UnitAdjusted],
		CASE WHEN PCT.On_Sale = 1 AND PM.POS_Code = ''RBX_GroupThreshold'' 
			THEN CONVERT(varchar(3), ISNULL(PBD.Sale_Earned_Disc1,P.Sale_Earned_Disc1) + 1) + ''/'' + CONVERT(varchar(10),(ISNULL(PBD.POSPrice, P.POSPrice)*ISNULL(PBD.Sale_Earned_Disc1,P.Sale_Earned_Disc1))+ISNULL(PBD.POSSale_Price,P.POSSale_Price))
			ELSE '''' END AS [RBX_GroupThresholdPrice],			
		CASE WHEN PCT.On_Sale = 1 AND PM.POS_Code = ''RBX_GroupAdjusted'' 
			THEN CONVERT(varchar(3), ISNULL(PBD.Sale_Multiple, P.Sale_Multiple)) + ''/'' + CONVERT(varchar(10),ISNULL(PBD.POSSale_Price,P.POSSale_Price))
			ELSE '''' END AS [RBX_GroupAdjustedPrice],

		CASE WHEN PCT.On_Sale = 1 AND PM.POS_Code = ''RBX_UnitAdjusted''
			THEN CONVERT(varchar(3), ISNULL(PBD.Sale_Earned_Disc3,P.Sale_Earned_Disc3)) + ''/'' + CONVERT(varchar(10),ISNULL(PBD.POSSale_Price,P.POSSale_Price))
			ELSE '''' END AS [RBX_UnitAdjustedPrice],
		I.Sign_Description,
		P.ItemSurcharge,
		[dbo].[fn_ConvertVarBinaryToHex](P.ItemSurcharge,1) as [ItemSurcharge_AsHex],
		CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + '','' + CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + '',''+ CAST(Scale_LabelStyle.Description AS VARCHAR(5)) AS [Digi_LNU],
		CASE WHEN ISO.Item_Key IS NOT NULL
			THEN ISO.Nutrifact_ID
			ELSE ItemScale.Nutrifact_ID END AS Nutrifact_ID,
		I.GiftCard,
		NULL AS CancelAllSales,
		0 AS PriceBatchDetailID,
		ssd.StorageData AS StorageText
FROM dbo.Price P 
INNER JOIN
	dbo.Item I 
	ON (I.Item_Key = P.Item_Key) '
	
	IF @IdentifierAdds = 1
	BEGIN
			SET @SQL = @SQL + ' AND I.Deleted_Item = 0 AND I.Remove_Item = 0 '
	END

	--Only pulling back non batchable changes for items that do not currently have a change batched up
	--in order to avoid sending duplicate changes that don't have the latest info to the POS.
	IF @IsItemNonBatchableChanges = 1
		BEGIN
			SET @SQL = @SQL + '		
				INNER JOIN 
					( 
						select inbc.*
						from dbo.ItemNonBatchableChanges inbc 
						where inbc.StartDate <= @Date
							and not exists
							(
								select 1 
								from PriceBatchDetail pbd 
								join PriceBatchHeader pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
									and pbh.PriceBatchStatusID = 5
								where pbd.Item_Key = inbc.Item_Key
									and pbh.StartDate <= @Date
							)
					) inbc ON I.Item_Key = inbc.Item_Key'
		END

	IF @IsScaleZoneData = 0 AND @POSDeAuthData = 1
	BEGIN
	SET @SQL = @SQL + '
INNER JOIN   
	dbo.StoreItem SI 
	ON SI.Item_Key = P.Item_Key  
	   AND SI.Store_No = P.Store_No   
	   AND SI.POSDeAuth = 1 
INNER JOIN
	dbo.StoreItemVendor SIV 
	ON SIV.Store_No = P.Store_No
		AND SIV.Item_Key = I.Item_Key 
		AND SIV.Vendor_ID = 
			(select top 1 
			SIV2.Vendor_ID
			from StoreItemVendor SIV2
			where SIV.Store_No = SIV2.Store_No
				and SIV.Item_Key = SIV2.Item_Key) '
	END
	ELSE IF @IsScaleZoneData = 1 AND @ScaleDeAuthData = 1
	BEGIN
	SET @SQL = @SQL + '
INNER JOIN   
	dbo.StoreItem SI 
	ON SI.Item_Key = P.Item_Key  
	   AND SI.Store_No = P.Store_No   
	   AND SI.POSDeAuth = 1 
INNER JOIN
	dbo.StoreItemVendor SIV 
	ON SIV.Store_No = P.Store_No
		AND SIV.Item_Key = I.Item_Key
		AND SIV.Vendor_ID = 
			(select top 1 
			SIV2.Vendor_ID
			from StoreItemVendor SIV2
			where SIV.Store_No = SIV2.Store_No
				and SIV.Item_Key = SIV2.Item_Key) ' 
	END
	ELSE IF @IsScaleZoneData = 1 AND @ScaleAuthData = 1 
	BEGIN
	SET @SQL = @SQL + '
INNER JOIN   
	dbo.StoreItem SI 
	ON SI.Item_Key = P.Item_Key  
	   AND SI.Store_No = P.Store_No   
	   AND SI.ScaleAuth = 1
INNER JOIN
	dbo.StoreItemVendor SIV 
	ON SIV.Store_No = P.Store_No
		AND SIV.Item_Key = I.Item_Key
		AND SIV.PrimaryVendor = 1 '
	END
	ELSE IF @IdentifierRefreshes = 1
	BEGIN
	SET @SQL = @SQL + '
INNER JOIN   
	dbo.StoreItem SI 
	ON SI.Item_Key = P.Item_Key  
	   AND SI.Store_No = P.Store_No   
	   AND SI.Refresh = 1
INNER JOIN
	dbo.StoreItemVendor SIV 
	ON SIV.Store_No = P.Store_No
		AND SIV.Item_Key = I.Item_Key
		AND SIV.PrimaryVendor = 1 '
	END
	ELSE
	BEGIN
	SET @SQL = @SQL + '
INNER JOIN   
	dbo.StoreItem SI 
	ON SI.Item_Key = P.Item_Key  
	   AND SI.Store_No = P.Store_No   
	   AND SI.Authorized = 1
INNER JOIN
	dbo.StoreItemVendor SIV 
	ON SIV.Store_No = P.Store_No
		AND SIV.Item_Key = I.Item_Key
		AND SIV.PrimaryVendor = 1 '
	END

SET @SQL = @SQL + '	    
INNER JOIN
	#Identifiers II
	ON II.Item_Key = P.Item_Key '
	IF @IdentifierAdds = 1
	BEGIN
			SET @SQL = @SQL + ' AND II.Add_Identifier = 1 AND II.Remove_Identifier = 0 '
	END

	IF @IdentifierDeletes = 1
	BEGIN
			SET @SQL = @SQL + ' AND II.Add_Identifier = 0 AND II.Remove_Identifier = 1 '
	END

	-- For POS Push, Adds are sent outside of this stored proc
	-- For Scale Push, Adds should be sent here with the Zone price records
	IF @IsScaleZoneData = 0 AND @IsPOSPush = 1
	BEGIN
			SET @SQL = @SQL + ' AND II.Add_Identifier = 0 '
	END

	--ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES	IF @IsScaleZoneData = 1 
	IF @IsScaleZoneData = 1 
	BEGIN
			SET @SQL = @SQL + ' AND II.Scale_Identifier = 1 '
	END

	-- LIMIT DATA TO PRICE CHANGES OR ITEM DELETES AND SCALE ITEMS ONLY: USED BY SCALE PUSH ZONE RECORDS
	IF @ExcludeSKUIdentifiers = 1 
	BEGIN
			SET @SQL = @SQL + ' AND II.IdentifierType <> ''S'''
	END

SET @SQL = @SQL + '	    
INNER JOIN
	#Stores Store 
	ON Store.Store_No = P.Store_No	
LEFT JOIN
	dbo.ItemOverride IO 
	ON IO.Item_Key = P.Item_Key 
		AND IO.StoreJurisdictionID = Store.StoreJurisdictionID
		AND @UseRegionalScaleFile = 0
		AND @UseStoreJurisdictions = 1
LEFT JOIN
	dbo.ItemScaleOverride ISO 
	ON ISO.Item_Key = P.Item_Key
		AND ISO.StoreJurisdictionID = Store.StoreJurisdictionID
		AND @UseRegionalScaleFile = 0
		AND @UseStoreJurisdictions = 1
LEFT JOIN
	dbo.ItemUomOverride IUO 
	ON IUO.Item_Key = P.Item_Key
		AND IUO.Store_No = Store.Store_No
LEFT JOIN
	dbo.StoreSubTeam SST 
	ON SST.Store_No = P.Store_No 
		AND SST.SubTeam_No = I.SubTeam_No					 
LEFT JOIN
	dbo.StoreSubTeam SST_Exception 
	ON SST_Exception.Store_No = P.Store_No 
		AND SST_Exception.SubTeam_No = P.ExceptionSubTeam_No 
LEFT JOIN
	dbo.StoreSubTeam SST_Scale 
	ON SST_Scale.Store_No = P.Store_No 
		AND SST_Scale.SubTeam_No = I.SubTeam_No	
LEFT JOIN
	dbo.SubTeam ST 
	ON ST.SubTeam_No = SST.SubTeam_No
LEFT JOIN
	dbo.SubTeam ST_Exception 
	ON ST_Exception.SubTeam_No = SST_Exception.SubTeam_No 
LEFT JOIN
	dbo.SubTeam ST_Scale 
	ON ST_Scale.SubTeam_No = SST_Scale.SubTeam_No    
LEFT JOIN 
	dbo.fn_VendorCostAll(@Date) VCA
	ON VCA.Item_Key = P.Item_Key 
		AND VCA.Store_No = P.Store_No 
		AND VCA.Vendor_ID = SIV.Vendor_ID'

--Breaking up the assignment of the from clause in order to keep the 
--dynamic SQL statement from being truncated
SET @SQL = @SQL + '
LEFT JOIN
	dbo.Vendor V 		  
	ON V.Vendor_ID = SIV.Vendor_ID
LEFT JOIN
	dbo.ItemVendor VI 
	ON VI.Vendor_ID = V.Vendor_Id
		AND VI.Item_Key = I.Item_Key 
		AND (VI.DeleteDate IS NULL OR VI.DeleteDate > @Date)
LEFT JOIN
	dbo.Price LP 
	ON LP.Store_No = P.Store_No
		AND LP.Item_Key = P.LinkedItem
LEFT JOIN
	#Identifiers LII
	ON P.LinkedItem = LII.Item_Key
		AND LII.Default_Identifier = 1	
LEFT JOIN
	dbo.ItemScale 
	ON ItemScale.Item_Key = I.Item_Key
LEFT JOIN 
	dbo.ItemCustomerFacingScale icfs  on ItemScale.Item_Key = icfs.Item_Key
LEFT JOIN 
	dbo.Scale_ExtraText 
	ON ItemScale.Scale_ExtraText_ID = Scale_ExtraText.Scale_ExtraText_ID
LEFT JOIN dbo.Scale_Ingredient SING  ON ItemScale.Scale_Ingredient_ID = SING.Scale_Ingredient_ID LEFT JOIN Scale_Allergen SA  ON ItemScale.Scale_Allergen_ID = SA.Scale_Allergen_ID
LEFT JOIN 
	dbo.Scale_ExtraText Scale_ExtraText_Override 
	ON ISO.Scale_ExtraText_ID = Scale_ExtraText_Override.Scale_ExtraText_ID
LEFT JOIN 
	dbo.Scale_Tare Scale_Tare 
	ON ItemScale.Scale_Tare_ID = Scale_Tare.Scale_Tare_ID
LEFT JOIN 
	dbo.Scale_Tare Scale_Tare_Override 
	ON ISO.Scale_Tare_ID = Scale_Tare_Override.Scale_Tare_ID
LEFT JOIN
	dbo.Scale_Tare Alt_Scale_Tare 
	ON ISNULL(ISO.Scale_Alternate_Tare_ID, ItemScale.Scale_Alternate_Tare_ID) = Alt_Scale_Tare.Scale_Tare_ID
LEFT JOIN
	dbo.Scale_Grade 
	ON ISNULL(ISO.Scale_Grade_ID, ItemScale.Scale_Grade_ID) = Scale_Grade.Scale_Grade_ID
LEFT JOIN
	Scale_LabelStyle 
	ON ISNULL(ISO.Scale_LabelStyle_ID, ItemScale.Scale_LabelStyle_ID) = Scale_LabelStyle.Scale_LabelStyle_ID
LEFT JOIN
	dbo.ItemUnit ScaleUOM 
	ON ScaleUOM.Unit_ID = ItemScale.Scale_ScaleUOMUnit_ID
LEFT JOIN
	dbo.ItemUnit ScaleUOM_Override 
	ON ScaleUOM_Override.Unit_ID = ISO.Scale_ScaleUOMUnit_ID
LEFT JOIN
	dbo.ItemUnit ScaleUOM_UOM_Override 
	ON ScaleUOM_UOM_Override.Unit_ID = IUO.Scale_ScaleUomUnit_ID
LEFT JOIN
	dbo.ItemUnit RU 
	ON RU.Unit_ID = I.Retail_Unit_ID
LEFT JOIN
	dbo.ItemUnit RU_Override 
	ON RU_Override.Unit_ID = IO.Retail_Unit_ID
LEFT JOIN
	dbo.ItemUnit RU_UOM_Override 
	ON RU_UOM_Override.Unit_ID = IUO.Retail_Unit_ID
LEFT JOIN
	dbo.ItemUnit PU 
	ON PU.Unit_ID = I.Package_Unit_ID
LEFT JOIN
	dbo.ItemUnit PU_Override 
	ON PU_Override.Unit_ID = IO.Package_Unit_ID
LEFT JOIN
	dbo.LabelType LT 
	ON LT.LabelType_ID = ISNULL(IO.LabelType_ID, I.LabelType_ID)
LEFT JOIN 
	dbo.ItemBrand IB 
	ON IB.Brand_ID = ISNULL(IO.Brand_ID, I.Brand_ID)
LEFT JOIN
	#PBDPrices PBD
	ON Store.Store_No = PBD.Store_No
	AND P.Item_Key = PBD.Item_Key
INNER JOIN
	dbo.PriceChgType PCT 
	ON PCT.PriceChgTypeID = ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID)
LEFT JOIN
	PricingMethod PM 
	ON PBD.PricingMethod_ID = PM.PricingMethod_ID
LEFT JOIN Scale_StorageData ssd (nolock)
	ON ItemScale.Scale_StorageData_ID = ssd.Scale_StorageData_ID
WHERE 
    (Mega_Store = 1 OR WFM_Store = 1) 
    AND (
            @IsScaleZoneData = 0 
            OR 
            (@IsScaleZoneData = 1 AND ((Mega_Store = 1 AND icfs.SendToScale = 1 AND dbo.fn_IsPosPlu(ii.Identifier) = 1) OR icfs.SendToScale IS NULL))
        )'
IF @AuditReport = 1
BEGIN
		SET @SQL = @SQL + ' AND (Store.Store_No = @Store_No) AND (ISNULL(PBD.Price, P.Price) > 0 OR (ISNULL(PBD.Price, P.Price) = 0 and I.Price_Required = 1))'
END

	SET @SQL = @SQL + ' ORDER BY Store.Store_No, PriceBatchHeaderID, I.Item_Key, II.Identifier'

-- Print the SQL query
IF @Debug = 1
	BEGIN
		PRINT @SQL
	END
 
-- Define the input parameters used within the query.
DECLARE @params nvarchar(MAX)
SELECT @params =   '@NewItemVal				bit,	
					@ItemChangeVal			bit,	
					@RemoveItemVal			bit,	
					@PIRUSHeaderActionVal	varchar(2),	
					@Deletes				bit,
					@PluDigitsSentToScale	varchar(20),
					@SmartX_DeletePendingName char(16),
					@SmartX_MaintenanceDateTime char(16),
					@UseStoreJurisdictions	bit,		
					@UseRegionalScaleFile	bit,
					@Date					datetime,
					@Store_No				int,
					@IsScaleZoneData		bit,
					@IsItemNonBatchableChanges bit,
					@CustomerFacingScaleDepartmentPrefix nvarchar(1)'
					
-- Execute the dynamic SQL.
EXEC sp_executesql @SQL, @params, 
						 @NewItemVal, 
						 @ItemChangeVal, 
						 @RemoveItemVal, 
						 @PIRUSHeaderActionVal, 
						 @Deletes, 
						 @PluDigitsSentToScale, 
						 @SmartX_DeletePendingName, 
						 @SmartX_MaintenanceDateTime, 
						 @UseStoreJurisdictions, 
						 @UseRegionalScaleFile, 
						 @Date,
						 @Store_No,
						 @IsScaleZoneData,
						 @IsItemNonBatchableChanges,
						 @CustomerFacingScaleDepartmentPrefix

DROP TABLE #PBDPrices
DROP TABLE #Identifiers

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Dynamic_POSSearchForNonBatchedChanges] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Dynamic_POSSearchForNonBatchedChanges] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Dynamic_POSSearchForNonBatchedChanges] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Dynamic_POSSearchForNonBatchedChanges] TO [IRMASchedJobsRole]
    AS [dbo];

