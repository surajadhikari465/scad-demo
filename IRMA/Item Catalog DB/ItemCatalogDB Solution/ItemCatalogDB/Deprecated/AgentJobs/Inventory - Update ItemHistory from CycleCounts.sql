 USE [msdb]
GO

/****** Object:  Job [Inventory - Update ItemHistory from CycleCounts]    Script Date: 06/18/2009 14:21:06 ******/
IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'Inventory - Update ItemHistory from CycleCounts')
EXEC msdb.dbo.sp_delete_job @job_name=N'Inventory - Update ItemHistory from CycleCounts', @delete_unused_schedule=1
GO



DECLARE @Environment varchar(10)
DECLARE @DBNAME VARCHAR(200)
IF EXISTS (SELECT * FROM master.dbo.sysdatabases WHERE Name = 'ItemCatalog_TEST')
	BEGIN
		SELECT @Environment = Environment FROM ItemCatalog_TEST.dbo.Version
		SET @DBNAME = 'ItemCatalog_TEST'
	END
ELSE
	BEGIN
		SELECT @Environment = Environment FROM ItemCatalog.dbo.Version
		SET @DBNAME = 'ItemCatalog'
	END
IF @Environment = 'TEST'
	SET @Environment = 'TST'
ELSE IF @Environment = 'PROD'
	SET @Environment = 'PRD'


/****** Object:  Job [INVENTORY - Update ItemHistory from Cycle Count]    Script Date: 08/25/2010 10:25:58 ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [Inventory]    Script Date: 08/25/2010 10:25:58 ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'Inventory' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'Inventory'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'Inventory - Update ItemHistory from CycleCounts', 
		@enabled=0, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'Inventory', 
		@owner_login_name=N'sa', 
		@job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Update Item History from Cycle Count]    Script Date: 08/25/2010 10:25:59 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Update Item History from Cycle Count', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'InsertItemHistoryCycleCountCursor', 
		@database_name=@DBNAME, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Item History Update - Cycle Count', 
		@enabled=0, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20060911, 
		@active_end_date=99991231, 
		@active_start_time=234500, 
		@active_end_time=235959, 
		@schedule_uid=N'07c32160-05ff-42ce-a20a-097302afc209'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO


