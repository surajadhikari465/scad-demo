USE [KitBuilder]
GO

DECLARE @regionId INT
DECLARE @metroId INT
DECLARE @regionCode VARCHAR(20)

--Roll Back for FL -> SO
SET @regionCode = 'FL'
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

UPDATE locale
SET MetroId = @metroId
	,regionid = @regionid
	,RegionCode = @regionCode
WHERE localeid IN (
		@localeIdTLH
		,@localeIdDES
		)

--Roll back for MA -> MW Region
SET @regionCode = 'MA'
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

UPDATE locale
SET MetroId = @metroId
	,regionid = @regionid
	,RegionCode = @regionCode
WHERE localeid IN (
		@localeIdDUB
		,@localeIdEYE
		,@localeIdEST
		)
--delete new Metro's
DELETE
FROM locale
WHERE localename IN (
		'MET_SFL'
		,'MET_SOH'
		)