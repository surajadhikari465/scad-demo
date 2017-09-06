CREATE TABLE [gpm].[MessageSequence]
(
	[MessageSequenceID] INT IDENTITY(1,1) NOT NULL,
	[ItemID] INT NOT NULL,
	[BusinessUnitID] INT NOT NULL,
    [PatchFamilyID] NVARCHAR(50) NOT NULL, 
    [GpmSequenceID] INT NOT NULL,
	[GpmMessageID] UNIQUEIDENTIFIER NULL,
    [InsertDateUtc] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
	[ModifiedDateUtc] DATETIME2(7) NULL,
	CONSTRAINT [PK_MessageSequence] PRIMARY KEY CLUSTERED ([MessageSequenceID] ASC) WITH (FILLFACTOR = 100),
	CONSTRAINT [UNQ_PatchFamilyID] UNIQUE (PatchFamilyID)
)

GO

CREATE TRIGGER [gpm].[Trigger_MessageSequence]
    ON [gpm].[MessageSequence]
    FOR UPDATE
    AS
    BEGIN
        SET NOCOUNT ON

		INSERT INTO [gpm].[MessageSequenceHistory]
		(
			MessageSequenceID,
			ItemID,
			BusinessUnitID,
			PatchFamilyID,
			GpmSequenceID,
			GpmMessageId,
			InsertDateUtc,
			ModifiedDateUtc,
			HistoryInsertDateUtc
		)
		SELECT
			deleted.ItemID,
			deleted.BusinessUnitID,
			deleted.MessageSequenceID,
			deleted.PatchFamilyID,
			deleted.[GpmSequenceID],
			deleted.GpmMessageId,
			deleted.InsertDateUtc,
			deleted.ModifiedDateUtc,
			SYSUTCDATETIME() as HistoryInsertDateUtc
		FROM deleted
    END
GO

CREATE NONCLUSTERED INDEX [IX_MessageSequence_PatchFamilyID] ON [gpm].[MessageSequence] ([PatchFamilyID])
GO



GRANT INSERT, UPDATE, SELECT ON [gpm].[MessageSequence] TO [TibcoRole]
GO
