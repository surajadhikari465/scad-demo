CREATE TABLE [dbo].[KitInstructionList]
(
	[KitInstructionListId] INT NOT NULL IDENTITY PRIMARY KEY, 
	[KitId] INT NOT NULL, 
    [InstructionListId] INT NOT NULL, 
    CONSTRAINT [FK_KitInstructionList_InstructionList] FOREIGN KEY ([InstructionListId]) REFERENCES [InstructionList]([InstructionListId]), 
    CONSTRAINT [FK_KitInstructionList_Kit] FOREIGN KEY ([KitId]) REFERENCES [Kit]([KitId])
)

GO

CREATE INDEX [IX_KitInstructionList_InstructionListId] ON [dbo].[KitInstructionList] ([InstructionListId])

GO

CREATE INDEX [IX_KitInstructionList_KitId] ON [dbo].[KitInstructionList] ([KitId])
