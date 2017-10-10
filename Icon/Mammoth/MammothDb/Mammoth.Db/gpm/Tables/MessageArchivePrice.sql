CREATE TABLE [gpm].[MessageArchivePrice]
(
	[MessageArchivePriceID] INT IDENTITY (1,1) NOT NULL, 
    [ItemID] INT NOT NULL,
    [BusinessUnitID] INT NOT NULL,
    [MessageID] NVARCHAR(50) NULL,
	[MessageHeaders] NVARCHAR(MAX) NOT NULL,
    [Message] XML NOT NULL,
	[ErrorCode] NVARCHAR(100) NULL,
	[ErrorDetails] NVARCHAR(MAX) NULL,
    [InsertDateUtc] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
	CONSTRAINT [PK_MessageArchivePrice] PRIMARY KEY (MessageArchivePriceID)
)
GO

CREATE NONCLUSTERED INDEX IX_gpmMessageArchivePrice_MessageID ON [gpm].[MessageArchivePrice] (MessageID ASC) WITH (FILLFACTOR = 100)
GO

GRANT INSERT on [gpm].[MessageArchivePrice] to [TibcoRole]
GO
