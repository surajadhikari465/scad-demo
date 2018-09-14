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

-- Run every time
:r ..\..\Security\SecurityGrants.sql
:r PopulateItemPriceTypeWithGsp.sql
:r AddIrmaItemKeyAttributes.sql --PBI 28239 Aug. 15, 2018
:r UpdateAppLogRetention.sql -- PBI 28749 8/27/2018
:r AddNumberOfDigitsForCFSItems.sql -- PBI 28686 09/13/2018