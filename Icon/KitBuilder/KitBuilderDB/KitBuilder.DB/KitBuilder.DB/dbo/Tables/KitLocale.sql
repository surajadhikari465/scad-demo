﻿CREATE TABLE [dbo].[KitLocale]
(
	[KitLocaleId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [KitId] INT NOT NULL, 
    [LocaleId] INT NOT NULL, 
    [MinimumCalories] INT NULL, 
    [MaximumCalories] INT NULL, 
    [Exclude] BIT NULL, 
    [StatusId] INT NOT NULL, 
    [InsertDateUtc] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(), 
	[LastUpdatedDateUtc] DATETIME2 NULL , 
    CONSTRAINT [FK_KitLocale_Locale] FOREIGN KEY ([LocaleId]) REFERENCES [Locale]([LocaleId]), 
    CONSTRAINT [FK_KitLocale_Kit] FOREIGN KEY ([KitId]) REFERENCES [Kit]([KitId]), 
    CONSTRAINT [FK_KitLocale_Status] FOREIGN KEY ([StatusId]) REFERENCES [Status]([StatusId])
)

GO

CREATE INDEX [IX_KitLocale_LocaleId] ON [dbo].[KitLocale] ([LocaleId])

GO

CREATE INDEX [IX_KitLocale_Kit] ON [dbo].[KitLocale] ([KitId])

GO

CREATE INDEX [IX_KitLocale_Status] ON [dbo].[KitLocale] ([StatusId])
