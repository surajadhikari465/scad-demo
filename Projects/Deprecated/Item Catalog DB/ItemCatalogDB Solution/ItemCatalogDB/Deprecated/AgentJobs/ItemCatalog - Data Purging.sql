USE [msdb]
GO

IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'ItemCatalog - Data Purging')
EXEC msdb.dbo.sp_delete_job @job_name=N'ItemCatalog - Data Purging', @delete_unused_schedule = 1
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

BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0

IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'ItemCatalog - Data Purging', 
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
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Purge ItemBrand Records', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'Declare  @CodeLocation varchar(128)
		,@RowsAffected int
		,@DBEnv varchar(8)
		,@EnvId uniqueidentifier
        ,@LogSystemName varchar(64)
        ,@LogAppName varchar(64)
        ,@LogAppID uniqueidentifier
        ,@LogLevel varchar(8)
        ,@LogThread varchar(8)
        ,@LogMsg varchar(256)
        ,@LogExceptionMsg varchar(2000)
        ,@UserID int
        ,@now datetime
        
select
		 @LogSystemName = ''IRMA CLIENT''
		,@LogAppName = ''ItemBrand Purge''
		,@LogLevel = ''INFO''
		,@LogThread = ''0''
		,@LogExceptionMsg = ''''
		
		-- Determine DB environment (from version table) so we can get appropriate app ID from app-config.  (Env short names from AppConfigEnv: ''''TST'''', ''''QA'''', ''''PRD''''.)
		select @DBEnv = case
			when Environment like ''%q%'' then ''QA''
			when Environment like ''%pr%'' then ''PRD''
			else ''TST''
			end
		from version
		
		select @UserID = User_ID 
		  from Users
		 where UserName = ''system''
		
		-- Get IRMA Client app GUID for logging calls (AppLogInsertEntry).
		select @LogAppID = a.ApplicationID,
			   @EnvId    = a.EnvironmentID
		from AppConfigApp a
		join AppConfigEnv e on a.EnvironmentID = e.EnvironmentID
		where e.ShortName = @DBEnv and a.Name = @LogSystemName

		--DEBUG: select DBEnv = @DBEnv, LogAppID = @LogAppID
		
		SELECT @CodeLocation = ''ItemBrand Purging Job Starts...''
		select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;
		
CREATE TABLE #BrandsToBeDeleted(Brand_ID int)
CREATE INDEX BrandID on #BrandsToBeDeleted(Brand_ID)

SELECT @CodeLocation = ''Temp table #BrandsToBeDeleted created ..''
select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;
			
INSERT INTO #BrandsToBeDeleted(Brand_ID)
select Brand_ID from ItemBrand 
where not exists
(
select * from Item
where Item.Brand_ID = ItemBrand.Brand_ID
and Deleted_Item = 0
)
and not exists
(
select * from ItemOverride
inner join Item on Item.Item_Key = ItemOverride.Item_Key
where ItemOverride.Brand_ID = ItemBrand.Brand_ID
and Deleted_Item = 0
)

select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + '' - Finish loading the table. Rows Affected: '' + cast(@@ROWCOUNT as varchar);
exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

SELECT @CodeLocation = ''DELETE ValidatedBrand...''
select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;				

Delete ValidatedBrand
  From ValidatedBrand vb
INNER JOIN #BrandsToBeDeleted bd WITH(INDEX(BrandID)) ON vb.IrmaBrandId = bd.Brand_ID

SELECT @CodeLocation = ''DELETE ItemBrand...''
select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;				
			
Delete ItemBrand
  From ItemBrand ib
INNER JOIN #BrandsToBeDeleted bd WITH(INDEX(BrandID)) ON ib.Brand_ID = bd.Brand_ID

select @RowsAffected = @@ROWCOUNT, @now = getdate(), @LogMsg = @CodeLocation + '' Rows Affected: '' + cast(@@ROWCOUNT as varchar);
exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @LogMsg, @LogExceptionMsg;

DROP TABLE #BrandsToBeDeleted

SELECT @CodeLocation = ''ItemBrand Purging Job Ends...''
select @now = getdate(); exec dbo.AppLogInsertEntry @now, @LogAppID, @LogThread, @LogLevel, @LogAppName, @CodeLocation, @LogExceptionMsg;', 
		@database_name=@DBNAME, 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Quarterly Data Purge Schedule', 
		@enabled=1, 
		@freq_type=32, 
		@freq_interval=6, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=1, 
		@freq_recurrence_factor=3, 
		@active_start_date=20140307, 
		@active_end_date=99991231, 
		@active_start_time=234500, 
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


