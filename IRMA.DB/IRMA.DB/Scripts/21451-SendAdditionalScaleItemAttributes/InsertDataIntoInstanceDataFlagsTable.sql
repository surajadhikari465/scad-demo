IF NOT EXISTS(SELECT 1 FROM [InstanceDataFlags]
			  WHERE FlagKey = 'EnableStorageData')
BEGIN
	INSERT INTO [ItemCatalog_Test].[dbo].[InstanceDataFlags]
			   ([FlagKey]
			   ,[FlagValue]
			   ,[CanStoreOverride])
		 VALUES
			   ('EnableStorageData',
			   0,
			   0)
END
GO




