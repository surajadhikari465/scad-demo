USE [msdb]
GO
/****** Object:  Job [Close EOP CycleCounts]    Script Date: 06/18/2009 14:21:06 ******/
IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'Inventory - Close EOP CycleCounts')
EXEC msdb.dbo.sp_delete_job @job_name=N'Inventory - Close EOP CycleCounts', @delete_unused_schedule=1
GO


DECLARE @Environment varchar(10)
DECLARE @DBNAME VARCHAR(200)
IF EXISTS (SELECT * FROM master.dbo.sysdatabases WHERE Name = 'ItemCatalog_Test')
	BEGIN
		SELECT @Environment = Environment FROM ItemCatalog_Test.dbo.Version
		SET @DBNAME = 'ItemCatalog_Test'
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
 
 
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [Inventory]    Script Date: 08/25/2010 10:25:51 ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'Inventory' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'Inventory'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'Inventory - Close EOP CycleCounts', 
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
/****** Object:  Step [Close EOP Cycle Counts]    Script Date: 08/25/2010 10:25:53 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Close EOP Cycle Counts', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- This should run on the first Thursday of each FP.

DECLARE 
	@CurrentPeriodBeginDate smalldatetime, 
	@LastPeriodEndDate smalldatetime, 
	@RegionCode varchar(2)

SET @CurrentPeriodBeginDate = dbo.fn_PeriodBeginDate(GETDATE())
SET @LastPeriodEndDate = DATEADD(day, -1, @CurrentPeriodBeginDate)
SET @RegionCode = (SELECT RegionCode FROM Region)

DECLARE @ErrorNumber int = 0

BEGIN TRAN

	DECLARE @CountDates TABLE (StoreNumber int, Subteam int, DateKey smalldatetime)

	INSERT INTO @CountDates (StoreNumber, Subteam, DateKey) SELECT DISTINCT
		s.Store_No,
		CASE 
			WHEN @RegionCode = ''SO'' THEN cds.SubTeamSID
			ELSE ISNULL(sst.Subteam_No, cds.SubTeamSID) 
		END,
		DATEADD(minute, 59, DATEADD(hour, 23, cds.Date_Key))
		
	FROM 
		CountDateSchedule			(nolock) cds
		INNER JOIN	Date			(nolock) d		ON	d.Year				= cds.FiscalYear
													AND d.Period			= cds.FiscalPeriod
		INNER JOIN	Store			(nolock) s		ON	cds.BusinessUnitID	= s.BusinessUnit_ID
		LEFT JOIN	StoreSubteam	(nolock) sst	ON	sst.Store_No		= s.Store_No 
													AND sst.PS_SubTeam_No	= cds.SubTeamSID
	WHERE 
		d.Date_Key = @LastPeriodEndDate

		

	-- Close EOP counts for last period that are still open.
	UPDATE CycleCountHeader SET 
		ClosedDate = GETDATE()

	FROM 
		CycleCountHeader				cch
		INNER JOIN CycleCountMaster		ccm	ON	cch.MasterCountID	= ccm.MasterCountID
		INNER JOIN @CountDates			cd	ON	ccm.EndScan			= cd.DateKey 
											AND ccm.SubTeam_No		= cd.Subteam 
											AND ccm.Store_No		= cd.StoreNumber

	WHERE 
		ccm.EndOfPeriod = 1 
		AND cch.ClosedDate IS NULL

	SELECT @ErrorNumber = @@ERROR

	IF @ErrorNumber = 0
		BEGIN

			UPDATE CycleCountMaster SET 
				ClosedDate = GETDATE(), 
				UpdateIH = 1, 
				SetNonCountedToZero = 1
	
			FROM 
				CycleCountMaster		ccm
				INNER JOIN @CountDates	cd	ON	ccm.EndScan		= cd.DateKey 
											AND ccm.SubTeam_No	= cd.Subteam 
											AND ccm.Store_No	= cd.StoreNumber

			WHERE 
				ccm.EndOfPeriod = 1 
				AND ccm.ClosedDate IS NULL

			SELECT @ErrorNumber = @@ERROR
		END

IF @ErrorNumber = 0
	COMMIT TRAN
ELSE
	ROLLBACK TRAN', 
		@database_name= @DBNAME, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Close EOP Cycle Counts', 
		@enabled=0, 
		@freq_type=8, 
		@freq_interval=16, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=4, 
		@active_start_date=20071011, 
		@active_end_date=99991231, 
		@active_start_time=100, 
		@active_end_time=235959, 
		@schedule_uid=N'b26f15d0-ad4f-4f13-878b-2b9f434f9e3c'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO