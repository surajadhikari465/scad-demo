DECLARE @Region AS VARCHAR(2)
SELECT @Region = RegionCode FROM Region

--The following regions schedules are set: MA, MW, NC, RM, SW, UK
IF @Region = 'FL'
BEGIN
	IF EXISTS (SELECT 1 FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge' AND TimeToStart = 21)
	BEGIN
		UPDATE [dbo].[RetentionPolicy]
		   SET TimeToStart = 22, 
			   TimeToEnd = 24
		 WHERE PurgeJobName = 'OrderPurge'
		   AND TimeToStart = 21
	END
	
	IF ((SELECT COUNT(1) FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge') = 1)
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
				   ,'OrderHeader'
				   ,'OrderDate'
				   ,1095
				   ,2
				   ,3
				   ,1
				   ,0
				   ,'OrderPurge'
				   ,NULL)
	END
END

IF @Region = 'NA'
BEGIN
	IF EXISTS (SELECT 1 FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge' AND TimeToStart = 20 AND TimeToEnd = 24)
	BEGIN
		UPDATE [dbo].[RetentionPolicy]
		   SET TimeToStart = 20, 
			   TimeToEnd = 22
		 WHERE PurgeJobName = 'OrderPurge'
		   AND TimeToStart = 20
		   AND TimeToEnd = 24
	END
	
	IF ((SELECT COUNT(1) FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge') = 1)
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
				   ,'OrderHeader'
				   ,'OrderDate'
				   ,1095
				   ,23
				   ,24
				   ,1
				   ,0
				   ,'OrderPurge'
				   ,NULL)
	END
END

IF @Region = 'NE'
BEGIN
	IF EXISTS (SELECT 1 FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge' AND TimeToStart = 18 AND TimeToEnd = 21)
	BEGIN
		UPDATE [dbo].[RetentionPolicy]
		   SET TimeToStart = 19, 
			   TimeToEnd = 22
		 WHERE PurgeJobName = 'OrderPurge'
		   AND TimeToStart = 18
		   AND TimeToEnd = 21
	END
END

IF @Region = 'PN'
BEGIN
	IF EXISTS (SELECT 1 FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge' AND TimeToStart = 20 AND TimeToEnd = 23)
	BEGIN
		UPDATE [dbo].[RetentionPolicy]
		   SET TimeToStart = 20, 
			   TimeToEnd = 21
		 WHERE PurgeJobName = 'OrderPurge'
		   AND TimeToStart = 20
		   AND TimeToEnd = 23
	END
	
	IF ((SELECT COUNT(1) FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge') = 1)
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
				   ,'OrderHeader'
				   ,'OrderDate'
				   ,1095
				   ,22
				   ,24
				   ,1
				   ,0
				   ,'OrderPurge'
				   ,NULL)
	END
END

IF @Region = 'SO'
BEGIN
	IF EXISTS (SELECT 1 FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge' AND TimeToStart = 21 AND TimeToEnd = 24)
	BEGIN
		UPDATE [dbo].[RetentionPolicy]
		   SET TimeToStart = 20, 
			   TimeToEnd = 22
		 WHERE PurgeJobName = 'OrderPurge'
		   AND TimeToStart = 21
		   AND TimeToEnd = 24
	END
	
	IF ((SELECT COUNT(1) FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge') = 1)
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
				   ,'OrderHeader'
				   ,'OrderDate'
				   ,1095
				   ,23
				   ,24
				   ,1
				   ,0
				   ,'OrderPurge'
				   ,NULL)
	END
END

IF @Region = 'SP'
BEGIN
	IF EXISTS (SELECT 1 FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge' AND TimeToStart = 21 AND TimeToEnd = 24)
	BEGIN
		UPDATE [dbo].[RetentionPolicy]
		   SET TimeToStart = 20, 
			   TimeToEnd = 22
		 WHERE PurgeJobName = 'OrderPurge'
		   AND TimeToStart = 21
		   AND TimeToEnd = 24
	END
	
	IF ((SELECT COUNT(1) FROM [dbo].[RetentionPolicy] WHERE PurgeJobName = 'OrderPurge') = 1)
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
				   ,'OrderHeader'
				   ,'OrderDate'
				   ,1095
				   ,23
				   ,24
				   ,1
				   ,0
				   ,'OrderPurge'
				   ,NULL)
	END
END