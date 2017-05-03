IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_GetConfigKeyList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_GetConfigKeyList]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AppConfig_GetConfigKeyList]

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