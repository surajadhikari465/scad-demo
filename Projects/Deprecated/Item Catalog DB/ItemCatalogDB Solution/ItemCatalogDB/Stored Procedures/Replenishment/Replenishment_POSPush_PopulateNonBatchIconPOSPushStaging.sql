IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Replenishment_POSPush_PopulateNonBatchIconPOSPushStaging]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	EXEC ('create PROCEDURE [dbo].[Replenishment_POSPush_PopulateNonBatchIconPOSPushStaging] (@foo int) as select 1')
GO

ALTER PROCEDURE dbo.Replenishment_POSPush_PopulateNonBatchIconPOSPushStaging
AS
	/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK		Description
----------------------------------------------------------------------------------------------
Jamali	08/29/2016	PBI17835	Wrote the code pull the code for the non-batch data and write it 
								directly to the IconPOSPushStaging table
							
***********************************************************************************************/

BEGIN

SET NOCOUNT ON

DECLARE @Date DATETIME

SET @Date = CURRENT_TIMESTAMP

--Using the regional scale file?
DECLARE @UseRegionalScaleFile bit
SELECT @UseRegionalScaleFile = (SELECT FlagValue FROM dbo.InstanceDataFlags  WHERE FlagKey='UseRegionalScaleFile')
		-- Check the Store Jurisdiction Flag
DECLARE @UseStoreJurisdictions int
SELECT @UseStoreJurisdictions = FlagValue FROM dbo.InstanceDataFlags  WHERE FlagKey = 'UseStoreJurisdictions'
 
DECLARE @CurrDay smalldatetime
SELECT @CurrDay = CONVERT(smalldatetime, CONVERT(varchar(255), @Date, 101))
DECLARE @ExcludedStoreNo varchar(250) = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'))

IF OBJECT_ID('tempdb..#Identifiers') IS NOT NULL
BEGIN
	DROP TABLE #Identifiers
END 

IF OBJECT_ID('tempdb..#PBDPrices') IS NOT NULL
BEGIN
	DROP TABLE #PBDPrices
END

IF OBJECT_ID('tempdb..#Stores') IS NOT NULL
BEGIN
	DROP TABLE #Stores
END

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

CREATE TABLE #Stores
(
	Store_Name VARCHAR(50)
	, Store_no INT
	, StoreJurisdictionID INT
	, PLUMStoreNo INT
	, Mega_Store BIT
	, WFM_Store BIT
	PRIMARY KEY CLUSTERED (Store_no)
)

--get all the R10 Stores
INSERT INTO #Stores
SELECT 
	S.Store_Name, 
	s.store_no, 
	s.StoreJurisdictionID, 
	s.PLUMStoreNo, 
	s.Mega_Store, 
	s.WFM_Store
FROM StorePOSConfig SPC(NOLOCK)
JOIN POSWriter POS(NOLOCK) ON SPC.POSFileWriterKey = POS.POSFileWriterKey
JOIN Store S(NOLOCK) ON SPC.Store_No = S.Store_No
WHERE POS.POSFileWriterCode = 'R10'  --only the data from the non-R10 stores is needed
	AND (Mega_Store = 1 OR WFM_Store = 1)
	AND (s.Internal = 1 AND s.BusinessUnit_ID IS NOT NULL)

--get the item identifiers that we need to work with
insert into #Identifiers
select Identifier_ID, Item_Key, Identifier, Default_Identifier, Deleted_Identifier
	, Add_Identifier, Remove_Identifier, National_Identifier, CheckDigit, IdentifierType
	, NumPluDigitsSentToScale, Scale_Identifier
from fn_GetItemIdentifiers()

UPDATE #Identifiers
SET Scale_Identifier = 1
FROM #Identifiers i
INNER JOIN dbo.ItemCustomerFacingScale icfs ON i.Item_Key = icfs.Item_Key
where icfs.SendToScale = 1


