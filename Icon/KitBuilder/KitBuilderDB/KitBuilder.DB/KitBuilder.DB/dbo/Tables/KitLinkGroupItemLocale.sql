CREATE TABLE [dbo].[KitLinkGroupItemLocale]
(
	[KitLinkGroupItemLocaleId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [KitLinkGroupItemId] INT NOT NULL, 
    [KitLocaleId] INT NOT NULL, 
    [Properties] NVARCHAR(MAX) NULL, 
    [DisplaySequence] INT NOT NULL, 
	[Exclude] BIT NULL, 
    [InsertDate] DATETIME2 NOT NULL DEFAULT getDate(), 
    [LastModifiedDate] DATETIME2 NULL, 
    [LastModifiedBy] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_KitLinkGroupItemLocale_KitLinkGroupItem] FOREIGN KEY ([KitLinkGroupItemId]) REFERENCES [KitLinkGroupItem]([KitLinkGroupItemId]), 
    CONSTRAINT [FK_KitLinkGroupItemLocale_KitLocale] FOREIGN KEY ([KitLocaleId]) REFERENCES [KitLocale]([KitLocaleId]) ON DELETE CASCADE
)

GO

CREATE INDEX [IX_KitLinkGroupItemLocale_KitLocaleId] ON [dbo].[KitLinkGroupItemLocale] ([KitLocaleId])

GO

CREATE INDEX [IX_KitLinkGroupItemLocale_KitLinkGroupItemId] ON [dbo].[KitLinkGroupItemLocale] ([KitLinkGroupItemId])

GO