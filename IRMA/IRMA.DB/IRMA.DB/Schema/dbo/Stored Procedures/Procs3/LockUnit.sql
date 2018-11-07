CREATE PROCEDURE dbo.LockUnit 
@Unit_ID int, 
@User_ID int 
AS 

UPDATE ItemUnit 
SET User_ID = @User_ID 
WHERE Unit_ID = @Unit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockUnit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockUnit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockUnit] TO [IRMAReportsRole]
    AS [dbo];

