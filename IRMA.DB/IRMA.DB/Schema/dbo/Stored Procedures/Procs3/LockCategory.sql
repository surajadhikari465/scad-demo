CREATE PROCEDURE dbo.LockCategory 
@Category_ID int, 
@User_ID int 
AS 

UPDATE ItemCategory 
SET User_ID = @User_ID 
WHERE Category_ID = @Category_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockCategory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockCategory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockCategory] TO [IRMAReportsRole]
    AS [dbo];

