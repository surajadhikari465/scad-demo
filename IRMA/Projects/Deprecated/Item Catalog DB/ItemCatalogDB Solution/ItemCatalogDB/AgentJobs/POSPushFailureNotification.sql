USE [msdb]
GO

/****** Object:  Job [POS Push Failure Notification]    Script Date: 11/5/2015 1:43:46 PM ******/
DECLARE @DBNAME VARCHAR(200)

IF EXISTS (SELECT * FROM master.dbo.sysdatabases WHERE Name = 'ItemCatalog_Test')
	BEGIN
		SET @DBNAME = 'ItemCatalog_Test'
	END
ELSE
	BEGIN
		SET @DBNAME = 'ItemCatalog'
	END


BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]]    Script Date: 11/5/2015 1:43:46 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'POS Push Failure Notification', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'IRMA Developers', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Push Monitoring]    Script Date: 11/5/2015 1:43:46 PM ******/
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
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Push Monitoring', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC dbo.SendPOSPushFailureNotification', 
		@database_name=@DBNAME, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
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
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'POS Push Monitoring', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=8, 
		@freq_subday_interval=1, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20151105, 
		@active_end_date=99991231, 
		@active_start_time=10000, 
		@active_end_time=55959, 
		@schedule_uid=N'798eb7fd-d454-4193-8244-0d4c330b472a'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO