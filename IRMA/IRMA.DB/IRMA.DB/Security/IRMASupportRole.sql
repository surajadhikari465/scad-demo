CREATE ROLE [IRMASupportRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMASupportRole', @membername = N'WFM\IRMA Support FL';


GO
EXECUTE sp_addrolemember @rolename = N'IRMASupportRole', @membername = N'WFM\IRMA Technical Support';


GO
EXECUTE sp_addrolemember @rolename = N'IRMASupportRole', @membername = N'IRMA_Teradata';


GO
EXECUTE sp_addrolemember @rolename = N'IRMASupportRole', @membername = N'IRMA_Informatica';


GO
EXECUTE sp_addrolemember @rolename = N'IRMASupportRole', @membername = N'WFM\IRMA Data Warehouse Support';


GO
EXECUTE sp_addrolemember @rolename = N'IRMASupportRole', @membername = N'WFM\IRMA Support MA';

