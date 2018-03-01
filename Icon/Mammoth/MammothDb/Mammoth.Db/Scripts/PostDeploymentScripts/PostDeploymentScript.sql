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

-- Normal deployments start here.
--:r Currency_RemoveTrailingSpaces_PostDeployment.sql

:r AddScaleItemAttributes.sql -- 2018.01.03
:r AddVendorRelatedCodesToAttributesTable.sql -- 2018.01.04
:r AddPrimeAffinityListenerToAppTable.sql -- 2018.01.16
:r AddPercentageTareWeightToAttributesTable.sql -- 2018.01.16
:r AddExpiringTprServiceToAppTable.sql -- 2018.01.16
:r AddAltRetailSizeUomCodesToAttributesTable.sql -- 2018.01.18
:r AddPrimePsgMessageType.sql
:r AddPrimeAffinityControllerToAppTable.sql
:r AddMessageArchiveDetailPrimePsgToRetentionPolicy.sql
:r UpdateActivePriceSentArchiveInRetentionPolicy.sql
:r AddRegionsToRegionGpmStatusTable.sql

-- Run every time
:r ..\..\Security\SecurityGrants.sql