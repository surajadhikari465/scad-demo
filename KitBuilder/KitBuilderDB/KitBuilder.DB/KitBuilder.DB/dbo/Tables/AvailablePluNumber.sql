CREATE TABLE [dbo].[AvailablePluNumber]
(
	[PluNumber] INT NOT NULL PRIMARY KEY,
	[InUse] Bit,
	[InsertDateUtc]      DATETIME2 (7)  CONSTRAINT [DF_InstructionMemberPluNumbers_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc] DATETIME2 (7)  NULL,
)
