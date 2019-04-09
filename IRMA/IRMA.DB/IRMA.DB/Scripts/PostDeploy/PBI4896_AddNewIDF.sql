-- PBI 4896: As IT Compliance, I want an Instance Data Flag that will Disable the Ability to Change One's Own Title in IRMA
GO

IF NOT EXISTS(SELECT 1 FROM dbo.InstanceDataFlags WHERE FlagKey= 'AllowChangeOwnTitle')
INSERT INTO [dbo].[InstanceDataFlags]
           ([FlagKey]
           ,[FlagValue]
           ,[CanStoreOverride])
     VALUES
           ('AllowChangeOwnTitle',
           0,
           0)
GO


