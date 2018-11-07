CREATE ROLE [IRMAClientRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMAClientRole', @membername = N'IRSUser';

