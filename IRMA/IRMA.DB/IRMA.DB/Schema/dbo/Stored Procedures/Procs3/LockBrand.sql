CREATE PROCEDURE dbo.LockBrand 
@Brand_ID int, 
@User_ID int 
AS 

UPDATE ItemBrand SET 
User_ID = @User_ID 

WHERE Brand_ID = @Brand_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockBrand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockBrand] TO [IRMAReportsRole]
    AS [dbo];

