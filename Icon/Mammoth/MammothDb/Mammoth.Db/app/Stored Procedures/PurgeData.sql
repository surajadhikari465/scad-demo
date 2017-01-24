﻿CREATE PROCEDURE [app].[PurgeData] 
AS 
BEGIN
	-- Loop through the tables to be purged and delete them according to the retention policy.
	DECLARE @DeleteCommands TABLE(Command NVARCHAR(255)
	)

	DECLARE @Command nvarchar(255)

	PRINT 'Inserting  Delete Commands'

	INSERT INTO @DeleteCommands

	SELECT 'DELETE FROM ' +
		QUOTENAME(rp.[Database]) + '.' + 
		QUOTENAME(rp.[Schema]) + '.' + 
		QUOTENAME(rp.[Table]) + 
		'WHERE InsertDate < DATEADD(d, -' + convert(nvarchar(8), rp.DaysToKeep) + ', GETDATE())
	'
	FROM app.RetentionPolicy rp


	DECLARE CommandsCursor CURSOR
	FOR SELECT * FROM @DeleteCommands

	OPEN CommandsCursor

	FETCH NEXT FROM CommandsCursor INTO @Command

	WHILE @@FETCH_STATUS = 0 BEGIN

		PRINT ''

		PRINT 'Executing - ' + @Command

		EXECUTE sp_executesql @Command

		FETCH NEXT FROM CommandsCursor INTO @Command

	END

	CLOSE CommandsCursor

	DEALLOCATE CommandsCursor
END