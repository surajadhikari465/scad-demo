CREATE TABLE [dbo].[KitLinkGroupItem]
(
	[KitLinkGroupItemId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [KitLinkGroupId] INT NOT NULL, 
    [LinkGroupItemId] INT NOT NULL, 
    [InsertDateUtc] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(), 
	[LastUpdatedDateUtc] DATETIME2 NULL , 
    CONSTRAINT [FK_KitLinkGroupItem_Kit] FOREIGN KEY ([KitLinkGroupId]) REFERENCES [KitLinkGroup]([KitLinkGroupId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_KitLinkGroupItem_LinkGroupItem] FOREIGN KEY ([LinkGroupItemId]) REFERENCES [LinkGroupItem]([LinkGroupItemId])
)

GO

CREATE INDEX [IX_KitLinkGroupItem_LinkGroupItemId] ON [dbo].[KitLinkGroupItem] ([LinkGroupItemId])

GO

CREATE INDEX [IX_KitLinkGroupItem_KitLinkGroupId] ON [dbo].[KitLinkGroupItem] ([KitLinkGroupId])

GO