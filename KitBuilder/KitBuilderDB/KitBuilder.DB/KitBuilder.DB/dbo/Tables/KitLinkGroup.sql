CREATE TABLE [dbo].[KitLinkGroup] (
    [KitLinkGroupId]     INT           IDENTITY (1, 1) NOT NULL,
    [KitId]              INT           NOT NULL,
    [LinkGroupId]        INT           NOT NULL,
    [InsertDateUtc]      DATETIME2 (7) CONSTRAINT [DF_KitLinkGroup_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc] DATETIME2 (7) NULL,
    CONSTRAINT [PK_KitLinkGroup] PRIMARY KEY CLUSTERED ([KitLinkGroupId] ASC),
    CONSTRAINT [FK_KitLinkGroup_Kit] FOREIGN KEY ([KitId]) REFERENCES [dbo].[Kit] ([KitId]),
    CONSTRAINT [FK_KitLinkGroup_LinkGroup] FOREIGN KEY ([LinkGroupId]) REFERENCES [dbo].[LinkGroup] ([LinkGroupId])
);





GO

CREATE INDEX [IX_KitLinkGroup_LinkGroupId] ON [dbo].[KitLinkGroup] ([LinkGroupId])

GO

CREATE INDEX [IX_KitLinkGroup_KitId] ON [dbo].[KitLinkGroup] ([KitId])
