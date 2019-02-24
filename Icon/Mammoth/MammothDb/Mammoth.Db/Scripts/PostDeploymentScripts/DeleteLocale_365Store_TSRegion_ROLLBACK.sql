-- This is a ROLLBACK script that goes with the 'DeleteLocale_365Store_TSRegion.sql' script.
-- This will not go with a Release, but instead with a specific CHG request

USE Mammoth
GO

SET IDENTITY_INSERT dbo.Locales_TS ON

INSERT INTO dbo.Locales_TS (
	[Region]
	,[LocaleID]
	,[BusinessUnitID]
	,[StoreName]
	,[StoreAbbrev]
	,[PhoneNumber]
	,[LocaleOpenDate]
	,[LocaleCloseDate]
	,[AddedDate]
	,[ModifiedDate]
	)
OUTPUT inserted.*
SELECT [Region]
	,[LocaleID]
	,[BusinessUnitID]
	,[StoreName]
	,[StoreAbbrev]
	,[PhoneNumber]
	,[LocaleOpenDate]
	,[LocaleCloseDate]
	,[AddedDate]
	,[ModifiedDate]
FROM dbo.tmp_DeletedStores

SET IDENTITY_INSERT dbo.Locales_TS OFF