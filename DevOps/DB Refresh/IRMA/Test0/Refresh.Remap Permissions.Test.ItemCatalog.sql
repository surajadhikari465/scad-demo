USE ItemCatalog_Test;
EXEC sp_change_users_login 'Auto_Fix', 'IMHA_User';
EXEC sp_change_users_login 'Auto_Fix', 'IRMA_Informatica';
EXEC sp_change_users_login 'Auto_Fix', 'IRMA_Teradata';
EXEC sp_change_users_login 'Auto_Fix', 'IRMAAdmin';
EXEC sp_change_users_login 'Auto_Fix', 'IRMAReports';
EXEC sp_change_users_login 'Auto_Fix', 'IRMASchedJobs';
EXEC sp_change_users_login 'Auto_Fix', 'IRSUser';
EXEC sp_change_users_login 'Auto_Fix', 'ops';
EXEC sp_change_users_login 'Auto_Fix', 'SLIM_User';

EXEC sp_change_users_login 'Auto_Fix', 'WFM\IRMA Data Warehouse Support';
EXEC sp_change_users_login 'Auto_Fix', 'WFM\IRMA DC Analysis';
EXEC sp_change_users_login 'Auto_Fix', 'WFM\sqlserverdev';
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IRMA.bsa')
	CREATE USER [WFM\IRMA.bsa] FOR LOGIN [WFM\IRMA.bsa]
EXEC sp_addrolemember N'db_datareader', N'WFM\IRMA.bsa'
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IconWebTest')
	CREATE USER [WFM\IconWebTest] FOR LOGIN [WFM\IconWebTest]
EXEC sp_addrolemember N'IConInterface', N'WFM\IconWebTest'
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IConInterfaceUserDev')
	CREATE USER [WFM\IConInterfaceUserDev] FOR LOGIN [WFM\IConInterfaceUserDev]
EXEC sp_addrolemember N'IConInterface', N'WFM\IConInterfaceUserDev'
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IConInterfaceUserTes')
	CREATE USER [WFM\IConInterfaceUserTes] FOR LOGIN [WFM\IConInterfaceUserTes]
EXEC sp_addrolemember N'IConInterface', N'WFM\IConInterfaceUserTes'
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'IconReports')
	CREATE USER [IconReports] FOR LOGIN [IconReports]
EXEC sp_addrolemember N'IconReportingRole', N'IconReports'
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\MammothDev')
	CREATE USER [WFM\MammothDev] FOR LOGIN [WFM\MammothDev]
EXEC sp_addrolemember N'MammothRole', N'WFM\MammothDev'
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\MammothTest')
	CREATE USER [WFM\MammothTest] FOR LOGIN [WFM\MammothTest]
EXEC sp_addrolemember N'MammothRole', N'WFM\MammothTest'
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\MammothQA')
	CREATE USER [WFM\MammothQA] FOR LOGIN [WFM\MammothQA]
EXEC sp_addrolemember N'MammothRole', N'WFM\MammothQA'
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'CRDT')
	CREATE USER [CRDT] FOR LOGIN [CRDT]
EXEC sp_addrolemember N'db_datareader', N'CRDT'
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'SLAW.DataReader.tst') and exists (select name from sys.server_principals where name like 'SLAW.DataReader.tst')
	CREATE USER [SLAW.DataReader.tst] FOR LOGIN [SLAW.DataReader.tst]
-- add permissions for VENUS DB sync
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'SLAW.DataReader.tst')
  BEGIN
       EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'SLAW.DataReader.tst';
       GRANT VIEW CHANGE TRACKING ON dbo.Item TO [SLAW.DataReader.tst];
       GRANT VIEW CHANGE TRACKING ON dbo.ItemIdentifier TO [SLAW.DataReader.tst];
       GRANT VIEW CHANGE TRACKING ON dbo.ItemVendor TO [SLAW.DataReader.tst];
       GRANT VIEW CHANGE TRACKING ON dbo.Store TO [SLAW.DataReader.tst];
       GRANT VIEW CHANGE TRACKING ON dbo.StoreItem TO [SLAW.DataReader.tst];
       GRANT VIEW CHANGE TRACKING ON dbo.StoreItemVendor TO [SLAW.DataReader.tst];
       GRANT VIEW CHANGE TRACKING ON dbo.Vendor TO [SLAW.DataReader.tst];
       GRANT VIEW CHANGE TRACKING ON dbo.VendorCostHistory TO [SLAW.DataReader.tst];
       PRINT 'Added VIEW CHANGE TRACKING permissions to user [SLAW.DataReader.tst].';
  END
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\PDXExtractUserDev')
	CREATE USER [WFM\PDXExtractUserDev] FOR LOGIN [WFM\PDXExtractUserDev]
