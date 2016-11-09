CREATE PROCEDURE [dbo].[AppConfig_AppList]

	@EnvironmentID As uniqueidentifier

AS

SELECT 
	AppConfigApp.ApplicationID, 
	AppConfigApp.EnvironmentID,
	AppConfigApp.[Name],
	AppConfigType.[Name] As [Type]
FROM AppConfigApp
INNER JOIN AppConfigType ON AppConfigType.TypeID = AppConfigApp.TypeID
WHERE AppConfigApp.EnvironmentID = @EnvironmentID 
AND AppConfigApp.Deleted = 0
ORDER BY AppConfigApp.[Name]
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_AppList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_AppList] TO [IRMAClientRole]
    AS [dbo];

