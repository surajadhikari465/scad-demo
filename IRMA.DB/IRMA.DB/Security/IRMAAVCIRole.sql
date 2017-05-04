CREATE ROLE [IRMAAVCIRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMAAVCIRole', @membername = N'WFM\IRMAAVCISQLPrdFL';


GO
EXECUTE sp_addrolemember @rolename = N'IRMAAVCIRole', @membername = N'WFM\IRMAAVCISQLDevMA';

