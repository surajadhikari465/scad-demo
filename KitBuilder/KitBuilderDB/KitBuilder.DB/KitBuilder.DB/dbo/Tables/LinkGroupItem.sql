CREATE TABLE [dbo].[LinkGroupItem] (
    [LinkGroupItemId]    INT           IDENTITY (1, 1) NOT NULL,
    [LinkGroupId]        INT           NOT NULL,
    [ItemId]             INT           NOT NULL,
    [InstructionListId]  INT           NULL,
    [InsertDateUtc]      DATETIME2 (7) CONSTRAINT [DF_LinkGroupItem_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc] DATETIME2 (7) NULL,
    CONSTRAINT [PK_LinkGroupItem] PRIMARY KEY CLUSTERED ([LinkGroupItemId] ASC),
    CONSTRAINT [FK_LinkGroupItem_InstructionList] FOREIGN KEY ([InstructionListId]) REFERENCES [dbo].[InstructionList] ([InstructionListId]),
    CONSTRAINT [FK_LinkGroupItem_Items] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Items] ([ItemId]),
    CONSTRAINT [FK_LinkGroupItem_LinkGroup] FOREIGN KEY ([LinkGroupId]) REFERENCES [dbo].[LinkGroup] ([LinkGroupId])
);





GO

CREATE INDEX [IX_LinkGroupItem_ItemId] ON [dbo].[LinkGroupItem] ([ItemId])

GO

CREATE INDEX [IX_LinkGroupItem_InstructionListId] ON [dbo].[LinkGroupItem] ([InstructionListId])

GO

CREATE INDEX [IX_LinkGroupItem_LinkGroupId] ON [dbo].[LinkGroupItem] ([LinkGroupId])

GO