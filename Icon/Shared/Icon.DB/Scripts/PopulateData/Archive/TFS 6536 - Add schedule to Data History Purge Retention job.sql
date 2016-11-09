USE msdb

GO

DECLARE @jobId uniqueidentifier = (select job_id from dbo.sysjobs_view where name = 'Data History Purge - Icon')

EXEC msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Nightly Data History Purge', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20150302, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959, 
		@schedule_uid=N'359b97c6-ac9c-4644-9238-3eb1ca43f04f'