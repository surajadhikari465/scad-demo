/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Please add a date-added comment to the end of your line, to help with tracking, maintenance, and archiving.

-- EXAMPLE :r .\TruncateItemLocaleStagingTable.sql -- 2018.02.27
:r .\23389_Delete_Job_CopyGpmDataForAudit.sql -- 2019.10.08
:r .\DeduplicateScanCodes_QA_PROD.sql -- 2020.01.08
:r .\32371_CleanUp_Hierarchy_NationalClass_Table.sql -- 2020-02-28 3237