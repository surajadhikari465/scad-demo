IF NOT EXISTS (SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey = 'EnableCancelAllSales')
BEGIN
	INSERT INTO [dbo].[InstanceDataFlags] ([FlagKey], [FlagValue], [CanStoreOverride])
		VALUES ('EnableCancelAllSales', 0, 0)
END