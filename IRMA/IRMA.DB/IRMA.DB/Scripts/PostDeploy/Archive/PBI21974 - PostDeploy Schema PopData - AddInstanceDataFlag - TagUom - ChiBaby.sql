IF NOT EXISTS (SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey = 'BatchTagUOMChanges')
BEGIN
	INSERT INTO [dbo].[InstanceDataFlags] ([FlagKey], [FlagValue], [CanStoreOverride])
		VALUES ('BatchTagUOMChanges', 0, 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey = 'BatchChicagoBabyChanges')
BEGIN
	INSERT INTO [dbo].[InstanceDataFlags] ([FlagKey], [FlagValue], [CanStoreOverride])
		VALUES ('BatchChicagoBabyChanges', 0, 1)
END