USE Icon
GO

DECLARE @storeId INT
DECLARE @metroId INT

SELECT @storeId = localeId
FROM locale
WHERE LocaleName = 'Boise'

SET @metroId = (
		SELECT TOP 1 localeId
		FROM locale
		WHERE LocaleName = 'MET_SAC'
		)

UPDATE locale
SET parentLocaleID = @metroId
WHERE localeid = @storeId
