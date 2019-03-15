CREATE TABLE [app].[MessageHistory] (
    [MessageHistoryId] INT           IDENTITY (1, 1) NOT NULL,
    [MessageTypeId]    INT           NOT NULL,
    [MessageStatusId]  INT           NOT NULL,
    [Message]          XML           NOT NULL,
    [MessageHeader]    NVARCHAR(MAX) NULL,
    [InsertDate]       DATETIME2 (7) CONSTRAINT [DF_MessageHistory_InsertDate] DEFAULT (sysdatetime()) NOT NULL,
	[InProcessBy]      INT NULL,
	[ProcessedDate]    DATETIME2 (7) NULL,
    CONSTRAINT [PK_MessageHistoryId] PRIMARY KEY CLUSTERED ([MessageHistoryId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_MessageHistory_MessageStatus] FOREIGN KEY ([MessageStatusId]) REFERENCES [app].[MessageStatus] ([MessageStatusId]),
    CONSTRAINT [FK_MessageHistory_MessageType] FOREIGN KEY ([MessageTypeId]) REFERENCES [app].[MessageType] ([MessageTypeId])
);