EXEC sp_addrolemember N'IRMAPDXExtractRole', N'WFM\PDXExtractUserDev'
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'IconPurgeUser')
	CREATE USER [IconPurgeUser] FROM LOGIN [IconPurgeUser];
EXEC sp_addrolemember 'db_datawriter', 'IconPurgeUser';
EXEC sp_addrolemember 'db_datareader', 'IconPurgeUser';
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'spice_user')
	CREATE USER [spice_user] FROM LOGIN [spice_user];
EXEC sp_addrolemember 'db_datareader', 'spice_user';
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select loginname from master.dbo.syslogins where name like '%vsts.agent%')
begin
	CREATE LOGIN [WFM\vsts.agent] FROM WINDOWS WITH DEFAULT_DATABASE=[master]
end
if not exists (select name from sys.database_principals where name like 'WFM\vsts.agent')
	CREATE USER [WFM\vsts.agent] FOR LOGIN [WFM\vsts.agent]
go
EXEC sp_addrolemember N'db_owner', N'WFM\vsts.agent'
---------------------------------------------------------------------
---------------------------------------------------------------------



--==============================================================================================================================



--USE ItemCatalog_Test
-- NOTE: Developers group is covered by DBOwner job.

CREATE USER [WFM\Deploy.IRMA.DB.NP] FOR LOGIN [WFM\Deploy.IRMA.DB.NP]
--ALTER ROLE [db_owner] ADD MEMBER [WFM\Deploy.IRMA.DB.NP] -- DOESNT WORK FOR OLDER VERSIONS OF SQL SERVER
EXEC sp_addrolemember 'db_owner', 'WFM\Deploy.IRMA.DB.NP';
go
---------------------------------------------------------------------
---------------------------------------------------------------------
EXEC sp_change_users_login 'Update_One', 'IRMAReports', 'IRMAReports';
EXEC sp_change_users_login 'Update_One', 'IRMASchedJobs', 'IRMASchedJobs';
EXEC sp_change_users_login 'Update_One', 'IRSUser', 'IRSUser';
EXEC sp_change_users_login 'Update_One', 'SLIM_User', 'SLIM_User';
EXEC sp_change_users_login 'Update_One', 'TibcoDataWriter', 'TibcoDataWriter';
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IconInterfaceUserTes')
	CREATE USER [WFM\IconInterfaceUserTes] FOR LOGIN [WFM\IconInterfaceUserTes]
--ALTER ROLE [db_datareader] ADD MEMBER [WFM\IconInterfaceUserTes] -- DOESNT WORK FOR OLDER VERSIONS OF SQL SERVER
--ALTER ROLE [IConInterface] ADD MEMBER [WFM\IconInterfaceUserTes] -- DOESNT WORK FOR OLDER VERSIONS OF SQL SERVER
EXEC sp_addrolemember 'db_datareader', 'WFM\IconInterfaceUserTes';
EXEC sp_addrolemember 'IConInterface', 'WFM\IconInterfaceUserTes';
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IconWebTest')
	CREATE USER [WFM\IconWebTest] FOR LOGIN [WFM\IconWebTest]
--ALTER ROLE [db_datareader] ADD MEMBER [WFM\IconWebTest] -- DOESNT WORK FOR OLDER VERSIONS OF SQL SERVER
--ALTER ROLE [IConInterface] ADD MEMBER [WFM\IconWebTest] -- DOESNT WORK FOR OLDER VERSIONS OF SQL SERVER
EXEC sp_addrolemember 'db_datareader', 'WFM\IconWebTest';
EXEC sp_addrolemember 'IConInterface', 'WFM\IconWebTest';
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\MammothTest')
	CREATE USER [WFM\MammothTest] FOR LOGIN [WFM\MammothTest]
--ALTER ROLE [MammothRole] ADD MEMBER [WFM\MammothTest] -- DOESNT WORK FOR OLDER VERSIONS OF SQL SERVER
EXEC sp_addrolemember 'MammothRole', 'WFM\MammothTest';
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IRMA Read Test')
	CREATE USER [WFM\IRMA Read Test] FOR LOGIN [WFM\IRMA Read Test]
--ALTER ROLE [db_datareader] ADD MEMBER [WFM\IRMA Read Test] -- DOESNT WORK FOR OLDER VERSIONS OF SQL SERVER
EXEC sp_addrolemember 'db_datareader', 'WFM\IRMA Read Test';
