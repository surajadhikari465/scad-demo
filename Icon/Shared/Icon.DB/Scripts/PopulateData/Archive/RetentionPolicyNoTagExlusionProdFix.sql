declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'NoTagItemExclusion RetentionPolicy Populate Prod Fix'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @prdServerName nvarchar(255) = 'SQLSHARED3-PRD3\SHARED3P'
	declare @currentServerName nvarchar(255) = (select @@SERVERNAME)

	declare @tempRetentionPolicy table
	(
		[Server] [nvarchar](50) NULL,
		[Database] [nvarchar](16) NULL,
		[Schema] [nvarchar](50) NULL,
		[Table] [nvarchar](64) NULL,
		[DaysToKeep] [int] NULL
	)

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