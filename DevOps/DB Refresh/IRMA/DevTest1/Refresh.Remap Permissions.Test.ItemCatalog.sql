USE ItemCatalog;
-- NOTE: Developers group is covered by DBOwner job.

CREATE USER [WFM\Deploy.IRMA.DB.NP] FOR LOGIN [WFM\Deploy.IRMA.DB.NP]
ALTER ROLE [db_owner] ADD MEMBER [WFM\Deploy.IRMA.DB.NP]
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
ALTER ROLE [db_datareader] ADD MEMBER [WFM\IconInterfaceUserTes]
ALTER ROLE [IConInterface] ADD MEMBER [WFM\IconInterfaceUserTes]
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IconWebTest')
	CREATE USER [WFM\IconWebTest] FOR LOGIN [WFM\IconWebTest]
ALTER ROLE [db_datareader] ADD MEMBER [WFM\IconWebTest]
ALTER ROLE [IConInterface] ADD MEMBER [WFM\IconWebTest]
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\MammothTest')
	CREATE USER [WFM\MammothTest] FOR LOGIN [WFM\MammothTest]
ALTER ROLE [MammothRole] ADD MEMBER [WFM\MammothTest]
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IRMA Read Test')
	CREATE USER [WFM\IRMA Read Test] FOR LOGIN [WFM\IRMA Read Test]
ALTER ROLE [db_datareader] ADD MEMBER [WFM\IRMA Read Test]
