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

--Static data
:r ..\PopulateData\DevBuild\PopulateAddressType.sql
:r ..\PopulateData\DevBuild\PopulateAddressUsage.sql
:r ..\PopulateData\DevBuild\PopulateAnimalWelfareRating.sql
:r ..\PopulateData\DevBuild\PopulateApp.sql
:r ..\PopulateData\DevBuild\PopulateCurrencyType.sql
:r ..\PopulateData\DevBuild\PopulateDeliverySystem.sql
:r ..\PopulateData\DevBuild\PopulateEcoScaleRating.sql
:r ..\PopulateData\DevBuild\PopulateEventType.sql
:r ..\PopulateData\DevBuild\PopulateHealthyEatingRating.sql
:r ..\PopulateData\DevBuild\PopulateHierarchy.sql
:r ..\PopulateData\DevBuild\PopulateHierarchyPrototype.sql
:r ..\PopulateData\DevBuild\PopulateItemPriceType.sql
:r ..\PopulateData\DevBuild\PopulateItemType.sql
:r ..\PopulateData\DevBuild\PopulateLocaleType.sql
:r ..\PopulateData\DevBuild\PopulateMammothEventType.sql
:r ..\PopulateData\DevBuild\PopulateMessageAction.sql
:r ..\PopulateData\DevBuild\PopulateMessageStatus.sql
:r ..\PopulateData\DevBuild\PopulateMessageType.sql
:r ..\PopulateData\DevBuild\PopulateMilkType.sql
:r ..\PopulateData\DevBuild\PopulatePartyType.sql
:r ..\PopulateData\DevBuild\PopulateParty.sql
:r ..\PopulateData\DevBuild\PopulateOrganizationType.sql
:r ..\PopulateData\DevBuild\PopulateOrganization.sql
--:r ..\PopulateData\DevBuild\PopulateProductionClaim.sql
:r ..\PopulateData\DevBuild\PopulateProductSelectionGroupType.sql
:r ..\PopulateData\DevBuild\PopulateRegions.sql
:r ..\PopulateData\DevBuild\PopulateScanCodeType.sql
:r ..\PopulateData\DevBuild\PopulateSeafoodCatchType.sql
:r ..\PopulateData\DevBuild\PopulateSeafoodFreshOrFrozen.sql
:r ..\PopulateData\DevBuild\PopulateStoreGroupType.sql
:r ..\PopulateData\DevBuild\PopulateStorePosType.sql
:r ..\PopulateData\DevBuild\PopulateTraitGroup.sql
:r ..\PopulateData\DevBuild\PopulateTrait.sql
:r ..\PopulateData\DevBuild\PopulateUOM.sql
:r ..\PopulateData\DevBuild\PopulateVimEventType.sql
:r ..\PopulateData\DevBuild\PopulateCountry.sql
:r ..\PopulateData\DevBuild\PopulateTimezone.sql
:r ..\PopulateData\DevBuild\PopulateTerritory.sql
:r ..\PopulateData\DevBuild\PopulateCounty.sql
:r ..\PopulateData\DevBuild\PopulatePostalCode.sql
:r ..\PopulateData\DevBuild\PopulateCity.sql

--Test Data
:r ..\PopulateData\DevBuild\TestData\PopulateHierarchyClass.sql
:r ..\PopulateData\DevBuild\TestData\PopulateHierarchyClassTrait.sql
:r ..\PopulateData\DevBuild\TestData\PopulateLocale.sql
:r ..\PopulateData\DevBuild\TestData\PopulateLocaleAddresses.sql
:r ..\PopulateData\DevBuild\TestData\PopulateLocaleTrait.sql
:r ..\PopulateData\DevBuild\TestData\PopulateItem.sql
:r ..\PopulateData\DevBuild\TestData\PopulateScanCode.sql
:r ..\PopulateData\DevBuild\TestData\PopulateItemTrait.sql
:r ..\PopulateData\DevBuild\TestData\PopulateItemHierarchyClass.sql