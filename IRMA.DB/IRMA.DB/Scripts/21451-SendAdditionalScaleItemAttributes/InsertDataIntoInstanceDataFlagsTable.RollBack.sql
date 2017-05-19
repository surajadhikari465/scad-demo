IF  EXISTS(SELECT 1 FROM [InstanceDataFlags]
			  WHERE FlagKey = 'EnableStorageData')
BEGIN
   DELETE [ItemCatalog_Test].[dbo].[InstanceDataFlags]
   WHERE [FlagKey] = 'EnableStorageData'
	
END
GO
