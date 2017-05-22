CREATE TABLE [gpm].[MessageSequenceHistory]
(
    [SequenceFamilyId] BIGINT NOT NULL, 
    [SequenceNumber] INT NOT NULL, 
    [InsertDateUtc] [datetime2](7) NOT NULL DEFAULT (SYSUTCDATETIME()),
	CONSTRAINT [PK_MessageSequenceHistory] PRIMARY KEY CLUSTERED ([SequenceFamilyId] ASC) WITH (FILLFACTOR = 100)
)
