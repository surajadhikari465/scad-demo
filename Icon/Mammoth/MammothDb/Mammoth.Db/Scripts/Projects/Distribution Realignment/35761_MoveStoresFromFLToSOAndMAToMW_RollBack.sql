USE Mammoth
GO

SET IDENTITY_INSERT dbo.Locales_FL ON

INSERT INTO dbo.Locales_FL (
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
WHERE Region = 'FL'

SET IDENTITY_INSERT dbo.Locales_FL OFF
SET IDENTITY_INSERT dbo.Locales_MA ON

INSERT INTO dbo.Locales_MA (
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
WHERE Region = 'MA'

SET IDENTITY_INSERT dbo.Locales_MA OFF

DELETE
FROM dbo.Locales_SO
WHERE BusinessUnitID IN (
		10478
		,10621
		)

DELETE
FROM dbo.Locales_MW
WHERE BusinessUnitID IN (
		10214
		,10385
		,10555
		)