USE Icon
GO

DECLARE @parentLocaleId INT
DECLARE @localeId INT

SELECT @parentLocaleId = localeId
FROM locale
WHERE LocaleName = 'Northern California'

SET @localeId = (
		SELECT TOP 1 localeId
		FROM locale
		WHERE LocaleName = 'MET_ID'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid = @localeId