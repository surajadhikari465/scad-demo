USE KitBuilder

DECLARE @regionId INT
DECLARE @regionCode VARCHAR(20)
DECLARE @localeName VARCHAR(20)

SET @localeName = 'MET_NEB'
SET @regionCode = 'MW'
SET @regionid = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Mid West'
		)

UPDATE locale
SET RegionId = @regionId
	,RegionCode = @regionCode
WHERE LocaleName = @localeName