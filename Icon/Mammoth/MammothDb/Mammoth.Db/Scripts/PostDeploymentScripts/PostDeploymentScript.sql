/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Please add a date-added comment to the end of your line, to help with tracking, maintenance, and archiving.
:r .\23063_PopulateAttributeGroupsTable.sql --2019-11-1
:r .\Add_Attribute_Listener_To_App_Table_23063.sql --2019-11-01
:r .\PopulateRetentionPolicyForStagingESLePlumTables.sql --2019-09-25 PBI 23939
:r .\22694_ExtractService_Jobs.sql -- 2019-11-17 PBI 22694
:r .\25982_IRMAItemAttributeFileSentToS3.sql --2019-12-06
:r .\24848_PopulateLocaleHieararchyJob_ForExtractService.sql -- 2019-12-12 24848
:r .\30224_Create_IVL_Extract_JobScheudle.sql -- 2020-02-25 30224
:r .\30224_Create_IVL_Item_Extract_JobSchedule.sql -- 2020-02-25 30224
:r .\32371_CleanUp_Hierarchy_NationalClass_Table.sql -- 2020-02-28 32371

-- Run every time (add your script above; keep this at bottom, as last post-deploy script).
:r ..\..\Security\SecurityGrants.sql
