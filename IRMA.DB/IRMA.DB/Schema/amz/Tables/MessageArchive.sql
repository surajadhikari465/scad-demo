CREATE TABLE [amz].[MessageArchive]
(
	[QueueID] INT,
	[EventTypeID] INT NOT NULL,
	[KeyID] INT NOT NULL,
	[SecondaryKeyID] INT NULL,
	[InsertDate] DATETIME2(7) NOT NULL,
	[Status] NCHAR(1) NOT NULL,
	[ErrorDescription] NVARCHAR(MAX) NULL,
	[LastProcessedTime] DATETIME2(7) NULL,
	[MessageTimestampUtc] DATETIME2(7) NOT NULL,
	[Message] XML NOT NULL,
	[LastReprocess] DATETIME2(7) NULL,
	[LastReprocessID] NCHAR(13) NULL
)
