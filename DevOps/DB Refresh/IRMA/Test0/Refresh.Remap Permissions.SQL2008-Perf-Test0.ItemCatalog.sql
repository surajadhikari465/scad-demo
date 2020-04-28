USE ItemCatalog_Test;
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IconInterfaceUserQP')
	CREATE USER [WFM\IconInterfaceUserQP] FOR LOGIN [WFM\IconInterfaceUserQP]
EXEC sp_addrolemember 'db_datareader', 'WFM\IconInterfaceUserQP';
EXEC sp_addrolemember 'IconInterface', 'WFM\IconInterfaceUserQP';
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\IconWebQP')
	CREATE USER [WFM\IconWebQP] FOR LOGIN [WFM\IconWebQP]
EXEC sp_addrolemember 'db_datareader', 'WFM\IconWebQP';
EXEC sp_addrolemember 'IconInterface', 'WFM\IconWebQP';
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\MammothQP')
	CREATE USER [WFM\MammothQP] FOR LOGIN [WFM\MammothQP]
EXEC sp_addrolemember 'MammothRole', 'WFM\MammothQP';
GO
---------------------------------------------------------------------
---------------------------------------------------------------------
if not exists (select name from sys.database_principals where name like 'WFM\PDXExtractUserQP')
	create user [WFM\PDXExtractUserQP] for login [WFM\PDXExtractUserQP]
EXEC sp_addrolemember 'IRMAPDXExtractRole', 'WFM\PDXExtractUserQP';
