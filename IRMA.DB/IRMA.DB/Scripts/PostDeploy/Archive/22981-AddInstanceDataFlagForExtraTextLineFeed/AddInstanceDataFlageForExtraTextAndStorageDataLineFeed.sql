IF NOT EXISTS(SELECT 1 FROM [InstanceDataFlags]
			  WHERE FlagKey = 'EnableReturnsInExtraTextAndStorageData')
BEGIN
	IF @@SERVERNAME like '%UK%'
		INSERT INTO [ItemCatalog_Test].[dbo].[InstanceDataFlags]
			   ([FlagKey]
			   ,[FlagValue]
			   ,[CanStoreOverride])
		 VALUES
			   ('EnableReturnsInExtraTextAndStorageData',
			   1,
			   0)
	ELSE
		INSERT INTO [ItemCatalog_Test].[dbo].[InstanceDataFlags]
			   ([FlagKey]
			   ,[FlagValue]
			   ,[CanStoreOverride])
		 VALUES
			   ('EnableReturnsInExtraTextAndStorageData',
			   0,
			   0)
END
GO
