﻿/*
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

:r .\Scripts\PopulateData\PopulateNewItemSignAttributeColumnsWithExistingData.sql -- 12/03/2018 PBI 29201 / 29184
:r .\Scripts\PopulateData\UpdateAnimalWelfareTraitPattern.sql -- 12/04/2018 BUG 30597
:r .\Scripts\PopulateData\AppLogArchiveRetentionPolicy.sql --PBI 30945 2015-01-25

-- Always run this security script and make it the last entry in this script.
:r .\Security\Icon.Security.sql