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

SET IDENTITY_INSERT dbo.Locales_FL OFF

DELETE
FROM dbo.Locales_SO
WHERE BusinessUnitID IN (
		10478
		,10621
		)