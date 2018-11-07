CREATE PROCEDURE dbo.LockOrigin 
@Origin_ID int, 
@User_ID int 
AS 

UPDATE ItemOrigin 
SET User_ID = @User_ID 
WHERE Origin_ID = @Origin_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockOrigin] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockOrigin] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockOrigin] TO [IRMAReportsRole]
    AS [dbo];