-- Select the unexpired Price changes from PriceBatchDetail
-- All references to the corresponding fields in Price should be wrapped with an ISNULL, same as ItemOverride,
-- to capture any existing price changes for this item already in progress
CREATE TABLE #PBDPrices (Item_Key INT, Store_No INT, Price MONEY,	Multiple TINYINT, Sale_Multiple TINYINT, Sale_Price MONEY,	StartDate SMALLDATETIME, Sale_End_Date SMALLDATETIME, PriceChgTypeId INT, PricingMethod_ID INT, Sale_Earned_Disc1 TINYINT, Sale_Earned_Disc2 TINYINT, Sale_Earned_Disc3 TINYINT, POSPrice MONEY, POSSale_Price MONEY)

/*
If the PriceBatchDetails contain the data for price change records(PriceChangeType == 'REG' or 'SAL') for a Store_No and Item_Key
then we use the price data from the PriceBatchDetail table instead of using it from the Price table.
Since the PriceBatchDetail can have more than one combination Store_No and Item_Key, we get the oldest record from the PriceBatchDetail record
based on the StartDate in the PriceBatchHeader table
*/
;WITH Prices AS 
(
	SELECT pbd.Item_Key, pbd.Store_No, pbd.Price, pbd.Multiple, pbd.Sale_Multiple
	, pbd.Sale_Price, pbd.StartDate, pbd.Sale_End_Date, pbd.PriceChgTypeId, pbd.PricingMethod_ID
	, pbd.Sale_Earned_Disc1, pbd.Sale_Earned_Disc2, pbd.Sale_Earned_Disc3, pbd.POSPrice, pbd.POSSale_Price 
	,  ROW_NUMBER() OVER(PARTITION BY pbd.Item_Key, pbd.Store_No ORDER BY pbh.StartDate) As RowNum
	FROM dbo.PriceBatchDetail pbd
	INNER JOIN dbo.PriceBatchHeader pbh  ON pbh.PriceBatchHeaderID = pbd.PriceBatchHeaderID
	INNER JOIN PriceChgType pc ON pbd.PriceChgTypeID = pc.PriceChgTypeID
	WHERE PBH.PriceBatchStatusID = 5
		AND PBD.Offer_ID IS NULL
		AND pc.PriceChgTypeDesc IS NOT NULL
		AND PBH.StartDate <= @CurrDay
)
INSERT INTO #PBDPrices
SELECT Item_Key, Store_No, Price, Multiple, Sale_Multiple
	, Sale_Price, StartDate, Sale_End_Date, PriceChgTypeId, PricingMethod_ID
	, Sale_Earned_Disc1, Sale_Earned_Disc2, Sale_Earned_Disc3, POSPrice, POSSale_Price
FROM Prices WHERE RowNum = 1

BEGIN TRANSACTION

