CREATE PROCEDURE dbo.UnlockBrand 
@Brand_ID int 
AS 

UPDATE ItemBrand 
SET User_ID = NULL 
WHERE Brand_ID = @Brand_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockBrand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockBrand] TO [IRMAReportsRole]
    AS [dbo];

