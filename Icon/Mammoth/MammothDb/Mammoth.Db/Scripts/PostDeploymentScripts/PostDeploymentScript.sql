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

--[Deployed 5/25 with PSG fixes]  :r UpdateFairTradeCertifiedAttribute.sql --2018.04.27 PBI 26696
:r AddCurrencyCodeAttribute.sql --2018.05.09 PBI 26341

-- Run every time
:r ..\..\Security\SecurityGrants.sql
