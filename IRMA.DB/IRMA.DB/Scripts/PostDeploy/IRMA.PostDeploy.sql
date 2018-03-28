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

-- This post-deploy script is in the base post-deploy folder, so you do not need to add a path or folder reference (you just specify your script name).
-- Please add a date-added comment to the end of your line, to help with tracking, maintenance, and archiving.
-- Example: :r Cool_Script_Stuff.sql -- 2018.01.01

:r "PBI25850-PostDeployDataUpdate-AddOrderedByInforPOSDataElement.SQL" -- 2018.03.06
:r "PBI25591-Add CancelAllSales Event to ItemChangeEventType.SQL" -- 2018.03.22
:r "PBI21565 - PostDeploy Schema PopData - AddInstanceDataFlag - HideSlimFunctionality.SQL" -- 2018.03.28

-- *This is a permanent entry (keep at bottom).
:r VersionUpdates.sql -- 2018.02.28
