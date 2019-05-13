DECLARE @testEnvironmentID uniqueidentifier, @qaEnvironmentID uniqueidentifier, @prdEnvironmentID uniqueidentifier, @typeID int

DECLARE @applicationID uniqueidentifier = '3A500027-5CCC-4285-BAD6-C6CCDBA6DC71',
		@applicationName varchar(50) = 'InStock Messaging Application'

IF NOT EXISTS (
	SELECT 1 FROM [dbo].[AppConfigType] WHERE NAME = 'Tibco Application'
)
INSERT INTO [dbo].[AppConfigType]
           ([Name])
     VALUES
           ('Tibco Application')

SELECT @testEnvironmentID = EnvironmentID
FROM AppConfigEnv
WHERE NAME = 'TEST'

SELECT @qaEnvironmentID = EnvironmentID
FROM AppConfigEnv
WHERE NAME = 'QUALITY ASSURANCE'

SELECT @prdEnvironmentID = EnvironmentID
FROM AppConfigEnv
WHERE NAME = 'PRODUCTION'

SELECT TOP 1 @typeID = TypeID
FROM AppConfigType
WHERE Name = 'Tibco Application'

DELETE AppConfigType
WHERE Name = 'Tibco Application'
AND TypeID <> @typeID

IF NOT EXISTS (
	SELECT 1 FROM [dbo].[AppConfigApp] WHERE EnvironmentID = @testEnvironmentID AND ApplicationID = @applicationID
)
INSERT INTO [dbo].[AppConfigApp]
           ([ApplicationID]
           ,[EnvironmentID]
           ,[TypeID]
           ,[Name]
           ,[Deleted]
           ,[LastUpdate]
           ,[LastUpdateUserID])
     VALUES
           (@applicationID
           ,@testEnvironmentID
           ,@typeID
           ,@applicationName
           ,0
           ,GETDATE()
           ,0)

IF NOT EXISTS (
	SELECT 1 FROM [dbo].[AppConfigApp] WHERE EnvironmentID = @qaEnvironmentID AND ApplicationID = @applicationID
)
INSERT INTO [dbo].[AppConfigApp]
           ([ApplicationID]
           ,[EnvironmentID]
           ,[TypeID]
           ,[Name]
           ,[Deleted]
           ,[LastUpdate]
           ,[LastUpdateUserID])
     VALUES
           (@applicationID
           ,@qaEnvironmentID
           ,@typeID
           ,@applicationName
           ,0
           ,GETDATE()
           ,0)

IF NOT EXISTS (
	SELECT 1 FROM [dbo].[AppConfigApp] WHERE EnvironmentID = @prdEnvironmentID AND ApplicationID = @applicationID
)
INSERT INTO [dbo].[AppConfigApp]
           ([ApplicationID]
           ,[EnvironmentID]
           ,[TypeID]
           ,[Name]
           ,[Deleted]
           ,[LastUpdate]
           ,[LastUpdateUserID])
     VALUES
           (@applicationID
           ,@prdEnvironmentID
           ,@typeID
           ,@applicationName
           ,0
           ,GETDATE()
           ,0)
GO