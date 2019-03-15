DECLARE @testEnvironmentID uniqueidentifier, @qaEnvironmentID uniqueidentifier, @prdEnvironmentID uniqueidentifier, @typeID int

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

SELECT @typeID = TypeID
FROM AppConfigType
WHERE Name = 'Tibco Application'

INSERT INTO [dbo].[AppConfigType]
           ([Name])
     VALUES
           ('Tibco Application')

INSERT INTO [dbo].[AppConfigApp]
           ([ApplicationID]
           ,[EnvironmentID]
           ,[TypeID]
           ,[Name]
           ,[Deleted]
           ,[LastUpdate]
           ,[LastUpdateUserID])
     VALUES
           ('3A500027-5CCC-4285-BAD6-C6CCDBA6DC71'
           ,@testEnvironmentID
           ,@typeID
           ,'InStock Messaging Application'
           ,0
           ,GETDATE()
           ,0)
GO


