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

:r .\Scripts\PopulateData\Release\AddCurrencyCodeTrait.sql -- 2018.05.09 PBI 26623
:r .\Scripts\PopulateData\Release\DeleteR10MessageResponseTable.sql
