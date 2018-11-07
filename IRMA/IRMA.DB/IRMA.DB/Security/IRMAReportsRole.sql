CREATE ROLE [IRMAReportsRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMAReportsRole', @membername = N'IRMAReports';

