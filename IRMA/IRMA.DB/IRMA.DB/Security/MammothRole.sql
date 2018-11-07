CREATE ROLE [MammothRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'MammothRole', @membername = N'WFM\MammothPrd';

