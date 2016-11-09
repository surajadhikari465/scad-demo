/*
 * Title: Global PLU Mapping - Update Regional PLUs to NOT send to scale in IRMA
 * Author: Benjamin Loving
 * Date: 7/11/2014
 * Description: This script is for the update of the Regional PLUs based on the PLU Mapping in IRMA.
				Set them to send to scale = FALSE (scale_identifier = 0)
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
PRINT '-----   Begin Global PLU Alignment Maintenance'
PRINT '---------------------------------------------------------------------------------------'

IF (EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN ('FL','MA','NA','EU','RM','NE')))
BEGIN

PRINT'----------------------------------------------------------------------'
PRINT'-- Regional PLUs to Update - Set Regional Plu scale_identifier = false'
PRINT'----------------------------------------------------------------------'
BEGIN TRY

	DECLARE @GlobalPluAlignmentId INT,
			@RegionalPlu_Item_Key INT,
			@RegionalPlu NVARCHAR(255),
			@RegionalPlu_Identifier_ID INT,
			@NumPluDigitsSentToScale INT,
			@Default_Identifier tinyint,
			@National_Identifier tinyint,
			@regionCode CHAR(2),
			@CodeLocation VARCHAR(4000),
			@GlobalPluAlignmentHistoryId INT,
			@SW_RunForSubteam VARCHAR(64)

	-- VALID VALUES ARE 'PREP FOODS' OR 'BAKERY'
	SELECT @SW_RunForSubteam = 'BAKERY'

	SELECT @regionCode = (SELECT [RegionCode] FROM [dbo].[Region])

	SELECT @GlobalPluAlignmentHistoryId = (SELECT MAX([GlobalPluAlignmentHistoryId]) FROM [dbo].[GlobalPluAlignmentHistory]) 

	SET @CodeLocation = '-- Regional PLUs to Update - Set Regional Plu scale_identifier = false - Get Data Set';
	SELECT @CodeLocation;

	IF OBJECT_ID('tempdb..#regionalPluUpdate') IS NOT NULL
		DROP TABLE #regionalPluUpdate
	CREATE TABLE #regionalPluUpdate
	(
		[GlobalPluAlignmentId] INT,
		[RegionalPlu_Item_Key] INT,
		[RegionalPlu] NVARCHAR(13),
		[Identifier_ID] INT,
		[NumPluDigitsSentToScale] INT NULL,
		[Default_Identifier] TINYINT ,
		[National_Identifier] TINYINT,
		[Processed] BIT DEFAULT 0,
		[Note] NVARCHAR(4000)
	)
	INSERT INTO #regionalPluUpdate
	(
		[GlobalPluAlignmentId],
		[RegionalPlu_Item_Key],
		[RegionalPlu],
		[Identifier_ID],
		[NumPluDigitsSentToScale],
		[Default_Identifier],
		[National_Identifier]
	)
	-- Set all regional PLUs send to scale FALSE, only if they are on the same item as the Global PLU??
	-- Check that the regional PLU is in fact NOT the Global PLU and truly a regional PLU. 
	-- Sometimes in the mapping RegionalPLU = Global. We don't want to incorrectly flag send to scale FALSE
	-- for Global PLUs.
	--	EXAMPLE:
	-- 		GlobalPluAlignmentId	Region	RegionalPlu	GlobalPlu	Timestamp
	--		3570					MW		23853900000	26903400000	2014-07-28 14:24:52.390
	--		4003					MW		23853900000	23853900000	2014-08-01 10:38:30.240
	--		4004					MW		23854000000	23854100000	2014-08-01 10:38:30.240
	--	RESULTS:
	--			Regional PLU 23854000000 is flagged as send to scale FALSE
	SELECT 
		gpa.[GlobalPluAlignmentId],
		[RegionalPlu_Item_Key] = i.[Item_Key],
		gpa.[RegionalPlu],
		ii.[Identifier_ID],
		ii.[NumPluDigitsSentToScale],
		ii.[Default_Identifier],
		ii.[National_Identifier]
	FROM [dbo].[GlobalPluAlignment] gpa
	INNER JOIN [dbo].[ItemIdentifier] ii ON ii.[Identifier] = gpa.[RegionalPlu]
										AND gpa.[RegionalPlu] != gpa.[GlobalPlu]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	INNER JOIN [dbo].[ItemIdentifier] iig ON iig.[Item_Key] = i.[Item_Key]
										AND iig.[Identifier] = gpa.[GlobalPlu]
	LEFT JOIN [dbo].[GlobalPluAlignment_DeliBakerySubteams] dbs ON dbs.[Subteam_No] = i.[SubTeam_No]
	WHERE ii.[Scale_Identifier] = 1
	AND i.[Deleted_Item] = 0
	AND i.[Remove_Item] = 0
	AND ii.[Deleted_Identifier] = 0
	AND ii.[Remove_Identifier] = 0
	AND NOT EXISTS (SELECT 1 FROM [dbo].[GlobalPluAlignment] gpa2 WHERE gpa.[RegionalPlu] = gpa2.[GlobalPlu])
	AND (
		 (@regionCode = 'SW' AND dbs.[Type] = @SW_RunForSubteam)
	  OR @regionCode NOT IN ('SW')
	  OR dbs.[Subteam_No] IS NULL)

	BEGIN TRANSACTION
	WHILE EXISTS (SELECT 1 FROM #regionalPluUpdate WHERE [Processed] = 0)
	BEGIN
		SELECT TOP 1
			@GlobalPluAlignmentId = rpu.[GlobalPluAlignmentId],
			@RegionalPlu_Item_Key = rpu.[RegionalPlu_Item_Key],
			@RegionalPlu_Identifier_ID = rpu.[Identifier_ID],
			@NumPluDigitsSentToScale = rpu.[NumPluDigitsSentToScale],
			@Default_Identifier = rpu.[Default_Identifier],
			@National_Identifier = rpu.[National_Identifier]
		FROM #regionalPluUpdate  rpu
		WHERE [Processed] = 0

		SELECT @CodeLocation = 'EXEC [dbo].[UpdateItemIdentifier] @Item_Key = ' + ISNULL(CONVERT(NVARCHAR(255), @RegionalPlu_Item_Key),'NULL') 
															 + ', @Identifier_ID = ' + ISNULL(CONVERT(NVARCHAR(255), @RegionalPlu_Identifier_ID),'NULL') 
															 + ', @DefaultID  = ' + ISNULL(CONVERT(NVARCHAR(255), @Default_Identifier),'NULL') 
															 + ', @NatID = ' + ISNULL(CONVERT(NVARCHAR(255), @National_Identifier),'NULL')
															 + ', @NumPluDigitsSentToScale = ' + ISNULL(CONVERT(NVARCHAR(255), @NumPluDigitsSentToScale),'NULL') 
															 + ', @Scale_Identifier = 0;'

		EXEC [dbo].[UpdateItemIdentifier] @Item_Key = @RegionalPlu_Item_Key,
										  @Identifier_ID = @RegionalPlu_Identifier_ID, 
										  @DefaultID  = @Default_Identifier, 
										  @NatID = @National_Identifier, 
										  @NumPluDigitsSentToScale = @NumPluDigitsSentToScale, 
										  @Scale_Identifier = 0;

		-- Record when the Regional PLU was update to send-to-scale false
		INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
		VALUES (@GlobalPluAlignmentId, @RegionalPlu_Item_Key, @RegionalPlu_Identifier_ID, 'Regional PLU was updated to send to scale false : ' + ISNULL(@CodeLocation,'@CodeLocation = NULL'))	

		-- Update Processed column
		UPDATE #regionalPluUpdate 
		SET [Processed] = 1,
		[Note] = CASE WHEN [Note] IS NOT NULL THEN [Note] + '; ' + ISNULL(@CodeLocation,'NULL') ELSE ISNULL(@CodeLocation,'NULL') END
		WHERE [GlobalPluAlignmentId] = @GlobalPluAlignmentId
	END
	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION

END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION

	PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
			+ 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10)
			+ CHAR(9) + ' at statement  ''' + ISNULL(@CodeLocation,'NULL') + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') + 'line ' + CONVERT(varchar, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10)
			+ REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
			+ 'Database changes were rolled back.' + CHAR(13) + CHAR(10)
			+ REPLACE(SPACE(120), SPACE(1), '-')

	SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_STATE() AS ErrorState,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage
END CATCH

	PRINT 'Display the results of the Regional PLU Update'
	SELECT
		gpa.[GlobalPluAlignmentId],
		gpa.[Region],
		st.[Subteam_No],
		st.[Subteam_Name],
		gpa.[RegionalPlu],
		gpa.[GlobalPlu],
		rpu.[GlobalPluAlignmentId],
		rpu.[RegionalPlu_Item_Key],
		rpu.[RegionalPlu],
		rpu.[Identifier_ID],
		rpu.[NumPluDigitsSentToScale],
		rpu.[Default_Identifier],
		rpu.[National_Identifier],
		rpu.[Processed],
		rpu.[Note] 
	FROM #regionalPluUpdate rpu
	INNER JOIN [dbo].[GlobalPluAlignment] gpa ON rpu.[GlobalPluAlignmentId] = gpa.[GlobalPluAlignmentId]
	INNER JOIN [dbo].[ItemIdentifier] ii ON rpu.RegionalPlu_Item_Key = ii.Item_Key
										AND rpu.RegionalPlu = ii.Identifier
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
	WHERE gpah.GlobalPluAlignmentHistoryId > @GlobalPluAlignmentHistoryId
	ORDER BY 
		gpa.[GlobalPluAlignmentId],
		gpa.[Region],
		st.[Subteam_No],
		st.[Subteam_Name],
		gpa.[RegionalPlu],
		gpa.[GlobalPlu]

	IF OBJECT_ID('tempdb..#regionalPluUpdate ') IS NOT NULL
		DROP TABLE #regionalPluUpdate 
END
ELSE
BEGIN
   SELECT 'Region ' + (SELECT regionCode FROM Region) + ' not scheduled to get Global PLU Alignment Script 2 Step 1 Update Regional PLUs set Send to Scale = False'
END
GO

SET NOCOUNT OFF
GO