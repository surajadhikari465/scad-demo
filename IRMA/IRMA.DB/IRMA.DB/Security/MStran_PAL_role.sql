CREATE ROLE [MStran_PAL_role]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'MStran_PAL_role', @membername = N'MSReplPAL_6_5';


GO
EXECUTE sp_addrolemember @rolename = N'MStran_PAL_role', @membername = N'MSReplPAL_6_6';


GO
EXECUTE sp_addrolemember @rolename = N'MStran_PAL_role', @membername = N'MSReplPAL_6_8';

