/*
 * Title: Global PLU Mapping - Update Global PLUs For Deli/Bakery to send to scale in IRMA
 * Author: Benjamin Loving
 * Date: 7/11/2014
 * Description: This script is for the update of the Deli/Bakery Global PLUs based on the PLU Mapping in IRMA.
				Set them to send to scale = true (scale_identifier = 1)
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
PRINT'-- Global PLUs to Update - Set Global Plu is to be default identifier '
PRINT'--						   and set send to scale = TRUE for deli/bakery items'
PRINT'----------------------------------------------------------------------'

DECLARE @GlobalPluAlignmentId INT,
		@GlobalPlu_Item_Key INT,
		@GlobalPlu NVARCHAR(255),
		@GlobalPlu_Identifier_ID INT,
		@GlobalPlu_NatlID INT,
		@GlobalPlu_SendToScale BIT,
		@NumPluDigitsSentToScale INT,
		@regionCode CHAR(2),
		@CodeLocation VARCHAR(255),
		@GlobalPluAlignmentHistoryId INT,
		@SW_RunForSubteam VARCHAR(64)

IF OBJECT_ID('tempdb..#globalPlusToUpdate') IS NOT NULL
	DROP TABLE #globalPlusToUpdate
CREATE TABLE #globalPlusToUpdate
(
	[GlobalPluAlignmentId] INT,
	[processed] BIT DEFAULT 0,
	[GlobalPlu_Item_Key] INT,
	[GlobalPlu] NVARCHAR(13),
	[GlobalPlu_Identifier_ID] INT,
	[GlobalPlu_NatlID] INT,
	[GlobalPlu_SendToScale] BIT DEFAULT 0,
	[NumPluDigitsSentToScale] INT,
	[Before_DefaultID] TINYINT, 
	[Before_NatID] TINYINT, 
	[Before_NumPluDigitsSentToScale] INT, 
	[Before_Scale_Identifier] BIT,
	[Subteam_Name] NVARCHAR(255),
	[Item_Description] NVARCHAR(255),
	[Note] NVARCHAR(255)
)

-- VALID VALUES ARE 'PREP FOODS' OR 'BAKERY'
SELECT @SW_RunForSubteam = 'BAKERY'

SELECT @regionCode = (SELECT [RegionCode] FROM [dbo].[Region])

SELECT @GlobalPluAlignmentHistoryId = (SELECT MAX([GlobalPluAlignmentHistoryId]) FROM [dbo].[GlobalPluAlignmentHistory]) 

BEGIN TRY

SET @CodeLocation = '--Global PLUs to Update : Get Data Set';
SELECT @codeLocation;

	INSERT INTO #globalPlusToUpdate
	(
		[GlobalPluAlignmentId], 
		[GlobalPlu_Item_Key], 
		[GlobalPlu], 
		[GlobalPlu_Identifier_ID],
		[GlobalPlu_SendToScale],
		[NumPluDigitsSentToScale],
		[Before_DefaultID], 
		[Before_NatID], 
		[Before_NumPluDigitsSentToScale], 
		[Before_Scale_Identifier]
	)
	SELECT 
		gpa.[GlobalPluAlignmentId],
		ii.[Item_Key], -- Get current item in case the GlobalPlu switched item's recently
		gpa.[GlobalPlu],
		ii.[Identifier_ID], 
		[GlobalPlu_SendToScale] = 1,
		-- if the global plu was already in the system and had a NumPluDigitsSentToScale set to 4, don't change that
		CASE WHEN ii.[NumPluDigitsSentToScale] IS NULL THEN 5 ELSE ii.[NumPluDigitsSentToScale] END,
		[Before_DefaultID] = ii.[Default_Identifier], 
		[Before_NatID] = ii.[National_Identifier], 
		[Before_NumPluDigitsSentToScale] = ii.[NumPluDigitsSentToScale], 
		[Before_Scale_Identifier] = ii.[Scale_Identifier]
	FROM [dbo].[GlobalPluAlignment] gpa
	INNER JOIN [dbo].[ItemIdentifier] ii ON ii.[Identifier] = gpa.[GlobalPlu]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	INNER JOIN [dbo].[ItemScale] si ON i.[Item_Key] = si.[Item_Key]
	INNER JOIN [dbo].[GlobalPluAlignment_DeliBakerySubteams] dbs ON dbs.[Subteam_No] = i.[SubTeam_No]
	WHERE gpa.[GlobalPlu] IS NOT NULL
	AND ii.[Scale_Identifier] = 0
	AND i.[Deleted_Item] = 0
	AND i.[Remove_Item] = 0
	AND ii.[Deleted_Identifier] = 0
	AND ii.[Remove_Identifier] = 0
	AND ((@regionCode = 'SW' AND dbs.[Type] = @SW_RunForSubteam)
	  OR @regionCode != 'SW')

	BEGIN TRANSACTION

	WHILE EXISTS (SELECT 1 FROM #globalPlusToUpdate WHERE [processed] = 0)
	BEGIN
		-- Get a Global PLU record to update
		SELECT TOP 1
			@GlobalPluAlignmentId = [GlobalPluAlignmentId],
			@GlobalPlu_Item_Key = [GlobalPlu_Item_Key],
			@GlobalPlu = [GlobalPlu],
			@GlobalPlu_Identifier_ID = [GlobalPlu_Identifier_ID],
			@GlobalPlu_SendToScale = [GlobalPlu_SendToScale],
			@NumPluDigitsSentToScale = [NumPluDigitsSentToScale],
			@GlobalPlu_NatlID = [Before_NatID]
		FROM #globalPlusToUpdate 
		WHERE [processed] = 0

		IF EXISTS (SELECT 1 FROM [dbo].[ItemIdentifier] WHERE [Item_Key] = @GlobalPlu_Item_Key AND [Identifier_ID] = @GlobalPlu_Identifier_ID )
		BEGIN
			SET @CodeLocation = 'EXEC [dbo].[UpdateItemIdentifier] @Item_Key = ' +  ISNULL(CONVERT(NVARCHAR(255), @GlobalPlu_Item_Key), 'NULL')  + ', '
													+ '@Identifier_ID = ' +  ISNULL(CONVERT(NVARCHAR(255), @GlobalPlu_Identifier_ID), 'NULL') + ', ' 
													+ '@DefaultID  = 1, '
													+ '@NatID = ' +  ISNULL(CONVERT(NVARCHAR(255), @GlobalPlu_NatlID), 'NULL')+ ', '
													+ '@NumPluDigitsSentToScale = ' +  ISNULL(CONVERT(NVARCHAR(255), @NumPluDigitsSentToScale), 'NULL') + ', '
													+ '@Scale_Identifier = ' +  ISNULL(CONVERT(CHAR(1), @GlobalPlu_SendToScale), 'NULL') + ';'

			EXEC [dbo].[UpdateItemIdentifier] @Item_Key = @GlobalPlu_Item_Key,
											  @Identifier_ID = @GlobalPlu_Identifier_ID, 
											  @DefaultID  = 1, 
											  @NatID = @GlobalPlu_NatlID, 
											  @NumPluDigitsSentToScale = @NumPluDigitsSentToScale, 
											  @Scale_Identifier = @GlobalPlu_SendToScale;

			-- Record the update statement executed on the global plu
			UPDATE #globalPlusToUpdate
			SET [Processed] = 1, [Note] = CASE WHEN [Note] IS NOT NULL THEN [Note] + '; ' + @codeLocation ELSE @codeLocation END
			WHERE [GlobalPluAlignmentId] = @GlobalPluAlignmentId

			-- Record when the Global PLU was updated to send to scale true
			INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
			VALUES (@GlobalPluAlignmentId, @GlobalPlu_Item_Key, @GlobalPlu_Identifier_ID, 'Global PLU was updated to send to scale true : ' + ISNULL(@CodeLocation,'@CodeLocation = NULL'))	
		END
		ELSE
		BEGIN
			UPDATE #globalPlusToUpdate
			SET [Processed] = 1, [Note] = 'Global PLU not found on the item!'
			WHERE [GlobalPluAlignmentId] = @GlobalPluAlignmentId

			-- Record when the Global PLU was Global PLU not found on the item!
			INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
			VALUES (@GlobalPluAlignmentId, @GlobalPlu_Item_Key, @GlobalPlu_Identifier_ID, 'Global PLU not found on the item!')	
		END
	END

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

	SELECT 'Results of the Global PLU Update'
	SELECT
	    ROW_NUMBER() OVER (ORDER BY st.[Subteam_Name], gpu.[GlobalPlu]) AS 'Row Number',
		st.[Subteam_No],
		st.[Subteam_Name],
		gpa.[RegionalPlu],
		gpu.[GlobalPlu], 
		i.[Item_Description],
		[Before_DefaultID] = gpu.[Before_DefaultID], 
		[After_DefaultID] = ii.[Default_Identifier],
		[Before_NatID] = gpu.[Before_NatID],
		[After_NatID] = ii.[National_Identifier], 
		[Before_Scale_Identifier] = gpu.[Before_Scale_Identifier],
		[After_Scale_Identifier] = ii.[Scale_Identifier],
		[Before_NumPluDigitsSentToScale] = gpu.[Before_NumPluDigitsSentToScale], 
		[After_NumPluDigitsSentToScale] = ii.[NumPluDigitsSentToScale],
		gpu.[GlobalPluAlignmentId], 
		gpu.[GlobalPlu_Item_Key], 
		gpu.[GlobalPlu_Identifier_ID],
		gpu.[processed],
		gpu.[Note]
	FROM #globalPlusToUpdate gpu
	INNER JOIN [dbo].[GlobalPluAlignment] gpa ON gpu.[GlobalPluAlignmentId] = gpa.[GlobalPluAlignmentId]
	INNER JOIN [dbo].[ItemIdentifier] ii ON gpu.[GlobalPlu_Identifier_ID] = ii.[Identifier_ID]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	INNER JOIN [dbo].[SubTeam] st ON i.[Subteam_No] = st.[Subteam_No]

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
	
	SELECT '	DATA - [dbo].[GlobalPluAlignment_DeliBakerySubteams]'
	SELECT * FROM [dbo].GlobalPluAlignment_DeliBakerySubteams

	IF OBJECT_ID('tempdb..#globalPlusToUpdate') IS NOT NULL
		DROP TABLE #globalPlusToUpdate
END
ELSE
BEGIN
   SELECT 'Region ' + (SELECT regionCode FROM Region) + ' not scheduled to get Global PLU Alignment Script 2 Step 2 Update Global PLUs set Send to Scale = True for Bakery and Prep Foods'
END
GO

SET NOCOUNT OFF
GO