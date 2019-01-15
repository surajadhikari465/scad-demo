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

DECLARE @JobName as VARCHAR(100), @Command as VARCHAR(MAX), @ProxyName as VARCHAR(15), @RegionCode as VARCHAR(2)
SET @RegionCode = 'PN'

-- Note: The password value in the following variable @Command is replaced with value ?????. During the depoloyment, the password value will be updated by the DBA implementer

IF RIGHT(@@SERVERNAME, 7) = 'SQL2014'
	BEGIN
		SET @JobName = N'Amazon Data Feed - IRMA Item Vendor Lane - ' + @RegionCode + '(QA)'
		SET @ProxyName = N'PDXExtractQA'
		SET @Command = N'/ISSERVER "\"\SSISDB\AmzDataFeeds\AmzExtract-FromIrma\ItemVendorLaneExtract-FromIrma.dtsx\"" /SERVER "\"' + @@SERVERNAME + '\"" /ENVREFERENCE 9 /Par "\"$Project::Environment\"";QA /Par "\"$Project::RegionCode\"";' + @RegionCode + ' /Par CompressFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\ZipFiles.bat\"" /Par DeleteFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\DeleteFiles.bat\"" /Par DeleteOldArchivedFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\DeleteOldArchiveFiles.bat\"" /Par ExtractSource;IRMA /Par FileArchivePath;"\"E:\AMZExtractData\QA\Archive\"" /Par FileArchiveRetention;3 /Par FileRootPath;"\"E:\AMZExtractData\QA\IRMA\ItemVendorLane\"" /Par esbCertificateName;"\"CN=CA-ROOT-I, O=Whole Foods Market\"" /Par esbJmsPassword;????? /Par esbJmsUsername;iconUser /Par esbMessageType;ByteMessage /Par esbSecurityCredentials;????? /Par esbSecurityPrincipal;jndiIconUser /Par esbServerUrl;"\"ssl://cerd1617.wfm.pvt:17293\"" /Par esbSslPassword;????? /Par esbTargetHostName;"\"cerd1617.wfm.pvt\"" /Par esbTopicName;"\"WFMESB.SupplyChain.Retail.InventoryMgmt.Info.Topic.V2\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E'
	END
ELSE
	BEGIN
		SET @JobName = N'Amazon Data Feed - IRMA Item Vendor Lane - ' + @RegionCode
		SET @ProxyName = N'PDXExtractPRD'
		SET @Command = N'/ISSERVER "\"\SSISDB\AmzDataFeeds\AmzExtract-FromIrma\ItemVendorLaneExtract-FromIrma.dtsx\"" /SERVER "\"' + @@SERVERNAME + '\"" /ENVREFERENCE 9 /Par "\"$Project::Environment\"";PRD /Par "\"$Project::RegionCode\"";' + @RegionCode + ' /Par CompressFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\ZipFiles.bat\"" /Par DeleteFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\DeleteFiles.bat\"" /Par DeleteOldArchivedFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\DeleteOldArchiveFiles.bat\"" /Par ExtractSource;IRMA /Par FileArchivePath;"\"E:\AMZExtractData\Archive\"" /Par FileArchiveRetention;3 /Par FileRootPath;"\"E:\AMZExtractData\IRMA\ItemVendorLane\"" /Par esbCertificateName;"\"CN=CA-ROOT-I, O=Whole Foods Market\"" /Par esbJmsPassword;????? /Par esbJmsUsername;iconUser /Par esbMessageType;ByteMessage /Par esbSecurityCredentials;????? /Par esbSecurityPrincipal;jndiIconUser /Par esbServerUrl;"\"ssl://cerp1616.wfm.pvt:37293,ssl://cerp1617.wfm.pvt:37293,ssl://odrp1609.wfm.pvt:37293,ssl://odrp1610.wfm.pvt:37293\"" /Par esbSslPassword;????? /Par esbTargetHostName;"\"*.wfm.pvt\"" /Par esbTopicName;"\"WFMESB.SupplyChain.Retail.InventoryMgmt.Info.Topic.V2\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E'
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
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'SSIS', 
		@command=@Command, 
		@database_name=N'master', 
		@flags=0, 
		@proxy_name=@ProxyName
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
		@active_start_time=3000, 
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