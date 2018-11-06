USE [msdb]
GO

IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'Amazon Vendor Lane  Data Feed - IRMA Extract')
EXEC msdb.dbo.sp_delete_job @job_name=N'Amazon Vendor Lane Data Feed - IRMA Extract', @delete_unused_schedule = 1
GO

BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0

IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'Amazon Vendor Lane Data Feed - IRMA Extract', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Daily Amazon Vendor Lane IRMA Extract Job', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@job_id = @jobId OUTPUT
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
		@command=N'/ISSERVER "\"\SSISDB\AmzDataFeeds\AmzExtract-FromIrma\ItemVendorLaneExtract-FromIrma.dtsx\"" /SERVER "\"vim-vm-dev\sql2014\"" /ENVREFERENCE 8 /Par "\"$Project::Environment\"";DEV /Par "\"$Project::RegionCode\"";RM|PN /Par CompressFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\ZipFiles.bat\"" /Par DeleteFileExecutable;"\"E:\inetpub\ftproot\AMZExtract\DeleteFiles.bat\"" /Par ExtractSource;IRMA /Par FileRootPath;"\"E:\AMZExtractData\Test\IRMA\"" /Par ParentRootPath;"\"E:\AMZExtractData\Test\"" /Par esbCertificateName;"\"CN=CA-ROOT-I, O=Whole Foods Market\"" /Par esbJmsPassword;Pjetuc9M7Kmi /Par esbJmsUsername;iconUser /Par esbQueueName;"\"WFMESB.SupplyChain.Retail.InventoryMgmt.Info.Queue.V2\"" /Par esbSecurityCredentials;jndiIconUser /Par esbSecurityPrincipal;jndiIconUser /Par esbServerUrl;"\"ssl://cerd1617.wfm.pvt:17293\"" /Par esbSslPassword;esb /Par esbTargetHostName;"\"cerd1617.wfm.pvt\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E', 
		@database_name=master, 
		@flags=0, 
		@proxy_name=N'PDXExtractDEV'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily - 24 Hours', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20170801, 
		@active_end_date=99991231, 
		@active_start_time=12000, 
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