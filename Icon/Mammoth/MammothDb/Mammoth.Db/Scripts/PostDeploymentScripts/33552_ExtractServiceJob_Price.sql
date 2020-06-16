DECLARE @scriptKey VARCHAR(128) = '33552_ExtractService_Price';

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @scriptKey
			)
		)
BEGIN

	DECLARE @current_region VARCHAR(5)
	DECLARE @ExtractService_Price_jobname VARCHAR(100)
	DECLARE @ExtractService_Price_queuename VARCHAR(100)
	DECLARE @ExtractService_Price_startdatetimeutc DATETIME
	DECLARE @ExtractService_Price_NextScheduledDateTimeUtc DATETIME
	DECLARE @ExtractService_Price_xmlobject VARCHAR(max)

	DECLARE region_cursor CURSOR FOR   
	SELECT Region
	FROM   dbo.Regions
	WHERE Region not in ('TS','UK') 

	OPEN region_cursor  
  
	FETCH NEXT FROM region_cursor   
	INTO @current_region
  
	WHILE @@FETCH_STATUS = 0  
	BEGIN 



		    SET @ExtractService_Price_jobname = 'Mammoth Price Extract - ' + @current_region
			SET @ExtractService_Price_queuename = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
			SET @ExtractService_Price_startdatetimeutc = '2020-06-15 05:00:00 AM'
			SET @ExtractService_Price_NextScheduledDateTimeUtc = '2020-06-16 05:00:00 AM'
			SET @ExtractService_Price_xmlobject = '
{
	"zipOutput": true,
	"compressionType": "gzip",
	"concatenateOutputFiles": false,
	"includeHeaders": true,
	"source": "Mammoth",
	"regions": [],
	"query": "exec  dbo.AuditGPMPrice_NoGrouping @Region",
	"parameters": [
	{
		"Key": "@Region",
		"Value": "'+ @current_region +'"
	}
	],
	"outputFileName": "{source}_Price_'+ @current_region +'_{date:MMddyyyyHHmmss}.txt",
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
					WHERE jobname = @ExtractService_Price_jobname
					)
			BEGIN
				INSERT INTO app.JobSchedule
				SELECT @ExtractService_Price_jobname AS JobName
					,null AS REGION
					,@ExtractService_Price_queuename AS DestinationQueueName
					,@ExtractService_Price_startdatetimeutc AS StartDateTimeUtc
					,NULL AS LastScheduledTimeUtc
					,NULL AS lastRunEndDateTimeUtc
					,@ExtractService_Price_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
					,86400 AS IntervalInSeconds
					,0 AS Enabled
					,'ready' AS STATUS
					,@ExtractService_Price_xmlobject AS XmlObject
					,NULL AS RunAdHoc
					,NULL AS InstanceId
			END
			ELSE
			BEGIN
				UPDATE app.JobSchedule
				SET Region = null
					,DestinationQueueName = @ExtractService_Price_queuename
					,StartDateTimeUtc = @ExtractService_Price_startdatetimeutc
					,LastScheduledDateTimeUtc = NULL
					,LastRunEndDateTimeUtc = NULL
					,NextScheduledDateTimeUtc = @ExtractService_Price_NextScheduledDateTimeUtc
					,IntervalInSeconds = 86400
					,Enabled = 0
					,STATUS = 'ready'
					,xmlobject = @ExtractService_Price_xmlobject
					,RunAdHoc = NULL
					,InstanceId = NULL
				WHERE jobname = @ExtractService_Price_jobname
			END

		
		FETCH NEXT FROM region_cursor   
		INTO @current_region
	END

	CLOSE region_cursor;  
	DEALLOCATE region_cursor ;  


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
