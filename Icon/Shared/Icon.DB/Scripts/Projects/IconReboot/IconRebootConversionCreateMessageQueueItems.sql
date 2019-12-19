DECLARE @itemIds esb.MessageQueueItemIdsType

INSERT INTO @itemIds
SELECT ItemId
	,SYSUTCDATETIME()
	,SYSUTCDATETIME()
FROM dbo.item

EXEC esb.AddMessageQueueItem @itemIds