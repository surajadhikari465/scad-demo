DECLARE @client_name varchar(50)    = 'USER AUDIT';
DECLARE @env varchar(5)                --= 'TST';  --'QA' 'TST' 'PRD'
DECLARE @key_1 varchar(150)            = 'NextRunAction'    
DECLARE @key_2 varchar(150)            = 'NextRunDate'

DELETE acv
FROM [dbo].[AppConfigValue] acv
	INNER JOIN [dbo].[AppConfigEnv] ace ON acv.EnvironmentID = ace.EnvironmentID 
    INNER JOIN [dbo].[AppConfigApp] aca ON acv.ApplicationID = aca.ApplicationID 
	INNER JOIN [dbo].[AppConfigKey] ack ON acv.KeyID = ack.KeyID 
	WHERE aca.[Name]=@client_name
		  AND (ack.[Name] = @key_1)
    
DELETE acv
FROM [dbo].[AppConfigValue] acv
	INNER JOIN [dbo].[AppConfigEnv] ace ON acv.EnvironmentID = ace.EnvironmentID 
    INNER JOIN [dbo].[AppConfigApp] aca ON acv.ApplicationID = aca.ApplicationID 
	INNER JOIN [dbo].[AppConfigKey] ack ON acv.KeyID = ack.KeyID 
    WHERE aca.[Name]=@client_name
          AND (ack.[Name] = @key_2)
      
GO 