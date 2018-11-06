CREATE PROCEDURE dbo.GetUserFullName
@User_ID int 
AS 

SELECT FullName 
FROM Users 
WHERE User_ID = @User_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserFullName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserFullName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserFullName] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserFullName] TO [IRMAReportsRole]
    AS [dbo];

