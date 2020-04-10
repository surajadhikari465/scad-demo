USE [icon]
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
		WHERE LocaleName = 'MET_FL'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid IN (
		@localeIdTallahassee
		,@localeIdDestin
		)

DELETE
FROM locale
WHERE localename = 'MET_SFL'