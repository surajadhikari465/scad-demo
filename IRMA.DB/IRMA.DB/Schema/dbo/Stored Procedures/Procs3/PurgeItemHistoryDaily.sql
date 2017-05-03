CREATE PROCEDURE [dbo].[PurgeItemHistoryDaily]
    @cutOffDate	DATETIME,
    @batchVolume	INT

AS 
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @RunTime AS INT
	   ,@ItemHistoryDailyPurgeStartTime AS INT
	   ,@ItemHistoryDailyPurgeEndTime AS INT
	   ,@today AS DATETIME = CONVERT(DATE, getdate())
	
SELECT  @RunTime = DATEPART(HOUR, GETDATE())
	   ,@ItemHistoryDailyPurgeStartTime = dbo.fn_GetAppConfigValue('ItemHistoryDailyPurgeStartTime', 'IRMA Client')
	   ,@ItemHistoryDailyPurgeEndTime = dbo.fn_GetAppConfigValue('ItemHistoryDailyPurgeEndTime', 'IRMA Client')

DECLARE  @itemHistoryPurgeCount INT
		,@maxPurgedItemHistoryId INT
		,@maxItemHistoryId INT

DECLARE  @CodeLocation varchar(128)
		,@DBEnv varchar(8)
		,@EnvId uniqueidentifier
		,@LogSystemName varchar(64)
		,@LogAppName varchar(64)
		,@LogAppID uniqueidentifier
		,@LogLevel varchar(8)
		,@LogThread varchar(8)
		,@LogMsg varchar(256)
		,@LogExceptionMsg varchar(2000)
		,@UserID int
		,@now datetime

SELECT
		@LogSystemName = 'IRMA CLIENT'
		,@LogAppName = 'ItemHistory Purge'
		,@LogLevel = 'INFO'
		,@LogThread = '0'
		,@LogExceptionMsg = ''

SET @maxPurgedItemHistoryId = 0
SET @maxItemHistoryId = 0

SELECT @DBEnv = case
	WHEN Environment like '%q%' then 'QA'
	WHEN Environment like '%pr%' then 'PRD'
	ELSE 'TST'
	END
FROM Version
		
SELECT @UserID = User_ID 
	FROM Users
	WHERE UserName = 'system'
		
-- Get IRMA Client app GUID for logging calls (AppLogInsertEntry).
SELECT @LogAppID = a.ApplicationID,
		@EnvId    = a.EnvironmentID
FROM AppConfigApp a
JOIN AppConfigEnv e on a.EnvironmentID = e.EnvironmentID
WHERE e.ShortName = @DBEnv and a.Name = @LogSystemName

