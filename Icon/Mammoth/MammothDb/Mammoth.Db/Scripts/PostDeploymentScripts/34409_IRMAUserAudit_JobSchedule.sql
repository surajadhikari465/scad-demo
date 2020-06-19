--PBI 34409
DECLARE @scriptKey VARCHAR(128) = '34409_IRMAUserAudit_JobSchedule';

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @scriptKey
			)
		)
BEGIN
	DECLARE @Region VARCHAR(2)
	DECLARE @irmaUserAudit_jobname VARCHAR(100)
	DECLARE @irmaUserAudit_queuename VARCHAR(100)
	DECLARE @irmaUserAudit_startdatetimeutc DATETIME
	DECLARE @irmaUserAudit_NextScheduledDateTimeUtc DATETIME
	DECLARE @irmaUserAudit_xmlobject VARCHAR(max)

	DECLARE region_cursor CURSOR
	FOR
	SELECT Region
	FROM Regions
	WHERE Region NOT IN (
			'TS'			
			);

	OPEN region_cursor;

	FETCH NEXT
	FROM region_cursor
	INTO @Region

	WHILE @@FETCH_STATUS = 0
	BEGIN
		BEGIN
			SET @irmaUserAudit_jobname = 'IRMA User Audit - ' + @Region
			SET @irmaUserAudit_queuename = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
			SET @irmaUserAudit_startdatetimeutc = '2020-06-15 12:00:00'
			SET @irmaUserAudit_NextScheduledDateTimeUtc = '2020-09-14 12:00:00'
			SET @irmaUserAudit_xmlobject = '
		{
		  "zipOutput": false,
		  "concatenateOutputFiles": false,
		  "includeHeaders": true,
		  "source": "IRMA",
		  "regions": [ "' + @Region + '" ],
		  "query": "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	SELECT s.Store_No,s.Store_Name,uo.* FROM users uo
	LEFT JOIN (
		SELECT u.user_id,ust.Store_No
		FROM users u
		JOIN UserStoreTeamTitle ust ON u.user_id = ust.user_id
		GROUP BY u.user_id, ust.Store_No
		HAVING count(ust.store_no) >= 1
		) us ON uo.user_id = us.user_id
	LEFT JOIN store s ON us.store_no = s.store_no
	ORDER BY s.Store_No;",
		  "parameters": [],
		  "outputFileName": "IRMA_UserAudit_{date:yyyyMMdd}.csv",
		  "delimiter": ",",
		  "destination": {
			"type": "file",			
			"path": "\\\\ODWD6902\\Test_IRMAUserAudit\\"
		  }
		}
		'

			IF NOT EXISTS (
					SELECT 1
					FROM app.jobschedule
					WHERE jobname = @irmaUserAudit_jobname
					)
			BEGIN
				INSERT INTO app.JobSchedule
				SELECT @irmaUserAudit_jobname AS JobName
					,@Region AS REGION
					,@irmaUserAudit_queuename AS DestinationQueueName
					,@irmaUserAudit_startdatetimeutc AS StartDateTimeUtc
					,NULL AS LastScheduledTimeUtc
					,NULL AS lastRunEndDateTimeUtc
					,@irmaUserAudit_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
					,7776000 AS IntervalInSeconds
					,0 AS Enabled
					,'ready' AS STATUS
					,@irmaUserAudit_xmlobject AS XmlObject
					,NULL AS RunAdHoc
					,NULL AS InstanceId
			END
			ELSE
			BEGIN
				UPDATE app.JobSchedule
				SET Region = @Region
					,DestinationQueueName = @irmaUserAudit_queuename
					,StartDateTimeUtc = @irmaUserAudit_startdatetimeutc
					,LastScheduledDateTimeUtc = NULL
					,LastRunEndDateTimeUtc = NULL
					,NextScheduledDateTimeUtc = @irmaUserAudit_NextScheduledDateTimeUtc
					,IntervalInSeconds = 7776000
					,Enabled = 0
					,STATUS = 'ready'
					,xmlobject = @irmaUserAudit_xmlobject
					,RunAdHoc = NULL
					,InstanceId = NULL
				WHERE jobname = @irmaUserAudit_jobname
			END
		END

		FETCH NEXT
		FROM region_cursor
		INTO @Region
	END;

	CLOSE region_cursor;

	DEALLOCATE region_cursor;

	INSERT INTO app.PostDeploymentScriptHistory (
		ScriptKey
		,RunTime
		)
	VALUES (
		@scriptKey
		,GETDATE()
		);
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] Script ' + @scriptKey + ' already applied.'
END
GO