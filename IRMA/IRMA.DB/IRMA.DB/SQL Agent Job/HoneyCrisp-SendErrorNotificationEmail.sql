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

DECLARE @JobName as VARCHAR(100), @Command as VARCHAR(MAX), @ProxyName as VARCHAR(15)

IF RIGHT(@@SERVERNAME, 7) = 'SQL2014'
	BEGIN
		SET @JobName = N'Amazon Real-time Messages - Send Error Notification Email - QA'
		SET @ProxyName = N'PDXExtractQA'
		SET @Command = N'/ISSERVER "\"\SSISDB\HoneyCrispSendErrorNotificationEmail\SendErrorMessageNotification\SendErrorMessageNotification.dtsx\"" /SERVER "\"' + @@SERVERNAME + '\"" /Par "\"$Project::Environment\"";QA /Par "\"$Project::RegionCode\"";SO|PN /Par "\"$Project::RunAsOfDate(DateTime)\"";"\"1/1/2016 12:00:00 AM\"" /Par "\"CheckMessagesBeforeThisNumberOfMinutes(Int32)\"";60 /Par Recipients;"\"IRMA.applications@wholefoods.com\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E'
	END
ELSE
	BEGIN
		SET @JobName = N'Amazon Real-time Messages - Send Error Notification Email'
		SET @ProxyName = N'PDXExtractPRD'
		SET @Command = N'/ISSERVER "\"\SSISDB\HoneyCrispSendErrorNotificationEmail\SendErrorMessageNotification\SendErrorMessageNotification.dtsx\"" /SERVER "\"' + @@SERVERNAME + '\"" /Par "\"$Project::Environment\"";PRD /Par "\"$Project::RegionCode\"";PN /Par "\"$Project::RunAsOfDate(DateTime)\"";"\"1/1/2016 12:00:00 AM\"" /Par "\"CheckMessagesBeforeThisNumberOfMinutes(Int32)\"";60 /Par Recipients;"\"IRMA.support@wholefoods.com\"" /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E'
	END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=@JobName, 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Run SSIS]    Script Date: 12/24/2018 1:10:15 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Run SSIS', 
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
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'HC Daily Error Email', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20181224, 
		@active_end_date=99991231, 
		@active_start_time=60000, 
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