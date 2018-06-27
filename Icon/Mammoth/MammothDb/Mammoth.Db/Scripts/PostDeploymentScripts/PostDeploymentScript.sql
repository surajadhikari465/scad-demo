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

:r AddDefaultValueToEplumSessionTable.sql --2018.06.11 PBI 27424
:r AddEmergencyPriceServiceToAppTable.sql --2018.06.12 PBI 27201

-- Run every time
:r ..\..\Security\SecurityGrants.sql
