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
		WHERE localeName = 'South'
		)

IF NOT EXISTS (
		SELECT *
		FROM locale
		WHERE localename = 'MET_SFL'
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
		3000
		,1
		,'MET_SFL'
		,getdate()
		,NULL
		,@localeTypeId
		,@localeId
		)

	SET IDENTITY_INSERT [dbo].[Locale] OFF
END
GO

DECLARE @localeIdTallahassee INT = (
		SELECT localeId
		FROM locale
		WHERE localename = 'Tallahassee'
		)
DECLARE @localeIdDestin INT = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Destin'
		)
DECLARE @parentLocaleid INT = (
		SELECT TOP 1 localeId
		FROM locale
		WHERE LocaleName = 'MET_SFL'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid IN (
		@localeIdTallahassee
		,@localeIdDestin
		)