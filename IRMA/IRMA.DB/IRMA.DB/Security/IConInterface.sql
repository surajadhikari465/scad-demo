CREATE ROLE [IConInterface]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IConInterface', @membername = N'WFM\IConInterfaceUserPrd';


GO
EXECUTE sp_addrolemember @rolename = N'IConInterface', @membername = N'WFM\IConWebPrd';

