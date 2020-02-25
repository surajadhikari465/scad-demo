DECLARE @ivl_scriptKey VARCHAR(128) = '30224_IVL_Item_JobSchedule';

IF(NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @ivl_scriptKey))
BEGIN

	DECLARE @ivl_jobname VARCHAR(100) = 'IRMA IVL Item'
	DECLARE @ivl_queuename VARCHAR(100) = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
	DECLARE @ivl_startdatetimeutc DATETIME = '2019-11-01 23:00:00'
	DECLARE @ivl_NextScheduledDateTimeUtc DATETIME = '2019-11-01 23:00:00'
	DECLARE @ivl_xmlobject VARCHAR(max) = '
		{
		  "zipOutput": true,
		  "compressionType": "zip",
		  "concatenateOutputFiles": true,
		  "includeHeaders": true,
		  "source": "IRMA",
		  "regions": [ "FL", "MA", "MW", "NA", "NC", "NE", "PN", "RM", "SO", "SP", "SW", "UK" ],
		  "query": "select i.Item_Key, ii.Identifier, ii.Default_Identifier, ii.Deleted_Identifier, i.Deleted_Item from item i inner join ItemIdentifier ii on i.item_key = ii.Item_Key",
		  "parameters": [],
		  "outputFileName": "itemvendorlane_item_{source}_{date:yyyyMMdd}.csv",
		  "delimiter": "|",
		  "destination": {
			"type": "file",
			"path": "\\\\10.8.166.169\\14447-scmpa-cap-qa\\cap\\inbound\\"
		  }
		}
		'

	IF NOT EXISTS (
			SELECT 1
			FROM app.jobschedule
			WHERE jobname = @ivl_jobname
			)
	BEGIN
		INSERT INTO app.JobSchedule
		SELECT @ivl_jobname AS JobName
			,NULL AS REGION
			,@ivl_queuename AS DestinationQueueName
			,@ivl_startdatetimeutc AS StartDateTimeUtc
			,NULL AS LastScheduledTimeUtc
			,NULL AS lastRunEndDateTimeUtc
			,@ivl_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
			,86400 AS IntervalInSeconds
			,0 AS Enabled
			,'ready' AS STATUS
			,@ivl_xmlobject AS XmlObject
			,NULL AS RunAdHoc
			,NULL AS InstanceId
	END
	ELSE
	BEGIN
		UPDATE app.JobSchedule
		SET Region = NULL
			,DestinationQueueName = @ivl_queuename
			,StartDateTimeUtc = @ivl_startdatetimeutc
			,LastScheduledDateTimeUtc = NULL
			,LastRunEndDateTimeUtc = NULL
			,NextScheduledDateTimeUtc = @ivl_NextScheduledDateTimeUtc
			,IntervalInSeconds = 86400
			,Enabled = 0
			,STATUS = 'ready'
			,xmlobject = @ivl_xmlobject
			,RunAdHoc = NULL
			,InstanceId = NULL
		WHERE jobname = @ivl_jobname
	END


	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@ivl_scriptKey ,GETDATE());
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] Script ' + @ivl_scriptKey + ' already applied.'
END
GO
