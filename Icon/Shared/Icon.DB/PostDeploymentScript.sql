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
Please add a date-added comment to the end of your line, to help with tracking, maintenance, and archiving.
EXAMPLE (showing Icon-DB scripts path):
:r .\Scripts\PopulateData\Release\__YOUR__SCRIPT__NAME.sql -- 2018.01.01 PBI 12345
*/

--:r .\Scripts\PopulateData\36704_PopulateItemGroupTypes.sql --2020-06-11 36704
--:r .\Scripts\PopulateData\36950_PopulateReservedEsbTraitCodesTable.sql --2020-06-09 36950
--:r .\Scripts\PopulateData\36709_PopulateSkuPriceLineAttributeGroups.sql --2020-06-17 36709
-- :r .\Scripts\PopulateData\36709_AddSkuPriceLineAttributes.sql --2020-06-18 36709
-- Always run this security script and make it the last entry in this script.
-- :r .\Scripts\PopulateData\36949_AddItemGroups.sql --2020-06-29 36949
--:r .\Scripts\PopulateData\36882_PopulateFeatureFlags_Sku_andPriceline.sql -- 2020-07-08 36882
--:r .\Scripts\PopulateData\40795_Populate_ItemGroup_Keywords.sql -- 2020-07-27 40795
--:r .\Scripts\PopulateData\37766-FixSkuPriceLinePattern.sql -- 2020-08-27 37766
:r .\Scripts\PopulateData\PopulateHealthCheck.sql
:r .\Scripts\PopulateData\SCM2-292_UpdateCoffeeSubteamFinancialHierarchy.sql -- 2020-12-11
:r .\Scripts\PopulateData\SCM2-386_SubTeamRealignment_Add_3100_1410_Update_1750.sql --2021-01-28
:r .\Scripts\PopulateData\SCM4-1889_Add_Canadian_Nutrition_Traits.sql --2021-07-16
:r .\Security\Icon.Security.sql