BEGIN TRY
	;WITH  NonBatchData AS
	(
		SELECT
			Mega_Store, WFM_Store, I.Deleted_Item, I.Remove_Item, II.Add_Identifier, II.Remove_Identifier, SI.Authorized, SIV.PrimaryVendor, SI.Refresh, si.POSDeAuth
			, 0 AS PriceBatchHeaderID, P.Store_No AS Store_No, i.Item_Key AS Item_Key, ii.Identifier AS Identifier, 'ScanCodeAdd' as ChangeType
			, @Date as InsertDate, ISNULL(IO.Package_Desc2, I.Package_Desc2) AS RetailSize --POS Push Code: Package_Desc2
			, ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '') AS RetailUom -- POS Push code: Package_Unit_Abbr
			, P.Discountable AS TMDiscountEligible, P.IBM_Discount AS Case_Discount
			, CASE WHEN P.AgeCode = 0 THEN NULL ELSE P.AgeCode END AS AgeCode, ISNULL(ISNULL([IO].Recall_Flag, I.Recall_Flag), 0) AS Recall_Flag
			, P.Restricted_Hours AS Restricted_Hours
			, CASE WHEN COALESCE(RU_UOM_Override.Weight_Unit, RU_Override.Weight_Unit, RU.Weight_Unit, 0) = 1 THEN 'true' ELSE 'false' END AS Sold_By_Weight--POS Push Code: RetailUnit_WeightUnit
			, ISNULL(ISO.ForceTare, ItemScale.ForceTare) AS ScaleForcedTare --POS Push Code: ForceTare
			, ISNULL([IO].Quantity_Required, I.Quantity_Required) AS Quantity_Required
			, ISNULL(IO.Price_Required, I.Price_Required) AS Price_Required
			, ISNULL(ISNULL([IO].QtyProhibit, I.QtyProhibit), 0) AS QtyProhibit -- POS Push Code: QtyProhibit_Boolean
			, CASE WHEN ISNULL(P.VisualVerify, 0) = 1 THEN 'true' ELSE 'false' END AS VisualVerify
			, CASE WHEN P.NotAuthorizedForSale = 1 THEN 'true' ELSE 'false' END AS RestrictSale --POS Push Code: NotAuthorizedForSale
			, ROUND(ISNULL(PBD.Price, P.Price), 2) AS Price  -- this value will be POSPrice unless item is on sale it will be POSSale_Price
			, ROUND(ISNULL(PBD.Sale_Price, P.Sale_Price), 2) AS Sale_Price, ISNULL(PBD.Multiple, P.Multiple) AS Multiple
			, ISNULL(PBD.Sale_Multiple, P.Sale_Multiple) AS SaleMultiple, ISNULL(PBD.StartDate, P.Sale_Start_Date) As Sale_Start_Date -- POS Push Code: StartDate
			, ISNULL(PBD.Sale_End_Date, P.Sale_End_Date) As Sale_End_Date, LII.Identifier AS LinkCode_ItemIdentifier, P.PosTare as POSTare, 0 AS PriceBatchDetailID		
		FROM dbo.Price P 
			INNER JOIN dbo.Item I ON I.Item_Key = P.Item_Key
			INNER JOIN #Identifiers II ON II.Item_Key = P.Item_Key    
			INNER JOIN dbo.#Stores Store ON Store.Store_No = P.Store_No	
			INNER JOIN dbo.StoreItem SI ON SI.Item_Key = P.Item_Key AND SI.Store_No = P.Store_No 
			INNER JOIN dbo.StoreItemVendor SIV ON SIV.Store_No = P.Store_No AND SIV.Item_Key = I.Item_Key 
			LEFT JOIN dbo.ItemOverride [IO] ON [IO].Item_Key = P.Item_Key AND [IO].StoreJurisdictionID = Store.StoreJurisdictionID AND @UseRegionalScaleFile = 0 AND @UseStoreJurisdictions = 1
			LEFT JOIN dbo.ItemScaleOverride ISO ON ISO.Item_Key = P.Item_Key AND ISO.StoreJurisdictionID = Store.StoreJurisdictionID AND @UseRegionalScaleFile = 0 AND @UseStoreJurisdictions = 1
			LEFT JOIN dbo.ItemUomOverride IUO ON IUO.Item_Key = P.Item_Key AND IUO.Store_No = Store.Store_No
			LEFT JOIN #Identifiers LII ON P.LinkedItem = LII.Item_Key AND LII.Default_Identifier = 1	
			LEFT JOIN dbo.ItemScale ON ItemScale.Item_Key = I.Item_Key		
			LEFT JOIN dbo.ItemUnit PU ON PU.Unit_ID = I.Package_Unit_ID
			LEFT JOIN dbo.ItemUnit PU_Override ON PU_Override.Unit_ID = [IO].Package_Unit_ID	
			LEFT JOIN #PBDPrices PBD ON Store.Store_No = PBD.Store_No AND P.Item_Key = PBD.Item_Key
			INNER JOIN dbo.PriceChgType PCT ON PCT.PriceChgTypeID = ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID)
			LEFT JOIN dbo.ItemUnit RU ON RU.Unit_ID = I.Retail_Unit_ID 
			LEFT JOIN dbo.ItemUnit RU_Override ON RU_Override.Unit_ID = [IO].Retail_Unit_ID
			LEFT JOIN dbo.ItemUnit RU_UOM_Override ON RU_UOM_Override.Unit_ID = IUO.Retail_Unit_ID
		WHERE PrimaryVendor = 1
	)
	INSERT INTO IconPOSPushStaging
	--GET THE DATA FOR THE NON BATCHABLE IDENTIFIER ADDS
	SELECT PriceBatchHeaderID, Store_No, Item_Key, Identifier, 'ScanCodeAdd' as ChangeType
			, InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount, AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
			, ISNULL(ScaleForcedTare, 0) AS ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale, Price, Sale_Price, Multiple
			, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier, POSTare, PriceBatchDetailID
	FROM NonBatchData NBD
		where Deleted_Item = 0 AND Remove_Item = 0
			AND Add_Identifier = 1 AND Remove_Identifier = 0
			AND Authorized = 1
	UNION ALL
	--GET THE DATA FOR THE NON BATCHABLE IDENTIFIER DELETES
	SELECT PriceBatchHeaderID, Store_No, Item_Key, Identifier, 'ScanCodeDelete' as ChangeType
			, InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount, AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
			, ISNULL(ScaleForcedTare, 0) AS ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale, Price, Sale_Price, Multiple
			, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier, POSTare, PriceBatchDetailID
	FROM NonBatchData NBD
	WHERE Authorized = 1
		AND Add_Identifier = 0 AND Remove_Identifier = 1
	UNION ALL
	--GET THE DATA FOR THE NON BATCHABLE IDENTIFIER REFRESHES
	SELECT PriceBatchHeaderID, Store_No, Item_Key, Identifier, 'ScanCodeAdd' as ChangeType
			, InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount, AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
			, ISNULL(ScaleForcedTare, 0) AS ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale, Price, Sale_Price, Multiple
			, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier, POSTare, PriceBatchDetailID
	FROM NonBatchData NBD
	WHERE Refresh = 1 
	
	--GET THE DATA FOR THE NON BATCHABLE POS Deauthorizations	
	--INSERTS THE DATA FOR THE pos DeAuth's
	INSERT INTO IconPOSPushStaging
	SELECT --'IdentifierPOSDeAuth' as IdentifierPOSDeAuth
		0 AS PriceBatchHeaderID, P.Store_No AS Store_No, i.Item_Key AS Item_Key, ii.Identifier AS Identifier, 'ScanCodeDeauthorization' as ChangeType
		, @Date as InsertDate, ISNULL(IO.Package_Desc2, I.Package_Desc2) AS RetailSize --POS Push Code: Package_Desc2
		, ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '') AS RetailUom -- POS Push code: Package_Unit_Abbr
		, P.Discountable AS TMDiscountEligible, P.IBM_Discount AS Case_Discount
		, CASE WHEN P.AgeCode = 0 THEN NULL ELSE P.AgeCode END AS AgeCode, ISNULL(ISNULL([IO].Recall_Flag, I.Recall_Flag), 0) AS Recall_Flag
		, P.Restricted_Hours AS Restricted_Hours
		, CASE WHEN COALESCE(RU_UOM_Override.Weight_Unit, RU_Override.Weight_Unit, RU.Weight_Unit, 0) = 1 THEN 'true' ELSE 'false' END AS Sold_By_Weight--POS Push Code: RetailUnit_WeightUnit
		, ISNULL(ISNULL(ISO.ForceTare, ItemScale.ForceTare), 0) AS ScaleForcedTare --POS Push Code: ForceTare
		, ISNULL([IO].Quantity_Required, I.Quantity_Required) AS Quantity_Required
		, ISNULL(IO.Price_Required, I.Price_Required) AS Price_Required
		, ISNULL(ISNULL([IO].QtyProhibit, I.QtyProhibit), 0) AS QtyProhibit -- POS Push Code: QtyProhibit_Boolean
		, CASE WHEN ISNULL(P.VisualVerify, 0) = 1 THEN 'true' ELSE 'false' END AS VisualVerify
		, CASE WHEN P.NotAuthorizedForSale = 1 THEN 'true' ELSE 'false' END AS RestrictSale --POS Push Code: NotAuthorizedForSale
		, ROUND(ISNULL(PBD.Price, P.Price), 2) AS Price  -- this value will be POSPrice unless item is on sale it will be POSSale_Price
		, ROUND(ISNULL(PBD.Sale_Price, P.Sale_Price), 2) AS Sale_Price, ISNULL(PBD.Multiple, P.Multiple) AS Multiple
		, ISNULL(PBD.Sale_Multiple, P.Sale_Multiple) AS SaleMultiple, ISNULL(PBD.StartDate, P.Sale_Start_Date) As Sale_Start_Date -- POS Push Code: StartDate
		, ISNULL(PBD.Sale_End_Date, P.Sale_End_Date) As Sale_End_Date, LII.Identifier AS LinkCode_ItemIdentifier, P.PosTare as POSTare, 0 AS PriceBatchDetailID	
	FROM dbo.Price P 
	INNER JOIN dbo.Item I ON I.Item_Key = P.Item_Key
	INNER JOIN dbo.StoreItem SI ON SI.Item_Key = P.Item_Key AND SI.Store_No = P.Store_No
	INNER JOIN dbo.StoreItemVendor SIV ON SIV.Store_No = P.Store_No AND SIV.Item_Key = I.Item_Key 
		AND SIV.Vendor_ID = (select top 1 SIV2.Vendor_ID from StoreItemVendor SIV2 where SIV.Store_No = SIV2.Store_No and SIV.Item_Key = SIV2.Item_Key)  
	INNER JOIN #Identifiers II ON II.Item_Key = P.Item_Key
	INNER JOIN dbo.#Stores Store ON Store.Store_No = P.Store_No	
	LEFT JOIN dbo.ItemOverride [IO] ON [IO].Item_Key = P.Item_Key AND [IO].StoreJurisdictionID = Store.StoreJurisdictionID AND @UseRegionalScaleFile = 0 AND @UseStoreJurisdictions = 1
	LEFT JOIN dbo.ItemScaleOverride ISO ON ISO.Item_Key = P.Item_Key AND ISO.StoreJurisdictionID = Store.StoreJurisdictionID AND @UseRegionalScaleFile = 0 AND @UseStoreJurisdictions = 1
	LEFT JOIN dbo.ItemUomOverride IUO ON IUO.Item_Key = P.Item_Key AND IUO.Store_No = Store.Store_No
	LEFT JOIN #Identifiers LII ON P.LinkedItem = LII.Item_Key AND LII.Default_Identifier = 1	
	LEFT JOIN dbo.ItemScale ON ItemScale.Item_Key = I.Item_Key
	LEFT JOIN dbo.ItemUnit PU ON PU.Unit_ID = I.Package_Unit_ID
	LEFT JOIN dbo.ItemUnit PU_Override ON PU_Override.Unit_ID = [IO].Package_Unit_ID	
	LEFT JOIN #PBDPrices PBD ON Store.Store_No = PBD.Store_No AND P.Item_Key = PBD.Item_Key
	INNER JOIN dbo.PriceChgType PCT ON PCT.PriceChgTypeID = ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID)
	LEFT JOIN dbo.ItemUnit RU ON RU.Unit_ID = I.Retail_Unit_ID 
	LEFT JOIN dbo.ItemUnit RU_Override ON RU_Override.Unit_ID = [IO].Retail_Unit_ID
	LEFT JOIN dbo.ItemUnit RU_UOM_Override ON RU_UOM_Override.Unit_ID = IUO.Retail_Unit_ID
	WHERE SI.POSDeAuth = 1
		AND II.Add_Identifier = 0 
	
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION 

	DECLARE @ErrorMessage NVARCHAR(MAX);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	
	SELECT 
		@ErrorMessage = '[Replenishment_POSPush_PopulateIconPOSPushPublish] failed with error: ' + ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE()

	RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
END CATCH
END
GO