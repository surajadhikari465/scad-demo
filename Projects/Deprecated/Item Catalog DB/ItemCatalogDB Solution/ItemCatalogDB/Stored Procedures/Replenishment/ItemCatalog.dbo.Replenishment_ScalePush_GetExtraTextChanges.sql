 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Replenishment_ScalePush_GetExtraTextChanges') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE dbo.Replenishment_ScalePush_GetExtraTextChanges
GO

CREATE PROCEDURE dbo.Replenishment_ScalePush_GetExtraTextChanges
AS

-- **************************************************************************
-- Procedure: Replenishment_ScalePush_GetExtraTextChanges()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called to get extra text changes to be sent to the scale(s)
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 04/25/2012	BJL   			4945	Renamed ExtraText column to ingredients in the output
--										added columns ScaleIdentifier, ScalePLU, Store_No and ScaleDept
--										aliased ET.ExtraText as Ingredients.
-- 05/25/2012	BJL				4945	Added IngredientNumber to match GetPLUMCorpChg.PRC
-- 03/12/2013	BAS				10919	Removed BEGIN TRAN to prevent locking the Queue table.
--										Also moved DELETE Scale_ExtraTextChgQueue to the end
--										if there are no errors.
-- 05/13/2013	BJL				11959	Changed fn_GetScalePLU function to take in the Identifier and to then return formatted ScalePLU.
-- 08/28/2014	DN				15405	Added logic to use either validated or non-validated identifiers
-- 12/18/2014	DN				15629	Used temp table #Identifiers instead of @ItemIdentifiers
-- 05/11/2014	KM				16148 (TFS Web 9115)	Add a clustered primary key index to the #Identifiers table.
-- 07/05/2015	DN				16250	Concatenation of Ingredient, Allergen & Extra Text fields
-- 07/07/2015	DN				16250	Reorder fields: Ingredients + Allergen + Extra Text
-- 09/17/2015	KM				11637	Add Item.Retail_Sale to the output;
-- 2017-04-13   MZ        23765(20859)	Move Allergens before Ingredients in the concatenation. Correct the order of the Ingredients field
--										Allergens + Ingredients + ExtraText
-- **************************************************************************

