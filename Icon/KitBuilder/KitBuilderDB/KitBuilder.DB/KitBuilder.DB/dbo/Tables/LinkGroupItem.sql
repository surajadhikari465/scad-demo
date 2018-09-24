CREATE TABLE [dbo].[LinkGroupItem]
(
	[LinkGroupItemId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [LinkGroupId] INT NOT NULL, 
    [ItemId] INT NOT NULL, 
    [InstructionListId] INT NULL, 
    [InsertDate] DATETIME2 NOT NULL DEFAULT getDate(), 
	[LastUpdatedDate] DATETIME2 NOT NULL DEFAULT getDate(), 
    CONSTRAINT [FK_LinkGroupItem_InstructionList] FOREIGN KEY ([InstructionListId]) REFERENCES [InstructionList]([InstructionListId]), 
    CONSTRAINT [FK_LinkGroupItem_Items] FOREIGN KEY ([ItemId]) REFERENCES [Items]([ItemId]), 
    CONSTRAINT [FK_LinkGroupItem_LinkGroup] FOREIGN KEY ([LinkGroupId]) REFERENCES [LinkGroup]([LinkGroupId])
)

GO

CREATE INDEX [IX_LinkGroupItem_ItemId] ON [dbo].[LinkGroupItem] ([ItemId])

GO

CREATE INDEX [IX_LinkGroupItem_InstructionListId] ON [dbo].[LinkGroupItem] ([InstructionListId])

GO

CREATE INDEX [IX_LinkGroupItem_LinkGroupId] ON [dbo].[LinkGroupItem] ([LinkGroupId])

GO