IF @RunTime >= @ItemHistoryDailyPurgeStartTime AND @RunTime < @ItemHistoryDailyPurgeEndTime
BEGIN
	SELECT @CodeLocation = 'ItemHistory Purging Job Starts...'
	SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

	IF Object_id('tempdb..#ItemHistoryId') IS NOT NULL
		DROP TABLE #ItemHistoryId

	CREATE TABLE #ItemHistoryId(
		[ItemHistoryID] [int] NOT NULL,
		CONSTRAINT [PK_ItemHistoryId] PRIMARY KEY CLUSTERED 
		(
			[ItemHistoryID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
	) ON [PRIMARY]

	SELECT @LogMsg = 'Populating #ItemHistoryId table with ItemHistoryId for deleted items ...'
	SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

	INSERT INTO #ItemHistoryId	
		SELECT Top (@batchVolume) ItemHistoryID
		  FROM ItemHistory
		 WHERE Item_Key in (select i.Item_Key
		  FROM Item i
	     WHERE deleted_item = 1
		   and (LastModifiedDate is null
		    or  LastModifiedDate < @cutOffDate))
		   and DateStamp < @cutOffDate
	ORDER BY ItemHistoryID

	DELETE ihu
	  FROM ItemHistoryUpload ihu
	  JOIN #ItemHistoryId ihi on ihu.ItemHistoryID = ihi.ItemHistoryID
		
	SELECT @itemHistoryPurgeCount = @@ROWCOUNT

	SELECT @LogMsg = 'ItemHistoryUpload records deleted: ' + CAST(@itemHistoryPurgeCount AS VARCHAR)
	SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

	SELECT @itemHistoryPurgeCount = COUNT(1) FROM #ItemHistoryId

	SELECT @LogMsg = 'Disabling trigger [dbo].[ItemHistoryDel] ...'
	SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

	;DISABLE TRIGGER [dbo].[ItemHistoryDel] ON [dbo].[ItemHistory]

	SELECT @LogMsg = 'Deleting ItemHistory records for deleted items starts ...'
	SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

	DELETE ih
		FROM ItemHistory ih
		JOIN #ItemHistoryId ihi on ih.ItemHistoryID = ihi.ItemHistoryID
	
	SELECT @LogMsg = 'Enabling trigger [dbo].[ItemHistoryDel] ...'
	SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

	;ENABLE TRIGGER [dbo].[ItemHistoryDel] ON [dbo].[ItemHistory]

	SELECT @LogMsg = 'Deleting ItemHistory records for deleted items ends. ItemHistory records deleted: ' + CAST(@itemHistoryPurgeCount AS VARCHAR)
	SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

	IF @itemHistoryPurgeCount < @batchVolume
	BEGIN
		TRUNCATE TABLE #ItemHistoryId

		INSERT INTO #ItemHistoryId
			SELECT Top (@batchVolume) ItemHistoryID
			  FROM ItemHistory
			 WHERE DateStamp < @cutOffDate
			   and ItemHistoryID > (SELECT MaxPurgedKeyID FROM MaxPurgeReference WHERE TableName = 'ItemHistory') 
		
		SELECT @maxPurgedItemHistoryId = ISNULL(MAX(ItemHistoryID),0) 
	      FROM #ItemHistoryId

		SELECT @LogMsg = 'Get the max ItemHistoryId from the ItemHistory table ... '
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		SELECT @maxItemHistoryId = MaxKeyID
		  FROM MaxPurgeReference
		 WHERE LastModifiedDateTime >= @today
		   AND TableName = 'ItemHistory'
		
		SELECT @LogMsg = 'Get saved @maxItemHistoryId = ' + CAST(@maxItemHistoryId AS VARCHAR)
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		--Only set the max ItemHistory Key that could be purged once a day
		IF @maxItemHistoryId = 0
		BEGIN
			SELECT @maxItemHistoryId = MAX(ItemHistoryID) 
			  FROM ItemHistory 
			 WHERE DateStamp < @cutOffDate

			UPDATE MaxPurgeReference
			   SET MaxKeyID = @maxItemHistoryId,
				   LastModifiedDateTime = @today
			 WHERE TableName = 'ItemHistory'
			
			SELECT @LogMsg = 'Save new @maxItemHistoryId = ' + CAST(@maxItemHistoryId AS VARCHAR)
			SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;		
		END

		IF @maxPurgedItemHistoryId >= @maxItemHistoryId
		BEGIN
			UPDATE MaxPurgeReference
			   SET MaxPurgedKeyID = 0
			 WHERE TableName = 'ItemHistory'
		END
		ELSE
		BEGIN
			UPDATE MaxPurgeReference
			   SET MaxPurgedKeyID = @maxPurgedItemHistoryId
			 WHERE TableName = 'ItemHistory'
		END

		SELECT @LogMsg = 'Temp table #ItemHistoryId populated. @maxPurgedItemHistoryId : ' + CAST(@maxPurgedItemHistoryId AS VARCHAR) + ' , @maxItemHistoryId : ' +  CAST(@maxItemHistoryId AS VARCHAR)
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		IF Object_id('tempdb..#ItemHistoryMostCurrent') IS NOT NULL
			DROP TABLE #ItemHistoryMostCurrent

		CREATE TABLE #ItemHistoryMostCurrent
		(
			Item_Key       INT NOT NULL,
			Store_No	   INT NOT NULL,
			Adjustment_ID  INT NOT NULL,
			DateStamp      DATETIME NOT NULL,
		CONSTRAINT [PK_ItemHistoryMostCurrent] PRIMARY KEY CLUSTERED 
		(
			Item_Key ASC,
			Store_No ASC,
			Adjustment_ID ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
	) ON [PRIMARY]

		SELECT @LogMsg = 'Populating #ItemHistoryMostCurrent table ...'
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		;WITH DISTINCTKEYS
		AS(
			SELECT DISTINCT Item_Key, Store_No, Adjustment_ID
				FROM ItemHistory ih
			    JOIN #ItemHistoryId ihi on ih.ItemHistoryID = ihi.ItemHistoryID
		)
		INSERT INTO #ItemHistoryMostCurrent 
			(Store_No, Item_Key, Adjustment_ID, DateStamp)
		SELECT ih.store_no, ih.Item_Key, ih.Adjustment_ID, max(ih.DateStamp)
			FROM ItemHistory ih  
			JOIN DISTINCTKEYS dk ON ih.Item_Key = dk.Item_Key AND ih.Store_No = dk.Store_No AND ih.Adjustment_ID = dk.Adjustment_ID		
		GROUP BY ih.store_no, ih.Item_Key, ih.Adjustment_ID

		SELECT @LogMsg = 'Removing ItemHistory records to be kept from #ItemHistoryId table ...'
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		DELETE ihi 
			FROM #ItemHistoryId ihi
			JOIN ItemHistory ih 
			ON ihi.ItemHistoryId = ih.ItemHistoryId
			JOIN #ItemHistoryMostCurrent  ihm 
			ON  ih.Item_Key = ihm.Item_Key 
			AND ih.Store_No = ihm.Store_No 
			AND ih.Adjustment_ID = ihm.Adjustment_ID
			AND ih.DateStamp = ihm.DateStamp

		DELETE ihu
			FROM ItemHistoryUpload ihu
			JOIN #ItemHistoryId ihi on ihu.ItemHistoryID = ihi.ItemHistoryID
		
		SELECT @itemHistoryPurgeCount = @@ROWCOUNT

		SELECT @LogMsg = 'ItemHistoryUpload records deleted: ' + CAST(@itemHistoryPurgeCount AS VARCHAR)
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		SELECT @LogMsg = 'Disabling trigger [dbo].[ItemHistoryDel] ...'
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		;DISABLE TRIGGER [dbo].[ItemHistoryDel] ON [dbo].[ItemHistory]

		SELECT @LogMsg = 'Deleting ItemHistory records outsie of retention period ...'
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		DELETE ih
			FROM ItemHistory ih
			JOIN #ItemHistoryId ihi on ih.ItemHistoryID = ihi.ItemHistoryID
		
		SELECT @LogMsg = 'Enabling trigger [dbo].[ItemHistoryDel] ...'
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
			
		;ENABLE TRIGGER [dbo].[ItemHistoryDel] ON [dbo].[ItemHistory]

		SELECT @itemHistoryPurgeCount = COUNT(1) FROM #ItemHistoryId

		SELECT @LogMsg = 'Deleting ItemHistory records outside of retention period ends. ItemHistory records deleted: ' + CAST(@itemHistoryPurgeCount AS VARCHAR)
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		TRUNCATE TABLE ItemHistoryInsertedQueue  -- Used in the Ave Cost Update job
		TRUNCATE TABLE ItemHistoryDeletedQueue	-- Used in the Ave Cost Update job

		SELECT @LogMsg = 'Truncated ItemHistoryInsertedQueue and ItemHistoryInsertedQueue tables ... '
		SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
	END
END
END