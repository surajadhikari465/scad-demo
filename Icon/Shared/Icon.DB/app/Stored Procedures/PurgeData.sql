CREATE PROCEDURE [app].[PurgeData]
AS
BEGIN
	-- Archive MessageQueue entries from the previous day.
	DECLARE 
		@FailedMessageStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Failed'),
		@ReadyMessageStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')

	PRINT 'Moving rows to app.MessageQueueItemLocalArchive table...'
	DELETE
		app.MessageQueueItemLocale
	OUTPUT
		deleted.* INTO app.MessageQueueItemLocaleArchive
	WHERE
		InsertDate < cast(getdate() as date)
		and MessageStatusId not in (@FailedMessageStatusId, @ReadyMessageStatusId);

	PRINT 'Moving rows to app.MessageQueuePriceArchive table...'
	DELETE
		app.MessageQueuePrice
	OUTPUT
		deleted.* INTO app.MessageQueuePriceArchive
	WHERE
		InsertDate < cast(getdate() as date)
		and MessageStatusId not in (@FailedMessageStatusId, @ReadyMessageStatusId);

	-- Execute Delete commands based on RetentionPolicy table.
	IF OBJECT_ID('tempdb..#DeteleCommands') IS NOT NULL
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

			while (@count > 0)
				begin
					DELETE top(@count) FROM ' +
					QUOTENAME(rp.[Server]) + '.' +
					QUOTENAME(rp.[Database]) + '.' +
					QUOTENAME(rp.[Schema]) + '.' +
					QUOTENAME(rp.[Table]) +
					' WHERE InsertDate < ' +
					'DATEADD(d, -' +
					convert(nvarchar(8), rp.DaysToKeep) +
					', GETDATE())

					set @count = @@rowcount
				end
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
