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
SET @RegionCode = 'FL'

IF RIGHT(@@SERVERNAME, 7) = 'SQL2014'
	BEGIN
		SET @JobName = N'Infor Data Feeds - Future Cost (' + @RegionCode + ') - (QA)'
		SET @ProxyName = N'PDXExtractQA'
		SET @Command = N'/ISSERVER "\"\SSISDB\InforDataFeeds\InforFutureCostExtract\InforFutureCostExtract.dtsx\"" /SERVER "\"' + @@SERVERNAME + '\"" /Par "\"$Project::Environment\"";QA /Par "\"$Project::RegionCode\"";' + @RegionCode + ' /Par "\"DeltaLoad(Boolean)\"";True /Par CompressFileExecutable;"\"E:\inetpub\ftproot\PDXExtract\QA\ZipFiles.bat\"" /Par DeleteFileExecutable;"\"E:\inetpub\ftproot\PDXExtract\QA\DeleteFiles.bat\"" /Par FileRootPath;"\"E:\PDXExtractData\QA\FutureCosts\"" /Par sFtpArgument;"\"\"\"open sftp://wfm:pirEtQuoor@data.dev.predictix.com -hostkey=\"\"\"\"ssh-rsa 2048 03:e9:1b:24:25:04:58:60:d4:66:b0:af:75:de:ad:9f\"\"\"\"\"\" \"\"cd /to_predictix/staging_APT/\"\"\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E'
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
		SET @JobName = N'Infor Data Feeds - Future Cost (' + @RegionCode + ')'
		SET @ProxyName = N'PDXExtractPRD'
		SET @Command = N'/ISSERVER "\"\SSISDB\InforDataFeeds\InforFutureCostExtract\InforFutureCostExtract.dtsx\"" /SERVER "\"' + @@SERVERNAME + '\"" /Par "\"$Project::Environment\"";PRD /Par "\"$Project::RegionCode\"";' + @RegionCode + ' /Par "\"DeltaLoad(Boolean)\"";True /Par CompressFileExecutable;"\"E:\inetpub\ftproot\PDXExtract\ZipFiles.bat\"" /Par DeleteFileExecutable;"\"E:\inetpub\ftproot\PDXExtract\DeleteFiles.bat\"" /Par FileRootPath;"\"E:\PDXExtractData\FutureCosts\"" /Par sFtpArgument;"\"\"\"open sftp://wfm-prod:THzhhVo61K@data.predictix.com -hostkey=\"\"\"\"ssh-rsa 2048 f4:ad:d3:36:d7:cb:88:2a:ed:8e:50:4a:29:af:b7:41\"\"\"\"\"\" \"\"cd /to_predictix/staging_APT/\"\"\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E'
		SET @CommandAlert=N'EXEC msdb.dbo.sp_send_dbmail
					@profile_name = ''SQLServerDbas'', 
					@recipients = ''irma.support@wholefoods.opsgenie.net'',
					@copy_recipients = ''scm-l3@wholefoods.com'', 
					@body =''This Job has failed to retry this step please execute the following T-SQL on VIM-VM-PRD\VIM2014:
					exec msdb.dbo.startjob @jobname = ''''' + @JobName  + '''''''' + ',   
					@subject = ''' + @JobName + ' has failed'''
	END

SELECT @ScheduleStartTime = CASE @RegionCode
	WHEN 'FL' THEN 	12000
	WHEN 'MA' THEN  12500
    WHEN 'MW' THEN  13000
    WHEN 'NA' THEN  13500
    WHEN 'NC' THEN  14000
    WHEN 'NE' THEN  14500
    WHEN 'PN' THEN  15000
    WHEN 'RM' THEN  15500
    WHEN 'SO' THEN  20000
    WHEN 'SP' THEN  20500
    WHEN 'SW' THEN  21000
	WHEN 'UK' THEN  21000
	END

IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = @JobName)
EXEC msdb.dbo.sp_delete_job @job_name=@JobName, @delete_unused_schedule = 1

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=@JobName, 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Daily Infor Future Costs Extract From IRMA Job', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@job_id = @jobId OUTPUT
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
/****** Object:  Step [Add Failure Alert]    Script Date: 10/30/2019 12:46:17 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Add Failure Alert', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=2, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=@CommandAlert, 
		@database_name=N'msdb', 
		@flags=0
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