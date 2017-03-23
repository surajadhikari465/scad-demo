IF EXISTS (
		SELECT job_id
		FROM msdb.dbo.sysjobs_view
		WHERE NAME = N'Brand Purge - Icon'
		)
	EXEC msdb.dbo.sp_delete_job @job_name = N'Brand Purge - Icon'
		,@delete_unused_schedule = 1