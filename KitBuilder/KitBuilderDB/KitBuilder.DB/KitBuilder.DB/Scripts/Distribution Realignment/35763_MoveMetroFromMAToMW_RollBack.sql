USE KitBuilder
--Roll back Move Metro from MW ->MA
DECLARE @regionId INT
DECLARE @regionCode VARCHAR(20)
DECLARE @localeName VARCHAR(20)

SET @localeName = 'MET_OH'
SET @regionCode = 'MA'
SET @regionid = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Mid Atlantic'
		)

UPDATE locale
SET RegionId = @regionId
	,RegionCode = @regionCode
WHERE LocaleName = @localeName