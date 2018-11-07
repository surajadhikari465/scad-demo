CREATE ROLE [IMHARole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IMHARole', @membername = N'imha_user';

