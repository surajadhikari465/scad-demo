CREATE PROCEDURE [dbo].[EIM_UploadSession]
        @UploadSession_ID integer,
        @FromSLIM bit,
		@UserID int,				-- Task 2178
        @Succeded bit OUTPUT
AS
        set nocount on

        -- ******************************************************
        -- Upload item, price, and cost data for EIM
        -- David Marine
        -- Modified to allow Item Deletes -  Alex Z 01/01/2009
        -- Modified - rows with upload exclusions should not be uploaded - Victoria A. 01/28/2011  
        -- ******************************************************
        
        DECLARE @Upload_Exclusion_UploadAttribute_ID INT
        
        SELECT  @Upload_Exclusion_UploadAttribute_ID = UploadAttribute_ID 
		FROM UploadAttribute 
		WHERE NAME = 'Upload_Exclusion'

        DECLARE
                @UploadToItemsStore bit,
                @IsNewItemSessionFlag bit,
                @IsDeleteItemSessionFlag bit,
                @HasItemData bit,
                @HasPriceData bit,
                @HasCostData bit,
                @UploadRow_ID int,
                @Item_Key int,
                @PreviousIdentifier varchar(20),
                @CurrentIdentifier varchar(20),
                @RetryRowUpload bit,
                @RetryCount int,
                @MaxRetryCount int,
                @FourLevelHierarchyFlag bit,
                @UseStoreJurisdictions bit,
                @IsDefaultJurisdction bit,
                @LoggingLevel varchar(10)

        -- used to recover log statements that are otherwise rolled back
        -- when a row-level transaction rolls back
        DECLARE @UploadLog_Temp As Table
                (
                        EntryType varchar(10) NOT NULL,
                        UploadSession_ID int NOT NULL,
                        UploadRow_ID int NULL,
                        RetryCount int NULL,
                        Item_Key int NULL,
                        Identifier varchar(13) NULL,
                        [Timestamp] datetime NOT NULL,
                        LogText varchar(max) NOT NULL
                )
        
        -- set the retry variables
        SET @RetryRowUpload = 0
        SET @RetryCount = 0
        SET @MaxRetryCount = 10
        
        SET @Succeded = 0

        -- clear out any log entries older than one month
        DELETE FROM dbo.UploadLog where [Timestamp] < DATEADD(mm, -6, GETDATE())
        
        -- determine store upload type
        SELECT @UploadToItemsStore = CASE WHEN count(*) = 0 THEN 1 ELSE 0 END
        FROM dbo.UploadSession (NOLOCK)
        JOIN dbo.UploadSessionUploadType (NOLOCK)
                ON UploadSessionUploadType.UploadSession_ID = UploadSession.UploadSession_ID
        JOIN dbo.UploadSessionUploadTypeStore (NOLOCK)
                ON dbo.UploadSessionUploadTypeStore.UploadSessionUploadType_ID = UploadSessionUploadType.UploadSessionUploadType_ID
        WHERE UploadSession.UploadSession_ID = @UploadSession_ID
        
        ----------------------------------------------------------------
        -- Check if the session is for creating new items or not
        ----------------------------------------------------------------

        SELECT
                @IsNewItemSessionFlag = ISNULL(IsNewItemSessionFlag,0),
                @IsDeleteItemSessionFlag = ISNULL(IsDeleteItemSessionFlag,0)
        FROM
                UploadSession (NOLOCK)
        WHERE
                UploadSession_id = @UploadSession_ID
                        
        SET @PreviousIdentifier = NULL
        
                -- get the flags
        SELECT @UseStoreJurisdictions = dbo.fn_InstanceDataValue('UseStoreJurisdictions', NULL)
        SELECT @FourLevelHierarchyFlag = dbo.fn_InstanceDataValue('FourLevelHierarchy', NULL)

        DECLARE @DoEIMTraceLogging bit

        SELECT @DoEIMTraceLogging = dbo.fn_InstanceDataValue('DoEIMTraceLogging', NULL)

        SELECT @LoggingLevel = CASE WHEN @DoEIMTraceLogging = 1 THEN 'TRACE' ELSE 'ERROR' END
        
        EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, NULL, NULL, @Item_key, NULL, '1.0 Upload Session -   [Begin Session Upload]'

        -- set flags for types of data in the uploaded session
        SELECT @HasItemData = CASE WHEN EXISTS(
                                        SELECT 1
                                        FROM UploadSessionUploadType ut (NOLOCK)
                                        WHERE ut.UploadSession_ID = @UploadSession_ID and ut.UploadType_Code = 'ITEM_MAINTENANCE'
                                        ) THEN 1 ELSE 0 END

        SELECT @HasPriceData = CASE WHEN EXISTS(
                                        SELECT 1
                                        FROM UploadSessionUploadType ut (NOLOCK)
                                        WHERE ut.UploadSession_ID = @UploadSession_ID and ut.UploadType_Code = 'PRICE_UPLOAD'
                                        ) THEN 1 ELSE 0 END

        SELECT @HasCostData = CASE WHEN EXISTS(
                                        SELECT 1
                                        FROM UploadSessionUploadType ut (NOLOCK)
                                        WHERE ut.UploadSession_ID = @UploadSession_ID and ut.UploadType_Code = 'COST_UPLOAD'
                                        ) THEN 1 ELSE 0 END

        ----------------------------------------------------------------
        -- Step 1. Upload each row in three passes:
        --                      Pass 1: Item Data
        --                      Pass 2: Cost Data
        --                      Pass 3: Price Data
        ----------------------------------------------------------------

    -- The @UploadPass counter is used to 
        -- split uploading into three passes:
        -- items, costs, and prices;
        -- while not replicating the logic for stepping
        -- through the uploaded rows and handling deadlocks.

        -- During upload processing, locks are created on most of
        -- the important IRMA tables and are not released until the transaction
        -- containing them has committed or rolled back.
        -- If all the item, price, and cost processing for each row is done
        -- within one row-level transaction then any locks created during item
        -- processing will be held during cost and price processing as well.
        -- Similarly, locks created during cost processing would be held
        -- during price processing. In other words, any lock created during any
        -- part of the upload processing of a row would be held until the
        -- end of the processing of the row. This has resulted in other users
        -- being locked out or experiencing severe system-wide performance
        -- degredation during large EIM uploads of item, price, and cost changes
        -- to many stores.

        -- This three pass approach addresses this by allowing us to split the transaction
        -- for each row into three transactions, one for each pass,
        -- which contain the duration of any locks that occur during
        -- uploading to only that processing in which they are created.

        DECLARE @UploadPass int
        SET @UploadPass = 0

        WHILE @UploadPass <= 2
        BEGIN

                IF @UseStoreJurisdictions = 1 AND @IsNewItemSessionFlag = 1
                BEGIN
                        -- if the session is for new items and
                        -- the region uses store jurisdictions we need
                        -- to order the upload rows to process the rows
                        -- for the default jurisdiction first
                        DECLARE Row_cursor CURSOR FOR
                        
     
                        
                               --SELECT ur.UploadRow_ID, ur.Item_Key, ur.Identifier,
                                --        (
                                --        SELECT uv.Value
                                --        FROM UploadValue (NOLOCK) uv
                                --                INNER JOIN UploadAttribute (NOLOCK) ua
                                --                ON uv.UploadAttribute_ID = ua.UploadAttribute_ID
                                --        WHERE uv.UploadRow_ID = ur.UploadRow_ID
                                --                AND LOWER(ua.ColumnNameorKey) = 'isdefaultjurisdiction'
                                --        ) As IsDefaultJurisdction
                                --FROM UploadRow (NOLOCK) ur
                                --WHERE ur.UploadSession_ID = @UploadSession_ID and ur.ValidationLevel < 2
                                --ORDER BY IsDefaultJurisdction DESC, ur.Identifier
                                
                                

						-- Only pull rows with upload_exception = false - Victoria
						Select a.UploadRow_ID, a.Item_Key, a.Identifier, a.IsDefaultJurisdction
						from
						(
							SELECT ur.UploadRow_ID, ur.Item_Key, ur.Identifier,
                                        (
                                        SELECT uv.Value
                                        FROM UploadValue (NOLOCK) uv
                                                INNER JOIN UploadAttribute (NOLOCK) ua
                                                ON uv.UploadAttribute_ID = ua.UploadAttribute_ID
                                        WHERE uv.UploadRow_ID = ur.UploadRow_ID
                                                AND LOWER(ua.ColumnNameorKey) = 'isdefaultjurisdiction'
                                        )  As IsDefaultJurisdction,
                                                                             
                                        (                                      
                                        SELECT uv.Value
                                        FROM UploadValue (NOLOCK) uv

                                        WHERE uv.UploadRow_ID = ur.UploadRow_ID
                                                AND UploadAttribute_ID = @Upload_Exclusion_UploadAttribute_ID

                                        ) As Upload_Exclusion
                                                                                                                        
                                FROM UploadRow (NOLOCK) ur
                                WHERE ur.UploadSession_ID = @UploadSession_ID and ur.ValidationLevel < 2 

                                
						) as a   
	
						where a.Upload_Exclusion = 'False'
						ORDER BY a.IsDefaultJurisdction DESC, a.Identifier    
                                
                               
                END
                ELSE
                BEGIN
                        -- same cursor as above except no need to sort  
                        DECLARE Row_cursor CURSOR FOR
                        								
  
                                --SELECT ur.UploadRow_ID, ur.Item_Key, ur.Identifier, 1 As IsDefaultJurisdction
                                --FROM UploadRow (NOLOCK) ur
                                --WHERE ur.UploadSession_ID = @UploadSession_ID and ur.ValidationLevel < 2
                                --ORDER BY ur.Identifier
  
  
								-- Only pull rows with upload_exception = false - Victoria   
								                           
                                Select a.UploadRow_ID, a.Item_Key, a.Identifier, a.IsDefaultJurisdction
								from
								(			                                                                                                                              
									SELECT ur.UploadRow_ID, ur.Item_Key, ur.Identifier, 1 As IsDefaultJurisdction,                                       
                                       (                                      
                                        SELECT uv.Value
                                        FROM UploadValue (NOLOCK) uv
                                        WHERE uv.UploadRow_ID = ur.UploadRow_ID
                                        AND UploadAttribute_ID = @Upload_Exclusion_UploadAttribute_ID
                                        ) As Upload_Exclusion
                                                                                                                        
                                FROM UploadRow (NOLOCK) ur
                                WHERE ur.UploadSession_ID = @UploadSession_ID and ur.ValidationLevel < 2                                                               
								) as a   
	
								where a.Upload_Exclusion = 'False'
								ORDER BY  a.Identifier
                           
                END
                
                -- open and fetch first row
                OPEN Row_cursor
                FETCH NEXT FROM Row_cursor INTO @UploadRow_ID, @Item_Key, @CurrentIdentifier, @IsDefaultJurisdction

                WHILE @RetryRowUpload = 1 OR @@FETCH_STATUS = 0
                BEGIN
                
                                BEGIN TRY
                
                                        BEGIN TRANSACTION
                                        
                                        -- append the "is default jurisdiction flag"
                                        -- so we create both the default and alternate
                                        -- jurisdictions as needed
                                        SET @CurrentIdentifier = @CurrentIdentifier + CAST(@IsDefaultJurisdction as varchar(1))

                                        EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '1.1 Upload Session - [Begin Row]'

                                        ----------------------------------------------------------------
                                        -- Pass 1. Create new or just update existing item for the row
                                        --         according to the IsNewItemSessionFlag value
                                        ----------------------------------------------------------------
                                        
                                        -- do item processing only for the first pass
                                        -- and check to see if we are suppose to do an item upload for the session                      
                                        IF @UploadPass = 0 AND @HasItemData = 1
                                        BEGIN
                                                -- if the upload doesn't include the IsDefaultJurisdction flag
                                                -- then assume it is true
                                                SET @IsDefaultJurisdction = ISNULL(@IsDefaultJurisdction, 1)

                                                -- only create one item per identifier
                                                IF @IsNewItemSessionFlag = 1 AND @IsDefaultJurisdction = 1 AND
                                                        (@PreviousIdentifier IS NULL OR @PreviousIdentifier <> @CurrentIdentifier) AND
                                                                @IsDeleteItemSessionFlag = 0
                                                BEGIN
                                                        -- this is a new item session
                                                        -- nothing will happen if the session has no item upload data
                                                        EXEC dbo.EIM_UploadSession_NewItems @UploadSession_ID, @UploadRow_ID, @RetryCount,
                                                                @FourLevelHierarchyFlag, @UseStoreJurisdictions, @LoggingLevel, @Item_Key OUTPUT
                                                END
                                                

                                                -- if this is a delete item session ...
                                                IF @IsDeleteItemSessionFlag = 1
                                                BEGIN
                                                EXEC dbo.EIM_UploadSession_DeleteItems @UploadSession_ID, @UploadRow_ID, @RetryCount, 
                                                        @LoggingLevel, @Item_Key, @UserID       -- Task 2178
                                                END
                                                ELSE
                                                BEGIN
                                                -- update the items whether or not it is a new item session (but not delete session)
                                                -- this allows for the updating of item data that is not passed
                                                -- as parameters into the InsertItem proc
                                                EXEC dbo.EIM_UploadSession_ExistingItems @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_Key,
                                                        @FourLevelHierarchyFlag, @UseStoreJurisdictions, @LoggingLevel
                                                        
                                                END
                                        END
                                                                        
                                        ----------------------------------------------------------------
                                        -- Pass 2. Upload row's cost data
                                        ----------------------------------------------------------------

                                        -- do cost processing only for the second pass
                                        -- and check to see if we are suppose to do a cost upload for the session
                                        IF @UploadPass = 1 AND @HasCostData = 1
                                        BEGIN
                                                EXEC EIM_UploadSessionCost @UploadSession_ID, @UploadRow_ID, @RetryCount, 
                                                        @Item_Key, @UploadToItemsStore, @LoggingLevel
                                        END
                                        
                                        ----------------------------------------------------------------
                                        -- Pass 3. Upload row's price data
                                        ----------------------------------------------------------------

                                        -- do price processing only for the third pass
                                        -- and check to see if we are suppose to do a price upload for the session                      
                                        IF @UploadPass = 2 AND @HasPriceData = 1
                                        BEGIN
                                                EXEC EIM_UploadSessionPrice @UploadSession_ID, @UploadRow_ID, @RetryCount, 
                                                        @Item_Key, @UploadToItemsStore, @LoggingLevel
                                        END
                                        
                                        ----------------------------------------------------------------
                                        -- Step 4. Update the Status flag of the SLIM items
                                        -- if loaded from SLIM
                                        ----------------------------------------------------------------
                                        
                                        IF @FromSLIM = 1
                                        BEGIN
                                        
                                                UPDATE ItemRequest SET ItemStatus_ID = 3
                                                WHERE ItemRequest_ID IN (SELECT ItemRequest_ID FROM UploadRow (NOLOCK) WHERE UploadRow_ID = @UploadRow_ID)
                                        END

                                        -- we successfully uploaded the row so
                                        -- clear the retry flag to continue
                                        -- with the next row
                                        IF @RetryRowUpload = 1
                                        BEGIN
                                                SET @RetryRowUpload = 0
                                                SET @RetryCount = 0
                                        END
                                        
                                        -- for only creating one item per identifier
                                        SET @PreviousIdentifier = @CurrentIdentifier                    

                                        COMMIT TRANSACTION

                                        EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '6.0 Upload Session - [End Row]'
                                        
                                END TRY
                                BEGIN CATCH

                                        -- hold on to any log entries
                                        -- for the current upload row
                                        -- before they get rolled back

                                        DELETE FROM @UploadLog_Temp

                                        INSERT INTO @UploadLog_Temp
                                                        SELECT
                                                                EntryType,
                                                                UploadSession_ID,
                                                                UploadRow_ID,
                                                                RetryCount,
                                                                Item_Key,
                                                                Identifier,
                                                                [Timestamp],
                                                                LogText
                                                        FROM dbo.UploadLog (NOLOCK)
                                                        WHERE UploadRow_ID = @UploadRow_ID
                                                        AND RetryCount = @RetryCount

                                        -- an error has occurred so rollback any partial changes to the row
                                        ROLLBACK TRANSACTION

                                        -- now re-insert the rolled back
                                        -- log entries for the upload row
                                        INSERT INTO dbo.UploadLog
                                                        SELECT
                                                                EntryType,
                                                                UploadSession_ID,
                                                                UploadRow_ID,
                                                                RetryCount,
                                                                Item_Key,
                                                                Identifier,
                                                                [Timestamp],
                                                                LogText
                                                        FROM @UploadLog_Temp
                                                        WHERE UploadRow_ID = @UploadRow_ID

                                        -- capture the error data
                                        DECLARE @ErrorNumber INT,
                                                @ErrorMessage VARCHAR(8000),
                                                @Severity INT

                                        SELECT @ErrorNumber = ERROR_NUMBER()

                                        SELECT @ErrorMessage = ERROR_MESSAGE()

                                        IF @ErrorMessage NOT LIKE '%Error Number: %'
                                        BEGIN
                                        SELECT @ErrorMessage =
                                                        'Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR(100)) + ' | ' +
                                                        'Line No: ' + CAST(ERROR_LINE() AS VARCHAR(100)) + ' | ' +
                                                        @ErrorMessage
                                        END

                                        -- check the error type
                                        IF @ErrorNumber = 1205 -- deadlock error
                                                        OR @ErrorMessage LIKE '%Error Number: 1205%'    -- this is to catch rethrown deadlock errors which always
                                                                                                                                                        -- overwrite the error # with 50000
                                        BEGIN

                                                -- we have a deadlock with another upload or other client activity
                                                -- so let's retry uploading the row
                                                                                        
                                                -- start counting the retries
                                                SET @RetryCount = @RetryCount + 1

                                                
                                                EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '6.1 Upload Session - [Retry Row]'
                                                
                                                -- give up if we have reached our limit
                                                IF @RetryCount > @MaxRetryCount
                                                BEGIN
                                                        -- throw the error out of the proc
                                                        -- thus ending the upload

                                                        SET @ErrorMessage = '6.2 Upload Session [Max Deadlock Retry Error] - Error Message:' + @ErrorMessage
                                                        EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, @ErrorMessage
                                        
                                                        SET @Succeded = 0

                                                        CLOSE Row_cursor
                                                        DEALLOCATE Row_cursor
                                                        
                                                        RETURN
                                                END
                                                
                                                -- we still have some retries so set the flag
                                                SET @RetryRowUpload = 1
                                                
                                                -- let's wait a second before retrying
                                                WAITFOR DELAY '00:00:01'
                                        END
                                        ELSE
                                        BEGIN
                                                -- an error has occurred that is not
                                                -- a deadlock so
                                                -- throw the error out of the proc
                                                -- thus ending the upload
                                                SET @ErrorMessage = '6.3 Upload Session [Process Row] - Error Message:' + @ErrorMessage
                                                EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, @ErrorMessage
                                        
                                                SET @Succeded = 0

                                                CLOSE Row_cursor
                                                DEALLOCATE Row_cursor
                                                        
                                                RETURN
                                        END
                                        
                                END CATCH

                        -- only load the next row if not retrying
                        If @RetryRowUpload = 0
                        BEGIN
                                -- fetch next row
                                FETCH NEXT FROM Row_cursor INTO @UploadRow_ID,@Item_Key, @CurrentIdentifier, @IsDefaultJurisdction
                        END
                END

                CLOSE Row_cursor
                DEALLOCATE Row_cursor

                -- increment the pass counter
                SET @UploadPass = @UploadPass + 1
        
        END -- @UploadPass <= 3
        ----------------------------------------------------------------
        -- Step 6. Update the UploadSession after Finish
        ----------------------------------------------------------------

        update UploadSession set IsUploaded = 1 where  UploadSession_ID = @UploadSession_ID

        -- set the succeded flag and commit the
        -- transaction
        SET @Succeded = 1

        EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, NULL, NULL, NULL, NULL, '6.4 Upload Session -        [End Session Upload]'
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_UploadSession] TO [IRMAClientRole]
    AS [dbo];

