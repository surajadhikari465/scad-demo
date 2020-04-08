USE [KitBuilder]
GO

DECLARE @localeTypeId INT
DECLARE @regionId INT
DECLARE @regionCode VARCHAR(20) = 'SO'

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
		WHERE localename = 'Metro_SFL'
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
		,'Metro_SFL'
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
GO

DECLARE @metroId INT
DECLARE @regionid INT
DECLARE @regionCode VARCHAR(20) = 'SO'

SET @regionid = (
		SELECT localeid
		FROM locale
		WHERE localename = 'south'
		)
SET @metroid = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Metro_SFL'
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