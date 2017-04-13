USE [msdb]
GO

/****** Object:  Job [MAMMOTH . Delete Expired Prices and Sales]    Script Date: 4/13/2017 11:01:15 AM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 4/13/2017 11:01:15 AM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
	EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
	IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'MAMMOTH . Delete Expired Prices and Sales', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Deletes prices from all regional data tables where the prices have expired', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Check Maintenance Mode]    Script Date: 4/13/2017 11:01:16 AM *****/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Check Maintenance Mode', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=4, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'IF (SELECT StatusFlagValue FROM app.DbStatus where FlagName = ''IsOfflineForMaintenance'') = 1
					RAISERROR(''Database is in maintenance mode.'', 16, 1)', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Delete prices and sales]    Script Date: 4/13/2017 11:01:16 AM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Delete prices and sales', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=3, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC dbo.DeleteExpiredPricesAndSales @MaxDeleteCount = 100000, @BatchSize = 20000', 
		@database_name=N'Mammoth', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [NotifyOnFailure]    Script Date: 4/13/2017 11:01:16 AM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'NotifyOnFailure', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=2, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC msdb.dbo.sp_send_dbmail
@recipients=N''DBA.SQLServer.Alert@wholefoods.com;IRMA.developers@wholefoods.com'',
@body= ''MAMMOTH . Delete Expired Prices and Sales has failed. Please check job history for details'', 
@subject = ''MAMMOTH . Delete Expired Prices and Sales - Failed'',
@profile_name = ''SQLServerDBAs''', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Report Maintenance Mode]    Script Date: 4/13/2017 11:01:16 AM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Report Maintenance Mode', 
		@step_id=4, 
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
			FROM app.DbStatus
			WHERE StatusFlagName = ''IsOfflineForMaintenance'' AND StatusFlagValue = 1
		)
		BEGIN
			DECLARE @statusMsg nvarchar(256), @appName nvarchar(128), @dbState varchar(128)

			SELECT @appName = ltrim(rtrim(program_name))
			FROM sys.sysprocesses
			WHERE spid = @@spid

			SELECT @dbState = N''DbName='' + [name] + '', IsReadOnly='' + convert(nvarchar,is_read_only) 
				+ N'', UserAccess='' + convert(nvarchar,user_access_desc collate SQL_Latin1_General_CP1_CI_AS )
				+ N'', State='' + state_desc
			FROM sys.databases
			WHERE NAME LIKE db_name()

			SELECT @statusMsg = ''** DB Offline For Maintenance ** --> '' 
				+ ''AppName='' + @appName + '', '' + @dbstate

			RAISERROR (@statusMsg, 16, 0)
		END',
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Nightly except Sundays from 1 to 4 AM', 
		@enabled=1, 
		@freq_type=8, 
		@freq_interval=126, 
		@freq_subday_type=8, 
		@freq_subday_interval=1, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=1, 
		@active_start_date=20170413, 
		@active_end_date=99991231, 
		@active_start_time=10000, 
		@active_end_time=40000
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO