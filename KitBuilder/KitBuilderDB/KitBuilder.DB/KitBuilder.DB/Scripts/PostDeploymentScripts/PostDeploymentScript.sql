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


:r InitialStatusTablePopulate.sql -- 09.11.2018
:r PopualteKitBuilderAppRoles.sql -- 05.06.2019
:r PopulateMessageTypes.sql  -- 05.07.2019

