CREATE TABLE [gpm].[PriceResetMessageHistory] (
    [PriceResetMessageHistoryId]	INT					IDENTITY (1, 1) NOT NULL,
	[MessageId]						UNIQUEIDENTIFIER	NOT NULL,
    [MessageTypeId]					INT					NOT NULL,
    [MessageStatusId]				INT					NOT NULL,
    [Message]						XML					NOT NULL,
	[MessageProperties]				NVARCHAR(MAX)		NOT NULL,
    [InsertDate]					DATETIME2 (7)		CONSTRAINT [DF_PriceResetMessageHistory_InsertDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_PriceResetMessageHistoryId] PRIMARY KEY CLUSTERED ([PriceResetMessageHistoryId] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_PriceResetMessageHistory_MessageStatus] FOREIGN KEY ([MessageStatusId]) REFERENCES [esb].[MessageStatus] ([MessageStatusId]),
    CONSTRAINT [FK_PriceResetMessageHistory_MessageType] FOREIGN KEY ([MessageTypeId]) REFERENCES [esb].[MessageType] ([MessageTypeId])
);