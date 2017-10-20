CREATE TABLE [gpm].[MessageArchivePriceR10]
(
	[MessageArchivePriceR10ID] INT IDENTITY (1,1) NOT NULL,
	[ItemID] INT NOT NULL,
	[BusinessUnitID] INT NOT NULL,
	[PriceType] NVARCHAR(3) NOT NULL,
	[StartDate] DATETIME2(7) NOT NULL,
	[MessageID] NVARCHAR(50) NOT NULL,
	[MessageDetailJson] NVARCHAR(MAX) NOT NULL,
	[InsertDateUtc] DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
	CONSTRAINT [PK_MessageArchivePriceR10] PRIMARY KEY (MessageArchivePriceR10ID)
)
GO

CREATE NONCLUSTERED INDEX IX_gpmMessageArchivePriceR10_MessageID ON [gpm].[MessageArchivePriceR10] (MessageID ASC) WITH (FILLFACTOR = 100)
GO

GRANT INSERT on [gpm].[MessageArchivePriceR10] to [TibcoRole]
GO
