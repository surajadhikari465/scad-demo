IF NOT EXISTS (SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey = 'BatchOrganicChanges')
BEGIN
	INSERT INTO [dbo].[InstanceDataFlags] ([FlagKey], [FlagValue], [CanStoreOverride])
		VALUES ('BatchOrganicChanges', 0, 0)
END