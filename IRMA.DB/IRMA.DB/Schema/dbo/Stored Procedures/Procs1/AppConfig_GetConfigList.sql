CREATE PROCEDURE [dbo].[AppConfig_GetConfigList]

AS 

SELECT     
		AppConfigValue.EnvironmentID,
		AppConfigValue.ApplicationID,
		AppConfigApp.TypeID, 
		AppConfigValue.KeyID,
		AppConfigEnv.Name AS EnvName,
		AppConfigApp.Name AS AppName,
		AppConfigType.Name AS TypeName,
		AppConfigKey.Name AS KeyName,
		AppConfigValue.Value,
		AppConfigValue.Deleted
FROM	AppConfigValue
		INNER JOIN AppConfigKey ON AppConfigValue.KeyID = AppConfigKey.KeyID
		INNER JOIN AppConfigApp ON AppConfigValue.ApplicationID = AppConfigApp.ApplicationID AND AppConfigValue.EnvironmentID = AppConfigApp.EnvironmentID
		INNER JOIN AppConfigEnv ON AppConfigApp.EnvironmentID = AppConfigEnv.EnvironmentID
		INNER JOIN AppConfigType ON AppConfigApp.TypeID = AppConfigType.TypeID
WHERE AppConfigEnv.Deleted = 0
AND AppConfigApp.Deleted = 0
AND AppConfigKey.Deleted = 0
ORDER BY AppConfigEnv.Name, AppConfigApp.Name, AppConfigKey.Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetConfigList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetConfigList] TO [IRMAClientRole]
    AS [dbo];

