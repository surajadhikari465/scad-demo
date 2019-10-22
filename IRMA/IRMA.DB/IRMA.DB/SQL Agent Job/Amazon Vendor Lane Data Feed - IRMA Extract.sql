USE [msdb]
GO

BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0

IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @JobName as VARCHAR(100), @Command as VARCHAR(MAX), @ProxyName as VARCHAR(15), @RegionCode as VARCHAR(2), @CommandAlert as VARCHAR(MAX), @ScheduleStartTime as int
SET @RegionCode = 'PN'

-- Note: The password value in the following variable @Command is replaced with value ?????. During the depoloyment, the password value will be updated by the DBA implementer

IF RIGHT(@@SERVERNAME, 7) = 'SQL2014'
	BEGIN
		SET @JobName = N'Amazon Data Feed - IRMA Item Vendor Lane - ' + @RegionCode + ' (QA)'
		SET @ProxyName = N'PDXExtractQA'
		SET @Command = N'/ISSERVER "\"\SSISDB\AmzDataFeeds\AmzExtract-FromIrma\ItemVendorLaneExtract-FromIrma.dtsx\"" /SERVER "\"' + @@SERVERNAME + '\"" /Par "\"$Project::Environment\"";QA /Par "\"$Project::RegionCode\"";' + @RegionCode + ' /Par CompressFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\ZipFiles.bat\"" /Par DeleteFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\DeleteFiles.bat\"" /Par DeleteOldArchivedFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\DeleteOldArchiveFiles.bat\"" /Par ExtractSource;IRMA /Par FileArchivePath;"\"E:\AMZExtractData\QA\Archive\"" /Par FileArchiveRetention;3 /Par FileRootPath;"\"E:\AMZExtractData\QA\IRMA\ItemVendorLane\"" /Par esbCertificateName;"\"E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US\"" /Par esbJmsPassword;????? /Par esbJmsUsername;iconUser /Par esbMessageType;ByteMessage /Par esbSecurityCredentials;????? /Par esbSecurityPrincipal;jndiIconUser /Par esbServerUrl;"\"ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293\"" /Par esbSslPassword;????? /Par esbTargetHostName;"\"PERF-ESB-EMS-1.wfm.pvt\"" /Par esbTopicName;"\"WFMESB.SupplyChain.Retail.InventoryMgmt.Info.Topic.V2\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E'
		SET @CommandAlert=N'EXEC msdb.dbo.sp_send_dbmail
					@profile_name = ''SQLServerDbas'', 
					@recipients = ''irma.developers@wholefoods.com'',
					@copy_recipients = ''irma.developers@wholefoods.com'', 
					@body =''This Job has failed to retry this step please execute the following T-SQL on VIM-VM-PRD\VIM2014:
					exec msdb.dbo.startjob @jobname = ''''' + @JobName  + '''''''' + ',   
					@subject = ''' + @JobName + ' has failed'''
	END
ELSE
	BEGIN
		SET @JobName = N'Amazon Data Feed - IRMA Item Vendor Lane - ' + @RegionCode
		SET @ProxyName = N'PDXExtractPRD'
		SET @Command = N'/ISSERVER "\"\SSISDB\AmzDataFeeds\AmzExtract-FromIrma\ItemVendorLaneExtract-FromIrma.dtsx\"" /SERVER "\"' + @@SERVERNAME + '\"" /Par "\"$Project::Environment\"";PRD /Par "\"$Project::RegionCode\"";' + @RegionCode + ' /Par CompressFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\ZipFiles.bat\"" /Par DeleteFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\DeleteFiles.bat\"" /Par DeleteOldArchivedFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\DeleteOldArchiveFiles.bat\"" /Par ExtractSource;IRMA /Par FileArchivePath;"\"E:\AMZExtractData\Archive\"" /Par FileArchiveRetention;3 /Par FileRootPath;"\"E:\AMZExtractData\IRMA\ItemVendorLane\"" /Par esbCertificateName;"\"E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US\"" /Par esbJmsPassword;????? /Par esbJmsUsername;iconUser /Par esbMessageType;ByteMessage /Par esbSecurityCredentials;????? /Par esbSecurityPrincipal;jndiIconUser /Par esbServerUrl;"\"ssl://PROD-ESB-EMS-1.wfm.pvt:37293,ssl://PROD-ESB-EMS-2.wfm.pvt:37293,ssl://PROD-ESB-EMS-3.wfm.pvt:37293,ssl://PROD-ESB-EMS-4.wfm.pvt:37293\"" /Par esbSslPassword;????? /Par esbTargetHostName;"\"PROD-ESB-EMS-1.wfm.pvt\"" /Par esbTopicName;"\"WFMESB.SupplyChain.Retail.InventoryMgmt.Info.Topic.V2\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E'
		SET @CommandAlert=N'EXEC msdb.dbo.sp_send_dbmail
					@profile_name = ''SQLServerDbas'', 
					@recipients = ''irma.support@wholefoods.opsgenie.net'',
					@copy_recipients = ''scm-l3@wholefoods.com'', 
					@body =''This Job has failed to retry this step please execute the following T-SQL on VIM-VM-PRD\VIM2014:
					exec msdb.dbo.startjob @jobname = ''''' + @JobName  + '''''''' + ',   
					@subject = ''' + @JobName + ' has failed'''
	END

SELECT @ScheduleStartTime = CASE @RegionCode
	WHEN 'FL' THEN 	40000
	WHEN 'MA' THEN  41000
    WHEN 'MW' THEN  42000
    WHEN 'NA' THEN  43000
    WHEN 'NC' THEN  44000
    WHEN 'NE' THEN  45000
    WHEN 'PN' THEN  50000
    WHEN 'RM' THEN  51000
    WHEN 'SO' THEN  52000
    WHEN 'SP' THEN  53000
    WHEN 'SW' THEN  54000
	END
 
DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=@JobName, 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Daily Item Vendor Lane Data Extract from IRMA to Feed into Amazon', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Run Package', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=1, 
		@retry_interval=5, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=@Command, 
		@database_name=N'master', 
		@flags=0, 
		@proxy_name=@ProxyName
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Add Failure Alert', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=2, 
		@on_success_step_id=2, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=@CommandAlert, 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'AMZ IVL', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20181106, 
		@active_end_date=99991231, 
		@active_start_time=@ScheduleStartTime, 
		@active_end_time=235959
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO