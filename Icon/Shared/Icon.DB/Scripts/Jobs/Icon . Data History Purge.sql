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


/*
Clean up old job name.
*/
IF EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'Data History Purge - Icon')
	EXEC msdb.dbo.sp_delete_job @job_name=N'Data History Purge - Icon', @delete_unused_schedule = 1


-----------------------------------------------------
-- NOTE: If you want to build this job for a Dev DB, specify the Dev DB name in this variable.
DECLARE @dbName sysname = 'Icon'
-----------------------------------------------------

DECLARE @jobName sysname = (@dbName + ' . Data History Purge')


-- Remove existing job.
IF EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = @jobName)
	EXEC msdb.dbo.sp_delete_job @job_name=@jobName, @delete_unused_schedule = 1

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=@jobName, 
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
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Check Maintenance Mode', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=5, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'IF (SELECT StatusFlagValue FROM app.DbStatus where StatusFlagName = ''IsOfflineForMaintenance'') = 1
		RAISERROR(''Database is in maintenance mode.  See Report step for other details.'', 16, 1)', 
		@database_name=@dbName, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Purge Data', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=4, 
		@on_success_step_id=3, 
		@on_fail_action=4, 
		@on_fail_step_id=4, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXECUTE app.PurgeData', 
		@database_name=@dbName, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Log Success', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'DECLARE @AppId int,
		@UserName nvarchar(10),
		@LogDate datetime2(3),
		@Level nvarchar(16),
		@Source nvarchar(255),
		@Message nvarchar(60);

SET @AppId = (select AppID from app.App where AppName = ''Icon Data Purge'');
SET	@UserName = ''IconPurgeUser'';
SET	@LogDate = GETDATE();
SET	@Level = ''Info'';
SET	@Source = ''Icon Data Purge'';
SET	@Message = ''Icon Data Purge was successful.'';

INSERT INTO app.AppLog(AppId, UserName, LogDate, Level, Logger, Message)
VALUES            		          (@AppId, @UserName, @LogDate, @Level, @Source, @Message)', 
		@database_name=@dbName, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Log Failure', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=2, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'DECLARE @AppId int,
		@UserName nvarchar(10),
		@LogDate datetime2(3),
		@Level nvarchar(16),
		@Source nvarchar(255),
		@Message nvarchar(70);

SET @AppId = (select AppID from app.App where AppName = ''Icon Data Purge'');
SET	@UserName = ''IconPurgeUser'';
SET	@LogDate = GETDATE();
SET	@Level = ''Error'';
SET	@Source = ''Icon Data Purge'';
SET	@Message = ''Icon Data Purge was unsuccessful. View job history for details.'';

INSERT INTO app.AppLog(AppId, UserName, LogDate, Level, Logger, Message)
VALUES            		          (@AppId, @UserName, @LogDate, @Level, @Source, @Message)', 
		@database_name=@dbName, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Report Maintenance Mode', 
		@step_id=5, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0,
		@subsystem=N'TSQL', 
		@command=N'declare @statusMsg nvarchar(256), @appName nvarchar(128), @dbState varchar(128)
select @appName = ltrim(rtrim(program_name)) from sys.sysprocesses where spid = @@spid
select @dbState = ''DbName='' + name + '', IsReadOnly='' + cast(is_read_only as varchar) + '', UserAccess='' + user_access_desc + '', State='' + state_desc from sys.databases where name like db_name()
select @statusMsg = ''** DB Offline For Maintenance ** --> '' + ''LogDate='' + convert(nvarchar, getdate(), 121) + '', AppName='' + @appName + '', '' + @dbstate
print @statusMsg;', 
		@database_name=@dbName, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Nightly Data History Purge', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20150302, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO