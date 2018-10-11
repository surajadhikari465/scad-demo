CREATE TABLE [dbo].[InstructionList] (
    [InstructionListId]  INT           IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (40) NOT NULL,
    [InstructionTypeId]  INT           NOT NULL,
    [StatusId]           INT           NOT NULL,
    [InsertDateUtc]      DATETIME2 (7) CONSTRAINT [DF_InstructioniList_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc] DATETIME2 (7) NULL,
    CONSTRAINT [PK_InstructionList] PRIMARY KEY CLUSTERED ([InstructionListId] ASC),
    CONSTRAINT [FK_InstructionList_InstructionType_InstructionTypeId] FOREIGN KEY ([InstructionTypeId]) REFERENCES [dbo].[InstructionType] ([InstructionTypeId]),
    CONSTRAINT [FK_InstructionList_Status_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Status] ([StatusID])
);





GO

CREATE INDEX [IX_InstructionList_InstructionTypeId] ON [dbo].[InstructionList] ([InstructionTypeId])

GO

CREATE INDEX [IX_InstructionList_StatusId] ON [dbo].[InstructionList] ([StatusId])
