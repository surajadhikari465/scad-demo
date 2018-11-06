CREATE PROCEDURE [dbo].[GetPLUMCorpChg]
(
	@ActionCode char(1),
	@Date		datetime,
	@Store_No	int = null  -- Store number if this is being used for the complete scale file process
)

AS

-- **************************************************************************
-- Procedure: GetPLUMCorpChg(ActionCode, Date, Store_No)
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called to get changes to be sent to the scale(s)
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 04/30/2012	BJL   			5592	Add column Digi_LNU.
-- 08/11/2012	BJL				6577	Add NutriFacts to support sending NutriFact as row 3 in Item ID Add
--										and Item Change. This will allow NutriFacts to be included in Build
--										Store Scale File.
-- 10/12/2012	DN				6744	Add conditions to extract scale PLU and UPC for non-type-2 identifiers
-- 2013-01-21	KM				9394	Check ItemScaleOverride for new 4.8 scale override values ForceTare and Scale_Alternate_Tare_ID (only in
--										sections of the code where it indicates override values should be checked);
-- 2013-01-25	KM				9382	More override columns: Scale_EatBy_ID, Scale_Grade_ID, and all of the PrintBlank columns;
-- 2013-01-31	KM				9393	One more override column: Nutrifact_ID;
-- 01/31/2013	DN				9995	Added conditions to extract scale PLU and UPC for type-2 identifiers
-- 08/28/2014	DN				15405	Added logic to use either validated or non-validated identifiers
-- 10/22/2014	DN				15467	Correct the order of the fields: NumPluDigitsSentToScale & Scale_Identifier
-- 12/18/2014	DN				15628	Used temp table #Identifiers instead of @ItemIdentifiers.
--										Moved BEGIN TRAN below the code block that populates @Identifiers
-- 07/02/2015	DN				16245	Renamed Columns:
--										Size			-> ServingsPerPortion
--										SizeText		-> ServingSizeDesc
--										PerContainer	-> ServingPerContainer
-- 07/03/2015	DN				16250	Concatenation of Ingredient, Allergen & Extra Text fields
-- 07/03/2015	DN				16245	Renamed the columns in the output to match with the filewriter column names
--										ServingsPerPortion	->	Size
--										ServingSizeDesc		->	SizeText
--										ServingPerContainer	->	PerContainer
-- 07/07/2015	DN				16250	Reorder the fields: Ingredients + Allergens + ExtraText
-- 07/08/2015	DN				16253	Correct the order of the Ingredients field
--										Ingredients + Allergens + ExtraText
-- 09/03/2015	KM				10720	Include Item.Retail_Sale in all output so that non-retail items can be filtered out in the scale push code.
-- 2015-10-23   MZ              16583 (12203)	Don't send nutrifacts to CAD stores if the alternate jurisdiction nutrifacts don't exist.
-- 2016-02-04	KM				13984	Updates for 365 - includes joins to ItemCustomerFacingScale and related WHERE condition; also includes clustered
--										index on the identifiers temp table.
-- 2016-08-22   MZ              20586 (17474)  Added Default_Identifier to the result set.
-- 2017-04-13   MZ              23765 (20859)   Move Allergens before Ingredients in the concatenation. Correct the order of the Ingredients field
--												Allergens + Ingredients + ExtraText
-- 2018-01-25   BJ              23898	Filtered out Removed and Deleted Items and Identifiers
-- 2018-02-22	BJ				20171	Modified Scale_FixedWeight, PlumUnitAbbr, and Scale_ByCount to return EPlum values based on
--										whether the store is on GPM
-- **************************************************************************

