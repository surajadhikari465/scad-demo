CREATE TABLE [dbo].[KitLinkGroup]
(
	[KitLinkGroupId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [KitId] INT NOT NULL, 
    [LinkGroupId] INT NOT NULL, 
    [InsertDate] DATETIME2 NOT NULL DEFAULT GetDate(), 
	[LastUpdatedDate] DATETIME2 NOT NULL DEFAULT getDate(), 
    CONSTRAINT [FK_KitLinkGroup_LinkGroup] FOREIGN KEY ([LinkGroupId]) REFERENCES [LinkGroup]([LinkGroupId]), 
    CONSTRAINT [FK_KitLinkGroup_Kit] FOREIGN KEY ([KitId]) REFERENCES [Kit]([KitId])
)

GO

CREATE INDEX [IX_KitLinkGroup_LinkGroupId] ON [dbo].[KitLinkGroup] ([LinkGroupId])

GO

CREATE INDEX [IX_KitLinkGroup_KitId] ON [dbo].[KitLinkGroup] ([KitId])
