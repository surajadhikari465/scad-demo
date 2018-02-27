﻿/*
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

-- This script (UpdateDefaulltScanCodeToNotNull.sql) is temporarily removed because the schema discrepancy in TEST, QA and PROD. 
-- It'll be needed when the DefaultScanCode is deployed to PROD.
:r .\UpdateDefaulltScanCodeToNotNull.sql -- 2018.01.15
:r .\TruncateItemLocaleStagingTable.sql
