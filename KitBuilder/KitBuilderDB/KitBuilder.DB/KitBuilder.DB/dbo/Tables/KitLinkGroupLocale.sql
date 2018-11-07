CREATE TABLE [dbo].[KitLinkGroupLocale] (
    [KitLinkGroupLocaleId] INT            IDENTITY (1, 1) NOT NULL,
    [KitLinkGroupId]       INT            NOT NULL,
    [KitLocaleId]          INT            NOT NULL,
    [Properties]           NVARCHAR (MAX) NULL,
    [DisplaySequence]      INT            NOT NULL,
    [MinimumCalories]      INT            NULL,
    [MaximumCalories]      INT            NULL,
    [Exclude]              BIT            NULL,
    [InsertDateUtc]        DATETIME2 (7)  CONSTRAINT [DF_KitLinkGroupLocale_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc]   DATETIME2 (7)  NULL,
    [LastModifiedBy]       NVARCHAR (100) NULL,
    CONSTRAINT [PK_KitLinkGroupLocale] PRIMARY KEY CLUSTERED ([KitLinkGroupLocaleId] ASC),
    CONSTRAINT [FK_KitLinkGroupLocale_KitLinkGroup] FOREIGN KEY ([KitLinkGroupId]) REFERENCES [dbo].[KitLinkGroup] ([KitLinkGroupId]),
    CONSTRAINT [FK_KitLinkGroupLocale_KitLocale] FOREIGN KEY ([KitLocaleId]) REFERENCES [dbo].[KitLocale] ([KitLocaleId]) ON DELETE CASCADE
);





GO

CREATE INDEX [IX_KitLinkGroupLocale_KitLocaleId] ON [dbo].[KitLinkGroupLocale] ([KitLocaleId])

GO

CREATE INDEX [IX_KitLinkGroupLocale_KitLinkGroupId] ON [dbo].[KitLinkGroupLocale] ([KitLinkGroupID])

GO