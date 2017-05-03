CREATE ROLE [IRMASchedJobsRole]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'IRMASchedJobsRole', @membername = N'IRMASchedJobs';

