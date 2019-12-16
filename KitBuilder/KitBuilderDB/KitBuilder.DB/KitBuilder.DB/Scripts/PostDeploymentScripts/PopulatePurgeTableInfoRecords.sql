IF NOT EXISTS (SELECT * FROM PurgeTableInfo WHERE SchemaName = 'App' AND TableName = 'AppLog')
INSERT INTO [dbo].[PurgeTableInfo]
           ([SchemaName]
           ,[TableName]
           ,[ReferenceColumn]
           ,[DaysToKeep]
           ,[TimeToStart]
           ,[TimeToEnd]
           ,[IsDailyPurge]
           ,[IsDailyPurgeCompleted]
           ,[PurgeJobName]
           ,[LastPurgedDate])
     VALUES
           ('App'
           ,'AppLog'
           ,'InsertDateUtc'
           ,30
           ,1
           ,2
           ,1
           ,0
           ,'Kit Builder Data Purge Service'
           ,null)

IF NOT EXISTS (SELECT * FROM PurgeTableInfo WHERE SchemaName = 'App' AND TableName = 'MessageHistory')
INSERT INTO [dbo].[PurgeTableInfo]
           ([SchemaName]
           ,[TableName]
           ,[ReferenceColumn]
           ,[DaysToKeep]
           ,[TimeToStart]
           ,[TimeToEnd]
           ,[IsDailyPurge]
           ,[IsDailyPurgeCompleted]
           ,[PurgeJobName]
           ,[LastPurgedDate])
     VALUES
           ('App'
           ,'MessageHistory'
           ,'InsertDateUtc'
           ,30
           ,1
           ,2
           ,1
           ,0
           ,'Kit Builder Data Purge Service'
           ,null)
GO