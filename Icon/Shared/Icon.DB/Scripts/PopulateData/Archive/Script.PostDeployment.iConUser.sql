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

print '[' + convert(nvarchar, getdate(), 121) + '] Creating iConUser'
go
USE [master]
GO
CREATE LOGIN [iConUser] WITH PASSWORD=N'3Kz5&33mZW', DEFAULT_DATABASE=[Icon.DB], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
USE [Icon.DB]
GO
CREATE USER [iConUser] FOR LOGIN [iConUser]
GO
USE [Icon.DB]
GO
ALTER USER [iConUser] WITH DEFAULT_SCHEMA=[app]
GO
USE [Icon.DB]
GO
ALTER ROLE [db_owner] ADD MEMBER [iConUser]
GO
