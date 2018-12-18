--Send Success Email
EXEC msdb.dbo.sp_send_dbmail
@recipients=N'DBA.SQLServer.Alert@wholefoods.com;IRMA.developers@wholefoods.com',
@body= 'icon-db01-dev.Icon Restore successful', 
@subject = 'icon-db01-dev.Icon successfully completed',
@profile_name = 'SQLServerDBAs'

GO