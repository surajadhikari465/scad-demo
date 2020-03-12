USE KitBuilder

DECLARE @metroNameToMove VARCHAR(20)
DECLARE @destinationRegionCode VARCHAR(20)
DECLARE @metroId INT
DECLARE @destinationRegionId INT
DECLARE @RegionLocaleTypeId INT

SET @metroNameToMove = 'Met_Id'
SET @destinationRegionCode = 'NC'
SET @RegionLocaleTypeId = (
		SELECT localeTypeId
		FROM localeType
		WHERE localeTypeCode = 'RG'
		)
SET @metroId = (
		SELECT LocaleId
		FROM Locale
		WHERE Localename = @metroNameToMove
		)
SET @destinationRegionId = (
		SELECT LocaleId
		FROM Locale
		WHERE RegionCode = @destinationRegionCode
			AND LocaleTypeId = @RegionLocaleTypeId
		)

UPDATE locale
SET RegionId = @destinationRegionId,
RegionCode = @destinationRegionCode
WHERE LocaleId = @metroId