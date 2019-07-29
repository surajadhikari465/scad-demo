BEGIN
	IF EXISTS (SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey= 'AllowChangeOwnTitle')
	BEGIN
		UPDATE [dbo].[InstanceDataFlags]
		SET FlagValue = 0
		WHERE FlagKey= 'AllowChangeOwnTitle'
	END
END