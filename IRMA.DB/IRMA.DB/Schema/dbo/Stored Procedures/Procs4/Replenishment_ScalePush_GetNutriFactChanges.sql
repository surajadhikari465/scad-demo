
CREATE PROCEDURE dbo.Replenishment_ScalePush_GetNutriFactChanges

AS

-- **************************************************************************
-- Procedure: Replenishment_ScalePush_GetNutriFactChanges()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called to get nutrifact changes to be sent to the scale(s).
--
-- Modification History:
-- Date       	Init	TFS   			Comment
-- 04/25/2012	BJL 	4945			Renamed ExtraText column to ingredients in the output
--										added columns ScaleIdentifier, ScalePLU, Store_No and ScaleDept
--										Removed zero padding and post-fix measurement abbreviations.
--										Added support for ItemScaleOverrides per Jurisdictional flag.
-- 07/19/2012	BJL		4945			Added scale_labelstyle_id for PN and ServingUnit1to999 for FL, SW and NC
-- 2013-02-06	KM		9393			UNION the first selection with a second selection that joins to ItemScaleOverride; change the Store
--										CROSS APPLY to an INNER JOIN for both selection sets; incorporate StoreJurisdictionID in the joins to account
--										for alternate jurisdiction attributes;
-- 2013-05-14	BJL		11959			Modified the fn_GetScalePLU to take the Identifier and then return the ScalePLU
-- 08/28/2014	DN		15405			Added logic to use either validated or non-validated identifiers
-- 04/02/2015	Lux		8408 & 15993	Converted table variable @ItemIdentifier to temp table #validatedItemIdentifier and added index.  Changed temp-table alias refs from "II" to "VII" to help differentiate.
-- 07/02/2015	DN		16245			Renamed Columns:
--										Size			-> ServingsPerPortion
--										Size Text		-> ServingSizeDesc
--										PerContainer	-> ServingPerContainer
-- 07/03/2015	DN		16245			Renamed the columns in the output to match with the filewriter column names
--										ServingsPerPortion	->	Size
--										ServingSizeDesc		->	SizeText
--										ServingPerContainer	->	PerContainer
-- 07/20/2015   FA		16266			Fixed the spelling error (PerCotainer to PerContainer) 
-- 09/17/2015	KM		11637			Add Item.Retail_Sale to the output;
-- **************************************************************************

BEGIN  
	SET NOCOUNT ON

	BEGIN TRAN
	DECLARE @error_no int
	SELECT @error_no = 0
	
	BEGIN
		-- Populate the NutriFactsChgQueueTmp with all of the current records in the NutriFactsChgQueue table.
		INSERT INTO NutriFactsChgQueueTmp  (NutriFactsChgQueue_ID, NutriFact_ID, ActionCode, Store_No)
			SELECT NutriFactsChgQueue_ID, NutriFactsID AS NutriFact_ID, ActionCode, Store_No FROM NutriFactsChgQueue
	END

	SELECT @error_no = @@ERROR
	
	IF @error_no = 0
	BEGIN
		-- Delete the records from the NutriFactsChgQueue table that were successfully entered into the 
		-- NutriFactsChgQueueTmp table.  
		DELETE NutriFactsChgQueue
		FROM NutriFactsChgQueue Q
		INNER JOIN
			NutriFactsChgQueueTmp T	ON Q.NutriFactsChgQueue_ID = T.NutriFactsChgQueue_ID
		SELECT @error_no = @@ERROR
	END

	-- Get the current date to use for all of the records that are returned.
	DECLARE @CurrDay DateTime
	SELECT @CurrDay = GetDate()
	
	DECLARE @SmartX_PendingName AS CHAR(15)
	SELECT @SmartX_PendingName = 'NUTRI: ' + CONVERT(CHAR(8),@CurrDay,10)
	
	DECLARE @SmartX_MaintenanceDateTime AS CHAR(16)
	SELECT @SmartX_MaintenanceDateTime = CONVERT(CHAR(8),@CurrDay,10) + CONVERT(CHAR(8),@CurrDay,8)

	-- Using the regional scale file?
	DECLARE @IsRegionalScaleFile as int, @RegionalOfficeStoreNo int

	-- determine if region uses regional scale file 
	SELECT @IsRegionalScaleFile = ISNULL(FlagValue, 0) FROM InstanceDataFlags WHERE FlagKey = 'UseRegionalScaleFile' 
	--select @IsRegionalScaleFile

	-- determine Store_no for regional office
	SELECT @RegionalOfficeStoreNo = ISNULL(Store_No, 0) FROM Store s WHERE Regional = 1

	--Determine how region wants to send down data to scales
	DECLARE @PluDigitsSentToScale varchar(20)
	SELECT @PluDigitsSentToScale = PluDigitsSentToScale FROM dbo.InstanceData (NOLOCK)

	-- Create indexed temp table to hold validated item identifiers (scan codes).
	if object_id('tempdb..#validatedItemIdentifier') is not null
	begin
		drop table #validatedItemIdentifier
	end
	create table #validatedItemIdentifier (
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
		Scale_Identifier		BIT
		PRIMARY KEY CLUSTERED (Item_Key, Identifier)
	)
	
	DECLARE @Status SMALLINT
	SET @Status = dbo.fn_ReceiveUPCPLUUpdateFromIcon()
	
	IF @Status = 0 -- Validated UPC & PLU flags have not been turned on for the region.
	BEGIN
		INSERT INTO #validatedItemIdentifier
		SELECT II.* FROM ItemIdentifier II (NOLOCK)
			
	END
