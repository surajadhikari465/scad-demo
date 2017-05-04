CREATE    PROCEDURE dbo.GetItemTypes AS
BEGIN
	SELECT ItemType_ID, ItemType_Name 
	FROM ItemType
	ORDER BY ItemType_Name
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemTypes] TO [IRMAReportsRole]
    AS [dbo];

