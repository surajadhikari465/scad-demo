USE [icon]

--Rollback changes
DECLARE @parentLocaleId INT
DECLARE @localeIdTLH INT = (
		SELECT localeId
		FROM locale
		WHERE localename = 'Tallahassee'
		)
DECLARE @localeIdDES INT = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Destin'
		)

SET @parentLocaleId = (
		SELECT TOP 1 localeId
		FROM locale
		WHERE LocaleName = 'MET_FL'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid IN (
		@localeIdTLH
		,@localeIdDES
		)

DECLARE @localeIdDUB INT = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Columbus'
		)
DECLARE @localeIdEYE INT = (
		SELECT localeId
		FROM locale
		WHERE localename = 'West Lane'
		)
DECLARE @localeIdEST INT = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Easton'
		)

SET @parentLocaleId = (
		SELECT TOP 1 localeId
		FROM locale
		WHERE LocaleName = 'MET_OH'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid IN (
		@localeIdDUB
		,@localeIdEYE
		,@localeIdEST
		)

--Delete new Metro's
DELETE
FROM locale
WHERE localename IN (
		'MET_SFL'
		,'MET_SOH'
		)