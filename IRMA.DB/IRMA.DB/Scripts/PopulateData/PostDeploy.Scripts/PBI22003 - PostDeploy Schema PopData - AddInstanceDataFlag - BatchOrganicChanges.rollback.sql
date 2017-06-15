IF EXISTS (SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey = 'BatchOrganicChanges')
BEGIN
	DELETE FROM [dbo].[InstanceDataFlags] WHERE [FlagKey] = 'BatchOrganicChanges'
END