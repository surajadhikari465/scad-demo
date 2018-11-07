CREATE ROLE [IRMAPromoRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMAPromoRole', @membername = N'WFM\IRMAPromoSQLPrdFL';


GO
EXECUTE sp_addrolemember @rolename = N'IRMAPromoRole', @membername = N'PromoPlanner_User';

