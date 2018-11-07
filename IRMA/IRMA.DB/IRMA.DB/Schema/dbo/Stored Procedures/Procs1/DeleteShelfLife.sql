CREATE PROCEDURE dbo.DeleteShelfLife 
@ShelfLife_ID int 
AS 

DELETE 
FROM ItemShelfLife 
WHERE ShelfLife_ID = @ShelfLife_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteShelfLife] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteShelfLife] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteShelfLife] TO [IRMAReportsRole]
    AS [dbo];

