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
IF EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'Map BusinessUnit to Region - Icon')
	EXEC msdb.dbo.sp_delete_job @job_name=N'Map BusinessUnit to Region - Icon', @delete_unused_schedule = 1

-----------------------------------------------------
-- NOTE: If you want to build this job for a Dev DB, specify the Dev DB name in this variable.
DECLARE @dbName sysname = 'Icon'
-----------------------------------------------------

DECLARE @jobName sysname = (@dbName + ' . Map BusinessUnit to Region')


-- Remove existing job.
IF EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = @jobName)
	EXEC msdb.dbo.sp_delete_job @job_name=@jobName, @delete_unused_schedule = 1

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=@jobName, 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Maintain the app.BusinessUnitRegionMapping table to keep it up-to-date', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'', @job_id = @jobId OUTPUT
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
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Update app.BusinessUnitRegionMapping script', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=3, 
		@on_fail_action=3, 
		@on_fail_step_id=3, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'DECLARE @BusinessUnitTable Table
(
idx smallint Primary Key IDENTITY(1,1),
businessUnit int not null,
regionCode varchar(4) null
)

insert @BusinessUnitTable(businessUnit, regionCode)
select Store.bu, case when ISNULL(Chain.ChainName,''365'') = ''365'' then ''RM'' else Region.regionCode end
from
(
	select l.localeID, l.localeName, l.localeTypeID, l.parentLocaleID, lt.traitValue as bu
	from Locale l
	join [dbo].[LocaleTrait] lt on lt.[localeID] = l.localeID
	join [dbo].[Trait] t on lt.traitID = t.traitID and t.traitDesc = ''PS Business Unit ID''
) Store 
inner join
(
	select l.localeID, l.parentLocaleID
	from Locale l
) Metro on Store.parentLocaleID = Metro.localeID
join
(  
	select l.localeID, l.parentLocaleID, lt.traitValue as regionCode
	from Locale l
	left join LocaleTrait lt on l.localeID = lt.localeID
	left join Trait t on lt.traitID = t.traitID and t.traitDesc = ''Region Abbreviation''
) Region on Metro.parentLocaleID = Region.localeID
join
(
	select l.localeID, l.localeTypeID, l.localeName as ChainName
	from Locale l
	where l.parentLocaleID is null
) Chain on Region.parentLocaleID = Chain.localeID


update app.BusinessUnitRegionMapping
set regionCode = but.regionCode, lastUpdateDate = GETDATE()
from app.BusinessUnitRegionMapping bum
join @BusinessUnitTable but on bum.businessUnit = but.businessUnit
where bum.regionCode <> but.regionCode

insert into app.BusinessUnitRegionMapping(businessUnit, regionCode)
select but.businessUnit, but.regionCode
from @BusinessUnitTable but
left join app.BusinessUnitRegionMapping bum on bum.businessUnit = but.businessUnit
where bum.businessUnit is null

delete app.BusinessUnitRegionMapping
from app.BusinessUnitRegionMapping bum
left join @BusinessUnitTable but on bum.businessUnit = but.businessUnit
where but.businessUnit is null

GO', 
		@database_name=@dbName, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Clean ItemMovementTransactionHistory Table', 
		@step_id=3,
		@cmdexec_success_code=0, 
		@on_success_action=3,
		@on_success_step_id=3,
		@on_fail_action=3,
		@on_fail_step_id=4,
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'Delete app.ItemMovementTransactionHistory
where InsertDate < dateadd(day, -3, getdate())
GO', 
		@database_name=@dbName, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Clean ItemMovement Table', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'DECLARE @cutoffdate as datetime
DECLARE @recPerTran as int = 50000
DECLARE @recDeleted as int = 50000

SET @cutoffdate = Convert(Date, getdate() - 3, 102)

WHILE (@recDeleted = @recPerTran)
BEGIN

delete TOP (@recPerTran)
	   [app].[ItemMovement]
where [ProcessFailedDate] < @cutoffdate

set @recDeleted = @@ROWCOUNT

END
GO', 
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
		@os_run_priority=0, @subsystem=N'TSQL', 
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
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'BusinessUnitRegionMapping Update Schedule', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20141112, 
		@active_end_date=99991231, 
		@active_start_time=235000, 
		@active_end_time=235959, 
		@schedule_uid=N'b878bb36-f549-4481-828f-49035273b325'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO