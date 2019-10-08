BEGIN TRANSACTION
DECLARE @ReturnCode INT=0

DECLARE @dbName sysname = 'Mammoth'
DECLARE @jobName sysname = (@dbName + '.CopyGpmDataForAudit')

-- Remove existing job.
IF EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = @jobName)
	EXEC @ReturnCode = msdb.dbo.sp_delete_job @job_name=@jobName, @delete_unused_schedule=1

IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
GOTO EndSave

QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION

EndSave:
    COMMIT TRANSACTION

GO