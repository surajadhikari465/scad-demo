DECLARE @app_name varchar(50)		= 'IRMA CLIENT'
DECLARE @key varchar(150)           = 'StoreBUsForAMZExtract'

DELETE acv
FROM [dbo].[AppConfigValue] acv
    INNER JOIN [dbo].[AppConfigApp] aca ON acv.ApplicationID = aca.ApplicationID 
	INNER JOIN [dbo].[AppConfigKey] ack ON acv.KeyID = ack.KeyID 
	WHERE aca.[Name]=@app_name
		  AND (ack.[Name] = @key)