CREATE PROCEDURE [dbo].[GetUserID]
@UserName varchar(255)
AS 

SELECT User_ID
FROM Users 
WHERE UserName = @UserName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserID] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserID] TO [IRMAReportsRole]
    AS [dbo];