BEGIN  --Begin stored procedure

	SET NOCOUNT ON

	DECLARE @ExtraTextChangeType INT = (SELECT TOP 1 POSChangeTypeKey FROM dbo.POSChangeType WHERE ChangeTypeDesc = 'ExtraText Data')

	IF EXISTS( 
				SELECT 1 FROM dbo.POSWriterFileConfig 
				WHERE POSFileWriterkey IN (SELECT POSFileWriterKey FROM  POSWriter WHERE posFileWriterClass = 'EPlum_Writer')
			    AND POSChangeTypekey = @ExtraTextChangeType
			 )
	BEGIN

	DECLARE @error_no int
	DECLARE @Severity smallint

	-- Maximum length for Ingredients column
	DECLARE @MaxWidthForIngredients AS INT

	SET @MaxWidthForIngredients = (SELECT character_maximum_length FROM information_schema.columns WHERE table_name = 'Scale_ExtraText' and column_name = 'ExtraText')
	
	SELECT @error_no = 0

	-- Populate the Scale_ExtraTextChgQueueTmp with all of the current records in the Scale_ExtraTextChgQueue table.
	INSERT INTO Scale_ExtraTextChgQueueTmp (Scale_ExtraTextChgQueue_ID, Scale_ExtraText_ID, ActionCode, Store_No)
		SELECT Scale_ExtraTextChgQueue_ID, Scale_ExtraText_ID, ActionCode, Store_No FROM Scale_ExtraTextChgQueue

	SELECT @error_no = @@ERROR

	IF @error_no <> 0	
	BEGIN
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
		SET NOCOUNT OFF
		RAISERROR ('Replenishment_ScalePush_GetExtraTextChanges - first transaction - failed with @@ERROR: %d', @Severity, 1, @error_no)
	END

	SELECT @error_no = 0

	--Using the regional scale file?
	DECLARE @IsRegionalScaleFile as int, @RegionalOfficeStoreNo int
	-- determine if region uses regional scale file 
	SELECT @IsRegionalScaleFile = ISNULL(FlagValue,0) FROM InstanceDataFlags WHERE FlagKey = 'UseRegionalScaleFile' 
	-- determine Store_no for regional office
	SELECT @RegionalOfficeStoreNo = ISNULL(Store_No,0) FROM Store s WHERE Regional = 1

	--Determine how region wants to send down data to scales
	DECLARE @PluDigitsSentToScale varchar(20)
	SELECT @PluDigitsSentToScale = PluDigitsSentToScale FROM dbo.InstanceData (NOLOCK)

	-- Using store jurisdictions for override values?
	DECLARE @UseStoreJurisdictions int
	SELECT @UseStoreJurisdictions = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'UseStoreJurisdictions'

	-- Get the current date to use for all of the records that are returned.
	DECLARE @CurrDay DateTime
	SELECT @CurrDay = GetDate()
	
	DECLARE @SmartX_PendingName AS CHAR(14)
	SELECT @SmartX_PendingName = 'TEXT: ' + CONVERT(CHAR(8),@CurrDay,10)
	
	DECLARE @SmartX_MaintenanceDateTime AS CHAR(16)
	SELECT @SmartX_MaintenanceDateTime = CONVERT(CHAR(8),@CurrDay,10) + CONVERT(CHAR(8),@CurrDay,8)

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

	SELECT @error_no = @@ERROR

	IF @error_no = 0
	BEGIN  		

		--Return first result set
		SELECT 
			DISTINCT		-- added to consolidate results into a single record when returning region-level results
				QT.ActionCode, 
				CASE WHEN ActionCode = 'A' THEN 'Z' -- Code for New 
					WHEN ActionCode = 'C' THEN 'W' -- Code for Change
					WHEN ActionCode = 'D' THEN 'Y' -- Code for Delete
				END AS SmartX_RecordType,
				@SmartX_PendingName AS SmartX_PendingName, 
				@SmartX_MaintenanceDateTime AS SmartX_MaintenanceDateTime,
				ITS.Scale_ExtraText_ID,
				LT.LinesPerLabel,
				LT.Characters,
				CASE WHEN ISNULL(SETO.ExtraText, ET.ExtraText) <> '' 
					THEN SUBSTRING(II.Identifier, 2, 5) 
					ELSE '0' 
				END As 'IngredientNumber',
				ISNULL(ISNULL(SETO.ExtraText, SUBSTRING(RTRIM(LTRIM(ISNULL(Scale_Allergen.Allergens, '') + ' ' + ISNULL(Scale_Ingredient.Ingredients, '') + ' ' + ISNULL(ET.ExtraText, ''))), 1, @MaxWidthForIngredients)),'') as 'Ingredients',
				II.Identifier as 'ScaleIdentifier',
				[dbo].[fn_GetScalePLU](II.Identifier, II.NumPluDigitsSentToScale, @PluDigitsSentToScale, 0) as 'ScalePLU',
				'S' As SmartX_Sequence,
				0 As SmartX_LineNum,
			-- Return Store_No as the regional office store number when returning results at region-level
			-- If a store_no is specified for an extra text record in the chg queue only send down the extra text only to that store
			-- otherwise send down the extra text record for all authorized stores.
				CASE @IsRegionalScaleFile WHEN 1 THEN @RegionalOfficeStoreNo ELSE ISNULL(QT.Store_No, S.Store_No) END as Store_No,
				st.ScaleDept,
				I.Retail_Sale
		-- Returns a record for each of the ItemScale records that are assigned to an Extra Text that had a change.
		FROM Scale_ExtraTextChgQueueTmp QT (nolock)
			INNER JOIN Scale_ExtraText ET (nolock)
				ON QT.Scale_ExtraText_ID = ET.Scale_ExtraText_ID
			INNER JOIN ItemScale ITS (nolock)
				ON ET.Scale_ExtraText_ID = ITS.Scale_ExtraText_ID
			LEFT JOIN Scale_Ingredient (nolock)
				ON ITS.Scale_Ingredient_ID = Scale_Ingredient.Scale_Ingredient_ID
			LEFT JOIN Scale_Allergen (nolock)
				ON ITS.Scale_Allergen_ID = Scale_Allergen.Scale_Allergen_ID
			INNER JOIN Item I (nolock)
				ON ITS.Item_Key = I.Item_Key 
			INNER JOIN #Identifiers II 
				ON I.Item_Key = II.Item_Key  
				AND II.Scale_Identifier = 1 --ONLY INCLUDE SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
			LEFT JOIN Scale_LabelType LT (nolock)
				ON ET.Scale_LabelType_ID = LT.Scale_LabelType_ID
			CROSS APPLY Store s
			INNER JOIN StoreItem si with (nolock)
				ON si.Item_Key = i.Item_Key
				AND si.Store_No = s.Store_No
			INNER JOIN SubTeam st with (nolock)
				ON st.SubTeam_No = i.SubTeam_No
			LEFT JOIN ItemScaleOverride ISO (nolock)
				ON ISO.Item_Key = I.Item_Key
					AND ISO.StoreJurisdictionID = s.StoreJurisdictionID
					AND @IsRegionalScaleFile = 0
					AND @UseStoreJurisdictions = 1
			LEFT JOIN Scale_ExtraText SETO (nolock)
				ON ISO.Scale_ExtraText_ID = SETO.Scale_ExtraText_ID
		WHERE @IsRegionalScaleFile = 1
				OR ((s.WFM_Store = 1 OR s.Mega_Store = 1)
					AND si.Authorized = 1)
		ORDER BY QT.ActionCode
			
		SELECT @error_no = @@ERROR 
	END  

	IF @error_no = 0
	BEGIN
		-- Delete the records from the Scale_ExtraTextChgQueue table that were successfully entered into the 
		-- Scale_ExtraTextChgQueueTmp table.  
		DELETE Scale_ExtraTextChgQueue
		FROM 
			Scale_ExtraTextChgQueue Q
			INNER JOIN Scale_ExtraTextChgQueueTmp T ON Q.Scale_ExtraTextChgQueue_ID = T.Scale_ExtraTextChgQueue_ID
		SELECT @error_no = @@ERROR
	END

	DROP TABLE #Identifiers
	END
END  --End stored procedure
GO
