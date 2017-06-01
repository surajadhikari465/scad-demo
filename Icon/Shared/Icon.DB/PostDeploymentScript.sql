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

/*
--:r .\Scripts\PopulateData\Release\IconMasterData.sql
--:r .\Scripts\PopulateData\Release\IconPopulateData.sql
:r .\Scripts\PopulateData\Release\IconTimezonesForPos.sql
:r .\Scripts\PopulateData\Release\PBI17488_RetentionPolicyIRMAAppLog.sql
:r .\Scripts\PopulateData\Release\AddFinancialHierarchyCodeToSubTeams.sql
:r .\Scripts\PopulateData\Release\AddInforProductMessageType.sql
:r .\Scripts\PopulateData\Release\FixPackageUnitTraitPattern.sql
:r .\Scripts\PopulateData\Release\FixSupplementsAndSpiritSubTeams.sql
:r .\Scripts\PopulateData\Release\FixTaxAbbreviationAndTaxRomanceTraitPattern.sql
:r .\Scripts\PopulateData\Release\InsertInforErrors.sql
:r .\Scripts\PopulateData\Release\InsertInforHierarchyClassErrors.sql
:r .\Scripts\PopulateData\Release\InsertInforHierarchyMessageType.sql
:r .\Scripts\PopulateData\Release\InsertInforHierarchyMismatchError.sql
:r .\Scripts\PopulateData\Release\RetentionPolicyInforArchiveTables.sql
:r .\Scripts\PopulateData\Release\RemoveCharactersInforCantConsumeFromMerchBrickNames.sql
:r .\Scripts\PopulateData\Release\AddBrandDeleteEventType.sql
:r .\Scripts\PopulateData\Release\AddNationalClassUpdateAndDeleteEventType.sql
:r .\Scripts\PopulateData\Release\InsertInforDuplicateTaxCodeHierarchyClassError.sql
:r .\Scripts\PopulateData\Release\UpdatePOSScaleTare.sql

-- Zhao, 3/14/17: PBI 20493: Removed the unneeded sql agent job.
:r .\Scripts\PopulateData\Release\AddBrandDeleteEventType.sql
*/


-- Lux, 4/11/17: PBI 21235: Add MaintFlag Row to DbStatus Table
:r .\Scripts\PopulateData\Release\AddMaintFlagRowToDbStatusTable.sql
:r .\Scripts\PopulateData\Release\RetentionPolicyEventQueueArchiveTable.sql
:r .\Scripts\PopulateData\Release\AddNationalClassUpdateAndDeleteEventType.sql
:r .\Scripts\PopulateData\Release\AddVimArchiveTablesToRetentionPolicy.sql
:r .\Scripts\PopulateData\Release\IconTimezonesForPos.sql
:r .\Scripts\PopulateData\Release\InsertInforOutOfSyncItemUpdateError.sql
:r .\Scripts\PopulateData\Release\PBI17488_RetentionPolicyIRMAAppLog.sql
:r .\Scripts\PopulateData\Release\RemoveInvalidPLUandNon-RetailItemNotification.sql
:r .\Scripts\PopulateData\Release\RetentionPolicyInforArchiveTables.sql
:r .\Scripts\PopulateData\Release\RetentionPolicyMessageArchiveProductOutOfSync.sql

-- ED, Jun-1-17: PBI 21913 Update Icon db to allow 0 - None to be added to HSH ref table
:r .\Scripts\PopulateData\Release\PBI21913_Add_HealthyEatingRating_of_None.sql