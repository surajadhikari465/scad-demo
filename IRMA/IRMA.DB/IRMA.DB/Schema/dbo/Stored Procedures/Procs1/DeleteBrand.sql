CREATE PROCEDURE dbo.DeleteBrand 
@Brand_ID int  
AS 

DELETE 
FROM ItemBrand 
WHERE Brand_ID = @Brand_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteBrand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteBrand] TO [IRMAReportsRole]
    AS [dbo];

