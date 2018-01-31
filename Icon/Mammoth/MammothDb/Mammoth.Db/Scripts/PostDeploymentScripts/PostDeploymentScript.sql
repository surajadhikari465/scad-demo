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

-- This post-deploy script is in the base post-deploy folder, so you shouldn't needed to add a path or folder reference (you just specify your script name).
-- Please add a date-added comment to the end of your line, to help with tracking, maintenance, and archiving.
-- Example: :r Cool_Script_Stuff.sql -- 2018.01.01

:r AddScaleItemAttributes.sql -- 2018.01.03
:r AddVendorRelatedCodesToAttributesTable.sql -- 2018.01.04
:r AddPrimeAffinityListenerToAppTable.sql -- 2018.01.16
:r AddPercentageTareWeightToAttributesTable.sql -- 2018.01.16
:r AddExpiringTprServiceToAppTable.sql -- 2018.01.16
:r AddAltRetailSizeUomCodesToAttributesTable.sql -- 2018.01.18
