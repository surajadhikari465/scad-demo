DECLARE @itemIds esb.MessageQueueItemIdsType

INSERT INTO @itemIds (ItemId, EsbReadyDateTimeUtc, InsertDateUtc)
SELECT ItemId
	,SYSUTCDATETIME()
	,SYSUTCDATETIME()
FROM dbo.item

EXEC esb.AddMessageQueueItem @itemIds