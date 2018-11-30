--Send Fail Email
EXEC msdb.dbo.sp_send_dbmail
@recipients=N'DBA.SQLServer.Alert@wholefoods.com;tom.lux@wholefoods.com',
@body= 'DB could not be restored.  Check job history for details.', 
@subject = 'Refresh icon-db01-dev.Icon failed',
@profile_name = 'SQLServerDBAs'
GO