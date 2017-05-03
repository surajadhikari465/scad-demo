USE [msdb]
GO

-- PO Reports
-- account
execute msdb.dbo.sysmail_add_account_sp
@account_name = 'PO Report',
@description = 'Email PO Report App',
@display_name = 'PO Report',
@email_address = 'spo_reports_qa@wholefoods.com',
@mailserver_name = 'smtp.wholefoods.com'
go

-- profile
execute msdb.dbo.sysmail_add_profile_sp
@description = 'PO Report Profile',
@profile_name = 'PO Report' 
go

-- link the two
execute msdb.dbo.sysmail_add_profileaccount_sp
@account_name = 'PO Report',
@profile_name = 'PO Report',
@sequence_number = 1 ;
Go
