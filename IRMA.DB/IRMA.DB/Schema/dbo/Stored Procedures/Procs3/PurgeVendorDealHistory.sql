CREATE PROCEDURE [dbo].[PurgeVendorDealHistory]
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
		   ,@VendorDealHistoryPurgeCount INT = 0
		   ,@maxPurgedVendorDealHistoryId INT = 0
		   ,@maxVendorDealHistoryId INT = 0
		   ,@dailyPurgeCompleted BIT
	
	SELECT @RunTime = DATEPART(HOUR, GETDATE())
		  
    SELECT @DailyPurgeStartTime = TimeToStart,
		   @DailyPurgeEndTime = TimeToEnd,
		   @RetentionPolicyId = RetentionPolicyId, 
		   @cutOffDate = CAST(DATEADD(d, -DaysToKeep, GETDATE()) AS DATE)
	  FROM RetentionPolicy
	 WHERE [Table] = 'VendorDealHistory'

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
			,@LogAppName = 'VendorDealHistory Purge'
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
	  WHERE [Table] = 'VendorDealHistory'
	    AND LastPurgedDateTime < @today

	 SELECT @dailyPurgeCompleted = DailyPurgeCompleted
	   FROM RetentionPolicy
      WHERE [Table] = 'VendorDealHistory'

	IF (@RunTime >= @DailyPurgeStartTime AND @RunTime < @DailyPurgeEndTime AND @dailyPurgeCompleted = 0)
	BEGIN
		SELECT @CodeLocation = 'VendorDealHistory Data Purge Starts...'
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;
		
		IF Object_id('tempdb..#PurgedVendorDealHistoryId') IS NOT NULL
			DROP TABLE #PurgedVendorDealHistoryId

		CREATE TABLE #PurgedVendorDealHistoryId(
			[VendorDealHistoryID] [int] NOT NULL,
			CONSTRAINT [PK_PurgedVendorDealHistoryID1] PRIMARY KEY CLUSTERED 
			(
				[VendorDealHistoryID] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
		) ON [PRIMARY]

		INSERT INTO #PurgedVendorDealHistoryId
		SELECT Top (@batchVolume) VendorDealHistoryID
		  FROM VendorDealHistory vch
		 WHERE EndDate < @cutOffDate
        
		SET @VendorDealHistoryPurgeCount = @@ROWCOUNT

		IF @VendorDealHistoryPurgeCount > 0
		BEGIN
			SELECT @LogMsg = 'Deleting VendorDealHistory records with EndDate earlier than ' + CAST(@cutOffDate AS VARCHAR)
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			IF EXISTS (select * from sys.objects where type = 'TR' and name = 'JDASync_VendorDealHistory_Delete')
			BEGIN
				;DISABLE TRIGGER [dbo].[JDASync_VendorDealHistory_Delete] ON [dbo].[VendorDealHistory]	
			END

			DELETE vch
				FROM VendorDealHistory vch
				JOIN #PurgedVendorDealHistoryId pvch on vch.VendorDealHistoryID = pvch.VendorDealHistoryID
			
			IF EXISTS (select * from sys.objects where type = 'TR' and name = 'JDASync_VendorDealHistory_Delete')
			BEGIN
				;ENABLE TRIGGER [dbo].[JDASync_VendorDealHistory_Delete] ON [dbo].[VendorDealHistory]
			END
			
			SELECT @LogMsg = 'Deleting VendorDealHistory records with EndDate earlier than ' + CAST(@cutOffDate AS VARCHAR) + ' ends. VendorDealHistory records deleted: ' + CAST(@VendorDealHistoryPurgeCount AS VARCHAR)
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		END
		
		IF @VendorDealHistoryPurgeCount < @batchVolume
		BEGIN
		    SELECT @maxVendorDealHistoryId = ISNULL(max(VendorDealHistoryID),0)
			  FROM VendorDealHistory
			 WHERE InsertDate < @cutOffDate

			IF @maxVendorDealHistoryId = 0
			BEGIN
				UPDATE RetentionPolicy
			       SET DailyPurgeCompleted = 1,
					   LastPurgedDateTime = GETDATE()
			     WHERE [Table] = 'VendorDealHistory'

				 UPDATE MaxPurgeReference
					SET MaxPurgedKeyID = 0,
						LastModifiedDateTime = getdate()
				  WHERE TableName = 'VendorDealHistory'
			END
			ELSE
			BEGIN
				TRUNCATE TABLE #PurgedVendorDealHistoryId

				INSERT INTO #PurgedVendorDealHistoryId
					SELECT Top (@batchVolume) VendorDealHistoryID
					  FROM VendorDealHistory
					 WHERE InsertDate < @cutOffDate
					   AND VendorDealHistoryID > (SELECT MaxPurgedKeyID FROM MaxPurgeReference WHERE TableName = 'VendorDealHistory') 
				  ORDER BY VendorDealHistoryID

				SELECT @maxPurgedVendorDealHistoryId = ISNULL(MAX(VendorDealHistoryID),0) 
				  FROM #PurgedVendorDealHistoryId

				SELECT @LogMsg = 'Temp table #PurgedVendorDealHistoryId populated. @maxPurgedVendorDealHistoryId : ' + CAST(@maxPurgedVendorDealHistoryId AS VARCHAR)
				SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg; 
				EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

				IF @maxPurgedVendorDealHistoryId = 0 --Just finished one full round of purging
				BEGIN
					UPDATE MaxPurgeReference
					   SET MaxPurgedKeyID = 0,
						   LastModifiedDateTime = getdate()
					  WHERE TableName = 'VendorDealHistory'

					SELECT @LogMsg = 'Finished a full round of purging. Reset references to start all over at the next run ... '
					SELECT @now = getdate(); 
					EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;	
				END
				ELSE
				BEGIN
					UPDATE MaxPurgeReference
					   SET MaxPurgedKeyID = @maxPurgedVendorDealHistoryId,
						   LastModifiedDateTime = getdate()
					 WHERE TableName = 'VendorDealHistory'

					SELECT @LogMsg = 'Updated MaxPurgedKeyID on the MaxPurgeReference table ... '
					SELECT @now = getdate(); 
					EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
				END

				IF Object_id('tempdb..#VendorDealHistoryMostCurrent') IS NOT NULL
				DROP TABLE #VendorDealHistoryMostCurrent

				CREATE TABLE #VendorDealHistoryMostCurrent(
					[VendorDealHistoryID] [int] NOT NULL,
					[StoreItemVendorID] [int] NOT NULL,
					[VendordealTypeID] [int] NOT NULL,
					CONSTRAINT [PK_VendorDealHistoryMostCurrentID] PRIMARY KEY CLUSTERED 
					(
						[VendorDealHistoryID] ASC
					)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
				) ON [PRIMARY]

				;WITH DISTINCTKEYS
				AS(
					SELECT DISTINCT StoreItemVendorID
						FROM VendorDealHistory vch
						JOIN #PurgedVendorDealHistoryId pvch on vch.VendorDealHistoryID = pvch.VendorDealHistoryID
				),
				DISTINCTDATE
				AS(
					SELECT MAX(InsertDate) AS InsertDate, vch.StoreItemVendorID, VendordealTypeID
					  FROM VendorDealHistory vch
					  JOIN DISTINCTKEYS dk ON vch.StoreItemVendorID = dk.StoreItemVendorID
					 WHERE StartDate <= @today
				  GROUP BY vch.StoreItemVendorID, vch.VendordealTypeID
				)
				INSERT INTO #VendorDealHistoryMostCurrent 
					(VendorDealHistoryID, StoreItemVendorID, VendordealTypeID)
				SELECT VendorDealHistoryID, vch.StoreItemVendorID, vch.VendordealTypeID
				  FROM VendorDealHistory vch
				  JOIN DISTINCTDATE dd ON vch.StoreItemVendorID = dd.StoreItemVendorID
									  AND vch.InsertDate = dd.InsertDate
									  AND vch.VendordealTypeID = dd.VendordealTypeID
			   
			
				SELECT @LogMsg = 'Populated #VendorDealHistoryMostCurrent table ...'
				SELECT @now = getdate(); 
				EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

				IF Object_id('tempdb..#VendorDealHistoryFuture') IS NOT NULL
					DROP TABLE #VendorDealHistoryFuture

				CREATE TABLE #VendorDealHistoryFuture(
					[VendorDealHistoryID] [int] NOT NULL,
					CONSTRAINT [PK_VendorDealHistoryFutureID] PRIMARY KEY CLUSTERED 
					(
						[VendorDealHistoryID] ASC
					)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
				) ON [PRIMARY]

				INSERT INTO #VendorDealHistoryFuture
				SELECT DISTINCT vch.VendorDealHistoryID
				FROM VendorDealHistory vch
				JOIN #VendorDealHistoryMostCurrent vdhmc ON vch.StoreItemVendorID = vdhmc.StoreItemVendorID 
														AND vch.VendordealTypeID = vdhmc.VendordealTypeID
				WHERE vch.VendorDealHistoryID > vdhmc.VendorDealHistoryID
			
				SELECT @LogMsg = 'Populated #VendorDealHistoryFuture table ...'
				SELECT @now = getdate(); 
				EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

				--Remove the most current cost from the to-be-purged records if they are on the #PurgedVendorDealHistoryId table

				DELETE pvdh
				  FROM #PurgedVendorDealHistoryId pvdh
				  JOIN #VendorDealHistoryMostCurrent  vdhc 
				   ON pvdh.VendorDealHistoryID = vdhc.VendorDealHistoryID
			
				--Remove future costs from the to-be-purged records if they are on the #PurgedVendorDealHistoryId table
				DELETE pvdh
				  FROM #PurgedVendorDealHistoryId pvdh
				  JOIN #VendorDealHistoryFuture  vdhf 
				   ON pvdh.VendorDealHistoryID = vdhf.VendorDealHistoryID
				
				SELECT @VendorDealHistoryPurgeCount = COUNT(1)
				  FROM #PurgedVendorDealHistoryId		
				
				IF @VendorDealHistoryPurgeCount > 0
				BEGIN
					IF EXISTS (select * from sys.objects where type = 'TR' and name = 'JDASync_VendorDealHistory_Delete')
					BEGIN
						;DISABLE TRIGGER [dbo].[JDASync_VendorDealHistory_Delete] ON [dbo].[VendorDealHistory]
					END

					DELETE vdh
						FROM VendorDealHistory vdh
						JOIN #PurgedVendorDealHistoryId pvdh on vdh.VendorDealHistoryID = pvdh.VendorDealHistoryID
					
					IF EXISTS (select * from sys.objects where type = 'TR' and name = 'JDASync_VendorDealHistory_Delete')
					BEGIN
						;ENABLE TRIGGER [dbo].[JDASync_VendorDealHistory_Delete] ON [dbo].[VendorDealHistory]
					END 
				END
				
				SELECT @LogMsg = 'Deleted VendorDealHistory records outside of retention period. VendorDealHistory records deleted: ' + CAST(@VendorDealHistoryPurgeCount AS VARCHAR)
				SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
			END
		
			UPDATE RetentionPolicy
			   SET LastPurgedDateTime = GETDATE()
			 WHERE [Table] = 'VendorDealHistory'
		END
		
		SELECT @LogMsg = 'VendorDealHistory Purge job ended ... '
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;	
	END
END
