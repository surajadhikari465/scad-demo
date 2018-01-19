DECLARE @Region AS VARCHAR(2)
SELECT @Region = RegionCode FROM Region

IF NOT EXISTS (SELECT 1 FROM [dbo].[RetentionPolicy] WHERE [Table] = 'OrderExportDeletedQueue')
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
			   ('dbo'
			   ,'OrderExportQueue'
			   ,'QueueInsertedDate'
			   ,366
			   ,20
			   ,24
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL),
			   ('dbo'
			   ,'OrderExportDeletedQueue'
			   ,'QueueInsertedDate'
			   ,366
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
			   ('dbo'
			   ,'OrderExportQueue'
			   ,'QueueInsertedDate'
			   ,366
			   ,21
			   ,24
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL),
			   ('dbo'
			   ,'OrderExportDeletedQueue'
			   ,'QueueInsertedDate'
			   ,366
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
			   ('dbo'
			   ,'OrderExportQueue'
			   ,'QueueInsertedDate'
			   ,366
			   ,18
			   ,21
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL),
			   ('dbo'
			   ,'OrderExportDeletedQueue'
			   ,'QueueInsertedDate'
			   ,366
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
			   ('dbo'
			   ,'OrderExportQueue'
			   ,'QueueInsertedDate'
			   ,366
			   ,20
			   ,23
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL),
			   ('dbo'
			   ,'OrderExportDeletedQueue'
			   ,'QueueInsertedDate'
			   ,366
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
			   ('dbo'
			   ,'OrderExportQueue'
			   ,'QueueInsertedDate'
			   ,366
			   ,15
			   ,18
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL),
			   ('dbo'
			   ,'OrderExportDeletedQueue'
			   ,'QueueInsertedDate'
			   ,366
			   ,15
			   ,18
			   ,1
			   ,0
			   ,'StraightPurge'
			   ,NULL)
	END
END
GO
