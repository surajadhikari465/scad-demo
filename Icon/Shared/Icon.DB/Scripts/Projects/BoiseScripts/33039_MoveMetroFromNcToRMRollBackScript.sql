--Roll Back

USE Icon
GO

DECLARE @parentLocaleID INT
DECLARE @localeid INT

SELECT @parentLocaleID = localeId
FROM locale
WHERE LocaleName = 'Rocky Mountain'

SET @localeid = (
		SELECT TOP 1 localeId
		FROM locale
		WHERE LocaleName = 'MET_ID'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleID
WHERE localeid = @localeid