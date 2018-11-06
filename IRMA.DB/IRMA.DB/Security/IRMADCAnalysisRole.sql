CREATE ROLE [IRMADCAnalysisRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMADCAnalysisRole', @membername = N'WFM\IRMA DC Analysis';

