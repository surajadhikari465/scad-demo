CREATE TABLE [infor].[LocaleSequence]
(
	[LocaleSequenceID] INT IDENTITY(1,1) NOT NULL,
	[LocaleID] INT NOT NULL,
	[SequenceID] NUMERIC(22,0)  NOT NULL,
	[InforMessageId] UNIQUEIDENTIFIER NOT NULL,
    [InsertDateUtc] DATETIME2(7) NOT NULL DEFAULT (SYSUTCDATETIME()),
	[ModifiedDateUtc] DATETIME2(7) NULL,
)
GO
ALTER TABLE [infor].[LocaleSequence] WITH CHECK ADD CONSTRAINT [LocaleSequence_LocaleID_FK] FOREIGN KEY (
[LocaleID]
)
REFERENCES [dbo].[Locale] (
[LocaleID]
)
GO
ALTER TABLE [infor].[LocaleSequence] ADD CONSTRAINT [LocaleSequenceID_PK] PRIMARY KEY CLUSTERED (
[LocaleSequenceID]
)
GO
CREATE UNIQUE NONCLUSTERED INDEX IX_LocaleSequence_LocaleID on  [infor].[LocaleSequence] (LocaleID)
GO
