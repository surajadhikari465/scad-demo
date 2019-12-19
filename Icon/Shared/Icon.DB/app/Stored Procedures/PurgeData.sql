CREATE PROCEDURE [app].[PurgeData]
AS
BEGIN
	-- Archive MessageQueue entries from the previous day.
	DECLARE 
		@ReadyMessageStatusId INT = (SELECT MessageStatusId FROM app.MessageStatus WHERE MessageStatusName = 'Ready'),
		@Today DATETIME = DATEADD(DAY, 0, DATEDIFF(DAY, 0, GETDATE())),
		@TotalDeletedCount INT = 0,
		@Count INT = 1;
		
	PRINT 'Moving rows to app.MessageQueueItemLocalArchive table at: ' + CONVERT(nvarchar, GETDATE(), 121)
	WHILE @Count > 0
	BEGIN
		DELETE TOP (20000)
			app.MessageQueueItemLocale
		OUTPUT
			deleted.* INTO app.MessageQueueItemLocaleArchive
		WHERE
			InsertDate < @Today
			and MessageStatusId <> (@ReadyMessageStatusId)

		SET @Count = @@ROWCOUNT
		SET @TotalDeletedCount += @Count
	END
	PRINT 'Deleted ' + CAST(@TotalDeletedCount AS NVARCHAR) + ' rows from app.MessageQueueItemLocale table.';

	SET @Count = 1;
	SET @TotalDeletedCount = 0;
	PRINT 'Moving rows to app.MessageQueuePriceArchive table at: ' + CONVERT(nvarchar, GETDATE(), 121)
	WHILE @Count > 0
	BEGIN
		DELETE TOP (20000)
			app.MessageQueuePrice
		OUTPUT
			deleted.* INTO app.MessageQueuePriceArchive
		WHERE
			InsertDate < @Today
			and MessageStatusId <> (@ReadyMessageStatusId);

		SET @Count = @@ROWCOUNT
		SET @TotalDeletedCount += @Count
	END
	PRINT 'Deleted ' + CAST(@TotalDeletedCount AS NVARCHAR) + ' rows from app.MessageQueuePrice table.';
	PRINT 'Finished archiving Message Queue tables.';

	PRINT 'Copying OutOfSync error rows from infor.MessageArchiveProduct to infor.MessageArchiveOutOfSync'

	INSERT INTO infor.MessageArchiveProductOutOfSync
		(ItemId
		,ScanCode
		,InforMessageId
		,Context
		,ErrorCode
		,ErrorDetails
		,InsertDate)
	SELECT
		ItemId
		,ScanCode
		,InforMessageId
		,Context
		,ErrorCode
		,ErrorDetails
		,InsertDate
	FROM infor.MessageArchiveProduct
	WHERE ErrorCode = 'OutOfSyncItemUpdateErrorCode'
		AND InsertDate > @Today - 1

	PRINT 'Finished copying OutOfSync error rows from infor.MessageArchiveProduct to infor.MessageArchiveOutOfSync';

	-- Execute Delete commands based on RetentionPolicy table.
	IF OBJECT_ID('tempdb..#DeleteCommands') IS NOT NULL
		BEGIN
			DROP TABLE #DeleteCommands
		END

	CREATE TABLE #DeleteCommands(Command NVARCHAR(max));

	PRINT 'Inserting delete commands...'
	INSERT INTO
		#DeleteCommands
	SELECT
		'
			declare @count int = 20000
			declare @deletedCount int = 0

			while (@count > 0)
				begin
					DELETE top(@count)' +
					QUOTENAME(rp.[Database]) + '.' +
					QUOTENAME(rp.[Schema]) + '.' +
					QUOTENAME(rp.[Table]) +
					' WHERE ' + QUOTENAME(rp.[ReferenceColumn]) + ' < ' +
					'DATEADD(d, -' +
					convert(nvarchar(8), rp.DaysToKeep) +
					', GETDATE())

					set @count = @@rowcount
					set @deletedCount += @count
				end
			PRINT ''Deleted '' + CAST(@deletedCount AS NVARCHAR) + '' rows''
		'      
	FROM
		app.RetentionPolicy rp

	DECLARE @Command nvarchar(max)

	DECLARE CommandsCursor CURSOR
		FOR SELECT * FROM #DeleteCommands
	OPEN CommandsCursor

	FETCH NEXT FROM CommandsCursor INTO @Command

	WHILE @@FETCH_STATUS = 0
	BEGIN
		PRINT ''
		PRINT 'Executing - ' + @Command
		EXECUTE sp_executesql @Command
		FETCH NEXT FROM CommandsCursor INTO @Command
	END

	CLOSE CommandsCursor
	DEALLOCATE CommandsCursor

	DROP TABLE #DeleteCommands
END
