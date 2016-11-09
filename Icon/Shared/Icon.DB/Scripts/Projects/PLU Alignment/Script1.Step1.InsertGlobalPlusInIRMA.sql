/*
 * Title: Script 1 - Step 1 - Global PLU Mapping - Insert Global PLUs in IRMA
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
PRINT 'Current System Time: ' + CONVERT(VARCHAR, @runTime, 121)
PRINT 'User Name: ' + @runUser
PRINT 'Running From Host: ' + @runHost
PRINT 'Connected To DB Server: ' + @@SERVERNAME
PRINT 'DB Name: ' + @runDB
PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Begin Global PLU Alignment Maintenance Script 1 Step 1'
PRINT '---------------------------------------------------------------------------------------'

IF (EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN ('FL','MA','NA','EU','RM','NE')))
BEGIN

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
	
	IF OBJECT_ID('tempdb..#GlobalPluAlreadyExists') IS NOT NULL
		DROP TABLE #GlobalPluAlreadyExists
	CREATE TABLE #GlobalPluAlreadyExists
	(
		[GlobalPluAlignmentId] INT,
		[Region] CHAR(2) NULL,
		[SubTeam_No] int NULL,
		[SubTeam_Name] varchar(100) NULL,
		[Item_Key] int NULL,
		[Identifier_ID] int NULL,
		[Identifier] varchar(13) NULL,
		[Item_Description] varchar(60) NULL,
		[Default_Identifier] tinyint NULL,
		[Deleted_Identifier] tinyint NULL,
		[Add_Identifier] tinyint NULL,
		[Remove_Identifier] tinyint NULL,
		[National_Identifier] tinyint NULL,
		[CheckDigit] char(1) NULL,
		[IdentifierType] char(1) NULL,
		[NumPluDigitsSentToScale] int NULL,
		[Scale_Identifier] bit NULL,
		[Timestamp] DATETIME DEFAULT CURRENT_TIMESTAMP
	)
	
	IF OBJECT_ID('tempdb..#ItemNotFound') IS NOT NULL
		DROP TABLE #ItemNotFound
	CREATE TABLE #ItemNotFound
	(
		[GlobalPluAlignmentId] INT,
		[Region] CHAR(2) NULL,
		[RegionalPlu] varchar(13) NULL,
		[GlobalPlu] varchar(13) NULL,
		[Timestamp] DATETIME DEFAULT CURRENT_TIMESTAMP
	)
	
	IF OBJECT_ID('tempdb..#ItemMarkedDeleted') IS NOT NULL
		DROP TABLE #ItemMarkedDeleted
	CREATE TABLE #ItemMarkedDeleted
	(
		[GlobalPluAlignmentId] INT,
		[Region] CHAR(2) NULL,
		[SubTeam_No] int NULL,
		[SubTeam_Name] varchar(100) NULL,
		[Item_Key] int NULL,
		[Identifier_ID] int NULL,
		[Identifier] varchar(13) NULL,
		[Item_Description] varchar(60) NULL,
		[Timestamp] DATETIME DEFAULT CURRENT_TIMESTAMP
	)

	IF OBJECT_ID('tempdb..#globalPlusToInsert') IS NOT NULL
		DROP TABLE #globalPlusToInsert
	CREATE TABLE #globalPlusToInsert
	(
		[GlobalPluAlignmentId] INT,
		[Item_Key] INT,
		[Identifier] NVARCHAR(13),
		[Scale_Identifier] BIT DEFAULT 0,
		[insert] BIT DEFAULT 0
	)

	PRINT '---------------------------------------------------------------------------------------'
	PRINT '-----   Global PLUs Insert'
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

	PRINT 'Get Item Information for the RegionalPlu if it is on an active item'
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

	PRINT 'Get Item Information for the GlobalPlu if it is on an active item'
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

	PRINT '---------------------------------------------------------------------------------------'
	PRINT '-- Analyze the Regional and Global Plus for current state and report out'
	PRINT '---------------------------------------------------------------------------------------'
	SET @CodeLocation = '-- Global PLUs to Insert : Analyze the Regional and Global Plus for current state '

	PRINT '---------------------------------------------------------------------------------------'
	PRINT '-- E. If Global PLU already exists, take no action'
	PRINT '--    a. Report these out in Excel (Identifier, Description, POS Subteam)'
	PRINT '---------------------------------------------------------------------------------------'
	INSERT INTO #GlobalPluAlreadyExists
	(
		[GlobalPluAlignmentId],
		[Region],
		[SubTeam_No],
		[SubTeam_Name],
		[Item_Key],
		[Identifier_ID],
		[Identifier],
		[Item_Description],
		[Default_Identifier],
		[Deleted_Identifier],
		[Add_Identifier],
		[Remove_Identifier],
		[National_Identifier],
		[CheckDigit],
		[IdentifierType],
		[NumPluDigitsSentToScale],
		[Scale_Identifier]
	)
	OUTPUT INSERTED.[GlobalPluAlignmentId], INSERTED.[Item_Key], INSERTED.[Identifier_ID], 'Global PLU already exists, take no action'
		INTO [dbo].[GlobalPluAlignmentHistory] ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
	SELECT
		gpm.[GlobalPluAlignmentId],
		gpm.[Region],
		st.[SubTeam_No],
		st.[SubTeam_Name],
		gpm.[GlobalPlu_Item_Key],
		gpm.[GlobalPlu_Identifier_ID],
		gpm.[GlobalPlu],
		i.[Item_Description],
		ii.[Default_Identifier],
		ii.[Deleted_Identifier],
		ii.[Add_Identifier],
		ii.[Remove_Identifier],
		ii.[National_Identifier],
		ii.[CheckDigit],
		ii.[IdentifierType],
		ii.[NumPluDigitsSentToScale],
		ii.[Scale_Identifier]
	FROM #GlobalPluMap gpm
	INNER JOIN [dbo].[ItemIdentifier] ii ON ii.[Identifier_ID] = gpm.[GlobalPlu_Identifier_ID]
										AND ii.[Item_Key] = gpm.[GlobalPlu_Item_Key]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	INNER JOIN [dbo].[Subteam] st ON i.[SubTeam_No] = st.[SubTeam_No]
	WHERE gpm.[GlobalPlu_Item_Key] IS NOT NULL
	AND gpm.[GlobalPlu_Identifier_ID] IS NOT NULL

	PRINT '---------------------------------------------------------------------------------------'
	PRINT '-- G. If item is not found in database, take no action'
	PRINT '--    a. Report these out in Excel (Identifier only)'
	PRINT '---------------------------------------------------------------------------------------'
	INSERT #ItemNotFound
	(
		[GlobalPluAlignmentId],
		[Region],
		[RegionalPlu],
		[GlobalPlu]
	)
	OUTPUT INSERTED.[GlobalPluAlignmentId], 'Regional PLU is not found on any item in the database, take no action.'
		INTO [dbo].[GlobalPluAlignmentHistory] ([GlobalPluAlignmentId], [Action])
	SELECT
		gpm.[GlobalPluAlignmentId],
		gpm.[Region],
		gpm.[RegionalPlu],
		gpm.[GlobalPlu]
	FROM #GlobalPluMap gpm
	WHERE gpm.[GlobalPlu_Item_Key] IS NULL
	AND gpm.[RegionalPlu_Item_Key] IS NULL
	AND NOT EXISTS (SELECT 1 FROM [dbo].[ItemIdentifier] ii WHERE ii.[Identifier] = gpm.[RegionalPlu])

	PRINT '---------------------------------------------------------------------------------------'
	PRINT '-- F. If item is marked for deletion, take no action'
	PRINT '--    a. Report these out in Excel (Identifier, Description, POS Subteam)'
	PRINT '---------------------------------------------------------------------------------------'
	INSERT INTO #ItemMarkedDeleted
	(
		[GlobalPluAlignmentId],
		[Region],
		[SubTeam_No],
		[SubTeam_Name],
		[Item_Key],
		[Identifier_ID],
		[Identifier],
		[Item_Description]
	)
	OUTPUT INSERTED.[GlobalPluAlignmentId], INSERTED.[Item_Key], INSERTED.[Identifier_ID], 'Regional PLU is on item marked for deletion, take no action'
		INTO [dbo].[GlobalPluAlignmentHistory] ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
	SELECT 
		data.[GlobalPluAlignmentId],
		data.[Region],
		st.[SubTeam_No],
		st.[SubTeam_Name],
		i.[Item_Key],
		ii.[Identifier_ID],
		ii.[Identifier],
		i.[Item_Description]
	FROM
	(
		SELECT 
			gpm.[GlobalPluAlignmentId],
			gpm.[Region],
			ii1.[Identifier_ID],
			MAX(ii1.[Item_Key]) AS [Item_Key]
		FROM #GlobalPluMap gpm
		INNER JOIN [dbo].[ItemIdentifier] ii1 ON ii1.[Identifier] = gpm.[RegionalPlu]
		WHERE gpm.[GlobalPlu_Item_Key] IS NULL
		AND gpm.[RegionalPlu_Item_Key] IS NULL
		GROUP BY
			gpm.[GlobalPluAlignmentId],
			gpm.[Region],
			ii1.[Identifier_ID]
	) as data
	INNER JOIN [dbo].[ItemIdentifier] ii ON ii.[Identifier_ID] = data.[Identifier_ID]	
										AND ii.[Item_Key] = data.[Item_Key]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	INNER JOIN [dbo].[Subteam] st ON i.[SubTeam_No] = st.[SubTeam_No]
	WHERE i.[Item_Key] IS NOT NULL
	-- Regional Plu exists, but is on a deleted item
	AND NOT EXISTS (SELECT 1 FROM #ItemNotFound inf WHERE inf.[GlobalPluAlignmentId] = data.[GlobalPluAlignmentId])

	----------------------------------------------------------------------------------------------------
	PRINT '-- Get the Data set for the Global PLUs to Insert'
	SET @CodeLocation = '-- Global PLUs to Insert : Get Data Set'
	SELECT @CodeLocation 
	INSERT INTO #globalPlusToInsert 
	(
		[GlobalPluAlignmentId], 
		[Item_Key], 
		[Identifier], 
		[Scale_Identifier]
	)
	SELECT DISTINCT
		gpa.[GlobalPluAlignmentId],
		gpa.[RegionalPlu_Item_Key],
		gpa.[GlobalPlu],
		-- [GlobalPlu_SendToScale] : query to determine send-to-scale. deli and bakery teams should be set to false.
		CASE WHEN si.[ItemScale_Id] IS NULL
		     THEN 0
			 ELSE CASE WHEN EXISTS (SELECT dbs.[Subteam_No]
									FROM [dbo].[GlobalPluAlignment_DeliBakerySubteams] dbs 
									WHERE dbs.[Subteam_No] = i.[SubTeam_No]) 
					   THEN 0 ELSE 1 END
			 END
	FROM #GlobalPluMap gpa
	INNER JOIN [dbo].[Item] i ON gpa.[RegionalPlu_Item_Key] = i.[Item_Key]
	LEFT JOIN (SELECT [item_Key], 
					  MAX([itemscale_id]) AS [itemscale_id]
			   FROM [dbo].[ItemScale]
			   GROUP BY [item_Key]) AS si ON i.[item_key] = si.[item_key]
	WHERE gpa.[RegionalPlu_Item_Key] IS NOT NULL
	AND gpa.[GlobalPlu_Item_Key] IS NULL

	PRINT '---------------------------------------------------------------------------------------'
	PRINT '-- Begin Transaction to Insert the Global PLUs...'
	PRINT '---------------------------------------------------------------------------------------'
	BEGIN TRANSACTION
	WHILE EXISTS (SELECT 1 FROM #globalPlusToInsert WHERE [insert] = 0)
	BEGIN
		SELECT TOP 1
			@GlobalPluAlignmentId = [GlobalPluAlignmentId],
			@Item_Key = [Item_Key],
			@Identifier = [Identifier],
			@Scale_Identifier = [Scale_Identifier]
		FROM #globalPlusToInsert 
		WHERE [insert] = 0
		
------------------------------------------------------------------------------------------------
-- Checks on the Item_Key and Identifier and Scale_Identifier to make sure about status.
------------------------------------------------------------------------------------------------

		---- Insert the Global PLU and set it to be the default identifier
		IF NOT EXISTS (SELECT 1 FROM [dbo].[ItemIdentifier] WHERE Item_Key = @Item_Key AND Identifier = @Identifier)
		BEGIN
			SET @CodeLocation = 'EXEC [dbo].[InsertItemIdentifier] @Item_Key = ' + ISNULL(CONVERT(NVARCHAR(255), @Item_Key), 'NULL') + ', ' 
																			     + '@IdentifierType = ''O'', ' 
																			     + '@Identifier = ''' + ISNULL(@Identifier,'NULL')  + ''', '
																			     + '@CheckDigit = NULL, '
																			     + '@National_Identifier = 1, '
																			     + '@NumPluDigitsSentToScale = 5,  '
																			     + '@Scale_Identifier = ' + ISNULL(CONVERT(CHAR(1), @Scale_Identifier),'NULL') + ';'

			EXEC [dbo].[InsertItemIdentifier] @Item_Key = @Item_Key, 
											  @IdentifierType = 'O',
											  @Identifier = @Identifier,
											  @CheckDigit = NULL,
											  @National_Identifier = 1,
											  @NumPluDigitsSentToScale = 5,
											  @Scale_Identifier = @Scale_Identifier;

			SELECT @Identifier_ID = (SELECT Identifier_ID FROM dbo.ItemIdentifier WHERE Item_Key = @Item_Key AND Identifier = @Identifier)

			---- Record when the Global PLU was inserted
			INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
			VALUES (@GlobalPluAlignmentId, @Item_Key, @Identifier_ID, 'Global PLU Inserted : ' + ISNULL(@CodeLocation,'@CodeLocation = NULL'))

			-- Get Information for the GlobalPlu if it's on an active item
			UPDATE #GlobalPluMap
			SET [GlobalPlu_Item_Key] = @Item_Key
			FROM #GlobalPluMap gpm
			WHERE [GlobalPluAlignmentId] = @GlobalPluAlignmentId

		END
		ELSE
		BEGIN
			SET @CodeLocation = 'Global PLU already exists on item_Key'
			-- Record if the Global PLU Global PLU already exists on item_Key
			INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
			VALUES (@GlobalPluAlignmentId, @Item_Key, @Identifier_ID, 'Whoops! Global PLU ' + ISNULL(@Identifier,'NULL') + ' already exists on item_key.')
		END

		UPDATE #globalPlusToInsert
		SET [insert] = 1
		WHERE [GlobalPluAlignmentId] = @GlobalPluAlignmentId
	END

	PRINT '---------------------------------------------------------------------------------------'
	PRINT '-- Commit Transaction to Insert the Global PLUs...'
	PRINT '---------------------------------------------------------------------------------------'
	SET @CodeLocation = '-- GlobalPluInsert - Commit Transaction'
	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION

END TRY
BEGIN CATCH
	        
		IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION

		PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
				+ 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10)
				+ CHAR(9) + ' at statement  ''' + ISNULL(@CodeLocation,'@CodeLocation = NULL') + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') + 'line ' + CONVERT(varchar, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10)
				+ REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
				+ 'GlobalPluInsert database changes were rolled back.' + CHAR(13) + CHAR(10)
				+ REPLACE(SPACE(120), SPACE(1), '-')

	SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_STATE() AS ErrorState,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage
END CATCH

	SELECT '-- RESULTS - Global Plu Already Exists'
	SELECT 
	ROW_NUMBER() OVER (ORDER BY gpa.[GlobalPluAlignmentId]) as 'Row Number',
	gpa.[GlobalPluAlignmentId],
	gpa.[Region],
	gpa.[RegionalPlu],
	gpa.[GlobalPlu],
	i.[GlobalPluAlignmentId],
	i.[Region],
	i.[SubTeam_No],
	i.[SubTeam_Name],
	i.[Item_Key],
	i.[Identifier_ID],
	i.[Identifier],
	i.[Item_Description],
	i.[Default_Identifier],
	i.[Deleted_Identifier],
	i.[Add_Identifier],
	i.[Remove_Identifier],
	i.[National_Identifier],
	i.[CheckDigit],
	i.[IdentifierType],
	i.[NumPluDigitsSentToScale],
	i.[Scale_Identifier],
	i.[Timestamp] 
	FROM #GlobalPluAlreadyExists i
	INNER JOIN [dbo].[GlobalPluAlignment] gpa ON i.[GlobalPluAlignmentId] = gpa.[GlobalPluAlignmentId]
	ORDER BY gpa.[GlobalPluAlignmentId]

	SELECT  '-- RESULTS - Regional PLU NOT Found'
	SELECT 
	ROW_NUMBER() OVER (ORDER BY i.[GlobalPluAlignmentId]) as 'Row Number',
	i.[GlobalPluAlignmentId],
	i.[Region],
	i.[RegionalPlu],
	i.[GlobalPlu],
	i.[Timestamp]
	FROM #ItemNotFound i
	ORDER BY i.[GlobalPluAlignmentId]

	SELECT  'RESULTS - Regional PLU on Item Marked Deleted'
	SELECT 
	ROW_NUMBER() OVER (ORDER BY gpa.[GlobalPluAlignmentId]) as 'Row Number',
	gpa.[GlobalPluAlignmentId],
	gpa.[Region],
	gpa.[RegionalPlu],
	gpa.[GlobalPlu],
	i.[GlobalPluAlignmentId],
	i.[Region],
	i.[SubTeam_No],
	i.[SubTeam_Name],
	i.[Item_Key],
	i.[Identifier_ID],
	i.[Identifier],
	i.[Item_Description],
	i.[Timestamp]
	FROM #ItemMarkedDeleted i
	INNER JOIN [dbo].[GlobalPluAlignment] gpa ON i.[GlobalPluAlignmentId] = gpa.[GlobalPluAlignmentId]
	ORDER BY gpa.[GlobalPluAlignmentId]

	SELECT  'RESULTS - Global PLUS Inserted'
	SELECT
	    ROW_NUMBER() OVER (ORDER BY st.[Subteam_Name], gpi.[Identifier]) AS 'Row Number',
		gpa.[GlobalPluAlignmentId],
		gpa.[Region],
		gpa.[RegionalPlu],
		gpa.[GlobalPlu],
		st.[Subteam_No],
		st.[Subteam_Name],
		gpi.[Item_Key], 
		gpi.[Identifier], 
		ii.[Identifier_ID],
		i.[Item_Description],
		ii.[Default_Identifier],
		ii.[National_Identifier], 
		ii.[NumPluDigitsSentToScale],
		ii.[Scale_Identifier],
		gpi.[GlobalPluAlignmentId]
	FROM #globalPlusToInsert gpi
	INNER JOIN [dbo].[GlobalPluAlignment] gpa ON gpi.[GlobalPluAlignmentId] = gpa.[GlobalPluAlignmentId]
	LEFT JOIN [dbo].[ItemIdentifier] ii ON gpi.[Item_Key] = ii.[Item_Key]
											AND gpi.[Identifier] = ii.[Identifier]
	LEFT JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	LEFT JOIN [dbo].[SubTeam] st ON i.[Subteam_No] = st.[Subteam_No]
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

	IF OBJECT_ID('tempdb..#GlobalPluAlreadyExists') IS NOT NULL
		DROP TABLE #GlobalPluAlreadyExists

	IF OBJECT_ID('tempdb..#ItemNotFound') IS NOT NULL
		DROP TABLE #ItemNotFound

	IF OBJECT_ID('tempdb..#ItemMarkedDeleted') IS NOT NULL
		DROP TABLE #ItemMarkedDeleted

	IF OBJECT_ID('tempdb..#globalPlusToInsert') IS NOT NULL
		DROP TABLE #globalPlusToInsert
END
ELSE
BEGIN
   SELECT 'Region ' + (SELECT regionCode FROM Region) + ' not scheduled to get Global PLU Alignment Script 1 Step 1 Insert Global PLUs'
END

GO

SET NOCOUNT OFF
GO