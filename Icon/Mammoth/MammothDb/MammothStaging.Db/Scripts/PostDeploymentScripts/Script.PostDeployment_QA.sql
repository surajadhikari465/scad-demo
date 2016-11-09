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

:r .\PopulateEtlTables.sql
--:r .\SecurityGrants.QA.sql
--:r "MAMMOTH . QA . 02A . ETL . Extract - Icon.sql"
--:r "MAMMOTH . QA . 02B . ETL . Extract - IRMA.sql"