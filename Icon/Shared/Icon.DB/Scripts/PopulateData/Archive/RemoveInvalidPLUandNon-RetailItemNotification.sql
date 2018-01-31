IF EXISTS (SELECT job_id 
            FROM msdb.dbo.sysjobs_view 
            WHERE name = N'Invalid PLU and Non-Retail Item Notification - Icon')
EXEC msdb.dbo.sp_delete_job @job_name=N'Invalid PLU and Non-Retail Item Notification - Icon'
                            , @delete_unused_schedule=1

IF EXISTS (SELECT job_id 
            FROM msdb.dbo.sysjobs_view 
            WHERE name = N'Invalid PLU and Non-Retail Item Notification - IconDev')
EXEC msdb.dbo.sp_delete_job @job_name=N'Invalid PLU and Non-Retail Item Notification - IconDev'
                            , @delete_unused_schedule=1