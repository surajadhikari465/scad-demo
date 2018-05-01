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

:r AddIrmaPriceMessageType.sql --2018.04.11 PBI 25853
:r AddPriceArchiveTablesToRetentionPolicyTable.sql --2018.04.13 PBI 25853
:r RemovePercentageTareWeightFromAttributesTable.sql --2018.04.11 PBI 26526
:r UpdateFairTradeCertifiedAttribute.sql --2018.04.27 PBI 26696

-- Run every time
:r ..\..\Security\SecurityGrants.sql
