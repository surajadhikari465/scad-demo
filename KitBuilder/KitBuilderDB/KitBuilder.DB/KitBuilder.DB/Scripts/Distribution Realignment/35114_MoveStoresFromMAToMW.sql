USE [KitBuilder]
GO

DECLARE @localeTypeId INT
DECLARE @regionId INT
DECLARE @regionCode VARCHAR(20) = 'MW'

SET @regionId = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Mid West'
		)
SET @localeTypeId = (
		SELECT localetypeid
		FROM localetype
		WHERE localetypedesc = 'Metro'
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
GO

DECLARE @metroId INT
DECLARE @regionid INT
DECLARE @regionCode VARCHAR(20) = 'MW'

SET @regionid = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Mid West'
		)
SET @metroid = (
		SELECT localeid
		FROM locale
		WHERE localename = 'MET_SOH'
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