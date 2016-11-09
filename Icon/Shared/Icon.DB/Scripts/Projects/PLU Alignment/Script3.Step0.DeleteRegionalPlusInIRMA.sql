	/*
 * Title: Global PLU Mapping - Delete Regional Plus in IRMA
 * Author: Benjamin Loving
 * Date: 7/14/2014
 * Description: This script deletes Regional PLUs mapped to Global PLUs associated to active Items.
 * Database: iCon
 * Instructions: 1. The query output of this script must be saved to a *.sql file
 *               2. Right click the query and select  Query Options
 * 			     3. Navigate to the options for "Results > Text"
 * 			     4. Uncheck the "Include Column Headers in the result set" option.
 * 			     5. Change the "Maximum number of characters to dispaly in each column" to 8000.
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

IF (EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN ('FL','EU')))
BEGIN

PRINT'----------------------------------------------------------------------'
PRINT'-- Regional PLUs Delete'
PRINT'----------------------------------------------------------------------'

DECLARE @GlobalPluAlignmentId INT,
		@RegionalPlu_Item_Key INT,
		@RegionalPlu NVARCHAR(255),
		@RegionalPlu_Identifier_ID INT,
		@regionCode CHAR(2),
		@CodeLocation VARCHAR(4000),
		@GlobalPluAlignmentHistoryId INT

IF OBJECT_ID('tempdb..#regionalPluDelete') IS NOT NULL
	DROP TABLE #regionalPluDelete
CREATE TABLE #regionalPluDelete
(
	[GlobalPluAlignmentId] INT,
	[RegionalPlu_Item_Key] INT,
	[RegionalPlu] NVARCHAR(13),
	[Identifier_ID] INT,
	[Processed] BIT DEFAULT 0,
	[Note] NVARCHAR(4000)
)

SELECT @regionCode = (SELECT [RegionCode] FROM [dbo].[Region])
SELECT @GlobalPluAlignmentHistoryId = (SELECT MAX([GlobalPluAlignmentHistoryId]) FROM [dbo].[GlobalPluAlignmentHistory]) 

SET @CodeLocation = '-- Regional PLUs to Delete - Get Data Set';
SELECT @CodeLocation;
BEGIN TRY
	BEGIN TRANSACTION

	-- Delete all mapped regional PLUs, ONLY when they are on the same item as the Global PLU.
	-- Two scenarioes need to be considered:
	-- 1) The Regional PLU  = Global PLU --> DO NOT DELETE THESE REGIONAL PLUs
	-- 2) The Regional PLU != Global PLU --> IF THE REGIONAL PLU IS ALSO A GLOBAL PLU, THEN DO NOT DELETE IT!
	--    EXAMPLE:
	-- 		GlobalPluAlignmentId	Region	RegionalPlu	GlobalPlu	Timestamp
	--		3570					MW		23853900000	26903400000	2014-07-28 14:24:52.390
	--		4003					MW		23853900000	23853900000	2014-08-01 10:38:30.240
	-- 3) The Regional PLU != Global PLU AND Regional PLU is not listed as a Global PLU, THEN DELETE IT!
	INSERT INTO #regionalPluDelete
	(
		[GlobalPluAlignmentId],
		[RegionalPlu_Item_Key],
		[RegionalPlu],
		[Identifier_ID]
	)
	SELECT 
		gpa.[GlobalPluAlignmentId],
		i.[Item_Key],
		gpa.[RegionalPlu],
		ii.[Identifier_ID]
	FROM [dbo].[GlobalPluAlignment] gpa
	INNER JOIN [dbo].[ItemIdentifier] ii ON ii.[Identifier] = gpa.[RegionalPlu]
										 AND gpa.[RegionalPlu] != gpa.[GlobalPlu]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	WHERE EXISTS (SELECT 1
				  FROM [dbo].[ItemIdentifier] ii2 
				  WHERE ii2.[Item_Key] = ii.[Item_Key]
				  AND ii2.[Identifier] = gpa.[GlobalPlu]
				  AND ii2.[Deleted_Identifier] = 0
				  AND ii2.[Remove_Identifier] = 0
				  AND ii2.[Default_Identifier] = 1)
	AND i.[Deleted_Item] = 0
	AND i.[Remove_Item] = 0
	AND ii.[Deleted_Identifier] = 0
	AND ii.[Remove_Identifier] = 0
	AND NOT EXISTS (SELECT 1 FROM [dbo].[GlobalPluAlignment] gpa2 WHERE gpa.[RegionalPlu] = gpa2.[GlobalPlu])

	SET @CodeLocation = 'Begin to Perform Deletion'
	SELECT @CodeLocation

	WHILE EXISTS (SELECT 1 FROM #regionalPluDelete WHERE [Processed] = 0)
	BEGIN
		SELECT TOP 1
			@GlobalPluAlignmentId = rpu.[GlobalPluAlignmentId],
			@RegionalPlu_Item_Key = rpu.[RegionalPlu_Item_Key],
			@RegionalPlu_Identifier_ID = rpu.[Identifier_ID]
		FROM #regionalPluDelete  rpu
		WHERE [Processed] = 0

		SELECT @CodeLocation = 'EXEC dbo.DeleteItemIdentifier @Identifier_ID = ' + ISNULL(CONVERT(NVARCHAR(255), @RegionalPlu_Identifier_ID),'NULL') + ';'

		-- Delete the RegionalPlu
		EXEC dbo.DeleteItemIdentifier @Identifier_ID = @RegionalPlu_Identifier_ID

		-- Update Processed column
		UPDATE #regionalPluDelete 
		SET [Processed] = 1,
		[Note] = CASE WHEN [Note] IS NOT NULL THEN [Note] + '; ' + ISNULL(@CodeLocation,'@CodeLocation = NULL') ELSE ISNULL(@CodeLocation,'@CodeLocation = NULL') END
		WHERE [GlobalPluAlignmentId] = @GlobalPluAlignmentId

		-- Record when the Regional PLU was marked for deletion
		INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Identifier_ID], [Action])
		VALUES (@GlobalPluAlignmentId, @RegionalPlu_Item_Key, @RegionalPlu_Identifier_ID, 'Regional PLU was marked for deletion : ' + ISNULL(@CodeLocation,'@CodeLocation = NULL'))
	END

	SET @CodeLocation = 'End Perform Deletion @@TRANCOUNT = ' + convert(varchar(255), @@TRANCOUNT)
	SELECT @CodeLocation

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

	PRINT'----------------------------------------------------------------------'
	PRINT'-- Display the results of the Regional PLU Delete'
	PRINT'----------------------------------------------------------------------'	
	SELECT
		rpd.[GlobalPluAlignmentId],
		rpd.[RegionalPlu_Item_Key],
		rpd.[RegionalPlu],
		rpd.[Identifier_ID],
		gpa.[GlobalPlu],
		rpd.[Processed],
		rpd.[Note]
	FROM #regionalPluDelete rpd
	INNER JOIN [dbo].[GlobalPluAlignment] gpa ON rpd.[GlobalPluAlignmentId] = gpa.[GlobalPluAlignmentId]
	INNER JOIN dbo.ItemIdentifier ii ON rpd.RegionalPlu_Item_Key = ii.Item_Key
										AND rpu.RegionalPlu = ii.Identifier

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

	IF OBJECT_ID('tempdb..#regionalPluDelete') IS NOT NULL
		DROP TABLE #regionalPluDelete
END
ELSE
BEGIN
   SELECT 'Region ' + (SELECT regionCode FROM Region) + ' not scheduled to get Global PLU Alignment Script 3 Step 0 Delete Regional PLUs'
END
GO

SET NOCOUNT OFF
GO