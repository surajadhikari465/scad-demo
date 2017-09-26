EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'BizTalk';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'IconPurgeUser';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'IRMAAdminRole';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'IRMASupportRole';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'IRMAReports';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'wfm\IRMA NonIRMA Support';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'WFM\zingonet';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'WFM\conns';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'flbluesky';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'WFM\IRMA Technical Support';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'IRMA_Refresh';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'BizTalk';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'WFM\IRMA.developers';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'WFM\FLIRMADBReaders';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'POReports';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'IConInterface';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'spice_user';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'IconPurgeUser';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'WFM\IRMA PRD Testing Users Read-Only';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'WFM\Infrastructure SQL Server OpsWatch Production';

/*
GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'MSSql.RplAgt.Log';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'MSSql.RplAgt.Snap';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'WFM\sql.rplagt.snap.prd';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'WFM\sql.rplagt.log.prd';
*/
