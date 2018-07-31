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

:r .\Scripts\PopulateData\Release\DeleteR10MessageResponseTable.sql -- 2018.06.25
:r .\Scripts\PopulateData\Release\AddVenueLocaleType.sql -- 2018.07.24
:r .\Scripts\PopulateData\Release\AddVenueCodeOccupantLocaleTrait.sql -- 2018.07.26