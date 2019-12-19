CREATE PROCEDURE [esb].[DequeueMessageQueueItem] 
@maxRecords INT
AS

WITH DequeueMessageQueueItemCte
AS (
	SELECT TOP (@maxRecords) *
	FROM [esb].MessageQueueItem WITH (
			ROWLOCK
			,READPAST
			)
    WHERE EsbReadyDateTimeUtc < SYSUTCDATETIME ()
	ORDER BY MessageQueueItemId
	)
DELETE
FROM DequeueMessageQueueItemCte
OUTPUT deleted.MessageQueueItemId
	,deleted.ItemId
	,deleted.EsbReadyDateTimeUtc
	,deleted.InsertDateUtc
