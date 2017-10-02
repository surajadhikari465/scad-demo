CREATE TABLE [gpm].[MessageSequenceHistory]
(
	[MessageSequenceID] INT NOT NULL,
	[ItemID] INT NOT NULL,
	[BusinessUnitID] INT NOT NULL,
    [PatchFamilyID] NVARCHAR(50) NOT NULL, 
    [PatchFamilySequenceID] NVARCHAR(50) NOT NULL,
	[MessageID] NVARCHAR(50) NULL,
    [InsertDateUtc] DATETIME2(7) NOT NULL,
	[ModifiedDateUtc] DATETIME2(7) NULL,
	[HistoryInsertDateUtc] [datetime2](7) NOT NULL DEFAULT (SYSUTCDATETIME())
)
GO

CREATE NONCLUSTERED INDEX [IX_MessageSequenceHistory_MessageSequenceID] ON [gpm].[MessageSequenceHistory] ([MessageSequenceID])
GO

CREATE NONCLUSTERED INDEX [IX_MessageSequenceHistory_PatchFamilyID] ON [gpm].[MessageSequenceHistory] ([PatchFamilyID])
GO

GRANT UPDATE, INSERT, SELECT ON [gpm].[MessageSequenceHistory] TO [TibcoRole]
GO