CREATE TABLE [dbo].[InstructionListMember] (
    [InstructionListMemberId] INT           IDENTITY (1, 1) NOT NULL,
    [InstructionListId]       INT           NOT NULL,
    [Group]                   NVARCHAR (60) NOT NULL,
    [Sequence]                INT           NOT NULL,
    [Member]                  NVARCHAR (15) NULL,
    [InsertDateUtc]           DATETIME2 (7) CONSTRAINT [DF_InstructionListMember_InsertDateUTC] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc]      DATETIME2 (7) NULL,
	[PluNumber] INT NOT NULL UNIQUE,
	[IsDeleted] BIT NULL,
    CONSTRAINT [PK_InstructionListMember] PRIMARY KEY CLUSTERED ([InstructionListMemberId] ASC),
	CONSTRAINT [FK_InstructionListMember_AvailablePluNumber] FOREIGN KEY ([PluNumber]) REFERENCES [dbo].[AvailablePluNumber] ([PluNumber]),
    CONSTRAINT [FK_InstructionListMember_InstructionList] FOREIGN KEY ([InstructionListId]) REFERENCES [dbo].[InstructionList] ([InstructionListId])
);

GO

CREATE INDEX [IX_InstructionListMember_InstructionListId] ON [dbo].[InstructionListMember] ([InstructionListId])