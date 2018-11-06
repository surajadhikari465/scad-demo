USE msdb
GO

-- Enable the jobs
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view
	WHERE name = N'DC Average Cost Import') 
EXEC dbo.sp_update_job @job_name = N'DC Average Cost Import', @enabled = 1
GO
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view
	WHERE name = N'Inventory - Close EOP CycleCounts') 
EXEC dbo.sp_update_job @job_name = N'Inventory - Close EOP CycleCounts', @enabled = 1
GO
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view
	WHERE name = N'Inventory - Update ItemHistory from CycleCounts') 
EXEC dbo.sp_update_job @job_name = N'Inventory - Update ItemHistory from CycleCounts', @enabled = 1
GO
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view
	WHERE name = N'Inventory - Close EOP CycleCounts') 
EXEC dbo.sp_update_job @job_name = N'Inventory - Close EOP CycleCounts', @enabled = 1
GO
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view
	WHERE name = N'Inventory - Update ItemHistory from CycleCounts') 
EXEC dbo.sp_update_job @job_name = N'Inventory - Update ItemHistory from CycleCounts', @enabled = 1
GO

IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'IRMA - Email Suspended PO List')
EXEC msdb.dbo.sp_delete_job @job_name=N'IRMA - Email Suspended PO List', @delete_unused_schedule=1

GO





