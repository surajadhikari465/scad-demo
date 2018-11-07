CREATE ROLE [iCONReportingRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'iCONReportingRole', @membername = N'iCONReports';

