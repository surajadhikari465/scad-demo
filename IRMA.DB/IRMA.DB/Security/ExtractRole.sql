CREATE ROLE [ExtractRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'ExtractRole', @membername = N'Planogram';


GO
EXECUTE sp_addrolemember @rolename = N'ExtractRole', @membername = N'Extract';

