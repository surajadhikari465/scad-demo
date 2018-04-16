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
:r .\Scripts\PopulateData\Release\__YOUR__SCRIPT__NAME.sql -- 2018.01.01
*/

:r .\Scripts\PopulateData\AddEslAndOnePlumTraitCodes.sql -- 2018.03.08 PBI 25777
:r .\Scripts\PopulateData\Release\RemovePercentageTareWeightFromIcon.sql -- 2018.04.11 PBI 26526
:r .\Scripts\PopulateData\Release\MoveMessageQueueProductRecordsOutOfTempTable.sql -- 2018.04.11 PBI 26526
:r .\Scripts\PopulateData\FixTraitLength.sql -- 2018.04.16 PBI 26623

