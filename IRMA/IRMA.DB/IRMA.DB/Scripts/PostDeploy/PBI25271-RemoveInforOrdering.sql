DECLARE @app_name varchar(50)		= 'IRMA CLIENT'
DECLARE @ConfigKey varchar(150)     = 'WFMBannerStoresForOrdering'
DECLARE @ConfigKeyId varchar(150)   = 'WFMBannerStoresForOrdering'
DECLARE @IDFKey varchar(150)        = 'Include365StoresForInforOrdering'
DECLARE @User_ID  int               = 0

DELETE acv
FROM [dbo].[AppConfigValue] acv
    INNER JOIN [dbo].[AppConfigApp] aca ON acv.ApplicationID = aca.ApplicationID 
	INNER JOIN [dbo].[AppConfigKey] ack ON acv.KeyID = ack.KeyID 
	WHERE aca.[Name]=@app_name
		  AND (ack.[Name] = @ConfigKey)

SELECT @ConfigKeyId = (SELECT KeyID from AppConfigKey where Name = @ConfigKey)

SELECT @User_ID = ISNULL((SELECT user_id FROM Users WHERE UserName = 'System'),0)

IF EXISTS(SELECT * FROM AppConfigKey WHERE Name = @ConfigKey AND Deleted = 0)
    EXEC AppConfig_RemoveKey null, null, @ConfigKeyId, @User_ID

DELETE dbo.InstanceDataFlags
	WHERE FlagKey = @IDFKey