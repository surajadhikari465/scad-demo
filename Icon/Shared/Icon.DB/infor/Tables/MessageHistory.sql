CREATE TABLE [infor].[MessageHistory]
(
	[MessageHistoryId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [MessageTypeId] INT NOT NULL, 
    [MessageStatusId] INT NOT NULL, 
    [Message] XML NOT NULL, 
	[InforMessageId] UNIQUEIDENTIFIER NOT NULL,
    [MessageProperties] NVARCHAR(MAX) NULL, 
    [InsertDate] DATETIME2 (7) CONSTRAINT [DF_InforMessageHistory_InsertDate] DEFAULT (sysdatetime()) NOT NULL
)