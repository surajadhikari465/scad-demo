CREATE PROCEDURE [dbo].[PurgeOrders]
    @batchVolume	 INT = 50000,
	@orderSessionBatch INT = 5000
AS 
--****************************************************************************************************************************************************
-- Procedure: PurgeOrders
--
-- Revision:
-- 09/11/2017  MZ	23008	This is called from the ItemCatalog - Order Purge job. The job is scheduled to run 24 hours daily although the actual purge 
--							timing, retention period, and Purge status are recorded in the RetentionPolicy table under the Table (column name) of OrderHeader. 
--
--							Within the actual purge window (can be different by regions), each run will grab @orderSessionBatch number of orders from the 
--                          OrderHeader table that beyond the retention limit in the sequence of OrderHeader_Id. The max OrderHeader_Id each time the job
--                          purged is recorded in MaxPurgeReference.MaxPurgeKeyID. This is to make sure each purge run the job will grab a new set of 
--                          OrderHeader records (with OrderHeader_ID > MaxPurgeReference.MaxPurgeKeyID).
--
--                          If an OrderItemID is referenced on other tables like ItemHistory, the OrderItem rows from the same order as well as the 
--                          OrderHeader record cannot be deleted due to referential constraint. So, we’ll skip the OrderHeader and OrderItem rows. 
--                          In the meantime, there’s another job that purges ItemHistory and the ItemHistoryQueue table every day. After the Order purge job 
--                          reaches to the retention limit (no more OrderHeader records with OrderHeader_ID > MaxPurgeReference.MaxPurgeKeyID that are
--                          to be purged), the order purge process will start all over by checking the records with the smallest OrderHeader_ID. 
--                          Hopefully in the next round of the order purge, some of the OrderHeader and OrderItem rows can be deleted after the 
--                          corresponding ItemHistory and or ItemHistoryQueue records were deleted.
--                   25179  Added Disable TRIGGER [dbo].[OrderHeaderDel] ON [dbo].[OrderHeader] when deleting OrderHeader records. 
--****************************************************************************************************************************************************
BEGIN
	DECLARE @RunTime INT
	       ,@Count INT 
		   ,@DailyPurgeStartTime INT
		   ,@DailyPurgeEndTime INT
		   ,@RecordDeletedCount INT
		   ,@UploadValueDeletedCount INT
		   ,@RetentionPolicyId INT
		   ,@cutOffDate	DATETIME
		   ,@today DATETIME = CONVERT(DATE, getdate())
		   ,@OrderHeaderPurgeCount INT = 0
		   ,@maxPurgedOrderHeaderId INT = 0
		   ,@dailyPurgeCompleted BIT
	
	SELECT @RunTime = DATEPART(HOUR, GETDATE())
		  ,@RecordDeletedCount = 0
		  ,@UploadValueDeletedCount = 0
		  
    SELECT @DailyPurgeStartTime = TimeToStart,
		   @DailyPurgeEndTime = TimeToEnd,
		   @RetentionPolicyId = RetentionPolicyId, 
		   @cutOffDate = CAST(DATEADD(d, -DaysToKeep, GETDATE()) AS DATE)
	  FROM RetentionPolicy
	 WHERE [Table] = 'OrderHeader'

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
			,@LogAppName = 'Order Purge'
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
	  WHERE [Table] = 'OrderHeader'
	    AND LastPurgedDateTime < @today

	 SELECT @dailyPurgeCompleted = DailyPurgeCompleted
	   FROM RetentionPolicy
      WHERE [Table] = 'OrderHeader'

	IF (@RunTime >= @DailyPurgeStartTime AND @RunTime < @DailyPurgeEndTime AND @dailyPurgeCompleted = 0)
	BEGIN
		SELECT @CodeLocation = 'Order Purge Starts...'
		SELECT @now = getdate(); 
		EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;
		
		IF Object_id('tempdb..#OrderHeaderId') IS NOT NULL
			DROP TABLE #OrderHeaderId

		CREATE TABLE #OrderHeaderId(
			[OrderHeaderID] [int] NOT NULL,
			CONSTRAINT [PK_OrderHeaderID] PRIMARY KEY CLUSTERED 
			(
				[OrderHeaderID] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
		) ON [PRIMARY]

		INSERT INTO #OrderHeaderId
		SELECT distinct TOP (@orderSessionBatch) oh.OrderHeader_ID
		  FROM OrderHeader oh
		 WHERE OrderDate < @cutOffDate
		   AND oh.OrderHeader_ID > (SELECT MaxPurgedKeyID FROM MaxPurgeReference WHERE TableName = 'OrderHeader') 
	  ORDER BY oh.OrderHeader_ID

		SELECT @maxPurgedOrderHeaderId = ISNULL(MAX(OrderHeaderID),0) 
		  FROM #OrderHeaderId

		IF @maxPurgedOrderHeaderId = 0  -----Just finished one full round of purging
		BEGIN
			UPDATE RetentionPolicy
			   SET DailyPurgeCompleted = 1,
				   LastPurgedDateTime = GETDATE()
			 WHERE [Table] = 'OrderHeader'

			UPDATE MaxPurgeReference
			   SET MaxPurgedKeyID = 0,
				   LastModifiedDateTime = getdate()
			WHERE TableName = 'OrderHeader'

			SELECT @LogMsg = 'Finished a full round of purging. Reset references to start all over at the next run ... '
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		END
		ELSE
		BEGIN
		    UPDATE MaxPurgeReference
			   SET MaxPurgedKeyID = @maxPurgedOrderHeaderId,
				   LastModifiedDateTime = getdate()
			 WHERE TableName = 'OrderHeader'

			SELECT @LogMsg = 'Updated MaxPurgedKeyID on the MaxPurgeReference table ... '
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			IF Object_id('tempdb..#OrderItemId') IS NOT NULL
				DROP TABLE #OrderItemId

			CREATE TABLE #OrderItemId(
				[OrderItemID] [int] NOT NULL,
				[OrderHeaderID] [int] NOT NULL,
				CONSTRAINT [PK_OrderItemID] PRIMARY KEY CLUSTERED 
				(
					[OrderItemID] ASC
				)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
			) ON [PRIMARY]

			INSERT INTO #OrderItemId (OrderItemID, OrderHeaderID)
			SELECT OrderItem_ID, OrderHeader_ID
			  FROM OrderItem oi
			  JOIN #OrderHeaderId oh on oh.OrderHeaderID = oi.OrderHeader_ID

			--Remove orders that are referenced as foreign keys on the ItemHistory table. ItemHistory table will be cleaned up in another job.
			DELETE oh
			  FROM #OrderHeaderId oh
			  JOIN #OrderItemId oi on oh.OrderHeaderID = oi.OrderHeaderID
			  JOIN ItemHistory ih ON ih.OrderItem_ID  = oi.OrderItemID

			DELETE oh
			  FROM #OrderHeaderId oh
			  JOIN #OrderItemId oi on oh.OrderHeaderID = oi.OrderHeaderID
			  JOIN ItemHistoryQueue ihq ON ihq.OrderItem_ID  = oi.OrderItemID

			DELETE oi
			  FROM #OrderItemId oi
		 LEFT JOIN #OrderHeaderId oh ON oh.OrderHeaderID  = oi.OrderHeaderID  
			 WHERE oh.OrderHeaderID is NULL
		
			SELECT @CodeLocation = 'Temp tables populated for Order tables purge'
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;

			IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4010Detail]') AND type in (N'U'))
			BEGIN
				TRUNCATE TABLE OrderItemCOOL4010Detail
			END

			IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4020Detail]') AND type in (N'U'))
			BEGIN
				TRUNCATE TABLE OrderItemCOOL4020Detail
			END
		
			IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]') AND type in (N'U'))
			BEGIN
				TRUNCATE TABLE SuspendedAvgCost
			END

			-- Purge OrderItem data
			SELECT @Count = @batchVolume
			WHILE (@Count = @batchVolume)
			BEGIN
				DELETE TOP (@batchVolume) oi
				  FROM OrderItem oi
				  JOIN #OrderItemId oii ON oi.OrderItem_ID = oii.OrderItemID 
		
				SELECT @Count = @@rowcount
				SELECT @RecordDeletedCount = @RecordDeletedCount + @Count
			END
			SELECT @CodeLocation = 'OrderItem Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' OrderItem records deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;
		
			-- Purge DeletedOrderItem data
			SELECT @Count = @batchVolume, @RecordDeletedCount = 0
			WHILE (@Count = @batchVolume)
			BEGIN
				DELETE TOP (@batchVolume) doi
				  FROM DeletedOrderItem doi
				  JOIN #OrderHeaderId ohi ON doi.OrderHeader_ID = ohi.OrderHeaderID 
		
				SELECT @Count = @@rowcount
				SELECT @RecordDeletedCount = @RecordDeletedCount + @Count
			END
			SELECT @CodeLocation = 'DeletedOrderItem Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' DeletedOrderItem records deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			-- Purge OrderHeaderApplyNewVendorCostQueue data
			SELECT @Count = @batchVolume, @RecordDeletedCount = 0
			WHILE (@Count = @batchVolume)
			BEGIN
				DELETE TOP (@batchVolume) oh
				  FROM OrderHeaderApplyNewVendorCostQueue oh
				  JOIN #OrderHeaderId ohi ON oh.OrderHeader_ID = ohi.OrderHeaderID 
		
				SELECT @Count = @@rowcount
				SELECT @RecordDeletedCount = @RecordDeletedCount + @Count
			END
			SELECT @CodeLocation = 'OrderHeaderApplyNewVendorCostQueue Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' OrderHeaderApplyNewVendorCostQueue records deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			-- Purge OrderInvoice data
			SELECT @Count = @batchVolume, @RecordDeletedCount = 0
			WHILE (@Count = @batchVolume)
			BEGIN
				DELETE TOP (@batchVolume) oi
				  FROM OrderInvoice oi
				  JOIN #OrderHeaderId ohi ON oi.OrderHeader_ID = ohi.OrderHeaderID 
		
				SELECT @Count = @@rowcount
				SELECT @RecordDeletedCount = @RecordDeletedCount + @Count
			END
			SELECT @CodeLocation = 'OrderInvoice Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' OrderInvoice records deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			-- Purge OrderInvoice_Freight3Party data
			SELECT @Count = @batchVolume, @RecordDeletedCount = 0
			WHILE (@Count = @batchVolume)
			BEGIN
				DELETE TOP (@batchVolume) oif
				  FROM OrderInvoice_Freight3Party oif
				  JOIN #OrderHeaderId ohi ON oif.OrderHeader_ID = ohi.OrderHeaderID 
		
				SELECT @Count = @@rowcount
				SELECT @RecordDeletedCount = @RecordDeletedCount + @Count
			END
			SELECT @CodeLocation = 'OrderInvoice_Freight3Party Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' OrderInvoice_Freight3Party records deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			-- Purge OrderTransmissionOverride data
			SELECT @Count = @batchVolume, @RecordDeletedCount = 0
			WHILE (@Count = @batchVolume)
			BEGIN
				DELETE TOP (@batchVolume) oto
				  FROM OrderTransmissionOverride oto
				  JOIN #OrderHeaderId ohi ON oto.OrderHeader_ID = ohi.OrderHeaderID 
		
				SELECT @Count = @@rowcount
				SELECT @RecordDeletedCount = @RecordDeletedCount + @Count
			END
			SELECT @CodeLocation = 'OrderTransmissionOverride Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' OrderTransmissionOverride records deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			-- Purge ReturnOrderList data
			SELECT @Count = @batchVolume, @RecordDeletedCount = 0
			WHILE (@Count = @batchVolume)
			BEGIN
				DELETE TOP (@batchVolume) rol
				  FROM ReturnOrderList rol
				  JOIN #OrderHeaderId ohi ON rol.OrderHeader_ID = ohi.OrderHeaderID 
		
				SELECT @Count = @@rowcount
				SELECT @RecordDeletedCount = @RecordDeletedCount + @Count
			END
			SELECT @CodeLocation = 'ReturnOrderList Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' ReturnOrderList records deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			IF Object_id('tempdb..#EnvoiceId') IS NOT NULL
			DROP TABLE #EnvoiceId

			CREATE TABLE #EnvoiceId(
				[EnvoiceID] [int] NOT NULL,
			CONSTRAINT [PK_EnvoiceID] PRIMARY KEY CLUSTERED 
				(
					[EnvoiceID] ASC
				)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
			) ON [PRIMARY]

			INSERT INTO #EnvoiceId
			SELECT ei.EInvoice_Id
			  FROM Einvoicing_Invoices ei 
			  JOIN OrderHeader oh ON ei.Invoice_Num = oh.InvoiceNumber
			  JOIN #OrderHeaderId ohi ON oh.OrderHeader_ID = ohi.OrderHeaderID
		 LEFT JOIN ExternalOrderInformation eoi ON oh.OrderHeader_ID = eoi.OrderHeader_ID
			 WHERE (cast(eoi.ExternalOrder_Id  AS VARCHAR) = ei.po_num_clean 
				OR cast(oh.orderheader_id AS VARCHAR) = ei.po_num_clean)

			SELECT @RecordDeletedCount = @@rowcount

			SELECT @LogMsg = CAST(@RecordDeletedCount AS VARCHAR) + ' EInvoice records to be deleted are identified.';
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			DELETE eeh
			  FROM EInvoicing_ErrorHistory eeh
			  JOIN #EnvoiceId ei ON eeh.EInvoiceId = ei.EnvoiceID
			
			SELECT @RecordDeletedCount = @@rowcount

			SELECT @CodeLocation = 'EInvoicing_ErrorHistory Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' EInvoicing_ErrorHistory records were deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			DELETE ehd
			  FROM EInvoicing_HeaderData ehd
			  JOIN #EnvoiceId ei ON ehd.Einvoice_id = ei.EnvoiceID
			
			SELECT @RecordDeletedCount = @@rowcount

			SELECT @CodeLocation = 'EInvoicing_HeaderData Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' EInvoicing_HeaderData records were deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			DELETE eh
			  FROM Einvoicing_Header eh
			  JOIN #EnvoiceId ei ON eh.Einvoice_id = ei.EnvoiceID
			
			SELECT @RecordDeletedCount = @@rowcount

			SELECT @CodeLocation = 'Einvoicing_Header Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' Einvoicing_Header records were deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			SELECT @Count = @batchVolume, @RecordDeletedCount = 0
			WHILE (@Count = @batchVolume)
			BEGIN
				DELETE TOP (@batchVolume) eid
				  FROM EInvoicing_ItemData eid
				  JOIN #EnvoiceId ei ON eid.Einvoice_id = ei.EnvoiceID
			
				SELECT @Count = @@rowcount
				SELECT @RecordDeletedCount = @RecordDeletedCount + @Count
			END

			SELECT @CodeLocation = 'EInvoicing_ItemData Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' EInvoicing_ItemData records were deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			SELECT @Count = @batchVolume, @RecordDeletedCount = 0
			WHILE (@Count = @batchVolume)
			BEGIN
				DELETE TOP (@batchVolume) eit
				  FROM EInvoicing_Item eit
				  JOIN #EnvoiceId ei ON eit.Einvoice_id = ei.EnvoiceID
			
				SELECT @Count = @@rowcount
				SELECT @RecordDeletedCount = @RecordDeletedCount + @Count
			END

			SELECT @CodeLocation = 'EInvoicing_Item Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' EInvoicing_Item records were deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			DELETE esd
			  FROM EInvoicing_SummaryData esd
			  JOIN #EnvoiceId ei ON esd.Einvoice_id = ei.EnvoiceID
			
			SELECT @RecordDeletedCount = @@rowcount

			SELECT @CodeLocation = ' EInvoicing_SummaryData Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' EInvoicing_SummaryData records deleted:' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			DELETE oh
			--SELECT oh.*
				FROM ExternalOrderInformation oh
				JOIN #OrderHeaderId ohi ON oh.OrderHeader_ID = ohi.OrderHeaderID 
		
			SELECT @RecordDeletedCount = @@rowcount
		
			SELECT @CodeLocation = 'ExternalOrderInformation Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' ExternalOrderInformation records deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			-- Purge OrderHeader data

			SELECT @LogMsg = 'Disabling trigger [dbo].[OrderHeaderDel] and delete [dbo].[OrderHeader] records ...'
			SELECT @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			;DISABLE TRIGGER [dbo].[OrderHeaderDel] ON [dbo].[OrderHeader]

			SELECT @RecordDeletedCount = 0
		
			DELETE oh
			--SELECT oh.*
				FROM OrderHeader oh
				JOIN #OrderHeaderId ohi ON oh.OrderHeader_ID = ohi.OrderHeaderID 
		
			SELECT @RecordDeletedCount = @@rowcount

			;ENABLE TRIGGER [dbo].[OrderHeaderDel] ON [dbo].[OrderHeader]
		
			SELECT @CodeLocation = 'Trigger [dbo].[OrderHeaderDel] re-enabled. OrderHeader Purging Ends... '; SELECT @LogMsg = @CodeLocation + ' OrderHeader records deleted: ' + CAST(@RecordDeletedCount AS VARCHAR);
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

			UPDATE RetentionPolicy
			   SET LastPurgedDateTime = getdate()
			 WHERE RetentionPolicyId = @RetentionPolicyId

			SELECT @CodeLocation = 'Order Data Purge Ends...'
			SELECT @now = getdate(); 
			EXEC dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;
		END
	END
END