﻿CREATE PROCEDURE [dbo].[AppConfig_GetConfigKeyList]

	@ApplicationID uniqueidentifier,
	@EnvironmentID uniqueidentifier
	
AS 

SELECT  '<add key="' + AppConfigKey.Name + '" value="' +  AppConfigValue.Value  + '"/>' As ConfigKey
FROM	AppConfigValue
		INNER JOIN AppConfigKey ON AppConfigValue.KeyID = AppConfigKey.KeyID AND AppConfigKey.Deleted = 0
		INNER JOIN AppConfigApp ON AppConfigValue.ApplicationID = AppConfigApp.ApplicationID AND AppConfigValue.EnvironmentID = AppConfigApp.EnvironmentID AND AppConfigApp.Deleted = 0
		INNER JOIN AppConfigEnv ON AppConfigApp.EnvironmentID = AppConfigEnv.EnvironmentID AND AppConfigEnv.Deleted = 0
		INNER JOIN AppConfigType ON AppConfigApp.TypeID = AppConfigType.TypeID
WHERE AppConfigValue.ApplicationID = @ApplicationID
AND AppConfigValue.EnvironmentID = @EnvironmentID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetConfigKeyList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetConfigKeyList] TO [IRMAClientRole]
    AS [dbo];

