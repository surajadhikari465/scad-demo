USE [msdb]
GO

/****** Object:  Job [Data History Purge - Icon]    Script Date: 10/15/2014 2:43:44 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]]    Script Date: 10/15/2014 2:43:44 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'Data History Purge - Icon', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'WFM\IconInterfaceUserTes', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Purge Data]    Script Date: 10/15/2014 2:43:44 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Purge Data', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=4, 
		@on_success_step_id=2, 
		@on_fail_action=4, 
		@on_fail_step_id=3, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXECUTE app.PurgeData', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Log Success]    Script Date: 10/15/2014 2:43:44 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Log Success', 
		@step_id=2, 
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
SET	@UserName = ''WFM\IconInterfaceUserTes'';
SET	@LogDate = GETDATE();
SET	@Level = ''Info'';
SET	@Source = ''Icon Data Purge'';
SET	@Message = ''Icon Data Purge was successful.'';

INSERT INTO app.AppLog(AppId, UserName, LogDate, Level, Logger, Message)
VALUES            		          (@AppId, @UserName, @LogDate, @Level, @Source, @Message)', 
		@database_name=N'iCON', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Log Failure]    Script Date: 10/15/2014 2:43:44 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Log Failure', 
		@step_id=3, 
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
SET	@UserName = ''WFM\IconInterfaceUserTes'';
SET	@LogDate = GETDATE();
SET	@Level = ''Error'';
SET	@Source = ''Icon Data Purge'';
SET	@Message = ''Icon Data Purge was unsuccessful. View job history for details.'';

INSERT INTO app.AppLog(AppId, UserName, LogDate, Level, Logger, Message)
VALUES            		          (@AppId, @UserName, @LogDate, @Level, @Source, @Message)', 
		@database_name=N'iCON', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO