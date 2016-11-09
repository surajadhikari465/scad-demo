declare @scriptKey varchar(128)

set @scriptKey = 'PopulateRetentionPolicyTable'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @serverName nvarchar(50);
	SET @serverName = @@SERVERNAME;

	-- DEV and TEST Instance
	IF @serverName = 'CEWD6587\MAMMOTH'
	BEGIN
	USE Mammoth_Dev
	SET IDENTITY_INSERT [app].[RetentionPolicy] ON

	-- DEV DB
	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth_Dev' and rp.[Table] = 'AppLog')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (1, N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth_Dev', N'app', N'AppLog', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth_Dev' and rp.[Table] = 'MessageQueueItemLocale')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (2, N'MAMMOTH-DB01-DEV\MAMMOTH',  N'Mammoth_Dev', N'esb', N'MessageQueueItemLocale', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth_Dev' and rp.[Table] = 'MessageQueuePrice')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (3, N'MAMMOTH-DB01-DEV\MAMMOTH',  N'Mammoth_Dev', N'esb', N'MessageQueuePrice', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth_Dev' and rp.[Table] = 'MessageQueueItemLocaleArchive')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (4, N'MAMMOTH-DB01-DEV\MAMMOTH',  N'Mammoth_Dev', N'esb', N'MessageQueueItemLocaleArchive', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth_Dev' and rp.[Table] = 'MessageQueuePriceArchive')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (5, N'MAMMOTH-DB01-DEV\MAMMOTH',  N'Mammoth_Dev', N'esb', N'MessageQueuePriceArchive', 10)
	SET IDENTITY_INSERT [app].[RetentionPolicy] OFF

	-- TEST DB
	USE Mammoth
	SET IDENTITY_INSERT [app].[RetentionPolicy] ON

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'AppLog')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (1, N'MAMMOTH-DB01-DEV\MAMMOTH', N'Mammoth', N'app', N'AppLog', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueueItemLocale')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (2, N'MAMMOTH-DB01-DEV\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueueItemLocale', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueuePrice')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (3, N'MAMMOTH-DB01-DEV\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueuePrice', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueueItemLocaleArchive')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (4, N'MAMMOTH-DB01-DEV\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueueItemLocaleArchive', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-DEV\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueuePriceArchive')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (5, N'MAMMOTH-DB01-DEV\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueuePriceArchive', 10)

	SET IDENTITY_INSERT [app].[RetentionPolicy] OFF
	END

	-- QA Instance
	IF @serverName = 'QA-01-MAMMOTH\MAMMOTH'
	BEGIN
	USE Mammoth
	SET IDENTITY_INSERT [app].[RetentionPolicy] ON
	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-QA\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'AppLog')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (1, N'MAMMOTH-DB01-QA\MAMMOTH', N'Mammoth', N'app', N'AppLog', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-QA\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueueItemLocale')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (2, N'MAMMOTH-DB01-QA\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueueItemLocale', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-QA\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueuePrice')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (3, N'MAMMOTH-DB01-QA\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueuePrice', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-QA\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueueItemLocaleArchive')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (4, N'MAMMOTH-DB01-QA\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueueItemLocaleArchive', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-QA\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueuePriceArchive')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (5, N'MAMMOTH-DB01-QA\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueuePriceArchive', 10)

	SET IDENTITY_INSERT [app].[RetentionPolicy] OFF
	END

	-- PRD Instance
	IF @serverName = 'PRD-01-MAMMOTH\MAMMOTH'
	BEGIN
	USE Mammoth
	SET IDENTITY_INSERT [app].[RetentionPolicy] ON
	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-PRD\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'AppLog')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (1, N'MAMMOTH-DB01-PRD\MAMMOTH', N'Mammoth', N'app', N'AppLog', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-PRD\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueueItemLocale')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (2, N'MAMMOTH-DB01-PRD\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueueItemLocale', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-PRD\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueuePrice')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (3, N'MAMMOTH-DB01-PRD\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueuePrice', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-PRD\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueueItemLocaleArchive')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (4, N'MAMMOTH-DB01-PRD\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueueItemLocaleArchive', 10)

	IF NOT EXISTS (SELECT * FROM [app].[RetentionPolicy] rp WHERE rp.[Server] = 'MAMMOTH-DB01-PRD\MAMMOTH' AND rp.[Database] = 'Mammoth' and rp.[Table] = 'MessageQueuePriceArchive')
		INSERT [app].[RetentionPolicy] ([RetentionPolicyId], [Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (5, N'MAMMOTH-DB01-PRD\MAMMOTH',  N'Mammoth', N'esb', N'MessageQueuePriceArchive', 10)
	SET IDENTITY_INSERT [app].[RetentionPolicy] OFF
	END

	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO