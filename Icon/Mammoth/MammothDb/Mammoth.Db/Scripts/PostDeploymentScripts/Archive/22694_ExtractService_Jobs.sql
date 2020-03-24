--PBI: 23063
DECLARE @scriptKey VARCHAR(128) = '22694_PopulateExtractServiceJobs';

IF(NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	
	IF NOT EXISTS (
		SELECT *
		FROM app.JobSchedule
		WHERE JobName = 'APT Item Store'
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
		'APT Item Store'
		,NULL
		,'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		,'2019-11-01 14:00:00.0000000'
		,NULL
		,NULL
		,'2019-11-13 14:26:59.0000000'
		,86400
		,0
		,'ready'
		,NULL
		,NULL
		,'{
  "zipOutput": false,
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
  "query": "exec extract.apt_itemstore",
  "parameters": [],
  "outputFileName": "{source}_Item_Store_{date:MMddyyyyHHmmss}.txt",
  "delimiter": "|",
  "destination": {
    "type": "file",
    "path": "\\\\10.8.164.236\\in\\"
  }
}'
		)

IF NOT EXISTS (
		SELECT *
		FROM app.JobSchedule
		WHERE JobName = 'APT Supplier Id'
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
		'APT Supplier Id'
		,NULL
		,'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		,'2019-11-01 14:00:00.0000000'
		,NULL
		,NULL
		,'2019-11-13 14:26:59.0000000'
		,86400
		,0
		,'ready'
		,NULL
		,NULL
		,'{
  "zipOutput": false,
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
  "query": "exec extract.APT_SupplierId",
  "parameters": [],
  "outputFileName": "{source}_Supplier_Id_{date:MMddyyyyHHmmss}.txt",
  "delimiter": "|",
  "destination": {
    "type": "file",
    "path": "\\\\10.8.164.236\\in\\"
  }
}'
		)


IF NOT EXISTS (
		SELECT *
		FROM app.JobSchedule
		WHERE JobName = 'APT Supplier Name'
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
		'APT Supplier Name'
		,NULL
		,'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		,'2019-11-01 14:00:00.0000000'
		,NULL
		,NULL
		,'2019-11-13 14:26:59.0000000'
		,86400
		,0
		,'ready'
		,NULL
		,NULL
		,'{
  "zipOutput": false,
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
  "query": "exec extract.APT_SupplierName",
  "parameters": [],
  "outputFileName": "{source}_Supplier_Name_{date:MMddyyyyHHmmss}.txt",
  "delimiter": "|",
  "destination": {
    "type": "file",
    "path": "\\\\10.8.164.236\\in\\"
  }
}'
		)


IF NOT EXISTS (
		SELECT *
		FROM app.JobSchedule
		WHERE JobName = 'APT Brand Hierarchy'
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
		'APT Brand Hierarchy'
		,NULL
		,'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		,'2019-11-01 14:00:00.0000000'
		,NULL
		,NULL
		,'2019-11-13 14:26:59.0000000'
		,86400
		,0
		,'ready'
		,NULL
		,NULL
		,'{
  "zipOutput": false,
  "concatenateOutputFiles": true,
  "includeHeaders": true,
  "source": "ICON",
  "regions": [],
  "query": "exec extract.ExtractHierarchy @HierarchyId, @TraitCodes, @TraitGroupCodes",
  "parameters": [
  {"Key":"@HierarchyId","Value":2},
  {"Key":"@TraitCodes","Value":"BA,ZIP,LCL,ARC,PCO,GRD"},
  {"Key":"@TraitGroupCodes","Value":""}
  ],
  "outputFileName": "{source}_Brand_Hierarchy_{date:MMddyyyyHHmmss}.txt",
  "delimiter": "|",
  "destination": {
    "type": "file",
    "path": "\\\\10.8.164.236\\in\\"
  }
}'
		)


IF NOT EXISTS (
		SELECT *
		FROM app.JobSchedule
		WHERE JobName = 'APT Locale Attributes'
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
		'APT Locale Attributes'
		,NULL
		,'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		,'2019-11-01 14:00:00.0000000'
		,NULL
		,NULL
		,'2019-11-13 14:26:59.0000000'
		,86400
		,0
		,'ready'
		,NULL
		,NULL
		,'{
  "zipOutput": false,
  "concatenateOutputFiles": true,
  "includeHeaders": true,
  "source": "ICON",
  "regions": [],
  "query": "exec extract.LocationAttributes",
  "parameters": [],
  "outputFileName": "{source}_Locale_Attributes_{date:MMddyyyyHHmmss}.txt",
  "delimiter": "|",
  "destination": {
    "type": "file",
    "path": "\\\\10.8.164.236\\in\\"
  }
}'
		)


IF NOT EXISTS (
		SELECT *
		FROM app.JobSchedule
		WHERE JobName = 'APT Locale Hierarchy'
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
		'APT Locale Hierarchy'
		,NULL
		,'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		,'2019-11-01 14:00:00.0000000'
		,NULL
		,NULL
		,'2019-11-13 14:26:59.0000000'
		,86400
		,0
		,'ready'
		,NULL
		,NULL
		,'{
  "zipOutput": false,
  "concatenateOutputFiles": true,
  "includeHeaders": true,
  "source": "ICON",
  "regions": [],
  "query": "exec extract.LocationHierarchy",
  "parameters": [],
  "outputFileName": "{source}_Locale_Hierarchy_{date:MMddyyyyHHmmss}.txt",
  "delimiter": "|",
  "destination": {
    "type": "file",
    "path": "\\\\10.8.164.236\\in\\"
  }
}'
		)





IF NOT EXISTS (
		SELECT *
		FROM app.JobSchedule
		WHERE JobName = 'APT Product Hierarchy'
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
		'APT Product Hierarchy'
		,NULL
		,'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		,'2019-11-01 14:00:00.0000000'
		,NULL
		,NULL
		,'2019-11-13 14:26:59.0000000'
		,86400
		,0
		,'ready'
		,NULL
		,NULL
		,'{
  "zipOutput": false,
  "concatenateOutputFiles": true,
  "includeHeaders": true,
  "source": "ICON",
  "regions": [],
  "query": "exec extract.ProductHierarchy",
  "parameters": [],
  "outputFileName": "{source}_Product_Hierarchy_{date:MMddyyyyHHmmss}.txt",
  "delimiter": "|",
  "destination": {
    "type": "file",
    "path": "\\\\10.8.164.236\\in\\"
  }
}'
		)

IF NOT EXISTS (
		SELECT *
		FROM app.JobSchedule
		WHERE JobName = 'APT Product Attributes'
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
		'APT Product Attributes'
		,NULL
		,'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		,'2019-11-01 14:00:00.0000000'
		,NULL
		,NULL
		,'2019-11-13 14:26:59.0000000'
		,86400
		,0
		,'ready'
		,NULL
		,NULL
		,'{
  "zipOutput": false,
  "concatenateOutputFiles": true,
  "includeHeaders": true,
  "source": "ICON",
  "regions": [],
  "query": "exec extract.ItemsWithAttributes",
  "parameters": [],
  "outputFileName": "{source}_Product_Attributes_{date:MMddyyyyHHmmss}.txt",
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




