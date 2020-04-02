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
Please add a date-added comment to the end of your line, to help with tracking, maintenance, and archiving.
EXAMPLE (showing Icon-DB scripts path):
:r .\Scripts\PopulateData\Release\__YOUR__SCRIPT__NAME.sql -- 2018.01.01 PBI 12345
*/

:r .\Scripts\PopulateData\32769_UpdateSubTeamName.sql -- 2020-03-12 32769
:r .\Scripts\PopulateData\RetentionPolicyContactUpload.sql --2020-03-20 PBI: 32330
:r .\Scripts\PopulateData\AddBulkItemUploadToAppTable.sql --2020-03-20 PBI: 32330
-- Always run this security script and make it the last entry in this script.
:r .\Security\Icon.Security.sql