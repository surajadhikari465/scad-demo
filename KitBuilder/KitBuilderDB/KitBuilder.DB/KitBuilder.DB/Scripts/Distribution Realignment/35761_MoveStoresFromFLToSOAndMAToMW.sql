USE [KitBuilder]
--scripts for moving stores from (FL MET_FL->SO MET_SFL and  MA MET_OH -> MW MET_SOH)

DECLARE @localeTypeId INT
DECLARE @regionId INT
DECLARE @regionCode VARCHAR(20)
DECLARE @metroId INT

--Move stores from FL -> SO
SET @regionCode = 'SO'

SET @regionId = (
		SELECT localeid
		FROM locale
		WHERE localename = 'South'
		)
SET @localeTypeId = (
		SELECT localetypeid
		FROM localetype
		WHERE localetypedesc = 'Metro'
		)

IF NOT EXISTS (
		SELECT *
		FROM locale
		WHERE localename = 'MET_SFL'
		)
	INSERT INTO [dbo].[Locale] (
		[LocaleId]
		,[LocaleName]
		,[LocaleTypeId]
		,[StoreId]
		,[MetroId]
		,[RegionId]
		,[ChainId]
		,[LocaleOpenDate]
		,[LocaleCloseDate]
		,[RegionCode]
		,[BusinessUnitId]
		,[StoreAbbreviation]
		,[CurrencyCode]
		,[Hospitality]
		)
	VALUES (
		3000
		,'MET_SFL'
		,@localeTypeId
		,NULL
		,NULL
		,@regionId
		,1
		,getdate()
		,NULL
		,@regionCode
		,NULL
		,NULL
		,NULL
		,0
		)

SET @metroid = (
		SELECT localeid
		FROM locale
		WHERE localename = 'MET_SFL'
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

--Move stores from MA ->MW
SET @regionCode  = 'MW'

SET @regionId = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Mid West'
		)

IF NOT EXISTS (
		SELECT *
		FROM locale
		WHERE localename = 'MET_SOH'
		)
	INSERT INTO [dbo].[Locale] (
		[LocaleId]
		,[LocaleName]
		,[LocaleTypeId]
		,[StoreId]
		,[MetroId]
		,[RegionId]
		,[ChainId]
		,[LocaleOpenDate]
		,[LocaleCloseDate]
		,[RegionCode]
		,[BusinessUnitId]
		,[StoreAbbreviation]
		,[CurrencyCode]
		,[Hospitality]
		)
	VALUES (
		3002
		,'MET_SOH'
		,@localeTypeId
		,NULL
		,NULL
		,@regionId
		,1
		,getdate()
		,NULL
		,@regionCode
		,NULL
		,NULL
		,NULL
		,0
		)

SET @metroid = (
		SELECT localeid
		FROM locale
		WHERE localename = 'MET_SOH'
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