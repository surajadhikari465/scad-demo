/*
 * Title: Script 1 - Step 2 - Global PLU Mapping - Update Global PLUs in IRMA
 * Author: Benjamin Loving
 * Date: 7/16/2014
 * Description: This script is for the insert and update of the Global PLUs based on the PLU Mapping in IRMA.
 * Database: ItemCatalog or ItemCatalog_TEST
 * Instructions: Run the script on each IRMA instance
 * 
 */
SET NOCOUNT ON
GO

DECLARE @runTime DATETIME,
		@runUser VARCHAR(128),
		@runHost VARCHAR(128),
		@runDB VARCHAR(128)
		
SELECT	@runTime = GETDATE(),
		@runUser = SUSER_NAME(),
		@runHost = HOST_NAME(),
		@runDB = DB_NAME()

PRINT '---------------------------------------------------------------------------------------'
PRINT '-- Current System Time: ' + CONVERT(VARCHAR, @runTime, 121)
PRINT '-- User Name: ' + @runUser
PRINT '-- Running From Host: ' + @runHost
PRINT '-- Connected To DB Server: ' + @@SERVERNAME
PRINT '-- DB Name: ' + @runDB
PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Begin Global PLU Alignment Maintenance Script 1 Step 2'
PRINT '---------------------------------------------------------------------------------------'

IF (EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN ('FL','MA','NA','EU','RM','NE')))
BEGIN

DECLARE @GlobalPluAlignmentHistoryId INT
SELECT @GlobalPluAlignmentHistoryId = (SELECT MAX(GlobalPluAlignmentHistoryId) FROM [dbo].[GlobalPluAlignmentHistory])

