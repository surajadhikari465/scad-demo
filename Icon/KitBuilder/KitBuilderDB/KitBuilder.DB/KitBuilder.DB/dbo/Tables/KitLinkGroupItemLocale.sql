CREATE TABLE [dbo].[KitLinkGroupItemLocale]
(
	[KitLinkGroupItemLocaleId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [KitLinkGroupItemId] INT NOT NULL, 
    [KitLinkGroupLocaleId] INT NOT NULL, 
    [Properties] NVARCHAR(MAX) NULL, 
    [DisplaySequence] INT NOT NULL, 
	[Exclude] BIT NULL, 
    [InsertDateUtc] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(), 
    [LastUpdatedDateUtc] DATETIME2 NULL, 
    [LastModifiedBy] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_KitLinkGroupItemLocale_KitLinkGroupItem] FOREIGN KEY ([KitLinkGroupItemId]) REFERENCES [KitLinkGroupItem]([KitLinkGroupItemId]), 
    CONSTRAINT [FK_KitLinkGroupItemLocale_KitLocale] FOREIGN KEY ([KitLinkGroupLocaleId]) REFERENCES [KitLinkGroupLocale]([KitLinkGroupLocaleId]) ON DELETE CASCADE
)

GO

CREATE INDEX [IX_KitLinkGroupItemLocale_KitLinkGroupLocaleId] ON [dbo].[KitLinkGroupItemLocale] ([KitLinkGroupLocaleId])

GO

CREATE INDEX [IX_KitLinkGroupItemLocale_KitLinkGroupItemId] ON [dbo].[KitLinkGroupItemLocale] ([KitLinkGroupItemId])

GO