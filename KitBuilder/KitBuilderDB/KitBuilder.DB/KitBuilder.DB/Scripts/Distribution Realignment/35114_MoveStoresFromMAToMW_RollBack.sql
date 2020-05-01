USE [KitBuilder]
GO

DECLARE @regionId INT
DECLARE @metroId INT
DECLARE @regionCode VARCHAR(20) = 'MA'

SET @regionId = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Mid Atlantic'
		)
SET @metroid = (
		SELECT localeid
		FROM locale
		WHERE localename = 'MET_OH'
		)

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

UPDATE locale
SET MetroId = @metroId
	,regionid = @regionid
	,RegionCode = @regionCode
WHERE localeid IN (
		@localeIdCIN
		,@localeIdMSO
		,@localeIdDAY
		,@localeIdKWD
		)

DELETE
FROM locale
WHERE localename = 'MET_SOH'