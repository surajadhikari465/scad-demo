/*
 * Title: Global PLU Mapping - Queue Global PLUs For Deli/Bakery to send to scale in IRMA
 * Author: Benjamin Loving
 * Date: 8/19/2014
 * Description: This script will queue up change records for the global plus for deli and bakery
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

IF (EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN ('FL', 'MA','NA','EU','RM','NE')))
BEGIN

PRINT'----------------------------------------------------------------------'
PRINT'-- Global PLUs to Update - Queuing the global plus to send to scale  '
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

IF OBJECT_ID('tempdb..#globalPlusToQueue') IS NOT NULL
	DROP TABLE #globalPlusToQueue
CREATE TABLE #globalPlusToQueue
(
	[GlobalPluAlignmentId] INT,
	[queued] BIT DEFAULT 0,
	[GlobalPlu_Item_Key] INT,
	[GlobalPlu_Identifier_ID] INT,
	[GlobalPlu] NVARCHAR(13),
	[Store_No] INT,
	[Note] varchar(255)
)

-- VALID VALUES ARE 'PREP FOODS' OR 'BAKERY'
SELECT @SW_RunForSubteam = 'BAKERY'

SELECT @regionCode = (SELECT [RegionCode] FROM [dbo].[Region])

SELECT @GlobalPluAlignmentHistoryId = (SELECT MAX([GlobalPluAlignmentHistoryId]) FROM [dbo].[GlobalPluAlignmentHistory]) 

BEGIN TRY

SET @CodeLocation = 'Global PLUs to Queue : Get Data Set';
SELECT @codeLocation;

	INSERT INTO #globalPlusToQueue
	(
		[GlobalPluAlignmentId], 
		[GlobalPlu_Item_Key], 
		[GlobalPlu_Identifier_ID],
		[GlobalPlu], 
		[store_no]
	)
	SELECT 
		gpa.[GlobalPluAlignmentId],
		ii.[Item_Key], -- Get current item in case the GlobalPlu switched item's recently
		ii.[Identifier_ID],
		gpa.[GlobalPlu],
		StoreItem.[store_no]
	FROM [dbo].[GlobalPluAlignment] gpa
	INNER JOIN [dbo].[ItemIdentifier] ii ON ii.[Identifier] = gpa.[GlobalPlu]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	INNER JOIN [dbo].[ItemScale] si ON i.[Item_Key] = si.[Item_Key]
	INNER JOIN [dbo].[GlobalPluAlignment_DeliBakerySubteams] dbs ON dbs.[Subteam_No] = i.[SubTeam_No]
	INNER JOIN [dbo].[StoreItem] ON StoreItem.item_key = si.item_key 
	INNER JOIN [dbo].[Store] s ON s.store_no = StoreItem.store_no 
 	WHERE gpa.[GlobalPlu] IS NOT NULL
	AND ii.[Scale_Identifier] = 1
	AND i.[Deleted_Item] = 0
	AND i.[Remove_Item] = 0
	AND ii.[Deleted_Identifier] = 0
	AND ii.[Remove_Identifier] = 0
	AND StoreItem.Authorized = 1
	AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
	AND ((@regionCode = 'SW' AND dbs.[Type] = @SW_RunForSubteam)
	  OR (@regionCode != 'SW'))

	BEGIN TRANSACTION

	SET @CodeLocation = 'Queuing the global plus to send to scale for bakery/deli'
		
	INSERT INTO PlumCorpChgQueue (Item_Key, ActionCode, Store_No)
	SELECT g.[GlobalPlu_Item_Key], 'C', g.[Store_No]
	FROM #globalPlusToQueue g
	WHERE NOT EXISTS (SELECT 1 FROM PlumCorpChgQueue p WHERE ActionCode = 'C' AND p.Store_No =  g.Store_No AND p.Item_Key = g.[GlobalPlu_Item_Key])
	AND NOT EXISTS (SELECT 1 FROM PlumCorpChgQueueTmp pt WHERE ActionCode = 'C' AND pt.Store_No =  g.Store_No AND pt.Item_Key = g.[GlobalPlu_Item_Key])

	-- Record when the Global PLU was updated to send to scale true
	INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
	SELECT [GlobalPluAlignmentId], [GlobalPlu_Item_Key], [GlobalPlu_Identifier_ID], 'Global PLU was queued to send to scale for store_no = ' + convert(varchar(62), Store_No)
	FROM #globalPlusToQueue

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
		gdbs.[Type],
		gpa.[RegionalPlu],
		gpu.[GlobalPlu], 
		i.[Item_Description],
		gpu.[Store_No],
		gpu.[GlobalPluAlignmentId], 
		gpu.[GlobalPlu_Item_Key], 
		gpu.[GlobalPlu_Identifier_ID],
		gpu.[queued],
		gpu.[Note]
	FROM #globalPlusToQueue gpu
	INNER JOIN [dbo].[GlobalPluAlignment] gpa ON gpu.[GlobalPluAlignmentId] = gpa.[GlobalPluAlignmentId]
	INNER JOIN [dbo].[ItemIdentifier] ii ON gpu.[GlobalPlu_Identifier_ID] = ii.[Identifier_ID]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	INNER JOIN [dbo].[SubTeam] st ON i.[Subteam_No] = st.[Subteam_No]
	LEFT JOIN  [dbo].[GlobalPluAlignment_DeliBakerySubteams] gdbs ON gdbs.subteam_no = st.subteam_no

	SELECT 'DATA - [dbo].[GlobalPluAlignment] & [dbo].[GlobalPluAlignmentHistory]'
	SELECT
		gpa.[GlobalPluAlignmentId],
		gpa.[Region],
		st.[Subteam_No],
		st.[Subteam_Name],
		gdbs.[Type],
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
	LEFT JOIN [dbo].[GlobalPluAlignment_DeliBakerySubteams] gdbs ON gdbs.subteam_no = st.subteam_no
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

	IF OBJECT_ID('tempdb..#globalPlusToQueue') IS NOT NULL
		DROP TABLE #globalPlusToQueue
END
ELSE
BEGIN
   SELECT 'Region ' + (SELECT regionCode FROM Region) + ' not scheduled to get Global PLU Alignment Script 2 Step 3 Queue Global PLUs set Send to Scale = True for Bakery and Prep Foods'
END
GO

SET NOCOUNT OFF
GO