/*
 * Title: Script 1 - Step 3 - Extra Text Descriptions to Update to Global PLU in IRMA
 * Author: Benjamin Loving
 * Date: 7/24/2014
 * Description: This script is for the update of Extra Text descriptions to be the default identifier in IRMA
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

DECLARE @GlobalPluAlignmentHistoryId INT
SELECT @GlobalPluAlignmentHistoryId = (SELECT MAX(GlobalPluAlignmentHistoryId) FROM [dbo].[GlobalPluAlignmentHistory])

PRINT '----------------------------------------------------------------------'
PRINT '-- Extra Text Descriptions to Update to Global PLU'
PRINT '----------------------------------------------------------------------'
BEGIN TRY
	DECLARE @GlobalPluAlignmentId INT,
			@GlobalPlu_Item_Key INT,
			@Scale_ExtraText_ID INT,
			@Override_Scale_ExtraText_ID INT,
			@GlobalPlu NVARCHAR(255),
			@Description NVARCHAR(255),
			@Override_Description NVARCHAR(255),
			@regionCode CHAR(2),
			@CodeLocation VARCHAR(1024)

	SELECT @regionCode = (SELECT [RegionCode] FROM [dbo].[Region])

	SET @CodeLocation = '-- Extra Text Description Update - Get Data Set';
	SELECT @CodeLocation;

	IF OBJECT_ID('tempdb..#extraTextDescUpdate') IS NOT NULL
		DROP TABLE #extraTextDescUpdate
	CREATE TABLE #extraTextDescUpdate
	(
		[GlobalPluAlignmentId] INT,
		[GlobalPlu_Item_Key] INT,
		[GlobalPlu] NVARCHAR(13),
		[Scale_ExtraText_ID] INT,
		[Before_Description] NVARCHAR(255),
		[After_Description] NVARCHAR(255),
		[Processed] BIT DEFAULT 0, 
		[Override_Scale_ExtraText_ID] INT,
		[Override_StoreJurisdictionDesc] NVARCHAR(255),
		[Override_Before_Description] NVARCHAR(255),
		[Override_After_Description] NVARCHAR(255),
		[Override_Processed] BIT DEFAULT 0,
		[Note] NVARCHAR(1020)
	)
	INSERT INTO #extraTextDescUpdate 
	(
		[GlobalPluAlignmentId],
		[GlobalPlu_Item_Key],
		[GlobalPlu],
		[Scale_ExtraText_ID],
		[Before_Description],
		[After_Description],
		[Override_Scale_ExtraText_ID],
		[Override_StoreJurisdictionDesc],
		[Override_Before_Description],
		[Override_After_Description]
	)
	SELECT 
		dataset.[GlobalPluAlignmentId],
		dataset.[GlobalPlu_Item_Key],
		dataset.[GlobalPlu],
		dataset.[Scale_ExtraText_ID],
		dataset.[Before_Description],
		dataset.[After_Description],
		dataset.[Override_Scale_ExtraText_ID],
		dataset.[StoreJurisdictionDesc],
		dataset.[Override_Before_Description],
		dataset.[Override_After_Description]
	FROM
	(
		SELECT DISTINCT
			gpa.[GlobalPluAlignmentId],
			[GlobalPlu_Item_Key] = ii.[Item_Key],
			gpa.[GlobalPlu],
			[Scale_ExtraText_ID] = et.[Scale_ExtraText_ID],
			[Before_Description] = et.[Description],
			[After_Description] = gpa.[GlobalPlu],
			[Override_Scale_ExtraText_ID] = eto.[Scale_ExtraText_ID],
			[StoreJurisdictionDesc] = sj.[StoreJurisdictionDesc],
			[Override_Before_Description] = eto.[Description],
			[Override_After_Description] = CASE WHEN eto.[Description] IS NOT NULL
												THEN CASE WHEN @regionCode = 'PN' THEN gpa.[GlobalPlu] + 'C' 
														  WHEN @regionCode = 'MW' THEN gpa.[GlobalPlu] + '1' ELSE eto.[Description]  END
												ELSE eto.[Description] END
		FROM [dbo].[GlobalPluAlignment] gpa 
		INNER JOIN [dbo].[ItemIdentifier] ii ON gpa.[GlobalPlu] = ii.[Identifier]
		INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
		LEFT JOIN (SELECT [item_Key], 
						   MAX([itemscale_id]) AS [itemscale_id]
					FROM [dbo].[ItemScale]
					GROUP BY [item_Key]) AS ms ON ii.[item_key] = ms.[item_key]
		LEFT JOIN [dbo].[ItemScale] si ON ms.[itemscale_id] = si.[itemscale_id]
		LEFT JOIN [dbo].[Scale_ExtraText] et ON si.[scale_extratext_id] = et.[scale_extratext_id]
		LEFT JOIN [dbo].[ItemScaleOverride] sio ON si.[item_key] = sio.[item_key]
		LEFT JOIN [dbo].[StoreJurisdiction] sj ON sio.[StoreJurisdictionId] = sj.[StoreJurisdictionId]
		LEFT JOIN [dbo].[Scale_ExtraText] eto ON sio.[scale_extratext_id] = eto.[scale_extratext_id]
												AND sio.[scale_extratext_id] != si.[scale_extratext_id]
		WHERE ii.[Scale_Identifier] = 1
		AND i.[Deleted_Item] = 0
		AND i.[Remove_Item] = 0
		AND ii.[Deleted_Identifier] = 0
		AND ii.[Remove_Identifier] = 0

	) AS dataset
	WHERE (dataset.[Before_Description] != dataset.[After_Description])
	OR (dataset.[Override_Before_Description] != dataset.[Override_After_Description])
--> IS NULL CHECKS ON THE Before/After on the description?

BEGIN TRANSACTION
WHILE EXISTS (SELECT 1 FROM #extraTextDescUpdate WHERE [Processed] = 0)
BEGIN
	SELECT TOP 1
		@GlobalPluAlignmentId = [GlobalPluAlignmentId],
		@GlobalPlu_Item_Key = [GlobalPlu_Item_Key],
		@GlobalPlu = [GlobalPlu],
		@Scale_ExtraText_ID = [Scale_ExtraText_ID],
		@Description = [After_Description],
		@Override_Scale_ExtraText_ID = [Override_Scale_ExtraText_ID],
		@Override_Description = [Override_After_Description]
	FROM #extraTextDescUpdate 
	WHERE [Processed] = 0

	-- Update the Extra Text Description
	IF (@Scale_ExtraText_ID IS NOT NULL AND @Description IS NOT NULL)
	BEGIN
		SELECT @CodeLocation = '-- Extra Text Desc Update - Update the Description to be the Global Scancode : '
								+ '[Scale_ExtraText_ID] = ' + ISNULL(convert(varchar(64), @Scale_ExtraText_ID),'NULL')  + ', '
								+ '[Description] = ' + ISNULL(@Description,'NULL')  + ', '
								+ '[GlobalPlu] = ' + ISNULL(@GlobalPlu,'NULL') 

		-- Update the Description to be the Global PLU'
		UPDATE [dbo].[Scale_ExtraText]
		SET [Description] = @Description
		WHERE [Scale_ExtraText_ID] = @Scale_ExtraText_ID

		-- Record when the scale extra text description was updated
		INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Scale_ExtraText_ID], [Action])
		VALUES (@GlobalPluAlignmentId, @GlobalPlu_Item_Key, @Scale_ExtraText_ID, 'Scale Extra Text description updated : ' + ISNULL(@CodeLocation,'@CodeLocation = NULL'))
	END

	UPDATE #extraTextDescUpdate 
	SET [Processed] = 1,
		[Note] = CASE WHEN [Note] IS NOT NULL THEN [Note] + '; ' + @CodeLocation ELSE @CodeLocation END
	WHERE [GlobalPluAlignmentId] = @GlobalPluAlignmentId
END

WHILE EXISTS (SELECT 1 FROM #extraTextDescUpdate WHERE [Override_Processed] = 0 and [Override_Scale_ExtraText_ID] is not null and [Override_After_Description] is not null)
BEGIN
	SELECT TOP 1
		@GlobalPluAlignmentId = [GlobalPluAlignmentId],
		@GlobalPlu_Item_Key = [GlobalPlu_Item_Key],
		@GlobalPlu = [GlobalPlu],
		@Scale_ExtraText_ID = [Scale_ExtraText_ID],
		@Description = [After_Description],
		@Override_Scale_ExtraText_ID = [Override_Scale_ExtraText_ID],
		@Override_Description = [Override_After_Description]
	FROM #extraTextDescUpdate 
	WHERE [Override_Processed] = 0
	AND [Override_Scale_ExtraText_ID] IS NOT NULL 
	AND [Override_After_Description] IS NOT NULL

	-- Update the Item Scale Override's Extra Text Description
	IF (@Override_Scale_ExtraText_ID IS NOT NULL AND @Override_Description IS NOT NULL)
	BEGIN
		SELECT @CodeLocation = '-- Extra Text Desc Update - Update the Override Description to be the Global PLU : '
							+ '[Override_Scale_ExtraText_ID] = ' + ISNULL(CONVERT(VARCHAR(64), @Override_Scale_ExtraText_ID),'NULL') + ', '
							+ '[Override_Description] = ' + ISNULL(@Override_Description,'NULL') + ', '
							+ '[GlobalPlu] = ' + ISNULL(@GlobalPlu,'NULL')

		-- Update the Override Description to be the Global ScanCode + C if in PN or Global ScanCode + 1 if in MW, otherwise do not update
		UPDATE [dbo].[Scale_ExtraText]
		SET [Description] = @Override_Description
		WHERE [Scale_ExtraText_ID] = @Override_Scale_ExtraText_ID

		-- Record when the scale extra text override description was updated
		INSERT INTO GlobalPluAlignmentHistory ([GlobalPluAlignmentId], [Item_Key], [Scale_ExtraText_ID], [Action])
		VALUES (@GlobalPluAlignmentId, @GlobalPlu_Item_Key, @Override_Scale_ExtraText_ID, 'Scale Extra Text Override description updated : ' + ISNULL(@CodeLocation,'@CodeLocation = NULL'))

		UPDATE #extraTextDescUpdate 
		SET [Override_Processed] = 1,
		    [Note] = CASE WHEN [Note] IS NOT NULL THEN [Note] + '; ' + @CodeLocation ELSE @CodeLocation END
		WHERE [GlobalPluAlignmentId] = @GlobalPluAlignmentId
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

	-- Display extraTextDescUpdate results
	SELECT
		ROW_NUMBER() OVER (ORDER BY st.[Subteam_Name], etdu.[GlobalPlu]) AS 'Row Number',
		st.[Subteam_No],
		st.[Subteam_Name],
		etdu.[GlobalPlu], 
		i.[Item_Description],
        ii.[NumPluDigitsSentToScale],
        [SendToScale] = CASE WHEN ii.[Scale_Identifier] = 1 THEN 'Yes' ELSE 'No' END,
		etdu.[GlobalPluAlignmentId],
		etdu.[GlobalPlu_Item_Key],
		etdu.[Scale_ExtraText_ID],
		etdu.[Before_Description],
		[After_Description] = et.[Description],
		[Processed] = CASE WHEN etdu.[Processed] = 1 THEN 'Y' ELSE 'N' END,
		etdu.[Override_Scale_ExtraText_ID],
		sj.[StoreJurisdictionDesc],
		etdu.[Override_Before_Description],
		[Override_After_Description] = eto.[Description],
		[Override_Processed] = CASE WHEN etdu.[Override_Processed] = 1 THEN 'Y' ELSE 'N' END,
		etdu.[Note]
	FROM #extraTextDescUpdate etdu
	INNER JOIN [dbo].[GlobalPluAlignment] gpa ON etdu.[GlobalPluAlignmentId] = gpa.[GlobalPluAlignmentId]
	INNER JOIN [dbo].[ItemIdentifier] ii ON gpa.[GlobalPlu] = ii.[Identifier]
	INNER JOIN [dbo].[Item] i ON ii.[Item_Key] = i.[Item_Key]
	INNER JOIN [dbo].[SubTeam] st ON i.[Subteam_No] = st.[Subteam_No]
	LEFT JOIN (SELECT [item_Key], 
						MAX([itemscale_id]) AS [itemscale_id]
				FROM [dbo].[ItemScale]
				GROUP BY [item_Key]) AS ms ON ii.[item_key] = ms.[item_key]
	LEFT JOIN [dbo].[ItemScale] si ON ms.[itemscale_id] = si.[itemscale_id]
	LEFT JOIN [dbo].[Scale_ExtraText] et ON si.[scale_extratext_id] = et.[scale_extratext_id]
	LEFT JOIN [dbo].[ItemScaleOverride] sio ON si.[item_key] = sio.[item_key]
	LEFT JOIN [dbo].[StoreJurisdiction] sj ON sio.[StoreJurisdictionId] = sj.[StoreJurisdictionId]
	LEFT JOIN [dbo].[Scale_ExtraText] eto ON sio.[scale_extratext_id] = eto.[scale_extratext_id]
	WHERE i.[Deleted_Item] = 0
	AND i.[Remove_Item] = 0
	AND ii.[Deleted_Identifier] = 0
	AND ii.[Remove_Identifier] = 0
	ORDER BY 1

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

	IF OBJECT_ID('tempdb..#extraTextDescUpdate') IS NOT NULL
		DROP TABLE #extraTextDescUpdate


END
ELSE
BEGIN
   SELECT 'Region ' + (SELECT regionCode FROM Region) + ' not scheduled to get Global PLU Alignment Script 1 Step 3 Update Scale Extra Text Description'
END

GO

SET NOCOUNT OFF
GO