CREATE TABLE [amz].[MessageArchive]
(   [MessageArchiveID] INT Identity(1,1) NOT NULL, 
	[QueueID] INT,
	[EventType] NVARCHAR(25) NOT NULL,
	[KeyID] INT NOT NULL,
	[SecondaryKeyID] INT NULL,
	[InsertDate] DATETIME2(7) NOT NULL  CONSTRAINT [DF_MessageArchive_InsertDate] DEFAULT (SYSDATETIME()),
	[Status] NCHAR(1) NOT NULL,
	[ErrorDescription] NVARCHAR(MAX) NULL,
	[LastProcessedTime] DATETIME2(7) NULL,
	[MessageTimestampUtc] DATETIME2(7) NOT NULL,
	[Message] XML NOT NULL,
	[ProcessTimes] SMALLINT NOT NULL Default(0),
	[LastReprocess] DATETIME2(7) NULL,
	[LastReprocessID] NCHAR(13) NULL
)