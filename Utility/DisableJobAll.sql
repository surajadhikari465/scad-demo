USE msdb
GO

-- Disable the jobs
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view
	WHERE name = N'DC Average Cost Import') 
EXEC dbo.sp_update_job @job_name = N'DC Average Cost Import', @enabled = 0
GO
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view
	WHERE name = N'P2P - Apply New Vendor Cost to POs') 
EXEC dbo.sp_update_job @job_name = N'P2P - Apply New Vendor Cost to POs', @enabled = 0
GO
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view
	WHERE name = N'P2P - GetVendorItemDiscrepancyCounts') 
EXEC dbo.sp_update_job @job_name = N'P2P - GetVendorItemDiscrepancyCounts', @enabled = 0
GO
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view
	WHERE name = N'Inventory - Close EOP CycleCounts') 
EXEC dbo.sp_update_job @job_name = N'Inventory - Close EOP CycleCounts', @enabled = 0
GO
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view
	WHERE name = N'Inventory - Update ItemHistory from CycleCounts') 
EXEC dbo.sp_update_job @job_name = N'Inventory - Update ItemHistory from CycleCounts', @enabled = 0
GO
IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'IRMA - Email Suspended PO List')
EXEC msdb.dbo.sp_delete_job @job_name=N'IRMA - Email Suspended PO List', @delete_unused_schedule=0
GO

USE ItemCatalog

GO
