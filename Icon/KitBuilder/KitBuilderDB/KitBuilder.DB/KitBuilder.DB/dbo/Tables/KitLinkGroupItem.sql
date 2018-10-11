CREATE TABLE [dbo].[KitLinkGroupItem] (
    [KitLinkGroupItemId] INT           IDENTITY (1, 1) NOT NULL,
    [KitLinkGroupId]     INT           NOT NULL,
    [LinkGroupItemId]    INT           NOT NULL,
    [InsertDateUtc]      DATETIME2 (7) CONSTRAINT [DF_KitLinkGroupItem_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc] DATETIME2 (7) NULL,
    CONSTRAINT [PK_KitLinkGroupItem] PRIMARY KEY CLUSTERED ([KitLinkGroupItemId] ASC),
    CONSTRAINT [FK_KitLinkGroupItem_Kit] FOREIGN KEY ([KitLinkGroupId]) REFERENCES [dbo].[KitLinkGroup] ([KitLinkGroupId]) ON DELETE CASCADE,
    CONSTRAINT [FK_KitLinkGroupItem_LinkGroupItem] FOREIGN KEY ([LinkGroupItemId]) REFERENCES [dbo].[LinkGroupItem] ([LinkGroupItemId])
);





GO

CREATE INDEX [IX_KitLinkGroupItem_LinkGroupItemId] ON [dbo].[KitLinkGroupItem] ([LinkGroupItemId])

GO

CREATE INDEX [IX_KitLinkGroupItem_KitLinkGroupId] ON [dbo].[KitLinkGroupItem] ([KitLinkGroupId])

GO