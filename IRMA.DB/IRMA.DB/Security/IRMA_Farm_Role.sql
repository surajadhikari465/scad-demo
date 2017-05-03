CREATE ROLE [IRMA_Farm_Role]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMA_Farm_Role', @membername = N'IRMA_FarmMW';

