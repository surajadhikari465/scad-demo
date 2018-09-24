CREATE TABLE [dbo].[KitLinkGroupItem]
(
	[KitLinkGroupItemId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [KitId] INT NOT NULL, 
    [LinkGroupItemId] INT NOT NULL, 
    [InsertDate] DATETIME2 NOT NULL DEFAULT getDate(), 
	[LastUpdatedDate] DATETIME2 NOT NULL DEFAULT getDate(), 
    CONSTRAINT [FK_KitLinkGroupItem_Kit] FOREIGN KEY ([KitId]) REFERENCES [Kit]([KitId]), 
    CONSTRAINT [FK_KitLinkGroupItem_LinkGroupItem] FOREIGN KEY ([LinkGroupItemId]) REFERENCES [LinkGroupItem]([LinkGroupItemId])
)

GO

CREATE INDEX [IX_KitLinkGroupItem_LinkGroupItemId] ON [dbo].[KitLinkGroupItem] ([LinkGroupItemId])

GO

CREATE INDEX [IX_KitLinkGroupItem_KitId] ON [dbo].[KitLinkGroupItem] ([KitId])

GO