--PBI: 25982
DECLARE @scriptKey VARCHAR(128) = '25982_IRMAItemAttributeFileSentToS3';

IF(NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	
	IF NOT EXISTS (
		SELECT *
		FROM app.JobSchedule
		WHERE JobName = 'IRMA Item Attribute'
		)
	INSERT INTO app.JobSchedule (
		JobName
		,Region
		,DestinationQueueName
		,StartDateTimeUtc
		,LastScheduledDateTimeUtc
		,LastRunEndDateTimeUtc
		,NextScheduledDateTimeUtc
		,IntervalInSeconds
		,Enabled
		,STATUS
		,RunAdHoc
		,InstanceId
		,XmlObject
		)
	VALUES (
		'IRMA Item Attribute'
		,NULL
		,'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		,'2019-12-09 14:00:00.0000000'
		,NULL
		,NULL
		,'2019-12-22 14:26:59.0000000'
		,86400
		,0
		,'ready'
		,NULL
		,NULL
		,'{
  "zipOutput": true,
  "concatenateOutputFiles": true,
  "includeHeaders": true,
  "source": "IRMA",
  "regions": [
    "FL",
    "MA",
    "MW",
    "NA",
    "NC",
    "NE",
    "PN",
    "RM",
    "SO",
    "SP",
    "SW",
    "UK"
  ],
  "query": "exec extract.ItemAttribute",
  "parameters": [],
  "outputFileName": "IRMA_ITEM_{date:YYYYMMDDHHmmss}.txt",
  "delimiter": "|",
  "destination": {
    "type": "file",
    "path": "\\\\10.8.164.236\\in\\"
  }
}'
		)

    INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE());
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] Script ' + @scriptKey + ' already applied.'
END
GO