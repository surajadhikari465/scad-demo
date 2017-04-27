-- Begin JDA Sync Procedures

GRANT EXECUTE ON dbo.JDASync_TransferData TO IRMASchedJobs
GRANT EXECUTE ON dbo.JDASync_TransferData_Failed TO IRMASchedJobs

GRANT EXECUTE ON dbo.JDASync_ExceptionAudit TO IRMASchedJobs
GRANT EXECUTE ON dbo.JDASync_ExceptionAudit_Failed TO IRMASchedJobs

GRANT EXECUTE ON dbo.JDASync_TransferLoadedCost TO IRMASchedJobs
GRANT EXECUTE ON dbo.JDASync_TransferLoadedCost_Failed TO IRMASchedJobs

-- End JDA Sync Procedures

-- Add admin access to Jobs and Tables 
GRANT select, insert, update  ON dbo.JDA_CostSync  TO IRMAClientRole

GRANT select, insert, update ON dbo.JDA_ItemIdentifierSync  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_ItemSync  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_ItemVendorSync      TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_PriceSync  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_StoreItemVendorSync  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_VendorSync TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_HierarchyMapping  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_itemBrandMapping  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_ItemUnitmapping  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_PriceChgTypeMapping  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_SyncJobLog  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDA_SyncNotification       TO IRMAClientRole
GRANT select, insert, update ON dbo.JDASync_AuditDetail  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDASync_AuditHeader  TO IRMAClientRole
GRANT select, insert, update ON dbo.JDASync_AuditScratch  TO IRMAClientRole
--			[JDA Sync]
--			[JDAto IRMA Loaded Cost Sync]
--			[JDA Sync Audit]
GRANT select ON dbo.JDASync_AuditDetail  TO [IRMAReports]
GRANT select ON dbo.JDASync_AuditHeader  TO [IRMAReports]
GRANT select ON dbo.JDASync_AuditScratch  TO [IRMAReports]


--------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------
----         The following section was moved to the S07_MSDB Setup for JDA Data Sync.sql script.          ----
-------------VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV--------------

-- Begin Set dbMail Security

-- The following configures the JDA Sync jobs to be able to use the dbmail feature of SQL Server 2005
--USE [msdb]
--GO
--
--CREATE USER [irmaschedjobs] FOR LOGIN [IRMASchedJobs] WITH DEFAULT_SCHEMA=[dbo]
--GO
--
--EXEC msdb.dbo.sp_addrolemember @rolename = 'DatabaseMailUserRole', @membername = 'irmaschedjobs' ;
--GO
--
-- End Set dbMail Security

------------^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^---------------
--------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------
