CREATE PROCEDURE [dbo].[AppConfig_GetConfigDoc]

	@ApplicationID uniqueidentifier,
	@EnvironmentID uniqueidentifier
	
AS 

SELECT Configuration 
FROM AppConfigApp 
WHERE EnvironmentID = @EnvironmentID
AND ApplicationID = @ApplicationID
AND Deleted = 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetConfigDoc] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetConfigDoc] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetConfigDoc] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetConfigDoc] TO [IRMASchedJobs]
    AS [dbo];

