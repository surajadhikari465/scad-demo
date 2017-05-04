CREATE VIEW [dbo].[Locale] AS 
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_FL
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate 
FROM [dbo].Locales_MA
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_MW
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_NA
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_NC
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_NE
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_PN
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_RM
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_SO
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_SP
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_SW
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_TS
UNION ALL
SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate
FROM [dbo].Locales_UK