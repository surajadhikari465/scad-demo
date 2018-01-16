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

-- Normal deployments start here.
--:r Currency_RemoveTrailingSpaces_PostDeployment.sql

:r AddMaintFlagRowToDbStatusTable.sql
:r AddAppsToAppTable.sql
:r PopulateRetentionPolicyForStagingTables.sql
:r AddErrorMonitorToAppTable.sql
:r AddJobSchedulerToAppTable.sql
:r AddWebSupportToAppTable.sql
:r AddGpmPriceTypes.sql
:r AddEslAttributesToAttributesTable.sql
:r AddScaleItemAttributes.sql
:r AddPrimeAffinityListenerToAppTable.sql

-- Run every time
:r ..\..\Security\SecurityGrants.sql