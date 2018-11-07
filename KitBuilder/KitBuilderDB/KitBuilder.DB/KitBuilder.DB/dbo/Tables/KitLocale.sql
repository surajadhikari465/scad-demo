CREATE TABLE [dbo].[KitLocale] (
    [KitLocaleId]        INT           IDENTITY (1, 1) NOT NULL,
    [KitId]              INT           NOT NULL,
    [LocaleId]           INT           NOT NULL,
    [MinimumCalories]    INT           NULL,
    [MaximumCalories]    INT           NULL,
    [Exclude]            BIT           NULL,
    [StatusId]           INT           NOT NULL,
    [InsertDateUtc]      DATETIME2 (7) CONSTRAINT [DF_KitLocale_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc] DATETIME2 (7) NULL,
    CONSTRAINT [PK_KitLocale] PRIMARY KEY CLUSTERED ([KitLocaleId] ASC),
    CONSTRAINT [FK_KitLocale_Kit] FOREIGN KEY ([KitId]) REFERENCES [dbo].[Kit] ([KitId]),
    CONSTRAINT [FK_KitLocale_Locale] FOREIGN KEY ([LocaleId]) REFERENCES [dbo].[Locale] ([LocaleId]),
    CONSTRAINT [FK_KitLocale_Status] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Status] ([StatusID])
);





GO

CREATE INDEX [IX_KitLocale_LocaleId] ON [dbo].[KitLocale] ([LocaleId])

GO

CREATE INDEX [IX_KitLocale_Kit] ON [dbo].[KitLocale] ([KitId])

GO

CREATE INDEX [IX_KitLocale_Status] ON [dbo].[KitLocale] ([StatusId])
