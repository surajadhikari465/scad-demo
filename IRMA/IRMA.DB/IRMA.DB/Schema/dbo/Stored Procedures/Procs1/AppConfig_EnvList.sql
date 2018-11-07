CREATE PROCEDURE [dbo].[AppConfig_EnvList]

AS

SELECT 
	EnvironmentID,
	[Name],
	Shortname
FROM AppConfigEnv
WHERE Deleted = 0
ORDER BY [Name]
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_EnvList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_EnvList] TO [IRMAClientRole]
    AS [dbo];

