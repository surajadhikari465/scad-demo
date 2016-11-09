CREATE PROCEDURE [dbo].[AppConfig_TypeList]
AS 

SELECT TypeID, [Name]
FROM AppConfigType
ORDER BY [Name]
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_TypeList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_TypeList] TO [IRMAClientRole]
    AS [dbo];

