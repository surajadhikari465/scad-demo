﻿USE Icon
GO

DECLARE @localeid INT
	,@parentLocaleid INT

SELECT @parentLocaleid = localeid
FROM locale
WHERE localeName = 'Mid West'

SET @localeId = (
		SELECT TOP 1 localeId
		FROM locale
		WHERE LocaleName = 'MET_NEB'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid = @localeId