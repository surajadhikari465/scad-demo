--PBI 26539
DECLARE @scriptKey VARCHAR(128) = '26539_FutureCostsExtract_JobSchedule';
IF(NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	DECLARE @futurecosts_jobname VARCHAR(100) = 'IRMA Future Costs'
	DECLARE @futurecosts_queuename VARCHAR(100) = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
	DECLARE @futurecosts_startdatetimeutc DATETIME = '2020-04-02 06:00:00'
	DECLARE @futurecosts_NextScheduledDateTimeUtc DATETIME = '2020-04-02 06:00:00'
	DECLARE @futurecosts_xmlobject VARCHAR(max) = '
		{
		  "zipOutput": true,
		  "compressionType": "gzip",
		  "concatenateOutputFiles": false,
		  "includeHeaders": false,
		  "source": "IRMA",
		  "regions": [ "FL", "MA", "MW", "NA", "NC", "NE", "PN", "RM", "SO", "SP", "SW", "UK" ],
		  "query": "Exec [extract].[APT_FutureCostsExtract]",
		  "parameters": [],
		  "outputFileName": "AP_{region}_pdx_future_cost_{date:yyyyMMdd}.csv",
		  "delimiter": "|",
		  "destination": {
			"type": "sftp",
			"credentialsKey": "predictix",
			"path": "/to_predictix/staging_APT/"
		  }
		}
		'
	IF NOT EXISTS (
			SELECT 1
			FROM app.jobschedule
			WHERE jobname = @futurecosts_jobname
			)
	BEGIN
		INSERT INTO app.JobSchedule
		SELECT @futurecosts_jobname AS JobName
			,NULL AS REGION
			,@futurecosts_queuename AS DestinationQueueName
			,@futurecosts_startdatetimeutc AS StartDateTimeUtc
			,NULL AS LastScheduledTimeUtc
			,NULL AS lastRunEndDateTimeUtc
			,@futurecosts_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
			,86400 AS IntervalInSeconds
			,0 AS Enabled
			,'ready' AS STATUS
			,@futurecosts_xmlobject AS XmlObject
			,NULL AS RunAdHoc
			,NULL AS InstanceId
	END
	ELSE
	BEGIN
		UPDATE app.JobSchedule
		SET Region = NULL
			,DestinationQueueName = @futurecosts_queuename
			,StartDateTimeUtc = @futurecosts_startdatetimeutc
			,LastScheduledDateTimeUtc = NULL
			,LastRunEndDateTimeUtc = NULL
			,NextScheduledDateTimeUtc = @futurecosts_NextScheduledDateTimeUtc
			,IntervalInSeconds = 86400
			,Enabled = 0
			,STATUS = 'ready'
			,xmlobject = @futurecosts_xmlobject
			,RunAdHoc = NULL
			,InstanceId = NULL
		WHERE jobname = @futurecosts_jobname
	END
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE());
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] Script ' + @scriptKey + ' already applied.'
END
GO
