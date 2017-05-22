CREATE TABLE [gpm].[MessageSequence]
(
    [SequenceFamilyId] BIGINT NOT NULL, 
    [SequenceNumber] INT NOT NULL, 
    [InsertDateUtc] [datetime2](7) NOT NULL DEFAULT (SYSUTCDATETIME()),
	CONSTRAINT [PK_MessageSequence] PRIMARY KEY CLUSTERED ([SequenceFamilyId] ASC) WITH (FILLFACTOR = 100)
)
