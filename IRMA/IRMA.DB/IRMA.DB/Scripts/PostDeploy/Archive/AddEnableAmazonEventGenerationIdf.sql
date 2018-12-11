IF NOT EXISTS(
	SELECT 1 FROM dbo.InstanceDataFlags
	WHERE FlagKey = 'EnableAmazonEventGeneration')
BEGIN
	INSERT INTO dbo.InstanceDataFlags (FlagKey, FlagValue, CanStoreOverride)
    VALUES ('EnableAmazonEventGeneration', 0, 0)
END