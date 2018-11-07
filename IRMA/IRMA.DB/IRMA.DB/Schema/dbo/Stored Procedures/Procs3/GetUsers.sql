CREATE PROCEDURE dbo.GetUsers
AS

SELECT User_ID, UserName 
FROM Users
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsers] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsers] TO [IRMAReportsRole]
    AS [dbo];

