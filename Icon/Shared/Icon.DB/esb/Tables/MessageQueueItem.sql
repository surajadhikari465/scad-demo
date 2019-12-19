CREATE TABLE esb.MessageQueueItem
(
    MessageQueueItemId BIGINT NOT NULL IDENTITY(1,1),
    EsbReadyDateTimeUtc DATETIME2(7) NOT NULL, -- when item change can flow to the ESB
    ItemId INT NOT NULL,
    InsertDateUtc DATETIME2(7) NOT NULL, 
    CONSTRAINT [PK_MessageQueueItem] PRIMARY KEY ([MessageQueueItemId])
);

GO