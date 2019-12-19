EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'WFM\WFM National Purchasing Systems Support Team';
GO



GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'WFM\Nexus.Dev';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'wfm\cen-dba security';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'WFM\Nexus.Prd';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'WFM\WFM National Purchasing Systems Support Team';


GO

EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'WFM\WFM NEXus Contractors';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'WFM\WFM NEXus Support';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'dblnk';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'WFM\WFM National Purchasing Systems Support Team';


GO

EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'WFM\WFM NEXus Contractors';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'WFM\WFM NEXus Support';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'dblnk';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'oos_backend';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'oos_backend';

