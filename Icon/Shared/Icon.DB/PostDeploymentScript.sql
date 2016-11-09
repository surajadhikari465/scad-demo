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
-- uncomment if you want to skip the scripts listed in SeedNonGreeFieldScripts for initial deployments.
--:r .\Scripts\PopulateData\Release\SeedNonGreenFieldScripts.sql

--:r .\Scripts\PopulateData\Release\IconMasterData.sql
--:r .\Scripts\PopulateData\Release\IconPopulateData.sql
:r .\Scripts\PopulateData\Release\IconTimezonesForPos.sql
:r .\Scripts\PopulateData\Release\PBI17488_RetentionPolicyIRMAAppLog.sql