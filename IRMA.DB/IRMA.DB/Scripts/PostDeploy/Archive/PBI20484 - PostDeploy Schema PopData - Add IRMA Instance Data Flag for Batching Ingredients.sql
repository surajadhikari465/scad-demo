IF NOT EXISTS (
        SELECT 1
        FROM [dbo].[InstanceDataFlags]
        WHERE FlagKey = 'BatchNonValidatedIngredients'
        )
BEGIN
	INSERT INTO [dbo].[InstanceDataFlags]
			([FlagKey], [FlagValue], [CanStoreOverride])
		VALUES
			('BatchNonValidatedIngredients', 0, 1)
END