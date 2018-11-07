CREATE ROLE [IRMAAdminRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMAAdminRole', @membername = N'IRMAAdmin';


GO
EXECUTE sp_addrolemember @rolename = N'IRMAAdminRole', @membername = N'WFM\IRMA Admin FL';


GO
EXECUTE sp_addrolemember @rolename = N'IRMAAdminRole', @membername = N'WFM\IRMA Admin MA';

