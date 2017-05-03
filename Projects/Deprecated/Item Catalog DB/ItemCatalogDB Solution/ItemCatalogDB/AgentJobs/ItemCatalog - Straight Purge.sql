USE [msdb]
GO

IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'ItemCatalog - Straight Purge')
EXEC msdb.dbo.sp_delete_job @job_name=N'ItemCatalog - Straight Purge', @delete_unused_schedule = 1
GO

DECLARE @Environment varchar(10)
DECLARE @DBNAME VARCHAR(200)
IF EXISTS (SELECT * FROM master.dbo.sysdatabases WHERE Name = 'ItemCatalog_TEST')
	BEGIN
		SET @DBNAME = 'ItemCatalog_TEST'
	END
ELSE
	BEGIN
		SET @DBNAME = 'ItemCatalog'
	END

/****** Object:  Job [ItemCatalog - Straight Purge]    Script Date: 3/23/2017 6:33:43 AM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 3/23/2017 6:33:43 AM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
	EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
	IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'ItemCatalog - Straight Purge', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Daily Straight Purge Job', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'IRMA Developers', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Check Maintenance Mode]    Script Date: 3/23/2017 6:33:43 AM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Check Maintenance Mode', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=3, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'IF (SELECT StatusFlagValue FROM dbo.DbStatus where FlagName = ''IsOfflineForMaintenance'') = 1
					RAISERROR(''Database is in maintenance mode.'', 16, 1)',
		@database_name=@DBNAME, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Purge Data]    Script Date: 3/23/2017 6:33:43 AM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Purge Data', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC [dbo].[PurgeStraight] @batchVolume = 50000', 
		@database_name=@DBNAME, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Report Maintenance Mode]    Script Date: 3/23/2017 6:33:43 AM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Report Maintenance Mode', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'IF EXISTS (
		SELECT *
		FROM dbo.DbStatus
		WHERE StatusFlagName = ''IsOfflineForMaintenance''
			AND StatusFlagValue = 1
		)
		BEGIN
			DECLARE @statusMsg NVARCHAR(256)
				,@appName NVARCHAR(128)
				,@dbState VARCHAR(128)

			SELECT @appName = ltrim(rtrim(program_name))
			FROM sys.sysprocesses
			WHERE spid = @@spid

			SELECT @dbState = ''DbName='' + NAME + '', IsReadOnly='' + cast(is_read_only AS VARCHAR) + '', UserAccess='' + user_access_desc + '', State='' + state_desc
			FROM sys.databases
			WHERE NAME LIKE db_name()

			SELECT @statusMsg = ''** DB Offline For Maintenance ** --> '' + ''AppName='' + @appName + '', '' + @dbstate
			
			RAISERROR (@statusMs, 16, 0)
		END', 
		@database_name=@DBNAME, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily - 24 Hours', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=4, 
		@freq_subday_interval=10, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20160919, 
		@active_end_date=99991231, 
		@active_start_time=0, 
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