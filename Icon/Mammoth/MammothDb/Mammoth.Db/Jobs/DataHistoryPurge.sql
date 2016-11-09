﻿USE [msdb]
GO

/****** Object:  Job [MAMMOTH . Data History Purge]    Script Date: 2/17/2016 4:12:23 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 2/17/2016 4:12:23 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'MAMMOTH . Data History Purge', 
              @enabled=1, 
              @notify_level_eventlog=0, 
              @notify_level_email=2, 
              @notify_level_netsend=0, 
              @notify_level_page=0, 
              @delete_level=0, 
              @description=N'No description available.', 
              @category_name=N'[Uncategorized (Local)]', 
              @owner_login_name=N'sa', 
              @notify_email_operator_name=N'SQL Server DBAs', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Purge Data]    Script Date: 2/17/2016 4:12:23 PM ******/
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
              @database_name=N'Mammoth', 
              @flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Log success]    Script Date: 2/17/2016 4:12:23 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Log success', 
              @step_id=2, 
              @cmdexec_success_code=0, 
              @on_success_action=1, 
              @on_success_step_id=0, 
              @on_fail_action=2, 
              @on_fail_step_id=0, 
              @retry_attempts=0, 
              @retry_interval=0, 
              @os_run_priority=0, @subsystem=N'TSQL', 
              @command=N'DECLARE @Now DATETIME = GETDATE();
DECLARE @Here SYSNAME = HOST_NAME();
DECLARE @User SYSNAME = (SELECT CURRENT_USER)
EXECUTE app.AddLogEvent 
  @AppName = ''Mammoth Data Purge''
, @UserName = @User
, @LogDate = @Now
, @Level = ''Info''
, @Source = ''Mammoth Data Purge''
, @Message = ''Mammoth Data Purge was successful.''
, @MachineName = @Here', 
              @database_name=N'Mammoth', 
              @flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Log failure]    Script Date: 2/17/2016 4:12:23 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Log failure', 
              @step_id=3, 
              @cmdexec_success_code=0, 
              @on_success_action=2, 
              @on_success_step_id=0, 
              @on_fail_action=2, 
              @on_fail_step_id=0, 
              @retry_attempts=0, 
              @retry_interval=0, 
              @os_run_priority=0, @subsystem=N'TSQL', 
              @command=N'DECLARE @Now DATETIME = GETDATE();
DECLARE @Here SYSNAME = HOST_NAME();
DECLARE @User SYSNAME = (SELECT CURRENT_USER)
EXECUTE app.AddLogEvent 
  @AppName = ''Mammoth Data Purge''
, @UserName = @User
, @LogDate = @Now
, @Level = ''Error''
, @Source = ''Mammoth Data Purge''
, @Message = ''Mammoth Data Purge was unsuccessful.  View job history for details.''
, @MachineName = @Here', 
              @database_name=N'Mammoth', 
              @flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'DAILY . MTWRFSN . 01X . 0000', 
              @enabled=1, 
              @freq_type=4, 
              @freq_interval=1, 
              @freq_subday_type=1, 
              @freq_subday_interval=0, 
              @freq_relative_interval=0, 
              @freq_recurrence_factor=0, 
              @active_start_date=20160209, 
              @active_end_date=99991231, 
              @active_start_time=0, 
              @active_end_time=235959, 
              @schedule_uid=N'a9e2f64d-0b45-4020-93b6-92a2dcdbfc14'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO
