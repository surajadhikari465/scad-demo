DECLARE @scriptKey VARCHAR(128) = '41985_AMZItemVendorLane_JobSchedule';
IF(NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN

DECLARE @Region VARCHAR(2)
DECLARE @AmzIVL_jobname VARCHAR(100) 
DECLARE @AmzIVL_queuename VARCHAR(100) 
DECLARE @AmzIVL_startdatetimeutc DATETIME
DECLARE @AmzIVL_NextScheduledDateTimeUtc DATETIME
DECLARE @AmzIVL_xmlobject VARCHAR(MAX)

DECLARE region_cursor CURSOR FOR
SELECT Region
FROM Regions
WHERE Region not in ('TS', 'UK');

OPEN region_cursor;
FETCH NEXT FROM region_cursor
INTO @Region

WHILE @@FETCH_STATUS = 0
   BEGIN
      BEGIN
		SET @AmzIVL_jobname = 'Amazon Item Vendor Lane From IRMA - ' + @Region 
		SET @AmzIVL_queuename = 'WFMSB1.SCAD.Audit.ExtractService.Queue.V1'
		SET @AmzIVL_startdatetimeutc = '2020-04-20 05:00:00'
		SET @AmzIVL_NextScheduledDateTimeUtc = '2020-04-20 05:00:00'
		SET @AmzIVL_xmlobject = '
		{
		  "zipOutput": true,
		  "compressionType": "gzip",
		  "concatenateOutputFiles": false,
		  "includeHeaders": true,
		  "source": "IRMA",
		  "regions": [ "' + @Region + '" ],
		  "stagingQuery": "Exec [amz].[PopulateVendorLaneExtract] ",
          "dynamicParameterQuery": "select distinct(STORE_NUMBER) as Value, ''STORE_NUMBER'' as [Key] from [dbo].[VendorLaneExtract]",
          "query" : "SELECT [UPC],[ITEM_KEY],[VIN],[STORE_NUMBER],[VENDOR_NUMBER],[VENDOR_NAME],[VENDOR_CASE_SIZE],RTRIM([VENDOR_CASE_UOM]) AS [VENDOR_CASE_UOM],RTRIM([VENDOR_COST_UOM]) AS [VENDOR_COST_UOM],[REG_COST],RTRIM([RETAIL_UOM]) AS [RETAIL_UOM],[RETAIL_PACK],CASE [PRIMARY_VENDOR] WHEN 1 THEN ''Y'' ELSE ''N'' END AS  [PRIMARY_VENDOR],[GLOBAL_SUBTEAM] FROM [dbo].[VendorLaneExtract] where STORE_NUMBER = @STORE_NUMBER order by STORE_NUMBER, UPC",
		  "parameters": [],
		  "outputFileName": "item_vendor_lane_{region}_{STORE_NUMBER}_{date:yyyyMMdd}.csv",
		  "delimiter": "|",
		  "destination": {
			"type": "esb",
			"credentialsKey": "inStock"
		  }
		}
		'
		IF NOT EXISTS (
			SELECT 1
			FROM app.jobschedule
			WHERE jobname = @AmzIVL_jobname
			)
		BEGIN
			INSERT INTO app.JobSchedule
			SELECT @AmzIVL_jobname AS JobName
				,@Region AS REGION
				,@AmzIVL_queuename AS DestinationQueueName
				,@AmzIVL_startdatetimeutc AS StartDateTimeUtc
				,NULL AS LastScheduledTimeUtc
				,NULL AS lastRunEndDateTimeUtc
				,@AmzIVL_NextScheduledDateTimeUtc AS NextScheduledDateTimeUtc
				,86400 AS IntervalInSeconds
				,0 AS Enabled
				,'ready' AS STATUS
				,@AmzIVL_xmlobject AS XmlObject
				,NULL AS RunAdHoc
				,NULL AS InstanceId
		END
		ELSE
		BEGIN
			UPDATE app.JobSchedule
			SET Region = @Region
				,DestinationQueueName = @AmzIVL_queuename
				,StartDateTimeUtc = @AmzIVL_startdatetimeutc
				,LastScheduledDateTimeUtc = NULL
				,LastRunEndDateTimeUtc = NULL
				,NextScheduledDateTimeUtc = @AmzIVL_NextScheduledDateTimeUtc
				,IntervalInSeconds = 86400
				,Enabled = 0
				,STATUS = 'ready'
				,xmlobject = @AmzIVL_xmlobject
				,RunAdHoc = NULL
				,InstanceId = NULL
			WHERE jobname = @AmzIVL_jobname
		END
	  END
   FETCH NEXT FROM region_cursor
	INTO @Region
   END;

CLOSE region_cursor;
DEALLOCATE region_cursor;
INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE());
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] Script ' + @scriptKey + ' already applied.'
END
GO

