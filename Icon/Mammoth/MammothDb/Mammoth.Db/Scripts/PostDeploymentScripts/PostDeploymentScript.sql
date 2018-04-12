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
*/

-- Please add a date-added comment to the end of your line, to help with tracking, maintenance, and archiving.

:r AlterItemAttributes_ExtAttributeValueColumnSize.sql --2018.03.16 PBI 25777
:r AddIrmaPriceMessageType.sql --2018.04.11 PBI 25853
-- Run every time
:r ..\..\Security\SecurityGrants.sql
