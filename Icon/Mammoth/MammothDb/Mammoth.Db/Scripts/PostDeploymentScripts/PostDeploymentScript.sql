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

:r AddPrimePsgMessageType.sql --2018.02.07
:r AddPrimeAffinityControllerToAppTable.sql --2018.02.07
:r AddProcessBODConfirmBODMessageType.sql --2018.02.21
:r AddMessageArchiveDetailPrimePsgToRetentionPolicy.sql --2018.02.27
:r UpdateActivePriceSentArchiveInRetentionPolicy.sql --2018.02.27
:r AddRegionsToRegionGpmStatusTable.sql

-- Run every time
:r ..\..\Security\SecurityGrants.sql
