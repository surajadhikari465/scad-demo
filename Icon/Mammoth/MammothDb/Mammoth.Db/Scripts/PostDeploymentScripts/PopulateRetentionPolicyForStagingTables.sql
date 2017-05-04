declare @scriptKey varchar(128)

set @scriptKey = 'PopulateRetentionPolicyForStagingTables'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @serverName nvarchar(50);
	SET @serverName = @@SERVERNAME;

	-- DEV and TEST Instance
	IF @serverName = 'CEWD6587\MAMMOTH'
	BEGIN
		USE Mammoth_Dev

		-- DEV DB
		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth_Dev' AND rp.[Schema] = 'stage' AND rp.[Table] = 'Price')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth_Dev', N'stage', N'Price', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth_Dev' AND rp.[Schema] = 'stage' AND rp.[Table] = 'ItemLocale')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth_Dev', N'stage', N'ItemLocale', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth_Dev' AND rp.[Schema] = 'stage' AND rp.[Table] = 'ItemLocaleExtended')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth_Dev', N'stage', N'ItemLocaleExtended', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth_Dev' AND rp.[Schema] = 'stage' AND rp.[Table] = 'Items')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth_Dev', N'stage', N'Items', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth_Dev' AND rp.[Schema] = 'stage' AND rp.[Table] = 'HierarchyClass')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth_Dev', N'stage', N'HierarchyClass', 1)

		-- TEST DB
		USE Mammoth
			IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'Price')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth', N'stage', N'Price', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'ItemLocale')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth', N'stage', N'ItemLocale', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'ItemLocaleExtended')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth', N'stage', N'ItemLocaleExtended', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'Items')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth', N'stage', N'Items', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'HierarchyClass')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth', N'stage', N'HierarchyClass', 1)
	END

	-- QA Instance
	IF @serverName = 'QA-01-MAMMOTH\MAMMOTH'
	BEGIN
		USE Mammoth
		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-QA\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'Price')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-QA\MAMMOTH', N'Mammoth', N'stage', N'Price', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-QA\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'ItemLocale')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-QA\MAMMOTH', N'Mammoth', N'stage', N'ItemLocale', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-QA\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'ItemLocaleExtended')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-QA\MAMMOTH', N'Mammoth', N'stage', N'ItemLocaleExtended', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-QA\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'Items')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-QA\MAMMOTH', N'Mammoth', N'stage', N'Items', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-QA\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'HierarchyClass')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-QA\MAMMOTH', N'Mammoth', N'stage', N'HierarchyClass', 1)
	END

	-- PRD Instance
	IF @serverName = 'PRD-01-MAMMOTH\MAMMOTH'
	BEGIN
		USE Mammoth
		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-PRD\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'Price')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-PRD\MAMMOTH', N'Mammoth', N'stage', N'Price', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-PRD\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'ItemLocale')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-PRD\MAMMOTH', N'Mammoth', N'stage', N'ItemLocale', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-PRD\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'ItemLocaleExtended')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-PRD\MAMMOTH', N'Mammoth', N'stage', N'ItemLocaleExtended', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-PRD\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'Items')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-PRD\MAMMOTH', N'Mammoth', N'stage', N'Items', 1)

		IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-PRD\MAMMOTH' AND rp.[Database] = 'Mammoth' AND rp.[Schema] = 'stage' AND rp.[Table] = 'HierarchyClass')
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'MAMMOTH-DB01-PRD\MAMMOTH', N'Mammoth', N'stage', N'HierarchyClass', 1)
	END

	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO