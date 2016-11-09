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
:r ..\PopulateScripts\SeedNonGreeFieldScript.sql -- run this script to skip all the "non green field" scripts.
:r ..\PopulateScripts\PopulateAppsInAppTable.sql
:r ..\PopulateScripts\PopulateAttributes.sql
:r ..\PopulateScripts\PopulateCurrency.sql
:r ..\PopulateScripts\PopulateHierarchies.sql
:r ..\PopulateScripts\PopulateItemPriceType.sql
:r ..\PopulateScripts\PopulateItemType.sql
:r ..\PopulateScripts\PopulateMessageAction.sql
:r ..\PopulateScripts\PopulateMessageStatus.sql
:r ..\PopulateScripts\PopulateMessageType.sql
:r ..\PopulateScripts\PopulateRetentionPolicyTable.sql
:r ..\PopulateScripts\PopulateUom.sql
:r ..\PopulateScripts\PopulateRegionsTable.sql
:r "..\ETL\MAMMOTH . 03 . ETL . Load.sql"

-- Normal deployments start here.
:r Currency_RemoveTrailingSpaces_PostDeployment.sql

-- Run every time
:r ..\..\Security\SecurityGrants.sql