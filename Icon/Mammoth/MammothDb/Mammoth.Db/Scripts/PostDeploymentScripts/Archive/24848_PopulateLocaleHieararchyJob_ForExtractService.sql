DECLARE @scriptKey VARCHAR(128) = '24848_PopulateLocaleHieararchyJob_ForExtractService';

IF(NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	
	IF NOT EXISTS (
		SELECT *
		FROM app.JobSchedule
		WHERE JobName = 'PDX Locale Hierarchy'
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
		'PDX Locale Hierarchy'
		,NULL
		,'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		,'2019-12-12 7:15:00.0000000'
		,NULL
		,NULL
		,'2019-12-13 7:15:00.0000000'
		,86400
		,1
		,'ready'
		,NULL
		,NULL
		,'{
  "zipOutput": true,
  "compressionType": "gzip",
  "concatenateOutputFiles": false,
  "includeHeaders": true,
  "source": "Icon",
  "regions": [],
  "query": "Exec [app].[PDX_LocationHierarchyFile]",
  "parameters": [],
  "outputFileName": "Location_Hierarchy_{date:yyyyMMdd}.csv",
  "delimiter": "|",
  "destination": {
    "type": "sftp",
    "credentialsKey": "predictix",
    "path": "/dev"
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
