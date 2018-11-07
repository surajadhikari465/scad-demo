CREATE ROLE [IRMAExcelRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMAExcelRole', @membername = N'SQLExcel';

