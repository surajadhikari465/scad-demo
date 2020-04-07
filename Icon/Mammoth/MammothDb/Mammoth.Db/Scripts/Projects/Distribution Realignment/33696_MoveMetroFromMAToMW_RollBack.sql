USE Mammoth
GO

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

SET IDENTITY_INSERT dbo.Locales_MA OFF

DELETE
FROM dbo.Locales_MW
WHERE BusinessUnitID IN (
		10186
		,10577
		)