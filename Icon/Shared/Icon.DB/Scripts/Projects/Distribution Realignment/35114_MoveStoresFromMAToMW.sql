USE [icon]
GO

DECLARE @localeTypeId INT = (
		SELECT localeTypeID
		FROM localeType
		WHERE localeTypeDesc = 'Metro'
		)
DECLARE @localeId INT = (
		SELECT localeid
		FROM locale
		WHERE localeName = 'Mid West'
		)

IF NOT EXISTS (
		SELECT *
		FROM locale
		WHERE localename = 'MET_SOH'
		)
BEGIN
	SET IDENTITY_INSERT [dbo].[Locale] ON

	INSERT INTO [dbo].[Locale] (
		[localeID]
		,[ownerOrgPartyID]
		,[localeName]
		,[localeOpenDate]
		,[localeCloseDate]
		,[localeTypeID]
		,[parentLocaleID]
		)
	VALUES (
		3002
		,1
		,'MET_SOH'
		,getdate()
		,NULL
		,@localeTypeId
		,@localeId
		)

	SET IDENTITY_INSERT [dbo].[Locale] OFF
END
GO

DECLARE @localeIdCIN INT = (
		SELECT localeId
		FROM locale
		WHERE localename = 'Cincinnati'
		)
DECLARE @localeIdMSO INT = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Mason'
		)
DECLARE @localeIdDAY INT = (
		SELECT localeId
		FROM locale
		WHERE localename = 'Dayton'
		)
DECLARE @localeIdKWD INT = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Kenwood'
		)
DECLARE @parentLocaleid INT = (
		SELECT TOP 1 localeId
		FROM locale
		WHERE LocaleName = 'MET_SOH'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid IN (
		@localeIdCIN
		,@localeIdMSO
		,@localeIdDAY
		,@localeIdKWD
		)