CREATE PROCEDURE [dbo].[PurgePriceHistoryDaily]
    @cutOffDate	DATETIME,
    @batchVolume	INT

AS 
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @RunTime AS INT
	   ,@PriceHistoryDailyPurgeStartTime AS INT
	   ,@PriceHistoryDailyPurgeEndTime AS INT
	   ,@today AS DATETIME = CONVERT(DATE, getdate())
	
SELECT @RunTime = DATEPART(HOUR, GETDATE())
		,@PriceHistoryDailyPurgeStartTime = dbo.fn_GetAppConfigValue('PriceHistoryDailyPurgeStartTime', 'IRMA Client')
		,@PriceHistoryDailyPurgeEndTime = dbo.fn_GetAppConfigValue('PriceHistoryDailyPurgeEndTime', 'IRMA Client')

DECLARE  @priceHistoryPurgeCount INT
		,@maxPurgedPriceHistoryId INT
		,@maxPriceHistoryId INT

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
		,@LogAppName = 'PriceHistory Purge'
		,@LogLevel = 'INFO'
		,@LogThread = '0'
		,@LogExceptionMsg = ''

SET @maxPurgedPriceHistoryId = 0
SET @maxPriceHistoryId = 0

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

IF @RunTime >= @PriceHistoryDailyPurgeStartTime AND @RunTime < @PriceHistoryDailyPurgeEndTime
BEGIN
	SELECT @CodeLocation = 'PriceHistory Purging Job Starts...'
	SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

	IF Object_id('tempdb..#PriceHistoryId') IS NOT NULL
		DROP TABLE #PriceHistoryId

	CREATE TABLE #PriceHistoryId(
		[PriceHistoryID] [int] NOT NULL,
		CONSTRAINT [PK_PriceHistoryID] PRIMARY KEY CLUSTERED 
		(
			[PriceHistoryID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
	) ON [PRIMARY]
   
   SELECT @LogMsg = 'Populating #PriceHistoryId table with PriceHistoryId for deleted items ...'
   SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

   INSERT INTO #PriceHistoryId	
		SELECT Top (@batchVolume) PriceHistoryID
		  FROM PriceHistory
		 WHERE Item_Key in (select i.Item_Key
		  FROM Item i
	     WHERE deleted_item = 1
		   and (LastModifiedDate is null
		    or  LastModifiedDate < @cutOffDate))
		   and Effective_Date < @cutOffDate
	  ORDER BY PriceHistoryID

	SELECT @priceHistoryPurgeCount = COUNT(1) FROM #PriceHistoryId

	SELECT @LogMsg = 'Deleting PriceHistory records for deleted items starts ...'
	SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

	DELETE ph
	  FROM PriceHistory ph
	  JOIN #PriceHistoryId phi on ph.PriceHistoryID = phi.PriceHistoryID

	SELECT @LogMsg = 'Deleting PriceHistory records for deleted items ends. PriceHistory records deleted: ' + CAST(@priceHistoryPurgeCount AS VARCHAR)
	SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

	IF @priceHistoryPurgeCount < @batchVolume
		BEGIN
			TRUNCATE TABLE #PriceHistoryId

			INSERT INTO #PriceHistoryId
				SELECT Top (@batchVolume) PriceHistoryID
				  FROM PriceHistory
				 WHERE Effective_Date < @cutOffDate
				   and PriceHistoryID > (SELECT MaxPurgedKeyID FROM MaxPurgeReference WHERE TableName = 'PriceHistory') 

			SELECT @maxPurgedPriceHistoryId = ISNULL(MAX(PriceHistoryID),0) 
			  FROM #PriceHistoryId

			SELECT @LogMsg = 'Get the max PriceHistoryId from the PriceHistory table ... '
			SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			SELECT @maxPriceHistoryId = MaxKeyID
		      FROM MaxPurgeReference
		     WHERE LastModifiedDateTime >= @today
			   AND TableName = 'PriceHistory'

			SELECT @LogMsg = 'Get saved @maxPriceHistoryId = ' + CAST(@maxPriceHistoryId AS VARCHAR)
			SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			--Only set the max PriceHistory Key that could be purged once a day
			IF @maxPriceHistoryId = 0
			BEGIN
				SELECT @maxPriceHistoryId = MAX(PriceHistoryID) 
				  FROM PriceHistory 
				 WHERE Effective_Date < @cutOffDate

				 UPDATE MaxPurgeReference
					SET MaxKeyID = @maxPriceHistoryId,
				        LastModifiedDateTime = @today
				  WHERE TableName = 'PriceHistory'
			
				SELECT @LogMsg = 'Save new @maxPriceHistoryId = ' + CAST(@maxPriceHistoryId AS VARCHAR)
				SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
			END
			
			IF @maxPurgedPriceHistoryId >= @maxPriceHistoryId
			BEGIN
				UPDATE MaxPurgeReference
				   SET MaxPurgedKeyID = 0
				 WHERE TableName = 'PriceHistory'
			END
			ELSE
			BEGIN
				UPDATE MaxPurgeReference
				   SET MaxPurgedKeyID = @maxPurgedPriceHistoryId
				 WHERE TableName = 'PriceHistory'
			END

			SELECT @LogMsg = 'Temp table #PriceHistoryId populated. @maxPurgedPriceHistoryId : ' + CAST(@maxPurgedPriceHistoryId AS VARCHAR) + ' , @maxPriceHistoryId : ' +  CAST(@maxPriceHistoryId AS VARCHAR)
			SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			IF Object_id('tempdb..#PriceHistoryMostCurrent') IS NOT NULL
				DROP TABLE #PriceHistoryMostCurrent

			CREATE TABLE #PriceHistoryMostCurrent
			(
				Store_No	   INT NOT NULL,
				Item_Key       INT NOT NULL,
				Effective_Date DATETIME NOT NULL,
			CONSTRAINT [PK_PriceHistoryMostCurrent] PRIMARY KEY CLUSTERED 
			(
				Store_No ASC,
				Item_Key ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
		) ON [PRIMARY]  


			SELECT @LogMsg = 'Populating #PriceHistoryMostCurrent table ...'
			SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			;WITH DISTINCTKEYS
			AS(
				SELECT DISTINCT Store_No, Item_Key
					FROM PriceHistory ph
					JOIN #PriceHistoryId phi on ph.PriceHistoryID = phi.PriceHistoryID
			)
			INSERT INTO #PriceHistoryMostCurrent 
				(Store_No, Item_Key, Effective_Date)
			SELECT ph.store_no, ph.Item_Key, max(ph.Effective_Date)
				FROM PriceHistory ph  
				JOIN DISTINCTKEYS dk ON ph.Store_No = dk.Store_No AND ph.Item_Key = dk.Item_Key	
			GROUP BY ph.store_no, ph.Item_Key

			SELECT @LogMsg = 'Deleting PriceHistory records outsie of retention period ...'
			SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			DELETE phi
			  FROM #PriceHistoryId phi
			  JOIN PriceHistory ph 
				ON phi.PriceHistoryId = ph.PriceHistoryId
			  JOIN #PriceHistoryMostCurrent  phm 
				ON ph.Store_No = phm.Store_No 
			   AND ph.Item_Key = phm.Item_Key 
			   AND ph.Effective_Date = phm.Effective_Date

			DELETE ih
				FROM PriceHistory ih
				JOIN #PriceHistoryId ihi on ih.PriceHistoryID = ihi.PriceHistoryID
			
			SET @priceHistoryPurgeCount = @@ROWCOUNT

			SELECT @LogMsg = 'Deleting PriceHistory records outside of retention period ends. PriceHistory records deleted: ' + CAST(@priceHistoryPurgeCount AS VARCHAR)
			SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		END
	END
END