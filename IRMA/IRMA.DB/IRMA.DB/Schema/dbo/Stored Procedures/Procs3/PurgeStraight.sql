
CREATE PROCEDURE [dbo].[PurgeStraight]
	@batchVolume	 INT
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SNAPSHOT

	UPDATE RetentionPolicy
	SET DailyPurgeCompleted = 0
	WHERE CONVERT(DATE, GETDATE()) <>  CONVERT(DATE, ISNULL(LastPurgedDateTime, GETDATE()))
	AND PurgeJobName = 'StraightPurge'
	AND DailyPurgeCompleted = 1

-- Execute Delete commands based on RetentionPolicy table.
	IF OBJECT_ID('tempdb..#DeteleCommands') IS NOT NULL
		BEGIN
			DROP TABLE #DeleteCommands
		END

	DECLARE @currentHour int 
	SELECT @currentHour = DATEPART(HOUR, DATEADD(d, -1, GETDATE()))

	CREATE TABLE #DeleteCommands(Command NVARCHAR(max));

	PRINT 'Inserting delete commands...'
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

				UPDATE RetentionPolicy
				SET DailyPurgeCompleted = 1
				WHERE @actualPurgeCount < ' + convert(varchar(5), @batchVolume) + '
				and RetentionPolicyId = ' + convert(varchar(5), rp.[RetentionPolicyId]) + '

				UPDATE RetentionPolicy
				SET LastPurgedDateTime = ''' + convert(varchar(25), getdate(), 120) + '''
				WHERE RetentionPolicyId = ' + convert(varchar(5), rp.[RetentionPolicyId]) + '
			end
		'      
	FROM
		RetentionPolicy rp
	WHERE 
	    rp.IncludedInDailyPurge = 1
	  AND
	    rp.DailyPurgeCompleted = 0
	  AND 
	    rp.PurgeJobName = 'StraightPurge'
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