PRINT '---------------------------------------------------------------------------------------'
PRINT '-- Begin Try...'
PRINT '---------------------------------------------------------------------------------------'
BEGIN TRY

	SELECT '-- Set up temporary variables...'
	DECLARE @GlobalPluAlignmentId INT,
			@Item_Key INT,
			@Identifier NVARCHAR(50),
			@Identifier_ID INT,
			@Scale_Identifier BIT,
			@National_Identifier TINYINT,
			@NumPluDigitsSentToScale INT,
			@regionCode CHAR(2),
			@CodeLocation VARCHAR(255)

	SELECT @regionCode = (SELECT [RegionCode] FROM [dbo].[Region])

	SELECT '-- Set up temporary table variables...'
	IF OBJECT_ID('tempdb..#GlobalPluMap') IS NOT NULL
		DROP TABLE #GlobalPluMap
	CREATE TABLE #GlobalPluMap
	(
		[GlobalPluAlignmentId] INT,
		[Region] CHAR(2) NULL,
		[RegionalPlu] NVARCHAR(13) NULL,
		[RegionalPlu_Item_Key] INT NULL,
		[RegionalPlu_Identifier_ID] INT NULL,
		[GlobalPlu] NVARCHAR(13) NULL,
		[GlobalPlu_Item_Key] INT NULL,
		[GlobalPlu_Identifier_ID] INT NULL,
		[Timestamp] DATETIME DEFAULT CURRENT_TIMESTAMP
	)

	IF OBJECT_ID('tempdb..#globalPlusToUpdate') IS NOT NULL
		DROP TABLE #globalPlusToUpdate
	CREATE TABLE #globalPlusToUpdate
	(
		[GlobalPluAlignmentId] INT,
		[Item_Key] INT,
		[Identifier] NVARCHAR(13),
		[Identifier_ID] INT,
		[Scale_Identifier] BIT DEFAULT 0,
		[National_Identifier] TINYINT,
		[NumPluDigitsSentToScale] INT,
		[update] BIT DEFAULT 0
	)

	PRINT '---------------------------------------------------------------------------------------'
	PRINT '-----   Global PLUs Mapping'
	PRINT '---------------------------------------------------------------------------------------'
	INSERT INTO #GlobalPluMap 
	(
		[GlobalPluAlignmentId],
		[Region],
		[RegionalPlu],
		[GlobalPlu]
	)
	SELECT 
		gpa.[GlobalPluAlignmentId],
		gpa.[Region],
		gpa.[RegionalPlu],
		gpa.[GlobalPlu]
	FROM [dbo].[GlobalPluAlignment] gpa
	WHERE gpa.Region = @regionCode

	------PRINT 'Get Item Information for the GlobalPlu if it is on an active item.'
	-------- Only use the following if this script is executed shortly after the insert Global Plu script Step1.Step1...
	-------- Use the GlobalPluAlignmentHistory to determine the Item_Key and Identifier_ID of the Global PLU
	------UPDATE #GlobalPluMap
	------SET [GlobalPlu_Item_Key] = gpah.[Item_Key],
	------	[GlobalPlu_Identifier_ID] = gpah.[Identifier_ID]
	------FROM #GlobalPluMap gpm
	------INNER JOIN (select GlobalPluAlignmentId, max(Item_Key) as Item_Key, Identifier_ID
	------			from [dbo].[GlobalPluAlignmentHistory]  
	------			where [Action] like 'Global PLU Inserted%'
	------			group by GlobalPluAlignmentId, Identifier_ID) as gpah ON gpm.GlobalPluAlignmentId = gpah.GlobalPluAlignmentId 
	------INNER JOIN [dbo].[ItemIdentifier] ii ON gpah.[Identifier_ID] = ii.[Identifier_ID]
	------									AND gpah.[Item_Key] = ii.[Item_Key]
	------									AND gpm.[GlobalPlu] = ii.[Identifier]
	------INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	------WHERE i.[Deleted_Item] = 0
	------AND i.[Remove_Item] = 0
	------AND ii.[Deleted_Identifier] = 0
	------AND ii.[Remove_Identifier] = 0
	------AND ii.[Default_Identifier] = 0

	PRINT 'Get Item Information for the GlobalPlu if it is on an active item.'
	-- Only use the following if this script is executed some time after the insert Global Plu script Step1.Step1...
	-- For example the next day or next week. Global PLUs might have switched items so just get the most current
	-- item key and identifier_id
	UPDATE #GlobalPluMap
	SET [GlobalPlu_Item_Key] = i.[Item_Key],
		[GlobalPlu_Identifier_ID] = ii.[Identifier_ID]
	FROM #GlobalPluMap gpm
	INNER JOIN [dbo].[ItemIdentifier] ii ON ii.[Identifier] = gpm.[GlobalPlu]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	WHERE i.[Deleted_Item] = 0
	AND i.[Remove_Item] = 0
	AND ii.[Deleted_Identifier] = 0
	AND ii.[Remove_Identifier] = 0
	AND ii.[Default_Identifier] = 0

	UPDATE #GlobalPluMap
	SET [RegionalPlu_Item_Key] = i.[Item_Key],
		[RegionalPlu_Identifier_ID] = ii.[Identifier_ID]
	FROM #GlobalPluMap gpm
	INNER JOIN [dbo].[ItemIdentifier] ii ON ii.[Identifier] = gpm.[RegionalPlu]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	WHERE i.[Deleted_Item] = 0
	AND i.[Remove_Item] = 0
	AND ii.[Deleted_Identifier] = 0
	AND ii.[Remove_Identifier] = 0
	
	----------------------------------------------------------------------------------------------------
	PRINT '-- Get the Data set for the Global PLUs to Update'
	SET @CodeLocation = '-- Global PLUs to Update : Get Data Set'
	SELECT @CodeLocation 
	INSERT INTO #globalPlusToUpdate 
	(
		[GlobalPluAlignmentId], 
		[Item_Key], 
		[Identifier], 
		[Identifier_ID],
		[Scale_Identifier],
		[National_Identifier],
		[NumPluDigitsSentToScale]
	)
	SELECT DISTINCT
		gpa.[GlobalPluAlignmentId],
		gpa.[RegionalPlu_Item_Key],
		gpa.[GlobalPlu],
		ii.[Identifier_ID],
		ii.[Scale_Identifier],
		ii.[National_Identifier],
		ii.[NumPluDigitsSentToScale]
	FROM #GlobalPluMap gpa
	INNER JOIN [dbo].[Item] i ON gpa.[GlobalPlu_Item_Key] = i.[Item_Key]
	INNER JOIN [dbo].[ItemIdentifier] ii ON gpa.[GlobalPlu_Identifier_ID] = ii.[Identifier_ID]
										 AND ii.[Default_Identifier] = 0
	LEFT JOIN (SELECT [item_Key], 
					  MAX([itemscale_id]) AS [itemscale_id]
			   FROM [dbo].[ItemScale]
			   GROUP BY [item_Key]) AS si ON i.[item_key] = si.[item_key]
	WHERE gpa.[RegionalPlu_Item_Key] IS NOT NULL
	AND gpa.[GlobalPlu_Item_Key] IS NOT NULL
	AND gpa.[RegionalPlu_Item_Key] = gpa.[GlobalPlu_Item_Key]

	PRINT '---------------------------------------------------------------------------------------'
	PRINT '-- Begin Transaction to Update the Global PLUs...'
	PRINT '---------------------------------------------------------------------------------------'
	BEGIN TRANSACTION
	WHILE EXISTS (SELECT 1 FROM #globalPlusToUpdate WHERE [Update] = 0)
	BEGIN
		SELECT TOP 1
			@GlobalPluAlignmentId = [GlobalPluAlignmentId],
			@Item_Key = [Item_Key],
			@Identifier = [Identifier],
			@Identifier_ID = [Identifier_ID],
			@National_Identifier = [National_Identifier],
			@NumPluDigitsSentToScale = [NumPluDigitsSentToScale],
			@Scale_Identifier = [Scale_Identifier]
		FROM #globalPlusToUpdate 
		WHERE [Update] = 0
	
		---- Update the Global PLU to be the default identifier
		IF (@Identifier_ID > 0)
		BEGIN
			SET @CodeLocation = 'EXEC [dbo].[UpdateItemIdentifier] @Item_Key = ' + ISNULL(CONVERT(nvarchar(255), @Item_Key), 'NULL')  + ', '
										+ '@Identifier_ID = ' + ISNULL(CONVERT(nvarchar(255), @Identifier_ID), 'NULL') + ', ' 
										+ '@DefaultID  = 1, '
										+ '@NatID  = ' + ISNULL(CONVERT(varchar(4), @National_Identifier), 'NULL') + ', '
										+ '@NumPluDigitsSentToScale = ' + ISNULL(CONVERT(varchar(64), @NumPluDigitsSentToScale), 'NULL') + ', '
										+ '@Scale_Identifier = ' + ISNULL(CONVERT(CHAR(1), @Scale_Identifier), 'NULL') + ';'

			EXEC [dbo].[UpdateItemIdentifier] @Item_Key = @Item_Key,
											  @Identifier_ID = @Identifier_ID, 
											  @DefaultID  = 1, 
											  @NatID = @National_Identifier, 
											  @NumPluDigitsSentToScale = @NumPluDigitsSentToScale, 
											  @Scale_Identifier = @Scale_Identifier;

			-- Record when the Global PLU updated to be the default identifier
			INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
			VALUES (@GlobalPluAlignmentId, @Item_Key, @Identifier_ID, 'Global PLU Updated : ' + ISNULL(@CodeLocation,'@CodeLocation = NULL') )

			-- Get Information for the GlobalPlu if it's on an active item
			UPDATE #GlobalPluMap
			SET [GlobalPlu_Identifier_ID] = @Identifier_ID
			FROM #GlobalPluMap gpm
			WHERE [GlobalPluAlignmentId] = @GlobalPluAlignmentId

		END
		ELSE
		BEGIN
			SET @CodeLocation = 'Global PLU not found on item_Key : '
			-- Record when the Global PLU not found
			INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
			VALUES (@GlobalPluAlignmentId, @Item_Key, @Identifier_ID, 'Whoops! Global PLU ' + ISNULL(@Identifier,'NULL') + ' not found on item_key ' + convert(nvarchar(255), @Item_Key))
		END

		UPDATE #globalPlusToUpdate
		SET [update] = 1
		WHERE [GlobalPluAlignmentId] = @GlobalPluAlignmentId
	END

	SET @CodeLocation = 'GlobalPluUpdate - Commit Transaction'
	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION

END TRY
BEGIN CATCH
	        
		IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION

		PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
				+ 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10)
				+ CHAR(9) + ' at statement  ''' + ISNULL(@CodeLocation,'@CodeLocation = NULL')  + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') + 'line ' + CONVERT(varchar, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10)
				+ REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
				+ 'GlobalPluUpdate database changes were rolled back.' + CHAR(13) + CHAR(10)
				+ REPLACE(SPACE(120), SPACE(1), '-')

	SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_STATE() AS ErrorState,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage
