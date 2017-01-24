CREATE PROCEDURE [app].[PurgeMessageQueues] 
AS 
BEGIN
	-- Archive MessageQueue entries from the previous day.
	DECLARE 
		@ReadyMessageStatusId int = (select MessageStatusId from esb.MessageStatus where MessageStatusName = 'Ready')

	DECLARE @numberOfMessageQueueRecords INT = 
		(
			SELECT 
				COUNT(*) 
			FROM 
				esb.MessageQueueItemLocale 
			WHERE 
				MessageStatusId <> @ReadyMessageStatusId
				AND MessageHistoryId IS NULL
				AND ProcessedDate IS NULL
		)

	WHILE @numberOfMessageQueueRecords > 0
	BEGIN
		DELETE TOP (10000)
			esb.MessageQueueItemLocale
		OUTPUT
			deleted.* INTO esb.MessageQueueItemLocaleArchive
		WHERE
			MessageStatusId <> @ReadyMessageStatusId
			AND MessageHistoryId IS NULL
			AND ProcessedDate IS NULL

		SET @numberOfMessageQueueRecords = 
		(
			SELECT 
				COUNT(*) 
			FROM 
				esb.MessageQueueItemLocale 
			WHERE
				MessageStatusId <> @ReadyMessageStatusId
				AND MessageHistoryId IS NULL
				AND ProcessedDate IS NULL
		)
	END

	SET @numberOfMessageQueueRecords = 
		(
			SELECT 
				COUNT(*) 
			FROM 
				esb.MessageQueuePrice 
			WHERE 
				MessageStatusId <> @ReadyMessageStatusId
				AND MessageHistoryId IS NULL
				AND ProcessedDate IS NULL
		)
	
	WHILE @numberOfMessageQueueRecords > 0
	BEGIN
		DELETE  TOP (10000)
			esb.MessageQueuePrice
		OUTPUT
			deleted.* INTO esb.MessageQueuePriceArchive
		WHERE
			MessageStatusId <> @ReadyMessageStatusId
			AND MessageHistoryId IS NULL
			AND ProcessedDate IS NULL

		SET @numberOfMessageQueueRecords = 
		(
			SELECT 
				COUNT(*) 
			FROM 
				esb.MessageQueuePrice 
			WHERE 
				MessageStatusId <> @ReadyMessageStatusId
				AND MessageHistoryId IS NULL
				AND ProcessedDate IS NULL
		)
	END
END