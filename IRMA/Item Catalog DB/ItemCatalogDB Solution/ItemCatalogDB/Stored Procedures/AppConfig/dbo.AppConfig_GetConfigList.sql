IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_GetConfigList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_GetConfigList]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
