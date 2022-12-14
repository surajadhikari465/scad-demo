CREATE TABLE [esb].[MessageArchive]
(
	[MessageArchiveID] INT IDENTITY(1,1) NOT NULL,
	[MessageID] NVARCHAR(50) NOT NULL,
	[MessageTypeID] INT NOT NULL,
	[MessageStatusID] INT NOT NULL,
	[MessageHeadersJson] NVARCHAR(MAX) NOT NULL,
	[MessageBody] XML NOT NULL,
	[InsertDateUtc] DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME()
	CONSTRAINT [PK_MessageArchiveID] PRIMARY KEY CLUSTERED ([MessageArchiveID] ASC) WITH (FILLFACTOR = 100),
	CONSTRAINT [FK_MessageArchive_MessageTypeID] FOREIGN KEY ([MessageTypeID]) REFERENCES [esb].[MessageType] ([MessageTypeId]),
	CONSTRAINT [FK_MessageArchive_MessageStatusID] FOREIGN KEY ([MessageStatusID]) REFERENCES [esb].[MessageStatus] ([MessageStatusId]),
)
GO

CREATE NONCLUSTERED INDEX IX_MessageArchive_MessageID ON [esb].[MessageArchive] ([MessageID] ASC) WITH (FILLFACTOR = 100)
GO

GRANT INSERT on [esb].[MessageArchive] to [TibcoRole]
GO
    
GRANT INSERT on [esb].[MessageArchive] to [MammothRole]
GO