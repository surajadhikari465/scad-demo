CREATE TABLE [dbo].[Locale]
(
	[LocaleId] INT NOT NULL PRIMARY KEY, 
    [LocaleName] NVARCHAR(255) NOT NULL, 
    [LocaleTypeId] INT NOT NULL, 
    [StoreId] INT NULL, 
    [MetroId] INT NULL, 
    [RegionId] INT NULL, 
    [ChainId] INT NULL, 
    [LocaleOpenDate] DATETIME2 NULL, 
    [LocaleCloseDate] DATETIME2 NULL, 
    [RegionCode] NVARCHAR(2) NULL, 
    [BusinessUnitId] INT NULL, 
    [StoreAbbreviation] NVARCHAR(5) NULL, 
    [CurrencyCode] NVARCHAR(5) NULL, 
    [Hospitality] BIT NULL , 
    CONSTRAINT [FK_Locale_LocaleType] FOREIGN KEY ([LocaleTypeId]) REFERENCES [LocaleType]([localeTypeId])
)

GO

CREATE INDEX [IX_Locale_Column] ON [dbo].[Locale] ([LocaleTypeId])
