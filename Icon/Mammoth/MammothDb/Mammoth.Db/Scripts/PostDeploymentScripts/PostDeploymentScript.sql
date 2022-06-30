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

-- :r .\SCM4-1906-AddTraitCodesForCanadaNutrition.sql --2021-07-23
-- :r .\50985_EsbDeprecation_MessageStatus.sql --2022-04-26
:r .\51243-AddingLockedForSaleAttribute.sql --2022-06-29

-- Run every time (add your script above; keep this at bottom, as last post-deploy script).
:r ..\..\Security\SecurityGrants.sql
