CREATE PROCEDURE dbo.GetItemAdminUsers
AS

SELECT 
	[User_ID], UserName, FullName  
FROM 
	Users
WHERE 
	SuperUser = 1 OR Item_Administrator = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemAdminUsers] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemAdminUsers] TO [IRMAClientRole]
    AS [dbo];

