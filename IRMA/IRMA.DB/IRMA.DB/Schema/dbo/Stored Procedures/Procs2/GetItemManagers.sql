CREATE PROCEDURE [dbo].[GetItemManagers]
AS 

BEGIN
	SELECT Manager_ID, Value
	FROM ItemManager (nolock)
	ORDER BY Value
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemManagers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemManagers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemManagers] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemManagers] TO [IRMAExcelRole]
    AS [dbo];