BEGIN  
	SET NOCOUNT ON

	DECLARE @error_no int = 0
	
	--Determine how region wants to send down data to scales
	DECLARE @PluDigitsSentToScale varchar(20) = (SELECT PluDigitsSentToScale FROM InstanceData)
	
	--Using the regional scale file?
	DECLARE @UseRegionalScaleFile bit = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey='UseRegionalScaleFile')
	
	--Push Scale NutriFact Data?
	DECLARE @PushScaleNutrifactData bit = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey='PushScaleNutrifactData')
	
	-- Using SmartX scale format?
	DECLARE @UseSmartXPriceData bit = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey='UseSmartXPriceData')
	
	-- Using store jurisdictions for override values?
	DECLARE @UseStoreJurisdictions int = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey = 'UseStoreJurisdictions')

	-- Leading zeros for scale UPC with length shorter than 13
	DECLARE @LeadingZeros varchar(13) = REPLICATE('0',13) 

	-- Maximum length for Ingredients column
	DECLARE @MaxWidthForIngredients AS INT = (SELECT character_maximum_length FROM information_schema.columns WHERE table_name = 'Scale_ExtraText' and column_name = 'ExtraText')

	-- CFS Department prefix
	DECLARE @CustomerFacingScaleDepartmentPrefix as nvarchar(1) = (
		select dbo.fn_GetAppConfigValue('CustomerFacingScaleDeptDigit', 'POS PUSH JOB'))

	IF @CustomerFacingScaleDepartmentPrefix is null
		begin
			set @CustomerFacingScaleDepartmentPrefix = ''
		end
	
	DECLARE @GlobalPriceManagementIdfKey nvarchar(21) = 'GlobalPriceManagement'

	-- Create a temporary table to hold validated item identifiers (scan codes)
	IF OBJECT_ID('tempdb..#Identifiers') IS NOT NULL
	BEGIN
		DROP TABLE #Identifiers
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
	FROM dbo.fn_GetItemIdentifiers()

	UPDATE #Identifiers
	SET Scale_Identifier = 1
	WHERE Item_Key in 
		(
			SELECT Item_Key FROM dbo.ItemCustomerFacingScale icfs WHERE icfs.SendToScale = 1
		)

	BEGIN TRAN

	-- If the ActionCode is F, this is a signal that a full scale item full is being requested for a single 
	-- store.  Return all of the authorized scale items for the store with this query.
	IF @ActionCode = 'F'
	BEGIN
		-- Add all of the items that are authorized for the store to the queue, with an action code of F
		INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
			SELECT SI.Item_Key, 'F', SI.Store_No
			FROM dbo.StoreItem SI (nolock)
			INNER JOIN #Identifiers II 
				ON II.Item_Key = SI.Item_Key 
					AND II.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
			WHERE SI.Authorized = 1 
					AND SI.Store_No = @Store_No
					
		-- Move all of the F records to the temp queue for processing
		INSERT INTO PLUMCorpChgQueueTmp (PLUMCorpChgQueueID, Item_Key, ActionCode, Store_No)
			SELECT PQ.PLUMCorpChgQueueID, PQ.Item_Key, PQ.ActionCode, Store_No
			FROM PLUMCorpChgQueue PQ (nolock)
			WHERE PQ.ActionCode='F' AND
				  NOT EXISTS (SELECT QT.Item_Key FROM PlumCorpChgQueueTmp QT (nolock) WHERE QT.Item_Key = PQ.Item_Key AND QT.ActionCode = PQ.ActionCode
							  AND QT.Store_No = PQ.Store_No)
				
		SELECT @error_no = @@ERROR
	END   
	
	-- MA Needs scale price changes to trigger corresponding item changes and vice versa.
	-- The function returns price change item keys that are not already queued as item changes.
	-- This sp gets called twice; once for item adds (ActionCode = A) and changes (ActionCode = C).
	-- To make sure we only pick up price changes once, we only perform this for the C's (could
	-- have also done it for A's...would not have made a difference).
	IF @ActionCode = 'C' AND @UseSmartXPriceData = 1
	BEGIN
		INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode)
		SELECT Item_Key, 'C'  FROM dbo.fn_GetPLUMCorpOrScalePriceChangeKeys(@Date, 'PRICE')
	END

	-- Populate the PLUMCorpChgQueueTmp table with all of the changes that are currently available for processing.
	IF @ActionCode <> 'F'
	BEGIN
		INSERT INTO PLUMCorpChgQueueTmp
			SELECT * 
			FROM PLUMCorpChgQueue PQ (nolock)
			WHERE NOT EXISTS (SELECT QT.Item_Key FROM PlumCorpChgQueueTmp QT (nolock) WHERE QT.Item_Key = PQ.Item_Key AND QT.ActionCode = PQ.ActionCode
							  AND QT.Store_No = PQ.Store_No)
			
	END
	SELECT @error_no = @@ERROR
	
	-- If this is a 'A' action code and the region is not using a corporate scale system, 
	-- add all of the scale authorizations to the temp queue for processing.
	DECLARE @CurrentScaleAuth TABLE (StoreItemAuthorizationID int, Store_No int, Item_Key int)
	
	IF @ActionCode = 'A' AND @UseRegionalScaleFile = 0
	BEGIN
		INSERT INTO @CurrentScaleAuth
			SELECT si.StoreItemAuthorizationID, si.Store_No, si.Item_Key
			FROM StoreItem si (nolock)
			WHERE si.ScaleAuth = 1 AND
				  EXISTS (SELECT pbh.PriceBatchStatusId
						  FROM PriceBatchHeader pbh
						  JOIN PriceBatchDetail pbd ON pbd.PriceBatchHeaderId = pbh.PriceBatchHeaderId						  
						  WHERE pbd.Item_Key = si.Item_Key AND
								pbd.Store_No = si.Store_No AND
								pbh.PriceBatchStatusId = (SELECT PriceBatchStatusId          
														  FROM PriceBatchStatus 
														  WHERE PriceBatchStatusDesc = 'Sent')
						  )

		SELECT @error_no = @@ERROR
			
		-- Generate another set of PLUMCorpChgQueue records for the new authorizations if they are not
		-- already part of the scale queues.
		IF @error_no = 0
		BEGIN
			INSERT INTO 
				PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
			SELECT DISTINCT Item_Key, 'S', Store_No	-- new action code added for scale authorizations 
			FROM @CurrentScaleAuth 
			WHERE
				Item_Key NOT IN (SELECT Item_Key FROM PLUMCorpChgQueue) AND
				Item_Key NOT IN (SELECT Item_Key FROM PLUMCorpChgQueueTmp)

			SELECT @error_no = @@ERROR
		END
		
		-- Add these to the PLUMCorpChgQueueTmp table as so they can be processed
		-- with the other 'A' records.
		IF @error_no = 0
		BEGIN
			INSERT INTO PLUMCorpChgQueueTmp (PLUMCorpChgQueueID, Item_Key, ActionCode, Store_No)
				SELECT PLUMCorpChgQueueID, Item_Key, ActionCode, Store_No
				FROM PLUMCorpChgQueue PQ (nolock)
				WHERE ActionCode='S' AND
					  NOT EXISTS (SELECT QT.Item_Key FROM PlumCorpChgQueueTmp QT (nolock) WHERE QT.Item_Key = PQ.Item_Key AND QT.ActionCode = PQ.ActionCode
								  AND QT.Store_No = PQ.Store_No)
			
			SELECT @error_no = @@ERROR
		END
	END
	
	IF @error_no = 0
	BEGIN
		-- Clear all of the records that are in the temp queue for processing from the original queue.  The temp queue
		-- remains populated until the scale push process successfully completes.
		DELETE PLUMCorpChgQueue
		FROM PLUMCorpChgQueue Q
		INNER JOIN
			PLUMCorpChgQueueTmp T
				ON Q.PLUMCorpChgQueueID = T.PLUMCorpChgQueueID
		SELECT @error_no = @@ERROR
	END
	
	DECLARE @CurrentNewItemBatch TABLE (Store_No int, Item_Key int, PriceChgTypeId tinyint, PricingMethod_ID int, POSPrice smallmoney, POSSale_Price smallmoney, Multiple tinyint, Sale_Multiple tinyint, ApplyDate datetime)
	DECLARE @CurrentPriceBatch TABLE (Store_No int, Item_Key int, ApplyDate datetime)
	DECLARE @CurrDay smalldatetime = CONVERT(smalldatetime, CONVERT(varchar(255), @Date, 101))

	IF @error_no = 0 AND @ActionCode = 'A'
	BEGIN  
		-- Begin @CurrentNewItemBatch block
		-- Create a table to store the scale items that are assigned to new item batches in the "Sent" state.
		-- This allows the Item Add records to be sent from IRMA to the Scale systems before the price exceptions 
		-- for these items will be included in the price change section of the scale file from IRMA. 
		INSERT INTO @CurrentNewItemBatch
			SELECT DISTINCT PBD.Store_No, PBD.Item_Key, PBD.PriceChgTypeId, PBD.PricingMethod_ID, 
							PBD.POSPrice, PBD.POSSale_Price, PBD.Multiple, PBD.Sale_Multiple, PBH.ApplyDate 
			FROM PriceBatchDetail PBD (nolock)
			INNER JOIN #Identifiers II 
				ON II.Item_Key = PBD.Item_Key 
				   AND II.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
			INNER JOIN PriceBatchHeader PBH (nolock)
				ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
			INNER JOIN Store (nolock)
				ON Store.Store_No = PBD.Store_No
			WHERE (Mega_Store = 1 OR WFM_Store = 1) 
				  AND PBH.PriceBatchStatusID = (SELECT PriceBatchStatusId FROM PriceBatchStatus WHERE PriceBatchStatusDesc = 'Sent')
				  AND PBH.StartDate <= @CurrDay  
				  AND PBD.Offer_ID IS NULL			   -- EXCLUDE OFFER PBD RECORDS
				  AND ISNULL(PBD.ItemChgTypeID, 0) = 1 -- NEW ITEM BATCHES ONLY

			UNION ALL

			SELECT DISTINCT PBD.Store_No, PBD.Item_Key, PBD.PriceChgTypeId, PBD.PricingMethod_ID, 
							PBD.POSPrice, PBD.POSSale_Price, PBD.Multiple, PBD.Sale_Multiple, PBH.ApplyDate 
			FROM PriceBatchDetail PBD (nolock)
			INNER JOIN #Identifiers II 
				ON II.Item_Key = PBD.Item_Key 
				   AND II.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
			INNER JOIN PriceBatchHeader PBH (nolock)
				ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
			INNER JOIN Store (nolock)
				ON Store.Store_No = PBD.Store_No
			WHERE (Mega_Store = 1 OR WFM_Store = 1) 
				  AND II.Add_Identifier = 1
				  AND PBH.StartDate <= @CurrDay  
				  AND PBD.Offer_ID IS NULL			   -- EXCLUDE OFFER PBD RECORDS
				  AND ISNULL(PBD.ItemChgTypeID, 0) = 1 -- NEW ITEM BATCHES ONLY
			ORDER BY Item_Key, Store_No, ApplyDate
		
		SELECT @error_no = @@ERROR
	END  --End @CurrentNewItemBatch block

	IF @error_no = 0 AND @ActionCode <> 'F'	-- Full item files do not look at pending price batches; they always include the current IRMA price.
	BEGIN  
		-- Begin @CurrentPriceBatch block
		-- Create a table to store the scale items that are assigned to batches in the "Sent" state.
		-- The price exceptions for these items will be included in the price change section of the scale file from IRMA,
		-- not in the corporate sections. 
		INSERT INTO @CurrentPriceBatch
			SELECT DISTINCT PBD.Store_No, PBD.Item_Key, PBH.ApplyDate
			FROM PriceBatchDetail PBD (nolock)
			INNER JOIN #Identifiers II
				ON II.Item_Key = PBD.Item_Key 
				   AND II.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
			INNER JOIN PriceBatchHeader PBH (nolock)
				ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
			INNER JOIN Store (nolock)
				ON Store.Store_No = PBD.Store_No
			WHERE (Mega_Store = 1 OR WFM_Store = 1) 
				  AND PBH.PriceBatchStatusID = (SELECT PriceBatchStatusId FROM PriceBatchStatus WHERE PriceBatchStatusDesc = 'Sent')
				  AND PBH.StartDate <= @CurrDay  
				  AND PBD.Offer_ID IS NULL -- EXCLUDE OFFER PBD RECORDS
			ORDER BY Item_Key, Store_No, ApplyDate
		
		SELECT @error_no = @@ERROR
	END  --End @CurrentPriceBatch block

	IF @error_no = 0
	BEGIN  
		--Begin return result set block
		DECLARE @SmartX_PendingName AS CHAR(14) = 'PEND: ' + CONVERT(CHAR(8), @Date, 10)
		
		DECLARE @SmartX_MaintenanceDateTime AS CHAR(16) = CONVERT(CHAR(8), @Date, 10) + CONVERT(CHAR(8), @Date, 8)
		
		IF @UseRegionalScaleFile = 1 
		BEGIN  
			-- Begin @UseRegionalScaleFile=1 block
			-- Return first result set
			-- DO NOT include override data for CORPORATE scale systems
			SELECT DISTINCT
				II.Identifier,
				Item.SubTeam_No, 
				ISNULL(ItemScale.Scale_Description1, '') As ScaleDesc1, 
				ISNULL(ItemScale.Scale_Description2, '') As ScaleDesc2,
				ISNULL(ItemScale.Scale_Description3, '') As ScaleDesc3,
				ISNULL(ItemScale.Scale_Description4, '') As ScaleDesc4,
				SUBSTRING(RTRIM(LTRIM(ISNULL(Scale_Allergen.Allergens, '') + ' ' + ISNULL(Scale_Ingredient.Ingredients, '') + ' ' + ISNULL(Scale_ExtraText.ExtraText, ''))), 1, @MaxWidthForIngredients) As Ingredients, 
				ItemScale.Scale_ExtraText_ID,
				Scale_LabelType.Description AS Scale_LabelType_ID, 
				ItemScale.Nutrifact_ID,
				ItemScale.Scale_RandomWeightType_ID,
				ItemScale.Scale_LabelStyle_ID,
				Scale_LabelStyle.Description AS Scale_LabelStyle_Desc,
				ISNULL(RU.Unit_Abbreviation, '') AS Retail_Unit_Abbr,
				Item.Package_Desc1,
				Item.Package_Desc2, 
				ISNULL(PU.Unit_Abbreviation, '') As Package_Unit_Abbr,
				
				-- A price and vendor are required fields for some of the scale systems.  The 
				-- price per store is communicated in the zone record, but this price allows the
				-- corporate record to be successfully created.  The StoreItemVendor relationship
				-- is not communicated at this time.
				ROUND(ISNULL((SELECT TOP 1 POSPrice
					FROM Price (nolock)
					WHERE Price.Item_Key = QT.Item_Key 
						AND POSPrice > 0), 0.01), 2) 
					AS POSCurrPrice,
				ISNULL((SELECT TOP 1 Multiple
					FROM Price (nolock)
					WHERE Price.Item_Key = QT.Item_Key 
						AND POSPrice > 0), 1) 
					AS CurrMultiple,
				ISNULL((SELECT TOP 1 V.Vendor_Key FROM Vendor V (nolock)
					WHERE Vendor_ID = 
						(SELECT TOP 1 SIV.Vendor_ID FROM StoreItemVendor SIV (nolock) WHERE
							SIV.Item_Key = QT.Item_Key AND
							SIV.PrimaryVendor = 1)), 0) AS SmartX_VendorKey,
				CASE WHEN ActionCode = 'A' THEN 1 
					 WHEN ActionCode = 'S' THEN 1
					 WHEN ActionCode = 'F' THEN 1
					 ELSE 0 END AS New_Item, -- Adds, Authorizations, and Full Store are New Items
				CASE WHEN ActionCode = 'C' THEN 1 ELSE 0 END AS Item_Change, 
				CASE WHEN ActionCode = 'D' THEN 1 ELSE 0 END AS Remove_Item,
				CASE WHEN ActionCode = 'A' THEN 'Z' -- Code for New 
					 WHEN ActionCode = 'S' THEN 'Z' -- Code for New (these are auth records)
					 WHEN ActionCode = 'F' THEN 'Z' -- Code for New (these are full store load records)
					 WHEN ActionCode = 'C' THEN 'W' -- Code for Change
					 WHEN ActionCode = 'D' THEN 'Y' -- Code for Delete
				END AS SmartX_RecordType,
				@SmartX_PendingName AS SmartX_PendingName, 
				@SmartX_MaintenanceDateTime AS SmartX_MaintenanceDateTime,
				CONVERT(CHAR(8),@Date,10) As SmartX_EffectiveDate,
				ST.ScaleDept, 
				dbo.fn_GetScalePLU(II.Identifier, II.NumPluDigitsSentToScale, @PluDigitsSentToScale, icfs.SendToScale) AS ScalePLU,	
				CASE
						WHEN SUBSTRING(II.Identifier, 1, 1) = '2' 
							AND RIGHT(II.Identifier,5) = '00000'
							AND LEN(RTRIM(II.Identifier)) = 11
							THEN SUBSTRING(II.Identifier, 2, 5) -- TYPE-2 ITEM
						WHEN SUBSTRING(II.Identifier,1,1) != '2' 
							OR (SUBSTRING(II.Identifier, 1, 1) = '2'
							AND (RIGHT(II.Identifier,5) != '00000' 
							OR LEN(RTRIM(II.Identifier)) != 11))
							THEN RIGHT(@LeadingZeros + II.Identifier, 13) -- NON TYPE-2 ITEM
						ELSE SUBSTRING(II.Identifier, 2, 5)
						END	AS ScaleUPC,
				CASE ISNULL(RU.Unit_Abbreviation, '') 
					WHEN 'UNIT' THEN 'FW'
					WHEN 'LB' THEN 'LB'
					WHEN 'EA' THEN 'BC'
					END AS UnitOfMeasure,
				dbo.fn_GetEplumUnitOfMeasure(
					ScaleUOM.PlumUnitAbbr,
					gpmSellingUnit.Unit_Abbreviation,
					PU.Unit_Abbreviation,
					idf.FlagValue) AS PlumUnitAbbr,	
				ScaleUOM.Unit_Abbreviation AS ScaleUnitOfMeasure,
				CASE 
					WHEN ISNULL(RU.Unit_Abbreviation, '') = 'UNIT' 
						AND ISNULL(PU.Unit_Abbreviation, '') = 'OZ' 
					THEN ROUND(Package_Desc2,0)
					ELSE 0
					END As FixedWeightAmt,
				CASE -- If CurrMultiple > 0 Then CurrMultiple Else 1
					WHEN ISNULL(RU.Unit_Abbreviation, '') = 'EA' 
					THEN 
						CASE 
							WHEN ISNULL(
								(SELECT TOP 1 Multiple
									FROM Price (nolock)
									WHERE Price.Item_Key = QT.Item_Key 
										AND POSPrice > 0
								), 1) > 0 
							THEN ISNULL(
								(SELECT TOP 1 Multiple
									FROM Price (nolock)
									WHERE Price.Item_Key = QT.Item_Key 
										AND POSPrice > 0
								), 1) 
							ELSE 1 
							END
					ELSE 0
					END As ByCount,
				CASE WHEN SUBSTRING(RTRIM(LTRIM(ISNULL(Scale_ExtraText.ExtraText, '') + ' ' + ISNULL(Scale_Ingredient.Ingredients, '') + ' ' + ISNULL(Scale_Allergen.Allergens, ''))), 1, @MaxWidthForIngredients) <> '' 
					THEN SUBSTRING(II.Identifier, 2, 5) 
					ELSE 0 
					END As IngredientNumber,
				CAST(Scale_Tare.Zone1 AS int) AS ScaleTare_Int,
				dbo.fn_FormatSmartXScaleTares(Scale_Tare.Zone1,
										Scale_Tare.Zone2,
										Scale_Tare.Zone3,
										Scale_Tare.Zone4,
										Scale_Tare.Zone5,
										Scale_Tare.Zone6,
										Scale_Tare.Zone7,
										Scale_Tare.Zone8,
										Scale_Tare.Zone9,
										Scale_Tare.Zone10,
										ItemScale.ForceTare,
										ScaleUOM.Unit_Name,
										ItemScale.Scale_FixedWeight) As SmartX_Tare,
				CAST(Alt_Scale_Tare.Zone1 AS int) AS AltScaleTare_Int,
				dbo.fn_FormatSmartXScaleTares(Alt_Scale_Tare.Zone1,
										Alt_Scale_Tare.Zone2,
										Alt_Scale_Tare.Zone3,
										Alt_Scale_Tare.Zone4,
										Alt_Scale_Tare.Zone5,
										Alt_Scale_Tare.Zone6,
										Alt_Scale_Tare.Zone7,
										Alt_Scale_Tare.Zone8,
										Alt_Scale_Tare.Zone9,
										Alt_Scale_Tare.Zone10,
										ItemScale.ForceTare,
										ScaleUOM.Unit_Name,
										ItemScale.Scale_FixedWeight) As SmartX_AltTare,
				CAST((Scale_Tare.Zone1 * 100) as int) AS PLUMStoreScaleTareZone1,
				CAST((Scale_Tare.Zone2 * 100) as int) AS PLUMStoreScaleTareZone2,
				CAST((Scale_Tare.Zone3 * 100) as int) AS PLUMStoreScaleTareZone3,
				CAST((Scale_Tare.Zone4 * 100) as int) AS PLUMStoreScaleTareZone4,
				CAST((Scale_Tare.Zone5 * 100) as int) AS PLUMStoreScaleTareZone5,
				CAST((Scale_Tare.Zone6 * 100) as int) AS PLUMStoreScaleTareZone6,
				CAST((Scale_Tare.Zone7 * 100) as int) AS PLUMStoreScaleTareZone7,
				CAST((Scale_Tare.Zone8 * 100) as int) AS PLUMStoreScaleTareZone8,
				CAST((Scale_Tare.Zone9 * 100) as int) AS PLUMStoreScaleTareZone9,
				CAST((Scale_Tare.Zone10 * 100) as int) AS PLUMStoreScaleTareZone10,
				CAST((Alt_Scale_Tare.Zone1 * 100) as int) AS PLUMStoreALTScaleTareZone1,
				CAST((Alt_Scale_Tare.Zone2 * 100) as int) AS PLUMStoreALTScaleTareZone2,
				CAST((Alt_Scale_Tare.Zone3 * 100) as int) AS PLUMStoreALTScaleTareZone3,
				CAST((Alt_Scale_Tare.Zone4 * 100) as int) AS PLUMStoreALTScaleTareZone4,
				CAST((Alt_Scale_Tare.Zone5 * 100) as int) AS PLUMStoreALTScaleTareZone5,
				CAST((Alt_Scale_Tare.Zone6 * 100) as int) AS PLUMStoreALTScaleTareZone6,
				CAST((Alt_Scale_Tare.Zone7 * 100) as int) AS PLUMStoreALTScaleTareZone7,
				CAST((Alt_Scale_Tare.Zone8 * 100) as int) AS PLUMStoreALTScaleTareZone8,
				CAST((Alt_Scale_Tare.Zone9 * 100) as int) AS PLUMStoreALTScaleTareZone9,
				CAST((Alt_Scale_Tare.Zone10 * 100) as int) AS PLUMStoreALTScaleTareZone10,
				ItemScale.Scale_EatBy_ID AS UseBy_ID,
				CASE WHEN ItemScale.ForceTare = 1 
					THEN 'Y'
					ELSE 'N'
					END AS ScaleForcedTare,
				ItemScale.ShelfLife_Length,
				dbo.fn_GetEplumFixedWeight(
					ItemScale.Scale_FixedWeight, 
					gpmSellingUnit.Unit_Abbreviation,
					PU.Unit_Abbreviation,
					Item.Package_Desc2,
					idf.FlagValue) AS Scale_FixedWeight,
				dbo.fn_GetEplumByCount(
					ItemScale.Scale_ByCount, 
					gpmSellingUnit.Unit_Abbreviation,
					PU.Unit_Abbreviation,
					idf.FlagValue) AS Scale_ByCount,
				Scale_Grade.Zone1 AS Grade,
				CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone1,0))+','+
				CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone2,0))+','+
				CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone3,0))+','+
				CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone4,0))+','+
				CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone5,0))+','+
				CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone6,0))+','+
				CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone7,0))+','+
				CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone8,0))+','+
				CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone9,0))+','+
				CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone10,0)) As SmartX_Grade,
				ItemScale.PrintBlankShelfLife,
				ItemScale.PrintBlankEatBy,
				ItemScale.PrintBlankPackDate,
				ItemScale.PrintBlankWeight,
				ItemScale.PrintBlankUnitPrice,
				ItemScale.PrintBlankTotalPrice,
				CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + ',' + CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + ','+ CAST(Scale_LabelStyle.Description AS VARCHAR(5)) AS [Digi_LNU],
				
				--NutriFacts
				NF.Scale_LabelFormat_ID,
				RTRIM(NF.ServingUnits) as ServingUnits,
				CASE WHEN NF.ServingUnits = 1 THEN 999 ELSE CAST(NF.ServingUnits AS VARCHAR(5)) END as 'ServingUnits1To999',
				NF.ServingSizeDesc AS SizeText,
				NF.ServingsPerPortion AS Size,
				ISNULL(CONVERT(VARCHAR, NF.ServingsPerPortion), '') + ' ' + ISNULL(NF.ServingSizeDesc,'') AS 'ServingSize',
				NF.SizeWeight,
				NF.Calories,
				NF.CaloriesFat,
				NF.CaloriesSaturatedFat,
				NF.ServingPerContainer AS PerContainer,
				NF.TotalFatWeight,
				NF.TotalFatPercentage,
				NF.SaturatedFatWeight,
				NF.SaturatedFatPercent,
				NF.PolyunsaturatedFat,
				NF.MonounsaturatedFat,
				NF.CholesterolWeight,
				NF.CholesterolPercent,
				NF.SodiumWeight,
				NF.SodiumPercent,
				NF.PotassiumWeight,
				NF.PotassiumPercent,
				NF.TotalCarbohydrateWeight,
				NF.TotalCarbohydratePercent,
				NF.DietaryFiberWeight,
				NF.DietaryFiberPercent,
				NF.SolubleFiber,
				NF.InsolubleFiber,
				NF.Sugar,
				NF.SugarAlcohol,
				NF.OtherCarbohydrates,
				NF.ProteinWeight,
				NF.ProteinPercent,
				NF.VitaminA,
				NF.Betacarotene,
				NF.VitaminC,
				NF.Calcium,
				NF.Iron,
				NF.VitaminD,
				NF.VitaminE,
				NF.Thiamin,
				NF.Riboflavin,
				NF.Niacin,
				NF.VitaminB6 ,
				NF.Folate,
				NF.VitaminB12,
				NF.Biotin,
				NF.PantothenicAcid,
				NF.Phosphorous,
				NF.Iodine,
				NF.Magnesium,
				NF.Zinc,
				NF.Copper,
				NF.Transfat,
				NF.CaloriesFromTransFat,
				NF.Om6Fatty,
				NF.Om3Fatty, 
				NF.Starch,
				NF.Chloride,
				NF.Chromium,
				NF.VitaminK,
				NF.Manganese,
				NF.Molybdenum,
				NF.Selenium,
				NF.TransFatWeight,
				Item.Retail_Sale,
				ssd.StorageData AS StorageText
			FROM PLUMCorpChgQueueTmp QT (nolock)
				INNER JOIN #Identifiers II
					ON II.Item_Key = QT.Item_Key  
					AND II.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
					AND II.Remove_Identifier = 0
					AND II.Deleted_Identifier = 0
				INNER JOIN Item (nolock)
					ON Item.Item_Key = QT.Item_Key
					AND Item.Remove_Item = 0
					AND Item.Deleted_Item = 0
				INNER JOIN ItemScale (nolock)
					ON ItemScale.Item_Key = QT.Item_Key
				LEFT JOIN ItemCustomerFacingScale (nolock) icfs
					ON ItemScale.Item_Key = icfs.Item_Key
    			LEFT JOIN  NutriFacts NF (nolock)
					ON ItemScale.NutriFact_ID = NF.NutriFactsID
					AND @PushScaleNutrifactData = 1
				LEFT JOIN Scale_ExtraText (nolock)
					ON ItemScale.Scale_ExtraText_ID = Scale_ExtraText.Scale_ExtraText_ID
				LEFT JOIN Scale_Ingredient (nolock)
					ON ItemScale.Scale_Ingredient_ID = Scale_Ingredient.Scale_Ingredient_ID
				LEFT JOIN Scale_Allergen (nolock)
					ON ItemScale.Scale_Allergen_ID = Scale_Allergen.Scale_Allergen_ID
				LEFT JOIN Scale_Tare Scale_Tare (nolock)
					ON ItemScale.Scale_Tare_ID = Scale_Tare.Scale_Tare_ID
				LEFT JOIN Scale_Tare Alt_Scale_Tare (nolock)
					ON ItemScale.Scale_Alternate_Tare_ID = Alt_Scale_Tare.Scale_Tare_ID 
				LEFT JOIN Scale_Grade (nolock)
					ON ItemScale.Scale_Grade_ID = Scale_Grade.Scale_Grade_ID
				LEFT JOIN Scale_LabelType (nolock)
					ON Scale_ExtraText.Scale_LabelType_ID = Scale_LabelType.Scale_LabelType_ID
				LEFT JOIN Scale_LabelStyle (nolock)
					ON ItemScale.Scale_LabelStyle_ID = Scale_LabelStyle.Scale_LabelStyle_ID
				LEFT JOIN SubTeam ST (nolock)
					ON Item.SubTeam_No = ST.SubTeam_No
				LEFT JOIN ItemUnit RU (nolock)
					ON RU.Unit_ID = Item.Retail_Unit_ID
				LEFT JOIN ItemUnit PU (nolock)
					ON PU.Unit_ID = Item.Package_Unit_ID
				LEFT JOIN ItemUnit ScaleUOM (nolock)
					ON ScaleUOM.Unit_ID = ItemScale.Scale_ScaleUOMUnit_ID
				LEFT JOIN Scale_StorageData ssd (nolock)
					ON ItemScale.Scale_StorageData_ID = ssd.Scale_StorageData_ID
				LEFT JOIN ItemUomOverride iuo (nolock)
					ON QT.Item_Key = iuo.Item_Key
					AND QT.Store_No = iuo.Store_No
				LEFT JOIN ItemUnit gpmSellingUnit (nolock) 
					ON iuo.Retail_Unit_ID = gpmSellingUnit.Unit_ID
				LEFT JOIN dbo.fn_GetInstanceDataFlagStoreValues(@GlobalPriceManagementIdfKey) idf 
					ON QT.Store_No = idf.Store_No
			WHERE 
				(@ActionCode <> 'A' AND ActionCode = @ActionCode) OR
				(@ActionCode  = 'A' AND (ActionCode = 'A' OR ActionCode = 'S')) -- add records include authorizations
		
			-- Return a second result set of zone pricing data for the items included in the 
			-- corporate change result set.
			-- For items that are not authorized for the store in IRMA, the price will always
			-- be set to zero.
			SELECT DISTINCT
				II.Identifier,
				ST.ScaleDept, 
				dbo.fn_GetScalePLU(II.Identifier, II.NumPluDigitsSentToScale, @PluDigitsSentToScale, icfs.SendToScale) AS ScalePLU,
				CASE
						WHEN SUBSTRING(II.Identifier, 1, 1) = '2' 
							AND RIGHT(II.Identifier,5) = '00000'
							AND LEN(RTRIM(II.Identifier)) = 11
							THEN SUBSTRING(II.Identifier, 2, 5) -- TYPE-2 ITEM
						WHEN SUBSTRING(II.Identifier,1,1) != '2' 
							OR (SUBSTRING(II.Identifier, 1, 1) = '2'
							AND (RIGHT(II.Identifier,5) != '00000' 
							OR LEN(RTRIM(II.Identifier)) != 11))
							THEN RIGHT(@LeadingZeros + II.Identifier, 13) -- NON TYPE-2 ITEM
						ELSE SUBSTRING(II.Identifier, 2, 5)
						END	AS ScaleUPC,
				Store.PLUMStoreNo,		
				-- Return the price for the item based on:
				-- 1) StoreItem.Authorized (price is always 0 for de-authorized items)
				-- 2) If item is authorized and on sale, look at the pricing method to use REG or SALE price
				-- 3) If item is authorized and not on sale, use the REG price
				ROUND(ISNULL(SI.Authorized,0) * 
					dbo.fn_PricingMethodMoney(Price.PriceChgTypeId, PricingMethod_ID, POSPrice, POSSale_Price)
					, 2)
					 AS POSCurrPrice,
				-- Valid values for PLUM_ItemStatus are:
				--		D if the PLU item is being deleted from a specific store
				--		Y for active or authorized status
				--		N for unauthorized status
				CASE 
					WHEN @ActionCode = 'D' 
						THEN 'D'
					ELSE (CASE 
						WHEN dbo.fn_PricingMethodMoney(Price.PriceChgTypeId, PricingMethod_ID, POSPrice, POSSale_Price) 
							> 0 
						THEN 'Y' -- using POSPrice logic from above
						ELSE 'N' END) -- item is not being deleted, but current price is not greater than zero
					END AS PLUM_ItemStatus, --PLUM specific 
				ISNULL(ItemScale.Scale_Description1, '') As ScaleDesc1, 
				ISNULL(ItemScale.Scale_Description2, '') As ScaleDesc2,
				ISNULL(ItemScale.Scale_Description3, '') As ScaleDesc3, 
				ISNULL(ItemScale.Scale_Description4, '') As ScaleDesc4,
				CASE ISNULL(RU.Unit_Abbreviation, '') 
					WHEN 'UNIT' THEN 'FW'
					WHEN 'LB' THEN 'LB'
					WHEN 'EA' THEN 'BC'
					END AS UnitOfMeasure,
				dbo.fn_GetEplumUnitOfMeasure(
					ScaleUOM.PlumUnitAbbr,
					gpmSellingUnit.Unit_Abbreviation,
					PU.Unit_Abbreviation,
					idf.FlagValue) AS PlumUnitAbbr,
				ScaleUOM.Unit_Abbreviation AS ScaleUnitOfMeasure,
				CAST(Scale_Tare.Zone1 AS int) AS ScaleTare_Int,
				CAST(Alt_Scale_Tare.Zone1 AS int) AS AltScaleTare_Int,
				CAST((Scale_Tare.Zone1 * 100) as int) AS PLUMStoreScaleTareZone1,
				CAST((Scale_Tare.Zone2 * 100) as int) AS PLUMStoreScaleTareZone2,
				CAST((Scale_Tare.Zone3 * 100) as int) AS PLUMStoreScaleTareZone3,
				CAST((Scale_Tare.Zone4 * 100) as int) AS PLUMStoreScaleTareZone4,
				CAST((Scale_Tare.Zone5 * 100) as int) AS PLUMStoreScaleTareZone5,
				CAST((Scale_Tare.Zone6 * 100) as int) AS PLUMStoreScaleTareZone6,
				CAST((Scale_Tare.Zone7 * 100) as int) AS PLUMStoreScaleTareZone7,
				CAST((Scale_Tare.Zone8 * 100) as int) AS PLUMStoreScaleTareZone8,
				CAST((Scale_Tare.Zone9 * 100) as int) AS PLUMStoreScaleTareZone9,
				CAST((Scale_Tare.Zone10 * 100) as int) AS PLUMStoreScaleTareZone10,
				CAST((Alt_Scale_Tare.Zone1 * 100) as int) AS PLUMStoreALTScaleTareZone1,
				CAST((Alt_Scale_Tare.Zone2 * 100) as int) AS PLUMStoreALTScaleTareZone2,
				CAST((Alt_Scale_Tare.Zone3 * 100) as int) AS PLUMStoreALTScaleTareZone3,
				CAST((Alt_Scale_Tare.Zone4 * 100) as int) AS PLUMStoreALTScaleTareZone4,
				CAST((Alt_Scale_Tare.Zone5 * 100) as int) AS PLUMStoreALTScaleTareZone5,
				CAST((Alt_Scale_Tare.Zone6 * 100) as int) AS PLUMStoreALTScaleTareZone6,
				CAST((Alt_Scale_Tare.Zone7 * 100) as int) AS PLUMStoreALTScaleTareZone7,
				CAST((Alt_Scale_Tare.Zone8 * 100) as int) AS PLUMStoreALTScaleTareZone8,
				CAST((Alt_Scale_Tare.Zone9 * 100) as int) AS PLUMStoreALTScaleTareZone9,
				CAST((Alt_Scale_Tare.Zone10 * 100) as int) AS PLUMStoreALTScaleTareZone10,
				ItemScale.Scale_EatBy_ID AS UseBy_ID,
				CASE WHEN ItemScale.ForceTare = 1 
					THEN 'Y'
					ELSE 'N'
					END AS ScaleForcedTare,
				ItemScale.ShelfLife_Length,
				dbo.fn_GetEplumFixedWeight(
					ItemScale.Scale_FixedWeight, 
					gpmSellingUnit.Unit_Abbreviation,
					PU.Unit_Abbreviation,
					Item.Package_Desc2,
					idf.FlagValue) AS Scale_FixedWeight,
				dbo.fn_GetEplumByCount(
					ItemScale.Scale_ByCount, 
					gpmSellingUnit.Unit_Abbreviation,
					PU.Unit_Abbreviation,
					idf.FlagValue) AS Scale_ByCount,
				Scale_Grade.Zone1 AS Grade,
				CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + ',' + CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + ','+ CAST(Scale_LabelStyle.Description AS VARCHAR(5)) AS [Digi_LNU],
				Item.Retail_Sale,
				ssd.StorageData AS StorageText
			FROM PLUMCorpChgQueueTmp QT (nolock)
				INNER JOIN #Identifiers II
					ON II.Item_Key = QT.Item_Key 					
					AND II.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
					AND II.Remove_Identifier = 0
					AND II.Deleted_Identifier = 0
				INNER JOIN Item (nolock)
					ON Item.Item_Key = QT.Item_Key
					AND Item.Remove_Item = 0
					AND Item.Deleted_Item = 0
				LEFT JOIN SubTeam ST (nolock)
					ON Item.SubTeam_No = ST.SubTeam_No
				LEFT JOIN ItemUnit RU (NOLOCK)
					ON RU.Unit_ID = Item.Retail_Unit_ID
				-- Join the Price, Store, and ItemScale tables to send zone pricing records with corporate records	
				INNER JOIN Price (nolock)
					ON (QT.Item_Key = Price.Item_Key)
				INNER JOIN Store (nolock)
					ON (Store.Store_No = Price.Store_No)
				INNER JOIN ItemScale (nolock)
					ON ItemScale.Item_Key = QT.Item_Key
				LEFT JOIN ItemCustomerFacingScale (nolock) icfs
					ON ItemScale.Item_Key = icfs.Item_Key
				LEFT JOIN Scale_Tare Scale_Tare (nolock)
					ON ItemScale.Scale_Tare_ID = Scale_Tare.Scale_Tare_ID
				LEFT JOIN Scale_Tare Alt_Scale_Tare (nolock)
					ON ItemScale.Scale_Tare_ID = Alt_Scale_Tare.Scale_Tare_ID
				LEFT JOIN Scale_Grade (nolock)
					ON ItemScale.Scale_Grade_ID = Scale_Grade.Scale_Grade_ID	
				LEFT JOIN Scale_LabelStyle (nolock)
					ON ItemScale.Scale_LabelStyle_ID = Scale_LabelStyle.Scale_LabelStyle_ID
				LEFT JOIN ItemUnit ScaleUOM (nolock)
					ON ScaleUOM.Unit_ID = ItemScale.Scale_ScaleUOMUnit_ID
				LEFT JOIN StoreItem SI (nolock)
					ON SI.Item_Key = Item.Item_Key AND
					   SI.Store_No = Store.Store_No
				LEFT JOIN Scale_StorageData ssd (nolock)
					ON ItemScale.Scale_StorageData_ID = ssd.Scale_StorageData_ID
				LEFT JOIN ItemUomOverride iuo (nolock)
					ON QT.Item_Key = iuo.Item_Key
					AND QT.Store_No = iuo.Store_No
				LEFT JOIN ItemUnit gpmSellingUnit (nolock) 
					ON iuo.Retail_Unit_ID = gpmSellingUnit.Unit_ID
				LEFT JOIN dbo.fn_GetInstanceDataFlagStoreValues(@GlobalPriceManagementIdfKey) idf 
					ON Store.Store_No = idf.Store_No
			WHERE ((@ActionCode <> 'A' AND ActionCode = @ActionCode) OR
				   (@ActionCode  = 'A' AND (ActionCode = 'A' OR ActionCode = 'S')))  -- add records include authorizations
				AND ((@ActionCode <> 'F') OR
				   (@ActionCode  = 'F' AND Store.Store_No = @Store_No))  -- full scale files only include price data for single store
				AND QT.Item_Key NOT IN (
					SELECT Item_Key 
					FROM @CurrentPriceBatch CPB 
					WHERE CPB.Store_No = Store.Store_No) -- item is not in a current price batch
				AND Price.POSPrice > 0 -- only send prices if they have been set in IRMA

		END  --End @UseRegionalScaleFile=1 block
		
		ELSE IF @UseRegionalScaleFile = 0 
		
		BEGIN  
			-- Begin @UseRegionalScaleFile=0 block
			-- DO include override data for STORE scale systems
			IF @ActionCode = 'A'
			BEGIN
				-- Only return A records that also have a corresponding NEW item PBD record that is in a batch in the Sent state.
				-- The price will not be sent in IRMA, so it must come from the NEW item batch.
				SELECT  DISTINCT
					II.Identifier,
					II.Default_Identifier,
					Item.SubTeam_No, 
					Store.PLUMStoreNo,
					Store.Store_No,
					ISNULL(ISNULL(ISO.Scale_Description1, ItemScale.Scale_Description1), '') AS ScaleDesc1,
					ISNULL(ISNULL(ISO.Scale_Description2, ItemScale.Scale_Description2), '') AS ScaleDesc2,
					ISNULL(ISNULL(ISO.Scale_Description3, ItemScale.Scale_Description3), '') AS ScaleDesc3,
					ISNULL(ISNULL(ISO.Scale_Description4, ItemScale.Scale_Description4), '') AS ScaleDesc4,
					ISNULL(ISNULL(Scale_ExtraText_Override.ExtraText, SUBSTRING(RTRIM(LTRIM(ISNULL(Scale_Allergen.Allergens, '') + ' ' + ISNULL(Scale_Ingredient.Ingredients, '') + ' ' + ISNULL(Scale_ExtraText.ExtraText, ''))), 1, @MaxWidthForIngredients)), '') AS Ingredients, 
					ISNULL(ISO.Scale_ExtraText_ID, ItemScale.Scale_ExtraText_ID) AS Scale_ExtraText_ID,
					ISNULL(Scale_LabelType_Override.Description, Scale_LabelType.Description) AS Scale_LabelType_ID,
					CASE WHEN ISO.Item_Key IS NOT NULL
					THEN ISO.Nutrifact_ID
					ELSE ItemScale.Nutrifact_ID
					END AS Nutrifact_ID,
					ISNULL(ISO.Scale_RandomWeightType_ID, ItemScale.Scale_RandomWeightType_ID) AS Scale_RandomWeightType_ID,
					ISNULL(ISO.Scale_LabelStyle_ID, ItemScale.Scale_LabelStyle_ID) AS Scale_LabelStyle_ID,
					ISNULL(Scale_LabelStyle_Override.Description, Scale_LabelStyle.Description) AS Scale_LabelStyle_Desc,
					ISNULL(ISNULL(RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation), '') AS Retail_Unit_Abbr,
					ISNULL(ItemOverride.Package_Desc1, Item.Package_Desc1) AS Package_Desc1,
					ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2) AS Package_Desc2, 
					ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '') As Package_Unit_Abbr,
					V.Vendor_Key AS SmartX_VendorKey,
					
					-- Return the price for the item based on:
					-- 1) If item is authorized and on sale, look at the pricing method to use REG or SALE price
					-- 2) If item is authorized and not on sale, use the REG price
					-- NOTE: De-Authorized items are not included in the store scale results
					ROUND(CASE WHEN ISNULL(SI.Authorized, 0) = 0 
							   THEN 0
							   ELSE dbo.fn_PricingMethodMoney(NIB.PriceChgTypeId, NIB.PricingMethod_ID, NIB.POSPrice, NIB.POSSale_Price)
							END, 2) AS POSCurrPrice,
					dbo.fn_PricingMethodInt(NIB.PriceChgTypeId, NIB.PricingMethod_ID, NIB.Multiple, NIB.Sale_Multiple)
						AS CurrMultiple,
					CASE WHEN ActionCode = 'A' THEN 1 
						 WHEN ActionCode = 'S' THEN 1
						 WHEN ActionCode = 'F' THEN 1
						 ELSE 0 END AS New_Item, -- Adds, Authorizations, and Full Store are New Items
					CASE WHEN ActionCode = 'C' THEN 1 ELSE 0 END AS Item_Change, 
					CASE WHEN ActionCode = 'D' THEN 1 ELSE 0 END AS Remove_Item,
					CASE WHEN ActionCode = 'A' THEN 'Z' -- Code for New 
						 WHEN ActionCode = 'S' THEN 'Z' -- Code for New (these are authorizations)
						 WHEN ActionCode = 'F' THEN 'Z' -- Code for New (these are full store load records)
						 WHEN ActionCode = 'C' THEN 'W' -- Code for Change
						 WHEN ActionCode = 'D' THEN 'Y' -- Code for Delete
					END AS SmartX_RecordType,
					@SmartX_PendingName AS SmartX_PendingName, 
					@SmartX_MaintenanceDateTime AS SmartX_MaintenanceDateTime,
					CONVERT(CHAR(8),@Date,10) As SmartX_EffectiveDate,
					case 
						when icfs.CustomerFacingScaleDepartment = 1 then cast(@CustomerFacingScaleDepartmentPrefix + cast(ST.ScaleDept as varchar) as int)
						else ST.ScaleDept
					end as ScaleDept, 
					dbo.fn_GetScalePLU(II.Identifier, II.NumPluDigitsSentToScale, @PluDigitsSentToScale, icfs.SendToScale) AS ScalePLU,	
					CASE
						WHEN SUBSTRING(II.Identifier, 1, 1) = '2' 
							AND RIGHT(II.Identifier,5) = '00000'
							AND LEN(RTRIM(II.Identifier)) = 11
							THEN SUBSTRING(II.Identifier, 2, 5) -- TYPE-2 ITEM
						WHEN SUBSTRING(II.Identifier,1,1) != '2' 
							OR (SUBSTRING(II.Identifier, 1, 1) = '2'
							AND (RIGHT(II.Identifier,5) != '00000' 
							OR LEN(RTRIM(II.Identifier)) != 11))
							THEN RIGHT(@LeadingZeros + II.Identifier, 13) -- NON TYPE-2 ITEM
						ELSE SUBSTRING(II.Identifier, 2, 5)
						END	AS ScaleUPC,
					CASE ISNULL(ISNULL(RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation), '') 
						WHEN 'UNIT' THEN 'FW'
						WHEN 'LB' THEN 'LB'
						WHEN 'EA' THEN 'BC'
						END AS UnitOfMeasure,
					dbo.fn_GetEplumUnitOfMeasure(
						ISNULL(ScaleUOM_Override.PlumUnitAbbr, ScaleUOM.PlumUnitAbbr),
						gpmSellingUnit.Unit_Abbreviation,
						PU.Unit_Abbreviation,
						idf.FlagValue) AS PlumUnitAbbr,
					ISNULL(ScaleUOM_Override.Unit_Abbreviation, ScaleUOM.Unit_Abbreviation) AS ScaleUnitOfMeasure,
					CASE 
						WHEN ISNULL(ISNULL(RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation), '') = 'UNIT' 
							AND ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '') = 'OZ' 
						THEN ROUND(ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2),0)
						ELSE 0
						END As FixedWeightAmt,
					CASE -- If CurrMultiple > 0 Then CurrMultiple Else 1
						WHEN ISNULL(ISNULL(RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation), '') = 'EA' 
						THEN dbo.fn_PricingMethodInt(NIB.PriceChgTypeId, NIB.PricingMethod_ID, NIB.Multiple, NIB.Sale_Multiple)
						ELSE 0
						END As ByCount,
					CASE WHEN ISNULL(Scale_ExtraText_Override.ExtraText, SUBSTRING(RTRIM(LTRIM(ISNULL(Scale_ExtraText.ExtraText, '') + ' ' + ISNULL(Scale_Ingredient.Ingredients, '') + ' ' + ISNULL(Scale_Allergen.Allergens, ''))), 1, @MaxWidthForIngredients)) <> '' 
						THEN SUBSTRING(II.Identifier, 2, 5) 
						ELSE 0 
						END As IngredientNumber,
					CAST(ISNULL(Scale_Tare_Override.Zone1, Scale_Tare.Zone1) AS int) AS ScaleTare_Int,
					CAST((ISNULL(Scale_Tare_Override.Zone1, Scale_Tare.Zone1) * 100) as int) AS PLUMStoreScaleTareZone1,
					CAST((ISNULL(Scale_Tare_Override.Zone2, Scale_Tare.Zone2) * 100) as int) AS PLUMStoreScaleTareZone2,
					CAST((ISNULL(Scale_Tare_Override.Zone3, Scale_Tare.Zone3) * 100) as int) AS PLUMStoreScaleTareZone3,
					CAST((ISNULL(Scale_Tare_Override.Zone4, Scale_Tare.Zone4) * 100) as int) AS PLUMStoreScaleTareZone4,
					CAST((ISNULL(Scale_Tare_Override.Zone5, Scale_Tare.Zone5) * 100) as int) AS PLUMStoreScaleTareZone5,
					CAST((ISNULL(Scale_Tare_Override.Zone6, Scale_Tare.Zone6) * 100) as int) AS PLUMStoreScaleTareZone6,
					CAST((ISNULL(Scale_Tare_Override.Zone7, Scale_Tare.Zone7) * 100) as int) AS PLUMStoreScaleTareZone7,
					CAST((ISNULL(Scale_Tare_Override.Zone8, Scale_Tare.Zone8) * 100) as int) AS PLUMStoreScaleTareZone8,
					CAST((ISNULL(Scale_Tare_Override.Zone9, Scale_Tare.Zone9) * 100) as int) AS PLUMStoreScaleTareZone9,
					CAST((ISNULL(Scale_Tare_Override.Zone10, Scale_Tare.Zone10) * 100) as int) AS PLUMStoreScaleTareZone10,
					CAST((Alt_Scale_Tare.Zone1 * 100) as int) AS PLUMStoreALTScaleTareZone1,
					CAST((Alt_Scale_Tare.Zone2 * 100) as int) AS PLUMStoreALTScaleTareZone2,
					CAST((Alt_Scale_Tare.Zone3 * 100) as int) AS PLUMStoreALTScaleTareZone3,
					CAST((Alt_Scale_Tare.Zone4 * 100) as int) AS PLUMStoreALTScaleTareZone4,
					CAST((Alt_Scale_Tare.Zone5 * 100) as int) AS PLUMStoreALTScaleTareZone5,
					CAST((Alt_Scale_Tare.Zone6 * 100) as int) AS PLUMStoreALTScaleTareZone6,
					CAST((Alt_Scale_Tare.Zone7 * 100) as int) AS PLUMStoreALTScaleTareZone7,
					CAST((Alt_Scale_Tare.Zone8 * 100) as int) AS PLUMStoreALTScaleTareZone8,
					CAST((Alt_Scale_Tare.Zone9 * 100) as int) AS PLUMStoreALTScaleTareZone9,
					CAST((Alt_Scale_Tare.Zone10 * 100) as int) AS PLUMStoreALTScaleTareZone10,
					dbo.fn_FormatSmartXScaleTares(ISNULL(Scale_Tare_Override.Zone1, Scale_Tare.Zone1),
												  ISNULL(Scale_Tare_Override.Zone2, Scale_Tare.Zone2),
												  ISNULL(Scale_Tare_Override.Zone3, Scale_Tare.Zone3),
												  ISNULL(Scale_Tare_Override.Zone4, Scale_Tare.Zone4),
												  ISNULL(Scale_Tare_Override.Zone5, Scale_Tare.Zone5),
												  ISNULL(Scale_Tare_Override.Zone6, Scale_Tare.Zone6),
												  ISNULL(Scale_Tare_Override.Zone7, Scale_Tare.Zone7),
												  ISNULL(Scale_Tare_Override.Zone8, Scale_Tare.Zone8),
												  ISNULL(Scale_Tare_Override.Zone9, Scale_Tare.Zone9),
												  ISNULL(Scale_Tare_Override.Zone10, Scale_Tare.Zone10),
												  ISNULL(ISO.ForceTare, ItemScale.ForceTare),
												  ISNULL(ScaleUOM_Override.Unit_Name, ScaleUOM.Unit_Name),
												  ISNULL(ISO.Scale_FixedWeight, ItemScale.Scale_FixedWeight)) As SmartX_Tare,
					CAST(Alt_Scale_Tare.Zone1 AS int) AS AltScaleTare_Int,
					dbo.fn_FormatSmartXScaleTares(Alt_Scale_Tare.Zone1,
												  Alt_Scale_Tare.Zone2,
												  Alt_Scale_Tare.Zone3,
												  Alt_Scale_Tare.Zone4,
												  Alt_Scale_Tare.Zone5,
												  Alt_Scale_Tare.Zone6,
												  Alt_Scale_Tare.Zone7,
												  Alt_Scale_Tare.Zone8,
												  Alt_Scale_Tare.Zone9,
												  Alt_Scale_Tare.Zone10,
												  ISNULL(ISO.ForceTare, ItemScale.ForceTare),
												  ISNULL(ScaleUOM_Override.Unit_Name, ScaleUOM.Unit_Name),
												  ISNULL(ISO.Scale_FixedWeight, ItemScale.Scale_FixedWeight)) As SmartX_AltTare,
					ISNULL(ISO.Scale_EatBy_ID, ItemScale.Scale_EatBy_ID) AS UseBy_ID,
					CASE WHEN ISNULL(ISO.ForceTare, ItemScale.ForceTare) = 1 
						THEN 'Y'
						ELSE 'N'
						END AS ScaleForcedTare,
					ISNULL(ISO.ShelfLife_Length, ItemScale.ShelfLife_Length) AS ShelfLife_Length,
					dbo.fn_GetEplumFixedWeight(
						ISNULL(ISO.Scale_FixedWeight, ItemScale.Scale_FixedWeight), 
						gpmSellingUnit.Unit_Abbreviation,
						PU.Unit_Abbreviation,
						Item.Package_Desc2,
						idf.FlagValue) AS Scale_FixedWeight,
					dbo.fn_GetEplumByCount(
						ISNULL(ISO.Scale_ByCount, ItemScale.Scale_ByCount), 
						gpmSellingUnit.Unit_Abbreviation,
						PU.Unit_Abbreviation,
						idf.FlagValue) AS Scale_ByCount,
					Scale_Grade.Zone1 AS Grade,
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone1,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone2,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone3,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone4,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone5,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone6,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone7,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone8,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone9,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone10,0)) As SmartX_Grade,
					ISNULL(ISO.PrintBlankShelfLife, ItemScale.PrintBlankShelfLife) AS PrintBlankShelfLife,
					ISNULL(ISO.PrintBlankEatBy, ItemScale.PrintBlankEatBy) AS PrintBlankEatBy,
					ISNULL(ISO.PrintBlankPackDate, ItemScale.PrintBlankPackDate) AS PrintBlankPackDate,
					ISNULL(ISO.PrintBlankWeight, ItemScale.PrintBlankWeight) AS PrintBlankWeight,
					ISNULL(ISO.PrintBlankUnitPrice, ItemScale.PrintBlankUnitPrice) AS PrintBlankUnitPrice,
					ISNULL(ISO.PrintBlankTotalPrice, ItemScale.PrintBlankTotalPrice) AS PrintBlankTotalPrice,
					CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + ',' + CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + ','+ CAST(Scale_LabelStyle.Description AS VARCHAR(5)) AS [Digi_LNU],
				  
					--NutriFacts
					NF.Scale_LabelFormat_ID,
					RTRIM(NF.ServingUnits) as ServingUnits,
					CASE WHEN NF.ServingUnits = 1 THEN 999 ELSE CAST(NF.ServingUnits AS VARCHAR(5)) END as 'ServingUnits1To999',
					NF.ServingSizeDesc AS SizeText,
					NF.ServingsPerPortion AS Size,
					ISNULL(CONVERT(VARCHAR, NF.ServingsPerPortion), '') + ' ' + ISNULL(NF.ServingSizeDesc,'') AS 'ServingSize',
					NF.SizeWeight,
					NF.Calories,
					NF.CaloriesFat,
					NF.CaloriesSaturatedFat,
					NF.ServingPerContainer AS PerContainer,
					NF.TotalFatWeight,
					NF.TotalFatPercentage,
					NF.SaturatedFatWeight,
					NF.SaturatedFatPercent,
					NF.PolyunsaturatedFat,
					NF.MonounsaturatedFat,
					NF.CholesterolWeight,
					NF.CholesterolPercent,
					NF.SodiumWeight,
					NF.SodiumPercent,
					NF.PotassiumWeight,
					NF.PotassiumPercent,
					NF.TotalCarbohydrateWeight,
					NF.TotalCarbohydratePercent,
					NF.DietaryFiberWeight,
					NF.DietaryFiberPercent,
					NF.SolubleFiber,
					NF.InsolubleFiber,
					NF.Sugar,
					NF.SugarAlcohol,
					NF.OtherCarbohydrates,
					NF.ProteinWeight,
					NF.ProteinPercent,
					NF.VitaminA,
					NF.Betacarotene,
					NF.VitaminC,
					NF.Calcium,
					NF.Iron,
					NF.VitaminD,
					NF.VitaminE,
					NF.Thiamin,
					NF.Riboflavin,
					NF.Niacin,
					NF.VitaminB6 ,
					NF.Folate,
					NF.VitaminB12,
					NF.Biotin,
					NF.PantothenicAcid,
					NF.Phosphorous,
					NF.Iodine,
					NF.Magnesium,
					NF.Zinc,
					NF.Copper,
					NF.Transfat,
					NF.CaloriesFromTransFat,
					NF.Om6Fatty,
					NF.Om3Fatty, 
					NF.Starch,
					NF.Chloride,
					NF.Chromium,
					NF.VitaminK,
					NF.Manganese,
					NF.Molybdenum,
					NF.Selenium,
					NF.TransFatWeight,
					Item.Retail_Sale,
					ssd.StorageData AS StorageText
				FROM 
					PLUMCorpChgQueueTmp QT (nolock)
					INNER JOIN #Identifiers II
						ON II.Item_Key = QT.Item_Key  
						AND II.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
						AND II.Remove_Identifier = 0
						AND II.Deleted_Identifier = 0
					INNER JOIN Item (nolock)
						ON Item.Item_Key = QT.Item_Key
						AND Item.Remove_Item = 0
						AND Item.Deleted_Item = 0
					LEFT JOIN SubTeam ST (nolock)
						ON Item.SubTeam_No = ST.SubTeam_No
					INNER JOIN @CurrentNewItemBatch NIB 
						ON QT.Item_Key = NIB.Item_Key 
					INNER JOIN Store (nolock)
						ON Store.Store_No = NIB.Store_No
					LEFT JOIN ItemScale (nolock)
						ON ItemScale.Item_Key = QT.Item_Key
					LEFT JOIN
						dbo.ItemOverride (nolock)
						ON ItemOverride.Item_Key = QT.Item_Key 
							AND ItemOverride.StoreJurisdictionID = Store.StoreJurisdictionID
							AND @UseRegionalScaleFile = 0
							AND @UseStoreJurisdictions = 1
					LEFT JOIN
						dbo.ItemScaleOverride ISO (nolock)
						ON ISO.Item_Key = QT.Item_Key
							AND ISO.StoreJurisdictionID = Store.StoreJurisdictionID
							AND @UseRegionalScaleFile = 0
							AND @UseStoreJurisdictions = 1
					LEFT JOIN
						dbo.ItemCustomerFacingScale icfs (nolock) on ItemScale.Item_Key = icfs.Item_Key
        			LEFT JOIN  NutriFacts NF (nolock)
							ON CASE 
									WHEN ISO.Item_Key IS NOT NULL
									THEN ISO.Nutrifact_ID
									ELSE ItemScale.Nutrifact_ID
							   END = NF.NutriFactsID
							AND @PushScaleNutrifactData = 1
					LEFT JOIN Scale_ExtraText (nolock)
						ON ItemScale.Scale_ExtraText_ID = Scale_ExtraText.Scale_ExtraText_ID
					LEFT JOIN Scale_Ingredient (nolock)
						ON ItemScale.Scale_Ingredient_ID = Scale_Ingredient.Scale_Ingredient_ID
					LEFT JOIN Scale_Allergen (nolock)
						ON ItemScale.Scale_Allergen_ID = Scale_Allergen.Scale_Allergen_ID
					LEFT JOIN Scale_ExtraText Scale_ExtraText_Override (nolock)
						ON ISO.Scale_ExtraText_ID = Scale_ExtraText_Override.Scale_ExtraText_ID
					LEFT JOIN Scale_Tare Scale_Tare (nolock)
						ON ItemScale.Scale_Tare_ID = Scale_Tare.Scale_Tare_ID
					LEFT JOIN Scale_Tare Scale_Tare_Override (nolock)
						ON ISO.Scale_Tare_ID = Scale_Tare_Override.Scale_Tare_ID
					LEFT JOIN Scale_Tare Alt_Scale_Tare (nolock)
						ON ISNULL(ISO.Scale_Alternate_Tare_ID, ItemScale.Scale_Alternate_Tare_ID) = Alt_Scale_Tare.Scale_Tare_ID
					LEFT JOIN Scale_Grade (nolock)
						ON ISNULL(ISO.Scale_Grade_ID, ItemScale.Scale_Grade_ID) = Scale_Grade.Scale_Grade_ID	
					LEFT JOIN Scale_LabelType (nolock)
						ON Scale_ExtraText.Scale_LabelType_ID = Scale_LabelType.Scale_LabelType_ID
					LEFT JOIN Scale_LabelType Scale_LabelType_Override (nolock)
						ON Scale_ExtraText_Override.Scale_LabelType_ID = Scale_LabelType_Override.Scale_LabelType_ID
					LEFT JOIN Scale_LabelStyle (nolock)
						ON ItemScale.Scale_LabelStyle_ID = Scale_LabelStyle.Scale_LabelStyle_ID
					LEFT JOIN Scale_LabelStyle Scale_LabelStyle_Override (nolock)
						ON ISO.Scale_LabelStyle_ID = Scale_LabelStyle_Override.Scale_LabelStyle_ID
					LEFT JOIN ItemUnit ScaleUOM (nolock)
						ON ScaleUOM.Unit_ID = ItemScale.Scale_ScaleUOMUnit_ID
					LEFT JOIN ItemUnit ScaleUOM_Override (nolock)
						ON ScaleUOM_Override.Unit_ID = ISO.Scale_ScaleUOMUnit_ID
					INNER JOIN StoreItem SI (nolock)
						ON SI.Item_Key = Item.Item_Key AND
						   SI.Store_No = Store.Store_No AND
						   SI.Authorized = 1 -- ONLY INCLUDE AUTHORIZED ITEMS IN STORE SCALE PUSH
					LEFT JOIN StoreItemVendor SIV (nolock)
						ON SIV.Store_No = Store.Store_No
							AND SIV.Item_Key = Item.Item_Key
							AND SIV.PrimaryVendor = 1
					LEFT JOIN Vendor V (nolock)		  
						ON V.Vendor_ID = SIV.Vendor_ID
					LEFT JOIN ItemUnit RU (NOLOCK)
						ON RU.Unit_ID = Item.Retail_Unit_ID
					LEFT JOIN ItemUnit RU_Override (NOLOCK)
						ON RU_Override.Unit_ID = ItemOverride.Retail_Unit_ID
					LEFT JOIN ItemUnit PU (nolock)
						ON PU.Unit_ID = Item.Package_Unit_ID
					LEFT JOIN ItemUnit PU_Override (nolock)
						ON PU_Override.Unit_ID = ItemOverride.Package_Unit_ID
					LEFT JOIN Scale_StorageData ssd (nolock)
						ON ItemScale.Scale_StorageData_ID = ssd.Scale_StorageData_ID
					LEFT JOIN ItemUomOverride iuo (nolock)
						ON QT.Item_Key = iuo.Item_Key
						AND QT.Store_No = iuo.Store_No
					LEFT JOIN ItemUnit gpmSellingUnit (nolock) 
						ON iuo.Retail_Unit_ID = gpmSellingUnit.Unit_ID
					LEFT JOIN dbo.fn_GetInstanceDataFlagStoreValues(@GlobalPriceManagementIdfKey) idf 
						ON QT.Store_No = idf.Store_No
				WHERE 
					(ActionCode = 'A' OR ActionCode = 'S') -- adds include authorizations
					AND QT.Store_No = NIB.Store_No
					
					-- 365 non-scale PLUs (not matching 2#####00000) need to be marked as icfs.SendToScale in order to be included.  Normal scale items will not have that value.
					-- CFS items (icfs.SendToScale = 1) should only go to 365 Stores (Store.Mega_Store = 1)
					AND ((Store.Mega_Store = 1 and icfs.SendToScale = 1 and dbo.fn_IsPosPlu(ii.Identifier) = 1) or icfs.SendToScale is null)
				ORDER BY 
					Store.Store_No
			END  -- End @ActionCode = A block
			ELSE
			BEGIN
				SELECT  DISTINCT
					II.Identifier,
					II.Default_Identifier,
					Item.SubTeam_No, 
					Store.PLUMStoreNo,
					Store.Store_No,
					ISNULL(ISNULL(ISO.Scale_Description1, ItemScale.Scale_Description1), '') AS ScaleDesc1,
					ISNULL(ISNULL(ISO.Scale_Description2, ItemScale.Scale_Description2), '') AS ScaleDesc2,
					ISNULL(ISNULL(ISO.Scale_Description3, ItemScale.Scale_Description3), '') AS ScaleDesc3,
					ISNULL(ISNULL(ISO.Scale_Description4, ItemScale.Scale_Description4), '') AS ScaleDesc4,
					ISNULL(ISNULL(Scale_ExtraText_Override.ExtraText, SUBSTRING(RTRIM(LTRIM(ISNULL(Scale_Allergen.Allergens, '') + ' ' + ISNULL(Scale_Ingredient.Ingredients, '') + ' ' + ISNULL(Scale_ExtraText.ExtraText, ''))), 1, @MaxWidthForIngredients)), '') AS Ingredients, 
					ISNULL(ISO.Scale_ExtraText_ID, ItemScale.Scale_ExtraText_ID) AS Scale_ExtraText_ID,
					ISNULL(Scale_LabelType_Override.Description, Scale_LabelType.Description) AS Scale_LabelType_ID,
					CASE 
						WHEN ISO.Item_Key IS NOT NULL
						THEN ISO.Nutrifact_ID
						ELSE ItemScale.Nutrifact_ID
					END AS Nutrifact_ID,
					ISNULL(ISO.Scale_RandomWeightType_ID, ItemScale.Scale_RandomWeightType_ID) AS Scale_RandomWeightType_ID,
					ISNULL(ISO.Scale_LabelStyle_ID, ItemScale.Scale_LabelStyle_ID) AS Scale_LabelStyle_ID,
					ISNULL(Scale_LabelStyle_Override.Description, Scale_LabelStyle.Description) AS Scale_LabelStyle_Desc,
					ISNULL(ISNULL(RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation), '') AS Retail_Unit_Abbr,
					ISNULL(ItemOverride.Package_Desc1, Item.Package_Desc1) AS Package_Desc1,
					ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2) AS Package_Desc2, 
					ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '') As Package_Unit_Abbr,
					V.Vendor_Key AS SmartX_VendorKey,
					
					-- Return the price for the item based on:
					-- 1) If item is authorized and on sale, look at the pricing method to use REG or SALE price
					-- 2) If item is authorized and not on sale, use the REG price
					-- NOTE: De-Authorized items are not included in the store scale results
					ROUND((CASE WHEN ISNULL(SI.Authorized,0) = 0 
							   THEN 0
							   ELSE dbo.fn_PricingMethodMoney(Price.PriceChgTypeId, Price.PricingMethod_ID, Price.POSPrice, Price.POSSale_Price)
								END), 2
						) AS POSCurrPrice,
					dbo.fn_PricingMethodInt(Price.PriceChgTypeId, PricingMethod_ID, Multiple, Sale_Multiple)
						AS CurrMultiple,
					CASE WHEN ActionCode = 'A' THEN 1 
						 WHEN ActionCode = 'S' THEN 1
						 WHEN ActionCode = 'F' THEN 1
						 ELSE 0 END AS New_Item, -- Adds, Authorizations, and Full Store are New Items
					CASE WHEN ActionCode = 'C' THEN 1 ELSE 0 END AS Item_Change, 
					CASE WHEN ActionCode = 'D' THEN 1 ELSE 0 END AS Remove_Item,
					CASE WHEN ActionCode = 'A' THEN 'Z' -- Code for New 
						 WHEN ActionCode = 'S' THEN 'Z' -- Code for New (these are authorizations)
						 WHEN ActionCode = 'F' THEN 'Z' -- Code for New (these are full store load records)
						 WHEN ActionCode = 'C' THEN 'W' -- Code for Change
						 WHEN ActionCode = 'D' THEN 'Y' -- Code for Delete
					END AS SmartX_RecordType,
					@SmartX_PendingName AS SmartX_PendingName, 
					@SmartX_MaintenanceDateTime AS SmartX_MaintenanceDateTime,
					CONVERT(CHAR(8),@Date,10) As SmartX_EffectiveDate,
					case 
						when icfs.CustomerFacingScaleDepartment = 1 then cast(@CustomerFacingScaleDepartmentPrefix + cast(ST.ScaleDept as varchar) as int)
						else ST.ScaleDept
					end as ScaleDept, 
					dbo.fn_GetScalePLU(II.Identifier, II.NumPluDigitsSentToScale, @PluDigitsSentToScale, icfs.SendToScale) AS ScalePLU,	
					CASE
						WHEN SUBSTRING(II.Identifier, 1, 1) = '2' 
							AND RIGHT(II.Identifier,5) = '00000'
							AND LEN(RTRIM(II.Identifier)) = 11
							THEN SUBSTRING(II.Identifier, 2, 5) -- TYPE-2 ITEM
						WHEN SUBSTRING(II.Identifier,1,1) != '2' 
							OR (SUBSTRING(II.Identifier, 1, 1) = '2'
							AND (RIGHT(II.Identifier,5) != '00000' 
							OR LEN(RTRIM(II.Identifier)) != 11))
							THEN RIGHT(@LeadingZeros + II.Identifier, 13) -- NON TYPE-2 ITEM
						ELSE SUBSTRING(II.Identifier, 2, 5)
						END	AS ScaleUPC,
					CASE ISNULL(ISNULL(RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation), '') 
						WHEN 'UNIT' THEN 'FW'
						WHEN 'LB' THEN 'LB'
						WHEN 'EA' THEN 'BC'
						END AS UnitOfMeasure,
					dbo.fn_GetEplumUnitOfMeasure(
						ISNULL(ScaleUOM_Override.PlumUnitAbbr, ScaleUOM.PlumUnitAbbr),
						gpmSellingUnit.Unit_Abbreviation,
						PU.Unit_Abbreviation,
						idf.FlagValue) AS PlumUnitAbbr,
					ISNULL(ScaleUOM_Override.Unit_Abbreviation, ScaleUOM.Unit_Abbreviation) AS ScaleUnitOfMeasure,
					CASE 
						WHEN ISNULL(ISNULL(RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation), '') = 'UNIT' 
							AND ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '') = 'OZ' 
						THEN ROUND(ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2),0)
						ELSE 0
						END As FixedWeightAmt,
					CASE -- If CurrMultiple > 0 Then CurrMultiple Else 1
						WHEN ISNULL(ISNULL(RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation), '') = 'EA' 
						THEN dbo.fn_PricingMethodInt(Price.PriceChgTypeId, Price.PricingMethod_ID, Price.Multiple, Price.Sale_Multiple)
						ELSE 0
						END As ByCount,
					CASE WHEN ISNULL(Scale_ExtraText_Override.ExtraText, SUBSTRING(RTRIM(LTRIM(ISNULL(Scale_ExtraText.ExtraText, '') + ' ' + ISNULL(Scale_Ingredient.Ingredients, '') + ' ' + ISNULL(Scale_Allergen.Allergens, ''))), 1, @MaxWidthForIngredients)) <> '' 
						THEN SUBSTRING(II.Identifier, 2, 5) 
						ELSE 0 
						END As IngredientNumber,
					CAST(ISNULL(Scale_Tare_Override.Zone1, Scale_Tare.Zone1) AS int) AS ScaleTare_Int,
					CAST((ISNULL(Scale_Tare_Override.Zone1, Scale_Tare.Zone1) * 100) as int) AS PLUMStoreScaleTareZone1,
					CAST((ISNULL(Scale_Tare_Override.Zone2, Scale_Tare.Zone2) * 100) as int) AS PLUMStoreScaleTareZone2,
					CAST((ISNULL(Scale_Tare_Override.Zone3, Scale_Tare.Zone3) * 100) as int) AS PLUMStoreScaleTareZone3,
					CAST((ISNULL(Scale_Tare_Override.Zone4, Scale_Tare.Zone4) * 100) as int) AS PLUMStoreScaleTareZone4,
					CAST((ISNULL(Scale_Tare_Override.Zone5, Scale_Tare.Zone5) * 100) as int) AS PLUMStoreScaleTareZone5,
					CAST((ISNULL(Scale_Tare_Override.Zone6, Scale_Tare.Zone6) * 100) as int) AS PLUMStoreScaleTareZone6,
					CAST((ISNULL(Scale_Tare_Override.Zone7, Scale_Tare.Zone7) * 100) as int) AS PLUMStoreScaleTareZone7,
					CAST((ISNULL(Scale_Tare_Override.Zone8, Scale_Tare.Zone8) * 100) as int) AS PLUMStoreScaleTareZone8,
					CAST((ISNULL(Scale_Tare_Override.Zone9, Scale_Tare.Zone9) * 100) as int) AS PLUMStoreScaleTareZone9,
					CAST((ISNULL(Scale_Tare_Override.Zone10, Scale_Tare.Zone10) * 100) as int) AS PLUMStoreScaleTareZone10,
					CAST((Alt_Scale_Tare.Zone1 * 100) as int) AS PLUMStoreALTScaleTareZone1,
					CAST((Alt_Scale_Tare.Zone2 * 100) as int) AS PLUMStoreALTScaleTareZone2,
					CAST((Alt_Scale_Tare.Zone3 * 100) as int) AS PLUMStoreALTScaleTareZone3,
					CAST((Alt_Scale_Tare.Zone4 * 100) as int) AS PLUMStoreALTScaleTareZone4,
					CAST((Alt_Scale_Tare.Zone5 * 100) as int) AS PLUMStoreALTScaleTareZone5,
					CAST((Alt_Scale_Tare.Zone6 * 100) as int) AS PLUMStoreALTScaleTareZone6,
					CAST((Alt_Scale_Tare.Zone7 * 100) as int) AS PLUMStoreALTScaleTareZone7,
					CAST((Alt_Scale_Tare.Zone8 * 100) as int) AS PLUMStoreALTScaleTareZone8,
					CAST((Alt_Scale_Tare.Zone9 * 100) as int) AS PLUMStoreALTScaleTareZone9,
					CAST((Alt_Scale_Tare.Zone10 * 100) as int) AS PLUMStoreALTScaleTareZone10,
					dbo.fn_FormatSmartXScaleTares(ISNULL(Scale_Tare_Override.Zone1, Scale_Tare.Zone1),
												  ISNULL(Scale_Tare_Override.Zone2, Scale_Tare.Zone2),
												  ISNULL(Scale_Tare_Override.Zone3, Scale_Tare.Zone3),
												  ISNULL(Scale_Tare_Override.Zone4, Scale_Tare.Zone4),
												  ISNULL(Scale_Tare_Override.Zone5, Scale_Tare.Zone5),
												  ISNULL(Scale_Tare_Override.Zone6, Scale_Tare.Zone6),
												  ISNULL(Scale_Tare_Override.Zone7, Scale_Tare.Zone7),
												  ISNULL(Scale_Tare_Override.Zone8, Scale_Tare.Zone8),
												  ISNULL(Scale_Tare_Override.Zone9, Scale_Tare.Zone9),
												  ISNULL(Scale_Tare_Override.Zone10, Scale_Tare.Zone10),
												  ISNULL(ISO.ForceTare, ItemScale.ForceTare),
												  ISNULL(ScaleUOM_Override.Unit_Name, ScaleUOM.Unit_Name),
												  ISNULL(ISO.Scale_FixedWeight, ItemScale.Scale_FixedWeight)) As SmartX_Tare,
					CAST(Alt_Scale_Tare.Zone1 AS int) AS AltScaleTare_Int,
					dbo.fn_FormatSmartXScaleTares(Alt_Scale_Tare.Zone1,
												  Alt_Scale_Tare.Zone2,
												  Alt_Scale_Tare.Zone3,
												  Alt_Scale_Tare.Zone4,
												  Alt_Scale_Tare.Zone5,
												  Alt_Scale_Tare.Zone6,
												  Alt_Scale_Tare.Zone7,
												  Alt_Scale_Tare.Zone8,
												  Alt_Scale_Tare.Zone9,
												  Alt_Scale_Tare.Zone10,
												  ISNULL(ISO.ForceTare, ItemScale.ForceTare),
												  ISNULL(ScaleUOM_Override.Unit_Name, ScaleUOM.Unit_Name),
												  ISNULL(ISO.Scale_FixedWeight, ItemScale.Scale_FixedWeight)) As SmartX_AltTare,
					ISNULL(ISO.Scale_EatBy_ID, ItemScale.Scale_EatBy_ID) AS UseBy_ID,
					CASE WHEN ISNULL(ISO.ForceTare, ItemScale.ForceTare) = 1 
						THEN 'Y'
						ELSE 'N'
						END AS ScaleForcedTare,
					ISNULL(ISO.ShelfLife_Length, ItemScale.ShelfLife_Length) AS ShelfLife_Length,
					dbo.fn_GetEplumFixedWeight(
						ISNULL(ISO.Scale_FixedWeight, ItemScale.Scale_FixedWeight), 
						gpmSellingUnit.Unit_Abbreviation,
						PU.Unit_Abbreviation,
						Item.Package_Desc2,
						idf.FlagValue) AS Scale_FixedWeight,
					dbo.fn_GetEplumByCount(
						ISNULL(ISO.Scale_ByCount, ItemScale.Scale_ByCount), 
						gpmSellingUnit.Unit_Abbreviation,
						PU.Unit_Abbreviation,
						idf.FlagValue) AS Scale_ByCount,
					Scale_Grade.Zone1 AS Grade,
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone1,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone2,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone3,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone4,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone5,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone6,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone7,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone8,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone9,0))+','+
					CONVERT(VARCHAR, ISNULL(Scale_Grade.Zone10,0)) As SmartX_Grade,
					ISNULL(ISO.PrintBlankShelfLife, ItemScale.PrintBlankShelfLife) AS PrintBlankShelfLife,
					ISNULL(ISO.PrintBlankEatBy, ItemScale.PrintBlankEatBy) AS PrintBlankEatBy,
					ISNULL(ISO.PrintBlankPackDate, ItemScale.PrintBlankPackDate) AS PrintBlankPackDate,
					ISNULL(ISO.PrintBlankWeight, ItemScale.PrintBlankWeight) AS PrintBlankWeight,
					ISNULL(ISO.PrintBlankUnitPrice, ItemScale.PrintBlankUnitPrice) AS PrintBlankUnitPrice,
					ISNULL(ISO.PrintBlankTotalPrice, ItemScale.PrintBlankTotalPrice) AS PrintBlankTotalPrice,
					CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + ',' + CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + ','+ CAST(Scale_LabelStyle.Description AS VARCHAR(5)) AS [Digi_LNU],
					
					--NutriFacts
					NF.Scale_LabelFormat_ID,
					RTRIM(NF.ServingUnits) as ServingUnits,
					CASE WHEN NF.ServingUnits = 1 THEN 999 ELSE CAST(NF.ServingUnits AS VARCHAR(5)) END as 'ServingUnits1To999',
					NF.ServingSizeDesc AS SizeText,
					NF.ServingsPerPortion AS Size,
					ISNULL(CONVERT(VARCHAR, NF.ServingsPerPortion), '') + ' ' + ISNULL(NF.ServingSizeDesc,'') AS 'ServingSize',
					NF.SizeWeight,
					NF.Calories,
					NF.CaloriesFat,
					NF.CaloriesSaturatedFat,
					NF.ServingPerContainer AS PerContainer,
					NF.TotalFatWeight,
					NF.TotalFatPercentage,
					NF.SaturatedFatWeight,
					NF.SaturatedFatPercent,
					NF.PolyunsaturatedFat,
					NF.MonounsaturatedFat,
					NF.CholesterolWeight,
					NF.CholesterolPercent,
					NF.SodiumWeight,
					NF.SodiumPercent,
					NF.PotassiumWeight,
					NF.PotassiumPercent,
					NF.TotalCarbohydrateWeight,
					NF.TotalCarbohydratePercent,
					NF.DietaryFiberWeight,
					NF.DietaryFiberPercent,
					NF.SolubleFiber,
					NF.InsolubleFiber,
					NF.Sugar,
					NF.SugarAlcohol,
					NF.OtherCarbohydrates,
					NF.ProteinWeight,
					NF.ProteinPercent,
					NF.VitaminA,
					NF.Betacarotene,
					NF.VitaminC,
					NF.Calcium,
					NF.Iron,
					NF.VitaminD,
					NF.VitaminE,
					NF.Thiamin,
					NF.Riboflavin,
					NF.Niacin,
					NF.VitaminB6 ,
					NF.Folate,
					NF.VitaminB12,
					NF.Biotin,
					NF.PantothenicAcid,
					NF.Phosphorous,
					NF.Iodine,
					NF.Magnesium,
					NF.Zinc,
					NF.Copper,
					NF.Transfat,
					NF.CaloriesFromTransFat,
					NF.Om6Fatty,
					NF.Om3Fatty, 
					NF.Starch,
					NF.Chloride,
					NF.Chromium,
					NF.VitaminK,
					NF.Manganese,
					NF.Molybdenum,
					NF.Selenium,
					NF.TransFatWeight,
					Item.Retail_Sale,
					ssd.StorageData AS StorageText
				FROM PLUMCorpChgQueueTmp QT (nolock)
					INNER JOIN #Identifiers II
						ON II.Item_Key = QT.Item_Key  
						AND II.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
						AND II.Remove_Identifier = 0
						AND II.Deleted_Identifier = 0
					INNER JOIN Item (nolock)
						ON Item.Item_Key = QT.Item_Key
						AND Item.Remove_Item = 0
						AND Item.Deleted_Item = 0
					LEFT JOIN SubTeam ST (nolock)
						ON Item.SubTeam_No = ST.SubTeam_No
					-- Join the Price, Store, and ItemScale tables to send zone pricing records with corporate records	
					INNER JOIN Price (nolock)
						ON (QT.Item_Key = Price.Item_Key)
					INNER JOIN Store (nolock)
						ON (Store.Store_No = Price.Store_No)
					LEFT JOIN ItemScale (nolock)
						ON ItemScale.Item_Key = QT.Item_Key
					LEFT JOIN
						dbo.ItemOverride (nolock)
						ON ItemOverride.Item_Key = QT.Item_Key 
							AND ItemOverride.StoreJurisdictionID = Store.StoreJurisdictionID
							AND @UseRegionalScaleFile = 0
							AND @UseStoreJurisdictions = 1
					LEFT JOIN
						dbo.ItemScaleOverride ISO (nolock)
						ON ISO.Item_Key = QT.Item_Key
							AND ISO.StoreJurisdictionID = Store.StoreJurisdictionID
							AND @UseRegionalScaleFile = 0
							AND @UseStoreJurisdictions = 1
					LEFT JOIN
						dbo.ItemCustomerFacingScale icfs (nolock) on ItemScale.Item_Key = icfs.Item_Key
        			LEFT JOIN  NutriFacts NF (nolock)
							ON CASE WHEN ISO.Item_Key IS NOT NULL
									THEN ISO.Nutrifact_ID
									ELSE ItemScale.Nutrifact_ID
							   END = NF.NutriFactsID
							AND @PushScaleNutrifactData = 1
					LEFT JOIN Scale_ExtraText (nolock)
						ON ItemScale.Scale_ExtraText_ID = Scale_ExtraText.Scale_ExtraText_ID
					LEFT JOIN Scale_Ingredient (nolock)
						ON ItemScale.Scale_Ingredient_ID = Scale_Ingredient.Scale_Ingredient_ID
					LEFT JOIN Scale_Allergen (nolock)
						ON ItemScale.Scale_Allergen_ID = Scale_Allergen.Scale_Allergen_ID
					LEFT JOIN Scale_ExtraText Scale_ExtraText_Override (nolock)
						ON ISO.Scale_ExtraText_ID = Scale_ExtraText_Override.Scale_ExtraText_ID
					LEFT JOIN Scale_Tare Scale_Tare (nolock)
						ON ItemScale.Scale_Tare_ID = Scale_Tare.Scale_Tare_ID
					LEFT JOIN Scale_Tare Scale_Tare_Override (nolock)
						ON ISO.Scale_Tare_ID = Scale_Tare_Override.Scale_Tare_ID
					LEFT JOIN Scale_Tare Alt_Scale_Tare (nolock)
						ON ISNULL(ISO.Scale_Alternate_Tare_ID, ItemScale.Scale_Alternate_Tare_ID) = Alt_Scale_Tare.Scale_Tare_ID
					LEFT JOIN Scale_Grade (nolock)
						ON ISNULL(ISO.Scale_Grade_ID, ItemScale.Scale_Grade_ID) = Scale_Grade.Scale_Grade_ID	
					LEFT JOIN Scale_LabelType (nolock)
						ON Scale_ExtraText.Scale_LabelType_ID = Scale_LabelType.Scale_LabelType_ID
					LEFT JOIN Scale_LabelType Scale_LabelType_Override (nolock)
						ON Scale_ExtraText_Override.Scale_LabelType_ID = Scale_LabelType_Override.Scale_LabelType_ID
					LEFT JOIN Scale_LabelStyle (nolock)
						ON ItemScale.Scale_LabelStyle_ID = Scale_LabelStyle.Scale_LabelStyle_ID
					LEFT JOIN Scale_LabelStyle Scale_LabelStyle_Override (nolock)
						ON ISO.Scale_LabelStyle_ID = Scale_LabelStyle_Override.Scale_LabelStyle_ID
					LEFT JOIN ItemUnit ScaleUOM (nolock)
						ON ScaleUOM.Unit_ID = ItemScale.Scale_ScaleUOMUnit_ID
					LEFT JOIN ItemUnit ScaleUOM_Override (nolock)
						ON ScaleUOM_Override.Unit_ID = ISO.Scale_ScaleUOMUnit_ID
					INNER JOIN StoreItem SI (nolock)
						ON SI.Item_Key = Item.Item_Key AND
						   SI.Store_No = Store.Store_No AND
						   SI.Authorized = 1 AND				-- ONLY INCLUDE AUTHORIZED ITEMS IN STORE SCALE PUSH
						   (ScaleAuth = 1 OR ActionCode != 'S') -- IF NEW AUTH, ONLY SEND FOR NEW STORES
					LEFT JOIN StoreItemVendor SIV (nolock)
						ON SIV.Store_No = Store.Store_No
							AND SIV.Item_Key = Item.Item_Key
							AND SIV.PrimaryVendor = 1
					LEFT JOIN Vendor V (nolock)		  
						ON V.Vendor_ID = SIV.Vendor_ID
					LEFT JOIN ItemUnit RU (NOLOCK)
						ON RU.Unit_ID = Item.Retail_Unit_ID
					LEFT JOIN ItemUnit RU_Override (NOLOCK)
						ON RU_Override.Unit_ID = ItemOverride.Retail_Unit_ID
					LEFT JOIN ItemUnit PU (nolock)
						ON PU.Unit_ID = Item.Package_Unit_ID
					LEFT JOIN ItemUnit PU_Override (nolock)
						ON PU_Override.Unit_ID = ItemOverride.Package_Unit_ID
					LEFT JOIN Scale_StorageData ssd (nolock)
						ON ItemScale.Scale_StorageData_ID = ssd.Scale_StorageData_ID
					LEFT JOIN ItemUomOverride iuo (nolock)
						ON QT.Item_Key = iuo.Item_Key
						AND QT.Store_No = iuo.Store_No
					LEFT JOIN ItemUnit gpmSellingUnit (nolock) 
						ON iuo.Retail_Unit_ID = gpmSellingUnit.Unit_ID
					LEFT JOIN dbo.fn_GetInstanceDataFlagStoreValues(@GlobalPriceManagementIdfKey) idf 
						ON QT.Store_No = idf.Store_No
				WHERE 
					((@ActionCode <> 'A' AND ActionCode = @ActionCode) OR
					(@ActionCode  = 'A' AND (ActionCode = 'A' OR ActionCode = 'S'))) -- adds include authorizations
					
					AND 
						((@ActionCode <> 'F') OR
						(@ActionCode  = 'F' AND Store.Store_No = @Store_No))  -- full scale files only include price data for single store
					
					AND QT.Item_Key NOT IN (
						SELECT Item_Key 
						FROM @CurrentPriceBatch CPB 
						WHERE CPB.Store_No = Store.Store_No) -- item is not in a current price batch 
					
					AND Price.POSPrice > 0 -- only send prices if they have been set in IRMA
					AND QT.Store_No = Price.Store_No

					-- 365 non-scale PLUs need to be marked as SendToScale in order to be included.  Normal scale items will not have that value.
					AND ((Store.Mega_Store = 1 and icfs.SendToScale = 1 and dbo.fn_IsPosPlu(ii.Identifier) = 1) or icfs.SendToScale is null)
				ORDER BY 
					Store.Store_No
			END -- End @ActionCode <> A block
		END  --End @UseRegionalScaleFile=0 block
			
		SELECT @error_no = @@ERROR 

	END  --end return result set block

	-- Return another result set that list the authorizations included in this data.  This allows
	-- the Scale Push application to correctly reset the ScaleItem flag for these records.
	SELECT StoreItemAuthorizationID, Store_No, Item_Key FROM @CurrentScaleAuth

	-- Delete the PLUMCorpChgQueueTmp records for authorizations.  If the scale push does not
	-- succeed, these are reprocessed based on the ScaleItem flag, not the Queue records.
	DELETE FROM PLUMCorpChgQueueTmp WHERE ActionCode = 'S'	
			
	-- Delete the PLUMCorpChgQueueTmp records for full scale files.  If the full scale file does not
	-- succeed, these are reprocessed based on the StoreItem.Authorized flag, not the Queue records.
	DELETE FROM PLUMCorpChgQueueTmp WHERE ActionCode = 'F'	
			
	IF @error_no = 0
		BEGIN
			COMMIT TRAN
			SET NOCOUNT OFF
		END
	ELSE
		BEGIN
			IF @@TRANCOUNT <> 0
				ROLLBACK TRAN
			DECLARE @Severity smallint = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
			SET NOCOUNT OFF
			RAISERROR ('GetPLUMCorpChg failed with @@ERROR: %d', @Severity, 1, @error_no)
		END

	DROP TABLE #Identifiers
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMCorpChg] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMCorpChg] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMCorpChg] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMCorpChg] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMCorpChg] TO [IRMAReportsRole]
    AS [dbo];
