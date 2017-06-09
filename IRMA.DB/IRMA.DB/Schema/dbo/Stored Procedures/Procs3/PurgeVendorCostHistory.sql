CREATE PROCEDURE [dbo].[PurgeVendorCostHistory]
    @batchVolume	 INT
AS 
BEGIN
	
	DECLARE @RunTime INT
	       ,@Count   INT 
		   ,@DailyPurgeStartTime INT
		   ,@DailyPurgeEndTime   INT
		   ,@RetentionPolicyId   INT
		   ,@cutOffDate		     DATETIME 
		   ,@today               DATETIME = CONVERT(DATE, getdate())
		   ,@vendorCostHistoryPurgeCount INT = 0
		   ,@maxPurgedVendorCostHistoryId INT = 0
		   ,@maxVendorCostHistoryId INT = 0
		   ,@dailyPurgeCompleted BIT
	
	SELECT @RunTime = DATEPART(HOUR, GETDATE())
		  
    SELECT @DailyPurgeStartTime = TimeToStart,
		   @DailyPurgeEndTime = TimeToEnd,
		   @RetentionPolicyId = RetentionPolicyId, 
		   @cutOffDate = CAST(DATEADD(d, -DaysToKeep, GETDATE()) AS DATE)
	  FROM RetentionPolicy
	 WHERE [Table] = 'VendorCostHistory'

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
			,@LogAppName = 'VendorCostHistory Purge'
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
	
	--Rest the DailyPurgeCompleted flag every day
	 UPDATE RetentionPolicy
	    SET DailyPurgeCompleted = 0
	  WHERE [Table] = 'VendorCostHistory'
	    AND LastPurgedDateTime < @today

	 SELECT @dailyPurgeCompleted = DailyPurgeCompleted
	   FROM RetentionPolicy
      WHERE [Table] = 'VendorCostHistory'

	IF (@RunTime >= @DailyPurgeStartTime AND @RunTime < @DailyPurgeEndTime AND @dailyPurgeCompleted = 0)
	BEGIN
		SELECT @CodeLocation = 'VendorCostHistory Data Purge Starts...'
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;
		
		IF Object_id('tempdb..#PurgedVendorCostHistoryId') IS NOT NULL
			DROP TABLE #PurgedVendorCostHistoryId

		CREATE TABLE #PurgedVendorCostHistoryId(
			[VendorCostHistoryID] [int] NOT NULL,
			CONSTRAINT [PK_PurgedVendorCostHistoryID] PRIMARY KEY CLUSTERED 
			(
				[VendorCostHistoryID] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
		) ON [PRIMARY]

		INSERT INTO #PurgedVendorCostHistoryId
		SELECT Top (@batchVolume) VendorCostHistoryID
		  FROM VendorCostHistory vch
		 WHERE StartDate < @cutOffDate
		   AND EndDate < @cutOffDate

		SELECT @LogMsg = 'Deleting VendorCostHistory records with EndDate earlier than ' + CAST(@cutOffDate AS VARCHAR)
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		DELETE vch
		  FROM VendorCostHistory vch
		  JOIN #PurgedVendorCostHistoryId pvch on vch.VendorCostHistoryID = pvch.VendorCostHistoryID
		
		SET @vendorCostHistoryPurgeCount = @@ROWCOUNT

		SELECT @LogMsg = 'Deleting VendorCostHistory records with EndDate earlier than ' + CAST(@cutOffDate AS VARCHAR) + ' ends. VendorCostHistory records deleted: ' + CAST(@vendorCostHistoryPurgeCount AS VARCHAR)
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

		IF @vendorCostHistoryPurgeCount < @batchVolume
		BEGIN
			TRUNCATE TABLE #PurgedVendorCostHistoryId

			INSERT INTO #PurgedVendorCostHistoryId
				SELECT Top (@batchVolume) VendorCostHistoryID
				  FROM VendorCostHistory
				 WHERE InsertDate < @cutOffDate
				   AND VendorCostHistoryID > (SELECT MaxPurgedKeyID FROM MaxPurgeReference WHERE TableName = 'VendorCostHistory') 
			
			SELECT @maxPurgedVendorCostHistoryId = ISNULL(MAX(VendorCostHistoryID), 0) 
	          FROM #PurgedVendorCostHistoryId

			SELECT @LogMsg = 'Temp table #PurgedVendorCostHistoryId populated. @maxPurgedVendorCostHistoryId : ' + CAST(@maxPurgedVendorCostHistoryId AS VARCHAR)
			SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg; 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			IF @maxPurgedVendorCostHistoryId = 0 --Finish one full round of purging
			BEGIN
				UPDATE RetentionPolicy
			       SET DailyPurgeCompleted = 1,
					   LastPurgedDateTime = GETDATE()
			     WHERE [Table] = 'VendorCostHistory'

				UPDATE MaxPurgeReference
				   SET MaxPurgedKeyID = 0,
					   LastModifiedDateTime = getdate()
				  WHERE TableName = 'VendorCostHistory'

				SELECT @LogMsg = 'Finished a full round of purging. Reset references to start all over at the next run ... '
				SELECT @now = getdate(); 
				EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;	
			END
			ELSE
			BEGIN
				UPDATE MaxPurgeReference
				   SET MaxPurgedKeyID = @maxPurgedVendorCostHistoryId,
					   LastModifiedDateTime = getdate()
				 WHERE TableName = 'VendorCostHistory'

				SELECT @LogMsg = 'Updated MaxPurgedKeyID on the MaxPurgeReference table ... '
				SELECT @now = getdate(); 
				EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
			END

			IF Object_id('tempdb..#VendorCostHistoryMostCurrent') IS NOT NULL
			DROP TABLE #VendorCostHistoryMostCurrent

			CREATE TABLE #VendorCostHistoryMostCurrent(
				[VendorCostHistoryID] [int] NOT NULL,
				[StoreItemVendorID] [int] NOT NULL,
				CONSTRAINT [PK_VendorCostHistoryMostCurrentID] PRIMARY KEY CLUSTERED 
				(
					[VendorCostHistoryID] ASC
				)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
			) ON [PRIMARY]

			;WITH DISTINCTKEYS
			AS(
				SELECT DISTINCT StoreItemVendorID
					FROM VendorCostHistory vch
					JOIN #PurgedVendorCostHistoryId pvch on vch.VendorCostHistoryID = pvch.VendorCostHistoryID
			)
			INSERT INTO #VendorCostHistoryMostCurrent 
				(VendorCostHistoryID, StoreItemVendorID)
			SELECT max(VendorCostHistoryID), vch.StoreItemVendorID
			  FROM VendorCostHistory vch
			  JOIN DISTINCTKEYS dk ON vch.StoreItemVendorID = dk.StoreItemVendorID
			 WHERE StartDate <= @today
		  GROUP BY vch.StoreItemVendorID  
			
			SELECT @LogMsg = 'Populated #VendorCostHistoryMostCurrent table ...'
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			IF Object_id('tempdb..#VendorCostHistoryFuture') IS NOT NULL
				DROP TABLE #VendorCostHistoryFuture

			CREATE TABLE #VendorCostHistoryFuture(
				[VendorCostHistoryID] [int] NOT NULL,
				CONSTRAINT [PK_VendorCostHistoryFutureID] PRIMARY KEY CLUSTERED 
				(
					[VendorCostHistoryID] ASC
				)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
			) ON [PRIMARY]

			INSERT INTO #VendorCostHistoryFuture
			SELECT vch.VendorCostHistoryID
			FROM VendorCostHistory vch
			JOIN #VendorCostHistoryMostCurrent vchmc on vch.StoreItemVendorID = vchmc.StoreItemVendorID 
			WHERE vch.VendorCostHistoryID > vchmc.VendorCostHistoryID
			
			SELECT @LogMsg = 'Populated #VendorCostHistoryFuture table ...'
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			--Remove the most current cost from the to-be-purged records if they are on the #PurgedVendorCostHistoryId table
			DELETE pvch
			  FROM #PurgedVendorCostHistoryId pvch
			  JOIN #VendorCostHistoryMostCurrent  vchc 
			   ON pvch.VendorCostHistoryID = vchc.VendorCostHistoryID
			
			--Remove future costs from the to-be-purged records if they are on the #PurgedVendorCostHistoryId table
			DELETE pvch
			  FROM #PurgedVendorCostHistoryId pvch
			  JOIN #VendorCostHistoryFuture  vchf 
			   ON pvch.VendorCostHistoryID = vchf.VendorCostHistoryID

			DELETE vch
			  FROM VendorCostHistory vch
			  JOIN #PurgedVendorCostHistoryId pvch on vch.VendorCostHistoryID = pvch.VendorCostHistoryID
			
			SET @vendorCostHistoryPurgeCount = @@ROWCOUNT

			SELECT @LogMsg = 'Deleted VendorCostHistory records outside of retention period. VendorCostHistory records deleted: ' + CAST(@vendorCostHistoryPurgeCount AS VARCHAR)
			SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		END

		UPDATE RetentionPolicy
		   SET LastPurgedDateTime = GETDATE()
		 WHERE [Table] = 'VendorCostHistory'

		SELECT @LogMsg = 'VendorCostHistory Purge job ended ... '
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
	END
END
