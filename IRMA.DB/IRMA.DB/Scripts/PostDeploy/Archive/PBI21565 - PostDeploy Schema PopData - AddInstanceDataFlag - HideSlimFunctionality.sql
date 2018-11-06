IF NOT EXISTS (SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey = 'HideSlimFunctionality')
BEGIN
	INSERT INTO [dbo].[InstanceDataFlags] ([FlagKey], [FlagValue], [CanStoreOverride])
		VALUES ('HideSlimFunctionality', 0, 0)
END