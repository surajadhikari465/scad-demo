CREATE PROCEDURE dbo.LockShelfLife 
@ShelfLife_ID int, 
@User_ID int  
AS 

UPDATE ItemShelfLife 
SET User_ID = @User_ID 
WHERE ShelfLife_ID = @ShelfLife_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockShelfLife] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockShelfLife] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockShelfLife] TO [IRMAReportsRole]
    AS [dbo];

