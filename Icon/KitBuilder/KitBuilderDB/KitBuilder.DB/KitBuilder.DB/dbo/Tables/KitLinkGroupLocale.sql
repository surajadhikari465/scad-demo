CREATE TABLE [dbo].[KitLinkGroupLocale]
(
	[KitLinkGroupLocaleId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [KitLinkGroupId] INT NOT NULL, 
    [KitLocaleId] INT NOT NULL, 
    [Properties] NVARCHAR(MAX) NULL, 
    [DisplaySequence] INT NULL, 
    [MinimumCalories] INT NULL, 
    [MaximumCalories] INT NULL, 
    [Exclude] BIT NULL, 
    [InsertDate] DATETIME2 NOT NULL DEFAULT getDate(), 
    [LastModifiedDate] DATETIME2 NULL, 
    [LastModifiedBy] NVARCHAR(100) NULL, 
    CONSTRAINT [FK_KitLinkGroupLocale_KitLinkGroup] FOREIGN KEY ([KitLinkGroupId]) REFERENCES [KitLinkGroup]([KitLinkGroupId]), 
    CONSTRAINT [FK_KitLinkGroupLocale_KitLocale] FOREIGN KEY ([KitLocaleId]) REFERENCES [KitLocale]([KitLocaleId])
)

GO

CREATE INDEX [IX_KitLinkGroupLocale_KitLocaleId] ON [dbo].[KitLinkGroupLocale] ([KitLocaleId])

GO

CREATE INDEX [IX_KitLinkGroupLocale_KitLinkGroupId] ON [dbo].[KitLinkGroupLocale] ([KitLinkGroupID])

GO