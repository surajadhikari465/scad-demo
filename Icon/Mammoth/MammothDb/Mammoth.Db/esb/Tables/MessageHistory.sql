CREATE TABLE [esb].[MessageHistory] (
    [MessageHistoryId] INT				IDENTITY (1, 1) NOT NULL,
	[EsbMessageId]     UNIQUEIDENTIFIER	NULL,
    [MessageTypeId]    INT				NOT NULL,
    [MessageStatusId]  INT				NOT NULL,
    [Message]          XML				NOT NULL,
    [InsertDate]       DATETIME2 (7)	CONSTRAINT [DF_MessageHistory_InsertDate] DEFAULT (sysdatetime()) NOT NULL,
    [InProcessBy]      INT				NULL,
    [ProcessedDate]    DATETIME2 (7)	NULL,
    CONSTRAINT [PK_MessageHistoryId] PRIMARY KEY CLUSTERED ([MessageHistoryId] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_MessageHistory_MessageStatus] FOREIGN KEY ([MessageStatusId]) REFERENCES [esb].[MessageStatus] ([MessageStatusId]),
    CONSTRAINT [FK_MessageHistory_MessageType] FOREIGN KEY ([MessageTypeId]) REFERENCES [esb].[MessageType] ([MessageTypeId])
);

