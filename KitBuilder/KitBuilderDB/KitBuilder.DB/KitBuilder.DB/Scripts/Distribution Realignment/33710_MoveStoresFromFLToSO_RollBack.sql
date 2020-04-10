USE [KitBuilder]
GO

DECLARE @regionId INT
DECLARE @metroId INT
DECLARE @regionCode VARCHAR(20) = 'FL'

SET @regionId = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Florida'
		)
SET @metroid = (
		SELECT localeid
		FROM locale
		WHERE localename = 'MET_FL'
		)

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

UPDATE locale
SET MetroId = @metroId
	,regionid = @regionid
	,RegionCode = @regionCode
WHERE localeid IN (
		@localeIdTallahassee
		,@localeIdDestin
		)

DELETE
FROM locale
WHERE localename = 'MET_SFL'