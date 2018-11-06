-- Script to set up the WFM\PDXExtractUserQA account on VIM-VM_DEV to run PDX Extract SSIS packages
-- DBA needs to enter the password for the WFM\PDXExtractUserQA account
USE [master]
GO
CREATE CREDENTIAL [PDXExtractQA] WITH IDENTITY = N'WFM\PDXExtractUserQA', SECRET = N'XXXXXX' --Enter PW here.
GO
 
 
USE [msdb]
GO
EXEC msdb.dbo.sp_add_proxy @proxy_name=N'PDXExtractQA',@credential_name=N'PDXExtractQA', 
              @enabled=1
GO
EXEC msdb.dbo.sp_grant_proxy_to_subsystem @proxy_name=N'PDXExtractQA', @subsystem_id=11
GO 
