CREATE TABLE [dbo].[KitInstructionList] (
    [KitInstructionListId] INT IDENTITY (1, 1) NOT NULL,
    [KitId]                INT NOT NULL,
    [InstructionListId]    INT NOT NULL,
    CONSTRAINT [PK_KitInstructionList] PRIMARY KEY CLUSTERED ([KitInstructionListId] ASC),
    CONSTRAINT [FK_KitInstructionList_InstructionList] FOREIGN KEY ([InstructionListId]) REFERENCES [dbo].[InstructionList] ([InstructionListId]),
    CONSTRAINT [FK_KitInstructionList_Kit] FOREIGN KEY ([KitId]) REFERENCES [dbo].[Kit] ([KitId])
);





GO

CREATE INDEX [IX_KitInstructionList_InstructionListId] ON [dbo].[KitInstructionList] ([InstructionListId])

GO

CREATE INDEX [IX_KitInstructionList_KitId] ON [dbo].[KitInstructionList] ([KitId])
