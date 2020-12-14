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


--:r .\26539_FutureCostsExtract_JobSchedule.sql --2020-04-01
:r .\34409_IRMAUserAudit_JobSchedule.sql --2020-05-28
:r .\34822_ExtractServiceJob_Attributes.sql --2020-06-10
:r .\33552_ExtractServiceJob_Price.sql --2020-06-15
:r .\41985_AMZItemVendorLane_JobSchedule.sql --2020-08-04
:r .\37503_AddScaleItemToAttributes.sql --2020-08-20
:r .\SCM1-616_PopulateHealthCheck.sql --2020-12-02
-- Run every time (add your script above; keep this at bottom, as last post-deploy script).
:r ..\..\Security\SecurityGrants.sql
