IF NOT EXISTS (SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey = 'DisableNoTagLogicCheckboxes')
BEGIN
	INSERT INTO [dbo].[InstanceDataFlags] ([FlagKey], [FlagValue], [CanStoreOverride])
		VALUES ('DisableNoTagLogicCheckboxes', 0, 0)
END
