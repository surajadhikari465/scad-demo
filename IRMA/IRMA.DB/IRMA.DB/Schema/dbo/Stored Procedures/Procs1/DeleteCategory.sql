CREATE PROCEDURE dbo.DeleteCategory 
@Category_ID int 
AS 

DELETE 
FROM ItemCategory 
WHERE Category_ID = @Category_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCategory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCategory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCategory] TO [IRMAReportsRole]
    AS [dbo];

