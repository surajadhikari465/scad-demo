CREATE PROCEDURE [app].[PurgeData] 
	@batchVolume	 INT = 20000
AS
BEGIN
	UPDATE app.RetentionPolicy
	SET DailyPurgeCompleted = 0
	WHERE CONVERT(DATE, GETDATE()) <>  CONVERT(DATE, ISNULL(LastPurgedDateTime, GETDATE()))
	AND PurgeJobName = 'Data History Purge'
	AND DailyPurgeCompleted = 1

	-- Execute Delete commands based on RetentionPolicy table.
	IF OBJECT_ID('tempdb..#DeleteCommands') IS NOT NULL
		BEGIN
			DROP TABLE #DeleteCommands
		END

	DECLARE @currentHour int 
	SELECT @currentHour = DATEPART(HOUR, GETDATE())

	CREATE TABLE #DeleteCommands(Command NVARCHAR(max));

	INSERT INTO
		#DeleteCommands
	SELECT
		'
			declare @actualPurgeCount int = 0

			begin
				DELETE top(' + convert(varchar(5), @batchVolume) + ') FROM ' +
				QUOTENAME(rp.[Schema]) + '.' +
				QUOTENAME(rp.[Table]) +
				' WHERE ' + QUOTENAME(rp.[ReferenceColumn]) + ' < ' +
				'CAST(DATEADD(d, -' +
				convert(nvarchar(4), rp.DaysToKeep) +
				', GETDATE()) AS DATE)

				set @actualPurgeCount = @@rowcount

				UPDATE app.RetentionPolicy
				SET DailyPurgeCompleted = 1
				WHERE @actualPurgeCount < ' + convert(varchar(5), @batchVolume) + '
				and RetentionPolicyId = ' + convert(varchar(5), rp.[RetentionPolicyId]) + '

				UPDATE app.RetentionPolicy
				SET LastPurgedDateTime = ''' + convert(varchar(25), getdate(), 120) + '''
				WHERE RetentionPolicyId = ' + convert(varchar(5), rp.[RetentionPolicyId]) + '
			end
		'      
	FROM
		app.RetentionPolicy rp
	WHERE 
	    rp.IncludedInDailyPurge = 1
	  AND
	    rp.DailyPurgeCompleted = 0
	  AND 
	    rp.PurgeJobName = 'Data History Purge'
	  AND
		(@currentHour >= rp.TimeToStart AND @currentHour < rp.TimeToEnd)

	DECLARE @Command nvarchar(max)

	DECLARE CommandsCursor CURSOR
		FOR SELECT * FROM #DeleteCommands
	OPEN CommandsCursor

	FETCH NEXT FROM CommandsCursor INTO @Command

	WHILE @@FETCH_STATUS = 0
	BEGIN
		PRINT ''
		PRINT 'Executing - ' + @Command
		EXECUTE sp_executesql @Command
		FETCH NEXT FROM CommandsCursor INTO @Command
	END

	CLOSE CommandsCursor
	DEALLOCATE CommandsCursor

	DROP TABLE #DeleteCommands
END