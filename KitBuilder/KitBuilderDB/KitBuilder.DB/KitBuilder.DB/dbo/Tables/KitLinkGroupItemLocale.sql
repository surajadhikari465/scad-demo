CREATE TABLE [dbo].[KitLinkGroupItemLocale] (
    [KitLinkGroupItemLocaleId] INT            IDENTITY (1, 1) NOT NULL,
    [KitLinkGroupItemId]       INT            NOT NULL,
    [KitLinkGroupLocaleId]     INT            NOT NULL,
    [Properties]               NVARCHAR (MAX) NULL,
    [DisplaySequence]          INT            NOT NULL,
    [Exclude]                  BIT            NULL,
    [InsertDateUtc]            DATETIME2 (7)  CONSTRAINT [DF_KitLinkGroupItemLocale_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc]       DATETIME2 (7)  NULL,
    [LastModifiedBy]           NVARCHAR (100) NULL,
    CONSTRAINT [PK_KitLinkGroupItemLocale] PRIMARY KEY CLUSTERED ([KitLinkGroupItemLocaleId] ASC),
    CONSTRAINT [FK_KitLinkGroupItemLocale_KitLinkGroupItem] FOREIGN KEY ([KitLinkGroupItemId]) REFERENCES [dbo].[KitLinkGroupItem] ([KitLinkGroupItemId]),
    CONSTRAINT [FK_KitLinkGroupItemLocale_KitLocale] FOREIGN KEY ([KitLinkGroupLocaleId]) REFERENCES [dbo].[KitLinkGroupLocale] ([KitLinkGroupLocaleId]) ON DELETE CASCADE
);





GO

CREATE INDEX [IX_KitLinkGroupItemLocale_KitLinkGroupLocaleId] ON [dbo].[KitLinkGroupItemLocale] ([KitLinkGroupLocaleId])

GO

CREATE INDEX [IX_KitLinkGroupItemLocale_KitLinkGroupItemId] ON [dbo].[KitLinkGroupItemLocale] ([KitLinkGroupItemId])

GO