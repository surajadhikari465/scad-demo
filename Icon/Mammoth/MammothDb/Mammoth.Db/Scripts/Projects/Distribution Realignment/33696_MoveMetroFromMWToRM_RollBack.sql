USE Mammoth
GO

SET IDENTITY_INSERT dbo.Locales_MW ON

INSERT INTO dbo.Locales_MW (
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

SET IDENTITY_INSERT dbo.Locales_MW OFF

DELETE
FROM dbo.Locales_RM
WHERE BusinessUnitID IN (
		10169
		,10493
		)