declare @scriptKey varchar(128)

set @scriptKey = 'RetentionPolicyInforMessageArchiveNewItem'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey
	
	declare @currentServerName nvarchar(255) = (select @@SERVERNAME)

	declare @tempRetentionPolicy table
	(
		[Server] [nvarchar](50) NULL,
		[Database] [nvarchar](16) NULL,
		[Schema] [nvarchar](50) NULL,
		[Table] [nvarchar](64) NULL,
		[DaysToKeep] [int] NULL
	)

	INSERT @tempRetentionPolicy ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (@currentServerName, N'Icon', N'infor', N'MessageArchiveNewItem', 10)

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