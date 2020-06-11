--PBI 34822
DECLARE @scriptKey VARCHAR(128) = '34822_ExtractService_Attributes';

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @scriptKey
			)
		)
BEGIN


	
	DECLARE @ExtractService_Attributes_jobname VARCHAR(100)
	DECLARE @ExtractService_Attributes_queuename VARCHAR(100)
	DECLARE @ExtractService_Attributes_startdatetimeutc DATETIME
	DECLARE @ExtractService_Attributes_NextScheduledDateTimeUtc DATETIME
	DECLARE @ExtractService_Attributes_xmlobject VARCHAR(max)

		    SET @ExtractService_Attributes_jobname = 'Icon Attribute Definition'
			SET @ExtractService_Attributes_queuename = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
			SET @ExtractService_Attributes_startdatetimeutc = '2020-06-10 01:00:00 AM'
			SET @ExtractService_Attributes_NextScheduledDateTimeUtc = '2020-06-11 01:00:00 AM'
			SET @ExtractService_Attributes_xmlobject = '
		{
		  "zipOutput": true,
		  "compressionType": "gzip",
		  "concatenateOutputFiles": false,
		  "includeHeaders": true,
		  "source": "ICON",
		  "regions": [],
		  "query": "exec [extract].[ExtractAttributes]",
		  "parameters": [],
		  "outputFileName": "Icon_Attributes_{date:yyyyMMddHHmmss}.txt",
		  "delimiter": "|",
		  "destination": {
			"type": "pathkey",			
			"pathKey": "CAP"
		  }
		}
		'

			IF NOT EXISTS (
					SELECT 1
					FROM app.jobschedule
					WHERE jobname = @ExtractService_Attributes_jobname
					)
			BEGIN
				INSERT INTO app.JobSchedule
				SELECT @ExtractService_Attributes_jobname AS JobName
					,null AS REGION
					,@ExtractService_Attributes_queuename AS DestinationQueueName
					,@ExtractService_Attributes_startdatetimeutc AS StartDateTimeUtc
					,NULL AS LastScheduledTimeUtc
					,NULL AS lastRunEndDateTimeUtc
					,@ExtractService_Attributes_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
					,86400 AS IntervalInSeconds
					,0 AS Enabled
					,'ready' AS STATUS
					,@ExtractService_Attributes_xmlobject AS XmlObject
					,NULL AS RunAdHoc
					,NULL AS InstanceId
			END
			ELSE
			BEGIN
				UPDATE app.JobSchedule
				SET Region = null
					,DestinationQueueName = @ExtractService_Attributes_queuename
					,StartDateTimeUtc = @ExtractService_Attributes_startdatetimeutc
					,LastScheduledDateTimeUtc = NULL
					,LastRunEndDateTimeUtc = NULL
					,NextScheduledDateTimeUtc = @ExtractService_Attributes_NextScheduledDateTimeUtc
					,IntervalInSeconds = 86400
					,Enabled = 0
					,STATUS = 'ready'
					,xmlobject = @ExtractService_Attributes_xmlobject
					,RunAdHoc = NULL
					,InstanceId = NULL
				WHERE jobname = @ExtractService_Attributes_jobname
			END




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