ELSE
	IF @Status = 1 -- Only validated UPCs are passing from Icon to IRMA
		BEGIN
			INSERT INTO #validatedItemIdentifier		
			SELECT II.*
			FROM ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK)
			ON II.Identifier = VSC.ScanCode
			UNION
			SELECT II.*
			FROM ItemIdentifier II (NOLOCK)
			WHERE (LEN(Identifier) < 7 OR Identifier LIKE '2%00000')
			UNION
			SELECT II.*
			FROM Item I (NOLOCK) INNER JOIN ItemIdentifier II (NOLOCK)
			ON I.Item_Key = II.Item_Key
			WHERE I.Remove_Item = 1 OR II.Remove_Identifier = 1

				
		END
	ELSE
		IF @Status = 2 -- Only validated PLUs are passing from Icon to IRMA
				BEGIN
					INSERT INTO #validatedItemIdentifier			
					SELECT II.*
					FROM ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK)
					ON II.Identifier = VSC.ScanCode
					WHERE (LEN(Identifier) < 7 OR Identifier LIKE '2%00000')
					UNION
					SELECT II.*
					FROM ItemIdentifier II (NOLOCK)
					WHERE NOT (LEN(Identifier) < 7 OR Identifier LIKE '2%00000')
					UNION
					SELECT II.*
					FROM Item I (NOLOCK) INNER JOIN ItemIdentifier II (NOLOCK)
					ON I.Item_Key = II.Item_Key
					WHERE I.Remove_Item = 1 OR II.Remove_Identifier = 1
				
				END
			ELSE 
				IF @Status = 3 -- Both Validated UPC & PLU are passing from Icon to IRMA
					BEGIN				
						INSERT INTO #validatedItemIdentifier				
						SELECT II.*
						FROM ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK)
						ON II.Identifier = VSC.ScanCode 
						UNION
						SELECT II.*
						FROM Item I (NOLOCK) INNER JOIN ItemIdentifier II (NOLOCK)
						ON I.Item_Key = II.Item_Key
						WHERE I.Remove_Item = 1 OR II.Remove_Identifier = 1
					END

	SELECT @error_no = @@ERROR

	IF @error_no = 0
	BEGIN  		
		--Return first result set (Scale items whose ItemScale.Nutrifact_ID value matches the value of the Nutrifact_ID in the change queue table).  
		SELECT
			DISTINCT		-- added to consolidate results into a single record when returning region-level results 
				QT.ActionCode, 
				CASE WHEN ActionCode = 'A' THEN 'Z' -- Code for New 
					WHEN ActionCode = 'C' THEN 'W' -- Code for Change
					WHEN ActionCode = 'D' THEN 'Y' -- Code for Delete
				END AS SmartX_RecordType,
				@SmartX_PendingName AS SmartX_PendingName, 
				@SmartX_MaintenanceDateTime AS SmartX_MaintenanceDateTime,
				ITS.NutriFact_ID,
				VII.Identifier as 'ScaleIdentifier',
				[dbo].[fn_GetScalePLU](VII.Identifier, VII.NumPluDigitsSentToScale, @PluDigitsSentToScale, 0) as 'ScalePLU',
				NF.Scale_LabelFormat_ID,
				ITS.Scale_LabelStyle_ID,
				CASE WHEN NF.ServingUnits = 1 THEN 999 ELSE CAST(NF.ServingUnits AS VARCHAR(5)) END as 'ServingUnits1To999',
				RTRIM(NF.ServingUnits) as ServingUnits,
				NF.ServingSizeDesc AS SizeText,
				NF.ServingsPerPortion AS Size,
				ISNULL(CONVERT(VARCHAR, NF.ServingsPerPortion), '') + ' ' + ISNULL(NF.ServingSizeDesc,'') AS 'ServingSize',
				ISNULL(CONVERT(varchar, NF.SizeWeight),'0') AS SizeWeight,
				NF.Calories,
				NF.CaloriesFat,
				NF.CaloriesSaturatedFat,
				NF.ServingPerContainer AS PerContainer,
				ISNULL(CONVERT(varchar, NF.TotalFatWeight),'0') AS TotalFatWeight,
				ISNULL(CONVERT(varchar, NF.TotalFatPercentage),'0') AS TotalFatPercentage,
				ISNULL(CONVERT(varchar, NF.SaturatedFatWeight),'0') AS SaturatedFatWeight,
				ISNULL(CONVERT(varchar, NF.SaturatedFatPercent),'0') AS SaturatedFatPercent,
				ISNULL(CONVERT(varchar, NF.PolyunsaturatedFat),'0') AS PolyunsaturatedFat,
				ISNULL(CONVERT(varchar, NF.MonounsaturatedFat),'0') AS MonounsaturatedFat,
				ISNULL(CONVERT(varchar, NF.CholesterolWeight),'0') AS CholesterolWeight,
				ISNULL(CONVERT(varchar, NF.CholesterolPercent),'0') AS CholesterolPercent,
				ISNULL(CONVERT(varchar, NF.SodiumWeight),'0') AS SodiumWeight,
				ISNULL(CONVERT(varchar, NF.SodiumPercent),'0') AS SodiumPercent,
				ISNULL(CONVERT(varchar, NF.PotassiumWeight),'0') AS PotassiumWeight,
				ISNULL(CONVERT(varchar, NF.PotassiumPercent),'0') AS PotassiumPercent,
				ISNULL(CONVERT(varchar, NF.TotalCarbohydrateWeight),'0') AS TotalCarbohydrateWeight,
				ISNULL(CONVERT(varchar, NF.TotalCarbohydratePercent),'0') AS TotalCarbohydratePercent,
				ISNULL(CONVERT(varchar, NF.DietaryFiberWeight),'0') AS DietaryFiberWeight,
				ISNULL(CONVERT(varchar, NF.DietaryFiberPercent),'0') AS DietaryFiberPercent,
				ISNULL(CONVERT(varchar, NF.SolubleFiber),'0') AS SolubleFiber,
				ISNULL(CONVERT(varchar, NF.InsolubleFiber),'0') AS InsolubleFiber,
				ISNULL(CONVERT(varchar, NF.Sugar),'0') AS Sugar,
				ISNULL(CONVERT(varchar, NF.SugarAlcohol),'0') AS SugarAlcohol,
				ISNULL(CONVERT(varchar, NF.OtherCarbohydrates),'0') AS OtherCarbohydrates,
				ISNULL(CONVERT(varchar, NF.ProteinWeight),'0') AS ProteinWeight,
				ISNULL(CONVERT(varchar, NF.ProteinPercent),'0') AS ProteinPercent,
				ISNULL(CONVERT(varchar, NF.VitaminA),'0') AS VitaminA,
				ISNULL(CONVERT(varchar, NF.Betacarotene),'0') AS Betacarotene,
				ISNULL(CONVERT(varchar, NF.VitaminC),'0') AS VitaminC,
				ISNULL(CONVERT(varchar, NF.Calcium),'0') AS Calcium,
				ISNULL(CONVERT(varchar, NF.Iron),'0') AS Iron,
				ISNULL(CONVERT(varchar, NF.VitaminD),'0') AS VitaminD,
				ISNULL(CONVERT(varchar, NF.VitaminE),'0') AS VitaminE,
				ISNULL(CONVERT(varchar, NF.Thiamin),'0') AS Thiamin,
				ISNULL(CONVERT(varchar, NF.Riboflavin),'0') AS Riboflavin,
				ISNULL(CONVERT(varchar, NF.Niacin),'0') AS Niacin,
				ISNULL(CONVERT(varchar, NF.VitaminB6),'0') AS VitaminB6,
				ISNULL(CONVERT(varchar, NF.Folate),'0') AS Folate,
				ISNULL(CONVERT(varchar, NF.VitaminB12),'0') AS VitaminB12,
				ISNULL(CONVERT(varchar, NF.Biotin),'0') AS Biotin,
				ISNULL(CONVERT(varchar, NF.PantothenicAcid),'0') AS PantothenicAcid,
				ISNULL(CONVERT(varchar, NF.Phosphorous),'0') AS Phosphorous,
				ISNULL(CONVERT(varchar, NF.Iodine),'0') AS Iodine,
				ISNULL(CONVERT(varchar, NF.Magnesium),'0') AS Magnesium,
				ISNULL(CONVERT(varchar, NF.Zinc),'0') AS Zinc,
				ISNULL(CONVERT(varchar, NF.Copper),'0') AS Copper,
				
				-- Note: transfat is a smallint in the database, but it should be returned as a decimal.
				ISNULL(CONVERT(varchar,NF.Transfat),'0') AS Transfat,
				
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
				
				-- Return Store_No as the regional office store number when returning results at region-level
				-- If a store_no is specified for an extra text record in the chg queue only send down the extra text only to that store
				-- otherwise send down the extra text record for all authorized stores.
				CASE @IsRegionalScaleFile WHEN 1 THEN @RegionalOfficeStoreNo ELSE ISNULL(QT.Store_No, S.Store_No) END AS Store_No,
				
				st.ScaleDept,
				I.Retail_Sale
		
		-- Returns a record for each of the ItemScale records that are assigned to a NutriFact that had a change.  StoreJurisdictionID is included on the Item
		-- join so that this result set will not contain any alternate jurisdicion attributes.
		FROM 
			NutriFactsChgQueueTmp QT			(nolock)
			INNER JOIN NutriFacts NF			(nolock)	ON	QT.NutriFact_ID			= NF.NutriFactsID
			INNER JOIN Store s								ON	qt.Store_No				= s.Store_No
			INNER JOIN ItemScale ITS			(nolock)	ON	NF.NutriFactsID			= ITS.NutriFact_ID
			INNER JOIN Item I					(nolock)	ON	ITS.Item_Key			= I.Item_Key 
															AND s.StoreJurisdictionID	= i.StoreJurisdictionID
			INNER JOIN #validatedItemIdentifier VII			ON	I.Item_Key				= VII.Item_Key  
															AND VII.Scale_Identifier	= 1 --ONLY INCLUDE SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
			INNER JOIN StoreItem si							ON	si.Item_Key				= i.Item_Key
															AND si.Store_No				= s.Store_No
			INNER JOIN SubTeam st				(nolock)	ON	st.SubTeam_No			= i.SubTeam_No
		
		WHERE 
			@IsRegionalScaleFile = 1
				OR ((s.WFM_Store = 1 OR s.Mega_Store = 1)
					AND si.Authorized = 1
					AND i.Deleted_Item = 0)
		
		UNION

		--Return second result set (Scale items whose ItemScaleOverride.Nutrifact_ID value matches the value of the Nutrifact_ID in the change queue table).  
		SELECT
			DISTINCT		-- added to consolidate results into a single record when returning region-level results 
				QT.ActionCode, 
				CASE WHEN ActionCode = 'A' THEN 'Z' -- Code for New 
					WHEN ActionCode = 'C' THEN 'W' -- Code for Change
					WHEN ActionCode = 'D' THEN 'Y' -- Code for Delete
				END AS SmartX_RecordType,
				@SmartX_PendingName AS SmartX_PendingName, 
				@SmartX_MaintenanceDateTime AS SmartX_MaintenanceDateTime,
				ITS.NutriFact_ID,
				VII.Identifier as 'ScaleIdentifier',
				[dbo].[fn_GetScalePLU](VII.Identifier, VII.NumPluDigitsSentToScale, @PluDigitsSentToScale, 0) as 'ScalePLU',
				NF.Scale_LabelFormat_ID,
				ITS.Scale_LabelStyle_ID,
				CASE WHEN NF.ServingUnits = 1 THEN 999 ELSE CAST(NF.ServingUnits AS VARCHAR(5)) END as 'ServingUnits1To999',
				RTRIM(NF.ServingUnits) as ServingUnits,
				NF.ServingSizeDesc AS SizeText,
				NF.ServingsPerPortion AS Size,
				ISNULL(CONVERT(VARCHAR, NF.ServingsPerPortion), '') + ' ' + ISNULL(NF.ServingSizeDesc,'') AS 'ServingSize',
				ISNULL(CONVERT(varchar, NF.SizeWeight),'0') AS SizeWeight,
				NF.Calories,
				NF.CaloriesFat,
				NF.CaloriesSaturatedFat,
				NF.ServingPerContainer AS PerContainer,
				ISNULL(CONVERT(varchar, NF.TotalFatWeight),'0') AS TotalFatWeight,
				ISNULL(CONVERT(varchar, NF.TotalFatPercentage),'0') AS TotalFatPercentage,
				ISNULL(CONVERT(varchar, NF.SaturatedFatWeight),'0') AS SaturatedFatWeight,
				ISNULL(CONVERT(varchar, NF.SaturatedFatPercent),'0') AS SaturatedFatPercent,
				ISNULL(CONVERT(varchar, NF.PolyunsaturatedFat),'0') AS PolyunsaturatedFat,
				ISNULL(CONVERT(varchar, NF.MonounsaturatedFat),'0') AS MonounsaturatedFat,
				ISNULL(CONVERT(varchar, NF.CholesterolWeight),'0') AS CholesterolWeight,
				ISNULL(CONVERT(varchar, NF.CholesterolPercent),'0') AS CholesterolPercent,
				ISNULL(CONVERT(varchar, NF.SodiumWeight),'0') AS SodiumWeight,
				ISNULL(CONVERT(varchar, NF.SodiumPercent),'0') AS SodiumPercent,
				ISNULL(CONVERT(varchar, NF.PotassiumWeight),'0') AS PotassiumWeight,
				ISNULL(CONVERT(varchar, NF.PotassiumPercent),'0') AS PotassiumPercent,
				ISNULL(CONVERT(varchar, NF.TotalCarbohydrateWeight),'0') AS TotalCarbohydrateWeight,
				ISNULL(CONVERT(varchar, NF.TotalCarbohydratePercent),'0') AS TotalCarbohydratePercent,
				ISNULL(CONVERT(varchar, NF.DietaryFiberWeight),'0') AS DietaryFiberWeight,
				ISNULL(CONVERT(varchar, NF.DietaryFiberPercent),'0') AS DietaryFiberPercent,
				ISNULL(CONVERT(varchar, NF.SolubleFiber),'0') AS SolubleFiber,
				ISNULL(CONVERT(varchar, NF.InsolubleFiber),'0') AS InsolubleFiber,
				ISNULL(CONVERT(varchar, NF.Sugar),'0') AS Sugar,
				ISNULL(CONVERT(varchar, NF.SugarAlcohol),'0') AS SugarAlcohol,
				ISNULL(CONVERT(varchar, NF.OtherCarbohydrates),'0') AS OtherCarbohydrates,
				ISNULL(CONVERT(varchar, NF.ProteinWeight),'0') AS ProteinWeight,
				ISNULL(CONVERT(varchar, NF.ProteinPercent),'0') AS ProteinPercent,
				ISNULL(CONVERT(varchar, NF.VitaminA),'0') AS VitaminA,
				ISNULL(CONVERT(varchar, NF.Betacarotene),'0') AS Betacarotene,
				ISNULL(CONVERT(varchar, NF.VitaminC),'0') AS VitaminC,
				ISNULL(CONVERT(varchar, NF.Calcium),'0') AS Calcium,
				ISNULL(CONVERT(varchar, NF.Iron),'0') AS Iron,
				ISNULL(CONVERT(varchar, NF.VitaminD),'0') AS VitaminD,
				ISNULL(CONVERT(varchar, NF.VitaminE),'0') AS VitaminE,
				ISNULL(CONVERT(varchar, NF.Thiamin),'0') AS Thiamin,
				ISNULL(CONVERT(varchar, NF.Riboflavin),'0') AS Riboflavin,
				ISNULL(CONVERT(varchar, NF.Niacin),'0') AS Niacin,
				ISNULL(CONVERT(varchar, NF.VitaminB6),'0') AS VitaminB6,
				ISNULL(CONVERT(varchar, NF.Folate),'0') AS Folate,
				ISNULL(CONVERT(varchar, NF.VitaminB12),'0') AS VitaminB12,
				ISNULL(CONVERT(varchar, NF.Biotin),'0') AS Biotin,
				ISNULL(CONVERT(varchar, NF.PantothenicAcid),'0') AS PantothenicAcid,
				ISNULL(CONVERT(varchar, NF.Phosphorous),'0') AS Phosphorous,
				ISNULL(CONVERT(varchar, NF.Iodine),'0') AS Iodine,
				ISNULL(CONVERT(varchar, NF.Magnesium),'0') AS Magnesium,
				ISNULL(CONVERT(varchar, NF.Zinc),'0') AS Zinc,
				ISNULL(CONVERT(varchar, NF.Copper),'0') AS Copper,
				
				-- Note: transfat is a smallint in the database, but it should be returned as a decimal.
				ISNULL(CONVERT(varchar,NF.Transfat),'0') AS Transfat,
				
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
				
				-- Return Store_No as the regional office store number when returning results at region-level
				-- If a store_no is specified for an extra text record in the chg queue only send down the extra text only to that store
				-- otherwise send down the extra text record for all authorized stores.
				CASE @IsRegionalScaleFile WHEN 1 THEN @RegionalOfficeStoreNo ELSE ISNULL(QT.Store_No, S.Store_No) END AS Store_No,
				
				st.ScaleDept,
				I.Retail_Sale
		
		-- Returns a record for each of the ItemScaleOverride records that are assigned to a NutriFact that had a change.  StoreJursidictionID is included on
		-- the ItemScaleOverride join so that only alternate jurisdiction attributes will be selected.
		FROM 
			NutriFactsChgQueueTmp QT			(nolock)
			INNER JOIN NutriFacts NF			(nolock)	ON	QT.NutriFact_ID			= NF.NutriFactsID
			INNER JOIN Store s								ON	qt.Store_No				= s.Store_No
			INNER JOIN ItemScaleOverride ITS	(nolock)	ON	NF.NutriFactsID			= ITS.NutriFact_ID 
															AND s.StoreJurisdictionID	= its.StoreJurisdictionID
			INNER JOIN Item I					(nolock)	ON	ITS.Item_Key			= I.Item_Key 
			INNER JOIN #validatedItemIdentifier VII			ON	I.Item_Key				= VII.Item_Key  
															AND VII.Scale_Identifier	= 1 --ONLY INCLUDE SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
			INNER JOIN StoreItem si							ON	si.Item_Key				= i.Item_Key
															AND si.Store_No				= s.Store_No
			INNER JOIN SubTeam st				(nolock)	ON	st.SubTeam_No			= i.SubTeam_No
		
		WHERE 
			@IsRegionalScaleFile = 1
				OR ((s.WFM_Store = 1 OR s.Mega_Store = 1)
					AND si.Authorized = 1
					AND i.Deleted_Item = 0)
		
		ORDER BY 
			QT.ActionCode
				
		SELECT @error_no = @@ERROR 
	END  

	IF @error_no = 0
	BEGIN
		COMMIT TRAN
		SET NOCOUNT OFF
	END
	ELSE
	BEGIN
		IF @@TRANCOUNT <> 0
			ROLLBACK TRAN
		DECLARE @Severity smallint
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
		SET NOCOUNT OFF
		RAISERROR ('Replenishment_ScalePush_GetNutriFactChanges failed with @@ERROR: %d', @Severity, 1, @error_no)
	END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_GetNutriFactChanges] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_GetNutriFactChanges] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_GetNutriFactChanges] TO [IRMASchedJobsRole]
    AS [dbo];

