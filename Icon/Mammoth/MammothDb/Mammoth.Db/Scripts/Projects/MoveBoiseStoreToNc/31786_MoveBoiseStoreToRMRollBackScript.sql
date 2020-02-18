USE Mammoth
GO

SET IDENTITY_INSERT dbo.Locales_RM ON

INSERT INTO dbo.Locales_RM (
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
FROM dbo.tmp_DeletedStoresBoise

SET IDENTITY_INSERT dbo.Locales_RM OFF

DELETE
FROM dbo.Locales_NC
WHERE BusinessUnitID = 10284