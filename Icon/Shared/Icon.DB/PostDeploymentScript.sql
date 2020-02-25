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

:r .\Scripts\PopulateData\AddTouchPointGroupIdTrait.sql --2019.05.03 PBI 14513
:r .\Scripts\PopulateData\ItemSignAttribute_DeleteDuplicates.sql --2019.05.22 PBI 8247
:r .\Scripts\PopulateData\PopulateHospitalityTraits.sql -- 2019.05.26 PBI 21601
:r .\Scripts\PopulateData\PopulateAttributesWebConfiguration.sql -- 2019.07.13 PBI 14480
:r .\Scripts\PopulateData\RemoveMDTTraitsFromHierarchyClassTrait.sql -- 2019.07.11 PBI 22265
:r .\Scripts\PopulateData\PopulateBulkItemUploadStatus.sql -- 2019.09.16 PBI 24826
:r .\Scripts\PopulateData\PopulateBulkItemUploadFileTypes.sql -- 2019.09.25 PBI 25130
:r .\Scripts\PopulateData\AddItemPublisherToAppTable.sql -- 2019.10.10 BUG 26713
:r .\Scripts\PopulateData\PopulateAttributeMessageType.sql --2019.10.18 PBI 23351
:r .\Scripts\PopulateData\AddAttributePublisherAppId.sql --2019.10.18 PBI 23351
:r .\Scripts\PopulateData\AddTablesToRetentionPolicyTable.sql --2019.10.31 27691
:r .\Scripts\PopulateData\23417-PopulateManufacturerValues.sql --2019-11-08 23417
:r .\Scripts\PopulateData\28121-MoveBrandTraitsToHierarchyClass.sql --2019-11-08 28121
:r .\Scripts\PopulateData\28242IconRebootHierarchyUpdates.sql --2019-11-12 28242
:r .\Scripts\PopulateData\24848_AddAppIdForExtractService.sql -- 2019-12-15 24848
:r .\Scripts\PopulateData\PopulateContactType_24426.sql --2019-12-20
:r .\Scripts\PopulateData\24776_AddHierarchyContactTrait.sql --2020-01-09
:r .\Scripts\PopulateData\UpdateDeprecatedSubTeams.sql --2020-01-24 30580
:r .\Scripts\PopulateData\DeleteInvalidEventQueueRecords.sql -- 2020-02-25 32410
-- Always run this security script and make it the last entry in this script.
:r .\Security\Icon.Security.sql