CREATE TABLE [app].[MessageHistory] (
    [MessageHistoryId] INT            IDENTITY (1, 1) NOT NULL,
    [MessageTypeId]    INT            NOT NULL,
    [Message]          XML            NOT NULL,
    [MessageHeader]    NVARCHAR (MAX) NULL,
    [InsertDateUtc]    DATETIME2 (7)  CONSTRAINT [DF_MessageHistory_InsertDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_MessageHistoryId] PRIMARY KEY CLUSTERED ([MessageHistoryId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_MessageHistory_MessageType] FOREIGN KEY ([MessageTypeId]) REFERENCES [app].[MessageType] ([MessageTypeId])
);

