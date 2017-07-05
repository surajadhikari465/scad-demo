CREATE TABLE [gpm].[MessageSequence]
(
	[MessageSequenceID] INT IDENTITY(1,1) NOT NULL,
    [PatchFamilyID] NVARCHAR(50) NOT NULL, 
    [PatchNumber] INT NOT NULL,
	[GpmMessageId] UNIQUEIDENTIFIER NOT NULL,
    [InsertDateUtc] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
	[ModifiedDateUtc] DATETIME2(7) NULL,
	CONSTRAINT [PK_MessageSequence] PRIMARY KEY CLUSTERED ([MessageSequenceID] ASC) WITH (FILLFACTOR = 100)
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
			PatchFamilyID,
			PatchNumber,
			GpmMessageId,
			InsertDateUtc,
			ModifiedDateUtc,
			HistoryInsertDateUtc
		)
		SELECT
			deleted.MessageSequenceID,
			deleted.PatchFamilyID,
			deleted.PatchNumber,
			deleted.GpmMessageId,
			deleted.InsertDateUtc,
			deleted.ModifiedDateUtc,
			SYSUTCDATETIME() as HistoryInsertDateUtc
		FROM deleted
    END
GO

CREATE NONCLUSTERED INDEX [IX_MessageSequence_PatchFamilyID] ON [gpm].[MessageSequence] ([PatchFamilyID])
