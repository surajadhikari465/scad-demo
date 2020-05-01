USE [icon]
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
		WHERE LocaleName = 'MET_OH'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid IN (
		@localeIdCIN
		,@localeIdMSO
		,@localeIdDAY
		,@localeIdKWD
		)

DELETE
FROM locale
WHERE localename = 'MET_SOH'