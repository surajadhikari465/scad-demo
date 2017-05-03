CREATE ROLE [IRMASLIMRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMASLIMRole', @membername = N'SLIM_User';


GO
EXECUTE sp_addrolemember @rolename = N'IRMASLIMRole', @membername = N'StoreOrderGuideUser';

