/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r HC_PurgeInventoryQueue.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
DECLARE @Region AS VARCHAR(2)
SELECT @Region = RegionCode FROM [dbo].[Region]

IF NOT EXISTS (SELECT 1 FROM [dbo].[RetentionPolicy] WHERE [Table] = 'InventoryQueue')
BEGIN
	IF @Region = 'RM'
	BEGIN
	INSERT INTO [dbo].[RetentionPolicy]
			   ([Schema]
			   ,[Table]
			   ,[ReferenceColumn]
			   ,[DaysToKeep]
			   ,[TimeToStart]
			   ,[TimeToEnd]
			   ,[IncludedInDailyPurge]
			   ,[DailyPurgeCompleted]
			   ,[PurgeJobName]
			   ,[LastPurgedDateTime])
		 VALUES
			   ('amz'
			   ,'InventoryQueue'
			   ,'InsertedDate'
			   ,30
			   ,20
			   ,24
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL)  
	END

	IF @Region = 'NC' Or @Region = 'NA' Or @Region = 'MW' Or @Region = 'MA' Or @Region = 'FL' Or @Region = 'SW' Or @Region = 'SP' Or @Region = 'SO'
	BEGIN
	INSERT INTO [dbo].[RetentionPolicy]
			   ([Schema]
			   ,[Table]
			   ,[ReferenceColumn]
			   ,[DaysToKeep]
			   ,[TimeToStart]
			   ,[TimeToEnd]
			   ,[IncludedInDailyPurge]
			   ,[DailyPurgeCompleted]
			   ,[PurgeJobName]
			   ,[LastPurgedDateTime])
		 VALUES
			   ('amz'
			   ,'InventoryQueue'
			   ,'InsertedDate'
			   ,30
			   ,21
			   ,24
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL)
	END

	IF @Region = 'NE'
	BEGIN
	INSERT INTO [dbo].[RetentionPolicy]
			   ([Schema]
			   ,[Table]
			   ,[ReferenceColumn]
			   ,[DaysToKeep]
			   ,[TimeToStart]
			   ,[TimeToEnd]
			   ,[IncludedInDailyPurge]
			   ,[DailyPurgeCompleted]
			   ,[PurgeJobName]
			   ,[LastPurgedDateTime])
		 VALUES
			   ('amz'
			   ,'InventoryQueue'
			   ,'InsertedDate'
			   ,30
			   ,18
			   ,21
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL)
	END

	IF @Region = 'PN'
	BEGIN
	INSERT INTO [dbo].[RetentionPolicy]
			   ([Schema]
			   ,[Table]
			   ,[ReferenceColumn]
			   ,[DaysToKeep]
			   ,[TimeToStart]
			   ,[TimeToEnd]
			   ,[IncludedInDailyPurge]
			   ,[DailyPurgeCompleted]
			   ,[PurgeJobName]
			   ,[LastPurgedDateTime])
		 VALUES
			   ('amz'
			   ,'InventoryQueue'
			   ,'InsertedDate'
			   ,30
			   ,20
			   ,23
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL)
	END

	IF @Region = 'EU'
	BEGIN
	INSERT INTO [dbo].[RetentionPolicy]
			   ([Schema]
			   ,[Table]
			   ,[ReferenceColumn]
			   ,[DaysToKeep]
			   ,[TimeToStart]
			   ,[TimeToEnd]
			   ,[IncludedInDailyPurge]
			   ,[DailyPurgeCompleted]
			   ,[PurgeJobName]
			   ,[LastPurgedDateTime])
		 VALUES
			   ('amz'
			   ,'InventoryQueue'
			   ,'InsertedDate'
			   ,30
			   ,15
			   ,18
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL)
	END
END
GO