CREATE PROCEDURE [dbo].[AppConfig_KeyList]
AS 

SELECT KeyID, [Name]
FROM AppConfigKey
WHERE Deleted = 0
ORDER BY [Name]
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_KeyList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_KeyList] TO [IRMAClientRole]
    AS [dbo];

