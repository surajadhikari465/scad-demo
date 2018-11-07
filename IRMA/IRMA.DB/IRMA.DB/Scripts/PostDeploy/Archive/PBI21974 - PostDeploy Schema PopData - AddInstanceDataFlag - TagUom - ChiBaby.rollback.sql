IF EXISTS (SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey = 'BatchTagUOMChanges')
BEGIN
	DELETE FROM [dbo].[InstanceDataFlags] WHERE [FlagKey] = 'BatchTagUOMChanges'
END

IF EXISTS (SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey = 'BatchChicagoBabyChanges')
BEGIN
	DELETE FROM [dbo].[InstanceDataFlags] WHERE [FlagKey] = 'BatchChicagoBabyChanges'
END