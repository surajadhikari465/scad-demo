CREATE PROCEDURE dbo.LockOrderHeader 
@OrderHeader_ID int, 
@User_ID int 
AS 

UPDATE OrderHeader 
SET User_ID = @User_ID 
WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockOrderHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockOrderHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockOrderHeader] TO [IRMAReportsRole]
    AS [dbo];

