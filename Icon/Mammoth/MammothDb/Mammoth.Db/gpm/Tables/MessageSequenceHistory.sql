CREATE TABLE [gpm].[MessageSequenceHistory]
(
	[MessageSequenceID] INT NOT NULL,
    [PatchFamilyID] NVARCHAR(50) NOT NULL, 
    [PatchNumber] INT NOT NULL,
	[GpmMessageId] UNIQUEIDENTIFIER NOT NULL,
    [InsertDateUtc] DATETIME2(7) NOT NULL,
	[ModifiedDateUtc] DATETIME2(7) NULL,
	[HistoryInsertDateUtc] [datetime2](7) NOT NULL DEFAULT (SYSUTCDATETIME())
)
GO

CREATE NONCLUSTERED INDEX [IX_MessageSequenceHistory_MessageSequenceID] ON [gpm].[MessageSequenceHistory] ([MessageSequenceID])
GO

CREATE NONCLUSTERED INDEX [IX_MessageSequenceHistory_PatchFamilyID] ON [gpm].[MessageSequenceHistory] ([PatchFamilyID])
GO
