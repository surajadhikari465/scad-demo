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
		WHERE localename = 'Metro_SFL'
		)
	INSERT INTO [dbo].[Locale] (
		[ownerOrgPartyID]
		,[localeName]
		,[localeOpenDate]
		,[localeCloseDate]
		,[localeTypeID]
		,[parentLocaleID]
		)
	VALUES (
		1
		,'Metro_SFL'
		,getdate()
		,NULL
		,@localeTypeId
		,@localeId
		)
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
		WHERE LocaleName = 'Metro_SFL'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid IN (
		@localeIdTallahassee
		,@localeIdDestin
		)