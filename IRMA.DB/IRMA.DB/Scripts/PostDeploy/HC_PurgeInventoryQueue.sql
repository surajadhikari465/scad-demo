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
DECLARE @Region AS VARCHAR(2),
@TimeToStart AS INT = null,
@TimeToEnd AS INT = null
SELECT Top 1 @Region =LTrim(RTrim(RegionCode)) FROM [dbo].[Region]

IF NOT EXISTS (SELECT 1 FROM [dbo].[RetentionPolicy] WHERE [Table] = 'InventoryQueue')
BEGIN
	IF @Region = 'RM'
	BEGIN
		SET @TimeToStart = 20
		SET @TimeToEnd = 24
	END

	IF @Region in ('NC','NA','MW','MA','FL','SW','SP','SO')
	BEGIN
		SET @TimeToStart = 21
		SET @TimeToEnd = 24
	END

	IF @Region = 'NE'
	BEGIN
		SET @TimeToStart = 18
		SET @TimeToEnd = 21
	END

	IF @Region = 'PN'
	BEGIN
		SET @TimeToStart = 20
		SET @TimeToEnd = 23
	END

	IF @Region = 'EU'
	BEGIN
		SET @TimeToStart = 15
		SET @TimeToEnd = 18
	END

	IF (@TimeToStart != null OR @TimeToEnd != null)
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
				,[PurgeJobName])
			VALUES
				('amz'
				,'InventoryQueue'
				,'InsertedDate'
				,30
				,@TimeToStart
				,@TimeToEnd
				,1
				,0
				,'StraightPurge')
	END
END
GO
