CREATE ROLE [IRMARSTRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMARSTRole', @membername = N'WFM\IRMARSTSQLPrdFL';

