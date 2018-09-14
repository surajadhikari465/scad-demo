CREATE TABLE [dbo].[Kit]
(
	[KitId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [ItemId] INT NOT NULL, 
    [Description] NVARCHAR(255) NULL, 
    [InstructionListId] INT NULL, 
    [InsertDate] DATETIME2 NOT NULL DEFAULT getDate(), 
	[UpdatedDate] DATETIME2 NOT NULL DEFAULT getDate(), 
    CONSTRAINT [FK_Kit_Items] FOREIGN KEY ([ItemId]) REFERENCES [Items]([ItemId]), 
    CONSTRAINT [FK_Kit_InstructionList] FOREIGN KEY ([InstructionListId]) REFERENCES [InstructionList]([InstructionListId])
)

GO

CREATE INDEX [IX_Kit_ItemId] ON [dbo].[Kit] ([ItemId])

GO

CREATE INDEX [IX_Kit_InstructionListID] ON [dbo].[Kit] ([InstructionListId])
