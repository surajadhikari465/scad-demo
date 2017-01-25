CREATE PROCEDURE [app].[PurgeMessageQueues] 
AS 
BEGIN
	-- Archive MessageQueue entries from the previous day.
	DECLARE 
		@ReadyMessageStatusId int = (select MessageStatusId from esb.MessageStatus where MessageStatusName = 'Ready'),
		@FailedMessageStatusId int = (select MessageStatusId from esb.MessageStatus where MessageStatusName = 'Failed')

	DECLARE @numberOfMessageQueueRecords INT = 
		(
			SELECT 
				COUNT(*) 
			FROM 
				esb.MessageQueueItemLocale 
			WHERE 
				InProcessBy IS NULL
				AND ((MessageStatusId <> @ReadyMessageStatusId AND MessageHistoryId IS NOT NULL AND ProcessedDate IS NOT NULL)
				OR MessageStatusId = @FailedMessageStatusId)
		)

	WHILE @numberOfMessageQueueRecords > 0
	BEGIN
		DELETE TOP (10000)
			esb.MessageQueueItemLocale
		OUTPUT
			deleted.* INTO esb.MessageQueueItemLocaleArchive
		WHERE 
			InProcessBy IS NULL
			AND ((MessageStatusId <> @ReadyMessageStatusId AND MessageHistoryId IS NOT NULL AND ProcessedDate IS NOT NULL)
			OR MessageStatusId = @FailedMessageStatusId)

		SET @numberOfMessageQueueRecords = 
		(
			SELECT 
				COUNT(*) 
			FROM 
				esb.MessageQueueItemLocale 
			WHERE 
				InProcessBy IS NULL
				AND ((MessageStatusId <> @ReadyMessageStatusId AND MessageHistoryId IS NOT NULL AND ProcessedDate IS NOT NULL)
				OR MessageStatusId = @FailedMessageStatusId)
		)
	END

	SET @numberOfMessageQueueRecords = 
		(
			SELECT 
				COUNT(*) 
			FROM 
				esb.MessageQueuePrice 
			WHERE 
				InProcessBy IS NULL
				AND ((MessageStatusId <> @ReadyMessageStatusId AND MessageHistoryId IS NOT NULL AND ProcessedDate IS NOT NULL)
				OR MessageStatusId = @FailedMessageStatusId)
		)
	
	WHILE @numberOfMessageQueueRecords > 0
	BEGIN
		DELETE  TOP (10000)
			esb.MessageQueuePrice
		OUTPUT
			deleted.* INTO esb.MessageQueuePriceArchive
		WHERE
			InProcessBy IS NULL
			AND ((MessageStatusId <> @ReadyMessageStatusId AND MessageHistoryId IS NOT NULL AND ProcessedDate IS NOT NULL)
			OR MessageStatusId = @FailedMessageStatusId)

		SET @numberOfMessageQueueRecords = 
		(
			SELECT 
				COUNT(*) 
			FROM 
				esb.MessageQueuePrice 
			WHERE 
				InProcessBy IS NULL
				AND ((MessageStatusId <> @ReadyMessageStatusId AND MessageHistoryId IS NOT NULL AND ProcessedDate IS NOT NULL)
				OR MessageStatusId = @FailedMessageStatusId)
		)
	END
END