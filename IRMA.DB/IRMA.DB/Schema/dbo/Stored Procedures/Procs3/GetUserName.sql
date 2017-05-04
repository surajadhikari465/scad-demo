CREATE PROCEDURE dbo.GetUserName
@User_ID int 
AS 

SELECT UserName 
FROM Users 
WHERE User_ID = @User_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserName] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserName] TO [IRMAReportsRole]
    AS [dbo];

