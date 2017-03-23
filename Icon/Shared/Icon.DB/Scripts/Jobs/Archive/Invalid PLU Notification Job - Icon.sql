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

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'Invalid PLU and Non-Retail Item Notification - Icon', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'This job notifies the Global Data Team of invalid PLUs or non-retail items added to Icon from IRMA.  An invalid PLU or non-retail item is one which does not currently exist in Icon as New, Loaded, or Validated.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'WFM\IconInterfaceUserTes', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Notify GDT of Invalid PLUs and Invalid Non-Retail Items', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'DECLARE @InvalidIdentifiers table
(
	Identifier nvarchar(13),
	RegionCode nvarchar(3),
	InsertDate datetime2(7)
)

INSERT INTO @InvalidIdentifiers
SELECT	iis.identifier,
		iis.regioncode,
		iis.insertDate
FROM app.IRMAItemSubscription iis
WHERE	iis.identifier not in (select identifier from app.IRMAItem)
		AND iis.identifier not in (select scanCode from ScanCode)
		AND iis.insertDate > DATEADD(d, -1, GETDATE())

DECLARE @NumberOfInvalidIdentifiers int;
SET @NumberOfInvalidIdentifiers = (SELECT COUNT(*) FROM @InvalidIdentifiers)

IF(@NumberOfInvalidIdentifiers > 0)
BEGIN
	DECLARE @TableData NVARCHAR(MAX)
	DECLARE @Body NVARCHAR(MAX)
	DECLARE @Whitespace NVARCHAR(16)

	SET @TableData = CAST((SELECT td = Identifier, '''',
								  td = RegionCode, '''',
								  td = CONVERT(VARCHAR(24), InsertDate, 0), ''''
						   FROM @InvalidIdentifiers ORDER BY Identifier
						   FOR XML PATH(''tr''), TYPE) 
						   AS NVARCHAR(MAX))

	SET @Body = ''<html>'' +
					''<body>'' +
						''<p>The following PLUs or non-retail items were entered in IRMA but are not in Icon.</p>'' +
						''<table border = 1>'' +
							''<tr>'' +
								''<th>PLU</th>'' +
								''<th>REGION</th>'' +
								''<th>TIME</th>'' +
							''</tr>'' +
							@TableData +
						''</table>'' +
					''</body>'' +
				''</html>''

	EXEC msdb.dbo.sp_send_dbmail
		@profile_name = ''IRMA Support'',
		@recipients=N''wfmglobaldataintegrity@wholefoods.com'',
		@subject = ''Invalid PLUs or Non-Retail Items Entered in IRMA'',
		@body = @Body,
		@body_format = ''HTML''
END', 
		@database_name=N'iCON', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20141017, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959, 
		@schedule_uid=N'58b13fe7-b337-4340-8ec0-5b7d5d284b69'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO