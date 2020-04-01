DECLARE @ih_scriptKey VARCHAR(128) = '33668_Extract_IconHierarcies_JobSchedule';

IF(NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @ih_scriptKey))
BEGIN

	-- national

	DECLARE @ih_jobname VARCHAR(100) = 'Icon Hierarchy National'
	DECLARE @ih_queuename VARCHAR(100) = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
	DECLARE @ih_startdatetimeutc DATETIME = '2020-3-30 6:00:00'
	DECLARE @ih_NextScheduledDateTimeUtc DATETIME = '2020-3-31 6:00:00'
	DECLARE @ih_xmlobject VARCHAR(max) = '
		{
		  "zipOutput": true,
		  "concatenateOutputFiles": true,
		  "includeHeaders": true,
		  "source": "ICON",
		  "query": "set transaction isolation level read uncommitted; declare @NccTraitId int = (select traitid from trait where traitcode = ''NCC''); select hc.hierarchyClassID, hierarchyClassName, nationalClassCode=hct.traitValue,hierarchyParentClassID, hierarchyLevel from hierarchy h inner join HierarchyClass hc on h.hierarchyID = hc.hierarchyID inner join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID where h.hierarchyName = ''National'' and hct.traitID = @NccTraitId order by hierarchyLevel;",
		  "parameters": [],
		  "outputFileName": "{source}_hierarchy_national_{date:yyyyMMdd}.csv",
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
			WHERE jobname = @ih_jobname
			)
	BEGIN
		INSERT INTO app.JobSchedule
		SELECT @ih_jobname AS JobName
			,NULL AS REGION
			,@ih_queuename AS DestinationQueueName
			,@ih_startdatetimeutc AS StartDateTimeUtc
			,NULL AS LastScheduledTimeUtc
			,NULL AS lastRunEndDateTimeUtc
			,@ih_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
			,86400 AS IntervalInSeconds
			,0 AS Enabled
			,'ready' AS STATUS
			,@ih_xmlobject AS XmlObject
			,NULL AS RunAdHoc
			,NULL AS InstanceId
	END
	ELSE
	BEGIN
		UPDATE app.JobSchedule
		SET Region = NULL
			,DestinationQueueName = @ih_queuename
			,StartDateTimeUtc = @ih_startdatetimeutc
			,LastScheduledDateTimeUtc = NULL
			,LastRunEndDateTimeUtc = NULL
			,NextScheduledDateTimeUtc = @ih_NextScheduledDateTimeUtc
			,IntervalInSeconds = 86400
			,Enabled = 0
			,STATUS = 'ready'
			,xmlobject = @ih_xmlobject
			,RunAdHoc = NULL
			,InstanceId = NULL
		WHERE jobname = @ih_jobname
	END
	
	
	-- tax
	
	set @ih_jobname = 'Icon Hierarchy Tax'
	set @ih_queuename = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
	set @ih_xmlobject = '
		{
		  "zipOutput": true,
		  "concatenateOutputFiles": true,
		  "includeHeaders": true,
		  "source": "ICON",
		  "query": "set transaction isolation level read uncommitted; select hc.hierarchyClassID, hierarchyClassName from Hierarchy h inner join HierarchyClass hc on h.hierarchyID = hc.hierarchyID where h.hierarchyName = ''Tax'' order by hierarchyClassID;",
		  "parameters": [],
		  "outputFileName": "{source}_hierarchy_tax_{date:yyyyMMdd}.csv",
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
			WHERE jobname = @ih_jobname
			)
	BEGIN
		INSERT INTO app.JobSchedule
		SELECT @ih_jobname AS JobName
			,NULL AS REGION
			,@ih_queuename AS DestinationQueueName
			,@ih_startdatetimeutc AS StartDateTimeUtc
			,NULL AS LastScheduledTimeUtc
			,NULL AS lastRunEndDateTimeUtc
			,@ih_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
			,86400 AS IntervalInSeconds
			,0 AS Enabled
			,'ready' AS STATUS
			,@ih_xmlobject AS XmlObject
			,NULL AS RunAdHoc
			,NULL AS InstanceId
	END
	ELSE
	BEGIN
		UPDATE app.JobSchedule
		SET Region = NULL
			,DestinationQueueName = @ih_queuename
			,StartDateTimeUtc = @ih_startdatetimeutc
			,LastScheduledDateTimeUtc = NULL
			,LastRunEndDateTimeUtc = NULL
			,NextScheduledDateTimeUtc = @ih_NextScheduledDateTimeUtc
			,IntervalInSeconds = 86400
			,Enabled = 0
			,STATUS = 'ready'
			,xmlobject = @ih_xmlobject
			,RunAdHoc = NULL
			,InstanceId = NULL
		WHERE jobname = @ih_jobname
	END		


	-- financial
	
	set @ih_jobname  = 'Icon Hierarchy Financial'
	set @ih_queuename = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
	set @ih_xmlobject = '
		{
		  "zipOutput": true,
		  "concatenateOutputFiles": true,
		  "includeHeaders": true,
		  "source": "ICON",
		  "query": "set transaction isolation level read uncommitted; select hc.hierarchyClassID, hierarchyClassName from Hierarchy h inner join HierarchyClass hc on h.hierarchyID = hc.hierarchyID where h.hierarchyName = ''Financial'' order by hierarchyClassID;",
		  "parameters": [],
		  "outputFileName": "{source}_hierarchy_financial_{date:yyyyMMdd}.csv",
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
			WHERE jobname = @ih_jobname
			)
	BEGIN
		INSERT INTO app.JobSchedule
		SELECT @ih_jobname AS JobName
			,NULL AS REGION
			,@ih_queuename AS DestinationQueueName
			,@ih_startdatetimeutc AS StartDateTimeUtc
			,NULL AS LastScheduledTimeUtc
			,NULL AS lastRunEndDateTimeUtc
			,@ih_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
			,86400 AS IntervalInSeconds
			,0 AS Enabled
			,'ready' AS STATUS
			,@ih_xmlobject AS XmlObject
			,NULL AS RunAdHoc
			,NULL AS InstanceId
	END
	ELSE
	BEGIN
		UPDATE app.JobSchedule
		SET Region = NULL
			,DestinationQueueName = @ih_queuename
			,StartDateTimeUtc = @ih_startdatetimeutc
			,LastScheduledDateTimeUtc = NULL
			,LastRunEndDateTimeUtc = NULL
			,NextScheduledDateTimeUtc = @ih_NextScheduledDateTimeUtc
			,IntervalInSeconds = 86400
			,Enabled = 0
			,STATUS = 'ready'
			,xmlobject = @ih_xmlobject
			,RunAdHoc = NULL
			,InstanceId = NULL
		WHERE jobname = @ih_jobname
	END

	-- manufacturer
	
	set @ih_jobname = 'Icon Hierarchy Manufacturer'
	set @ih_queuename = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
	set @ih_xmlobject = '
		{
		  "zipOutput": true,
		  "concatenateOutputFiles": true,
		  "includeHeaders": true,
		  "source": "ICON",
		  "query": "set transaction isolation level read uncommitted; select hc.hierarchyClassID, hierarchyClassName from Hierarchy h inner join HierarchyClass hc on h.hierarchyID = hc.hierarchyID where h.hierarchyName = ''Manufacturer'' order by hierarchyClassID;",
		  "parameters": [],
		  "outputFileName": "{source}_hierarchy_manufacturer_{date:yyyyMMdd}.csv",
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
			WHERE jobname = @ih_jobname
			)
	BEGIN
		INSERT INTO app.JobSchedule
		SELECT @ih_jobname AS JobName
			,NULL AS REGION
			,@ih_queuename AS DestinationQueueName
			,@ih_startdatetimeutc AS StartDateTimeUtc
			,NULL AS LastScheduledTimeUtc
			,NULL AS lastRunEndDateTimeUtc
			,@ih_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
			,86400 AS IntervalInSeconds
			,0 AS Enabled
			,'ready' AS STATUS
			,@ih_xmlobject AS XmlObject
			,NULL AS RunAdHoc
			,NULL AS InstanceId
	END
	ELSE
	BEGIN
		UPDATE app.JobSchedule
		SET Region = NULL
			,DestinationQueueName = @ih_queuename
			,StartDateTimeUtc = @ih_startdatetimeutc
			,LastScheduledDateTimeUtc = NULL
			,LastRunEndDateTimeUtc = NULL
			,NextScheduledDateTimeUtc = @ih_NextScheduledDateTimeUtc
			,IntervalInSeconds = 86400
			,Enabled = 0
			,STATUS = 'ready'
			,xmlobject = @ih_xmlobject
			,RunAdHoc = NULL
			,InstanceId = NULL
		WHERE jobname = @ih_jobname
	END
	
	-- merch
	
	set @ih_jobname = 'Icon Hierarchy Merchandise'
	set @ih_queuename = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
	set @ih_xmlobject  = '
		{
		  "zipOutput": true,
		  "concatenateOutputFiles": true,
		  "includeHeaders": true,
		  "source": "ICON",
		  "query": "set transaction isolation level read uncommitted; select hc.hierarchyClassID, hierarchyClassName,hierarchyParentClassID, hierarchyLevel from Hierarchy h inner join HierarchyClass hc on h.hierarchyID = hc.hierarchyID where h.hierarchyName = ''Merchandise'' order by hierarchyLevel, hierarchyClassID;",
		  "parameters": [],
		  "outputFileName": "{source}_hierarchy_merchandise_{date:yyyyMMdd}.csv",
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
			WHERE jobname = @ih_jobname
			)
	BEGIN
		INSERT INTO app.JobSchedule
		SELECT @ih_jobname AS JobName
			,NULL AS REGION
			,@ih_queuename AS DestinationQueueName
			,@ih_startdatetimeutc AS StartDateTimeUtc
			,NULL AS LastScheduledTimeUtc
			,NULL AS lastRunEndDateTimeUtc
			,@ih_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
			,86400 AS IntervalInSeconds
			,0 AS Enabled
			,'ready' AS STATUS
			,@ih_xmlobject AS XmlObject
			,NULL AS RunAdHoc
			,NULL AS InstanceId
	END
	ELSE
	BEGIN
		UPDATE app.JobSchedule
		SET Region = NULL
			,DestinationQueueName = @ih_queuename
			,StartDateTimeUtc = @ih_startdatetimeutc
			,LastScheduledDateTimeUtc = NULL
			,LastRunEndDateTimeUtc = NULL
			,NextScheduledDateTimeUtc = @ih_NextScheduledDateTimeUtc
			,IntervalInSeconds = 86400
			,Enabled = 0
			,STATUS = 'ready'
			,xmlobject = @ih_xmlobject
			,RunAdHoc = NULL
			,InstanceId = NULL
		WHERE jobname = @ih_jobname
	END	


	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@ih_scriptKey ,GETDATE());
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] Script ' + @ih_scriptKey + ' already applied.'
END
GO