END CATCH

	SELECT  'RESULTS - Global PLUS Updated'
	SELECT
	    ROW_NUMBER() OVER (ORDER BY st.[Subteam_Name], gpi.[Identifier]) AS 'Row Number',
		gpa.[GlobalPluAlignmentId],
		gpa.[Region],
		gpa.[RegionalPlu],
		gpa.[GlobalPlu],
		st.[Subteam_No],
		st.[Subteam_Name],
		[Subteam_Type] = CASE WHEN db.[Type] = NULL THEN '' ELSE db.[type] END,
		gpi.[Item_Key], 
		gpi.[Identifier], 
		ii.[Identifier_ID],
		i.[Item_Description],
		ii.[Default_Identifier],
		ii.[National_Identifier], 
		ii.[NumPluDigitsSentToScale],
		ii.[Scale_Identifier]
	FROM #globalPlusToUpdate gpi
	INNER JOIN [dbo].[GlobalPluAlignment] gpa ON gpi.[GlobalPluAlignmentId] = gpa.[GlobalPluAlignmentId]
	LEFT JOIN [dbo].[ItemIdentifier] ii ON gpi.[Item_Key] = ii.[Item_Key]
											AND gpi.[Identifier] = ii.[Identifier]
	LEFT JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	LEFT JOIN [dbo].[SubTeam] st ON i.[Subteam_No] = st.[Subteam_No]
	LEFT JOIN [dbo].[GlobalPluAlignment_DeliBakerySubteams] db ON db.[Subteam_no] = st.[Subteam_No]
	ORDER BY gpa.[GlobalPluAlignmentId]

	SELECT 'DATA - [dbo].[GlobalPluAlignment] & [dbo].[GlobalPluAlignmentHistory]'
	SELECT
		gpa.[GlobalPluAlignmentId],
		gpa.[Region],
		st.[Subteam_No],
		st.[Subteam_Name],
		gpa.[RegionalPlu],
		gpa.[GlobalPlu],
		gpah.[GlobalPluAlignmentHistoryId],
		gpah.[Item_Key],
		gpah.[Identifier_ID],
		gpah.[Scale_ExtraText_ID],
		gpah.[Action]
	FROM [dbo].[GlobalPluAlignment] gpa
	LEFT JOIN [dbo].[GlobalPluAlignmentHistory] gpah ON gpa.[GlobalPluAlignmentId] = gpah.[GlobalPluAlignmentId]
	LEFT JOIN [dbo].[Item] i ON gpah.[Item_Key] = i.[Item_Key]
	LEFT JOIN [dbo].[SubTeam] st ON i.[Subteam_No] = st.[Subteam_No]
	WHERE gpah.GlobalPluAlignmentHistoryId > @GlobalPluAlignmentHistoryId
	ORDER BY 
		gpa.[GlobalPluAlignmentId],
		gpa.[Region],
		st.[Subteam_No],
		st.[Subteam_Name],
		gpa.[RegionalPlu],
		gpa.[GlobalPlu]

	SELECT 'DATA - [dbo].[GlobalPluAlignment_DeliBakerySubteams]'
	SELECT *
	FROM [dbo].GlobalPluAlignment_DeliBakerySubteams

	-- Clean up the temp tables
	IF OBJECT_ID('tempdb..#GlobalPluMap') IS NOT NULL
		DROP TABLE #GlobalPluMap

	IF OBJECT_ID('tempdb..#globalPlusToUpdate') IS NOT NULL
		DROP TABLE #globalPlusToUpdate 
END
ELSE
BEGIN
   SELECT 'Region ' + (SELECT regionCode FROM Region) + ' not scheduled to get Global PLU Alignment Script 1 Step 2 Update Global PLUs'
END

GO

SET NOCOUNT OFF
GO