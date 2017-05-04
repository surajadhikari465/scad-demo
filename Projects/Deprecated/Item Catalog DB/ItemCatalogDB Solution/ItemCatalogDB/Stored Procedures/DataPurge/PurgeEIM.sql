IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'PurgeEIM')
	DROP  PROCEDURE  dbo.PurgeEIM
GO

CREATE PROCEDURE [dbo].[PurgeEIM]
    @batchVolume	 INT,
	@eimSessionBatch INT
AS 
BEGIN
	SET TRANSACTION ISOLATION LEVEL SNAPSHOT

	DECLARE @RunTime INT
	       ,@Count INT 
		   ,@DailyPurgeStartTime INT
		   ,@DailyPurgeEndTime INT
		   ,@UploadRowDeletedCount INT
		   ,@UploadValueDeletedCount INT
		   ,@RetentionPolicyId INT
		   ,@cutOffDate		 DATETIME
	
	SELECT @RunTime = DATEPART(HOUR, GETDATE())
		  ,@UploadRowDeletedCount = 0
		  ,@UploadValueDeletedCount = 0
		  
    SELECT @DailyPurgeStartTime = TimeToStart,
		   @DailyPurgeEndTime = TimeToEnd,
		   @RetentionPolicyId = RetentionPolicyId, 
		   @cutOffDate = CAST(DATEADD(d, -DaysToKeep, GETDATE()) AS DATE)
	  FROM RetentionPolicy
	 WHERE [Table] = 'UploadSession'

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
			,@LogAppName = 'EIM Purge'
			,@LogLevel = 'INFO'
			,@LogThread = '0'
			,@LogExceptionMsg = ''

	SELECT @DBEnv = CASE
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

	IF (@RunTime >= @DailyPurgeStartTime AND @RunTime < @DailyPurgeEndTime)
	BEGIN
		SELECT @CodeLocation = 'EIM Data Purge Starts...'
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;
		
		IF Object_id('tempdb..#UploadSessionId') IS NOT NULL
			DROP TABLE #UploadSessionId

		CREATE TABLE #UploadSessionId(
			[UploadSession_ID] [int] NOT NULL,
			CONSTRAINT [PK_UploadSessionID] PRIMARY KEY CLUSTERED 
			(
				[UploadSession_ID] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
		) ON [PRIMARY]

		INSERT INTO #UploadSessionId
		SELECT TOP (@eimSessionBatch) UploadSession_ID
		  FROM UploadSession
		 WHERE CreatedDateTime < @cutOffDate
		
		IF Object_id('tempdb..#UploadRowId') IS NOT NULL
			DROP TABLE #UploadRowId

		CREATE TABLE #UploadRowId(
			[UploadRow_ID] [int] NOT NULL,
			CONSTRAINT [PK_UploadRowID] PRIMARY KEY CLUSTERED 
			(
				[UploadRow_ID] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
		) ON [PRIMARY]

		INSERT INTO #UploadRowId
		SELECT UploadRow_ID
		  FROM UploadRow ur
		  JOIN #UploadSessionId us on ur.UploadSession_ID = us.UploadSession_ID

		IF Object_id('tempdb..#UploadValueId') IS NOT NULL
			DROP TABLE #UploadValueId

		CREATE TABLE #UploadValueId(
			[UploadValue_ID] [int] NOT NULL,
			CONSTRAINT [PK_UploadValueID] PRIMARY KEY CLUSTERED 
			(
				[UploadValue_ID] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
		) ON [PRIMARY]

		INSERT INTO #UploadValueId
		SELECT uv.UploadValue_ID
		  FROM UploadValue uv
		  JOIN #UploadRowId ur on ur.UploadRow_ID = uv.UploadRow_ID

		IF Object_id('tempdb..#UploadSessionUploadTypeId') IS NOT NULL
			DROP TABLE #UploadSessionUploadTypeId

		CREATE TABLE #UploadSessionUploadTypeId(
			[UploadSessionUploadType_ID] [int] NOT NULL,
			CONSTRAINT [PK_UploadSessionUploadTypeID] PRIMARY KEY CLUSTERED 
			(
				[UploadSessionUploadType_ID] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
		) ON [PRIMARY]

		INSERT INTO #UploadSessionUploadTypeId
		SELECT UploadSessionUploadType_ID
		  FROM UploadSessionUploadType ut
		  JOIN #UploadSessionId us on ut.UploadSession_ID = us.UploadSession_ID
		
		SELECT @CodeLocation = 'Temp tables populated for EIM tables purge'
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

		-- Purge UploadValue data
		SELECT @Count = @batchVolume
		WHILE (@Count = @batchVolume)
		BEGIN
			DELETE TOP (@batchVolume) u
			  FROM UploadValue u
			  JOIN #UploadValueId uvi ON u.UploadValue_ID = uvi.UploadValue_ID 
		
			SELECT @Count = @@rowcount
			SELECT @UploadValueDeletedCount = @UploadValueDeletedCount + @Count
		END
		SELECT @CodeLocation = 'UploadValue Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' UploadValue records deleted: ' + CAST(@UploadValueDeletedCount AS VARCHAR);
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		-- Purge UploadRow data
		SELECT @Count = @batchVolume
		WHILE (@Count = @batchVolume)
		BEGIN
			DELETE TOP (@batchVolume) u
			  FROM UploadRow u
			  JOIN #UploadRowId uri ON u.UploadRow_ID = uri.UploadRow_ID 
		
			SELECT @Count = @@rowcount
			SELECT @UploadRowDeletedCount = @UploadRowDeletedCount + @Count
		END
		SELECT @CodeLocation = 'UploadRow Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' UploadRow records deleted: ' + CAST(@UploadRowDeletedCount AS VARCHAR);
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		
		-- Purge UploadSessionUploadTypeStore data
		DELETE u
		  FROM UploadSessionUploadTypeStore u 
		  JOIN #UploadSessionUploadTypeId usut ON u.UploadSessionUploadType_ID = usut.UploadSessionUploadType_ID 
		
		SET @Count = @@rowcount

		SELECT @CodeLocation = 'UploadSessionUploadTypeStore Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' UploadSessionUploadTypeStore records deleted: ' + CAST(@Count AS VARCHAR);
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		-- Purge UploadSessionUploadType data
		DELETE u
		  FROM UploadSessionUploadType u 
		  JOIN #UploadSessionId usi ON u.UploadSession_ID = usi.UploadSession_ID 
		
		SET @Count = @@rowcount

		SELECT @CodeLocation = 'UploadSessionUploadType Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' UploadSessionUploadType records deleted: ' + CAST(@Count AS VARCHAR);
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		-- Purge UploadSession data
		DELETE u
		  FROM UploadSession u
		  JOIN #UploadSessionId usi ON u.UploadSession_ID = usi.UploadSession_ID 

		SET @Count = @@rowcount

		SELECT @CodeLocation = 'UploadSession Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' UploadSession records deleted: ' + CAST(@Count AS VARCHAR);
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		UPDATE RetentionPolicy
		   SET LastPurgedDateTime = getdate()
		 WHERE RetentionPolicyId = @RetentionPolicyId

		SELECT @CodeLocation = 'EIM Data Purge Ends...'
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;
	END
END
