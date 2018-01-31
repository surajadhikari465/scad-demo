declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'NoTagItemExclusion RetentionPolicy Populate'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @testServerName nvarchar(255) = 'CEWD1815\SQLSHARED2012D'
	declare @qaServerName nvarchar(255) = 'QA-SQLSHARED3\SQLSHARED3Q'
	declare @prdServerName nvarchar(255) = 'IDP-ICON\SHARED3P'
	declare @currentServerName nvarchar(255) = (select @@SERVERNAME)

	declare @tempRetentionPolicy table
	(
		[Server] [nvarchar](50) NULL,
		[Database] [nvarchar](16) NULL,
		[Schema] [nvarchar](50) NULL,
		[Table] [nvarchar](64) NULL,
		[DaysToKeep] [int] NULL
	)

	if(@currentServerName = @testServerName)
	begin
		use [iCon]

		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-FL', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-MA', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-MW', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-NA', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-RM', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-SO', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-NC', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-NE', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-PN', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-SP', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-SW', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-UK', N'ItemCatalog_Test', N'dbo', N'NoTagItemExclusion', 30)	
	end

	if(@currentServerName = @qaServerName)
	begin
		use [iCon]

		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-FL\FLQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-MA\MAQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-MW\MWQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-NA\NAQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-RM\RMQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-SO\SOQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-NC\NCQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-NE\NEQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-PN\PNQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-SP\SPQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-SW\SWQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-UK\UKQ', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)	
	end

	if(@currentServerName = @prdServerName)
	begin
		use [iCon]

		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-FL\FLP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-MA\MAP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-MW\MWP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NA\NAP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-RM\RMP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SO\SOP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NC\NCP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NE\NEP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-PN\PNP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SP\SPP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SW\SWP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)
		INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-UK\UKP', N'ItemCatalog', N'dbo', N'NoTagItemExclusion', 30)	
	end

	merge app.RetentionPolicy rp
	using @tempRetentionPolicy as trp
	on 
		(rp.[Server] = trp.[Server] and
			rp.[Database] = trp.[Database] and
			rp.[Schema] = trp.[Schema] and
			rp.[Table] = trp.[Table])
	when matched then
		update set rp.DaysToKeep = trp.DaysToKeep
	when not matched then
		insert ([Server], [Database], [Schema], [Table], [DaysToKeep])
		values (trp.[Server], trp.[Database], trp.[Schema], trp.[Table], trp.[DaysToKeep]);

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO