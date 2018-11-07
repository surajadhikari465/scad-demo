CREATE PROCEDURE dbo.DeleteItemVideo
@ItemVideo_ID int 
AS 

DELETE 
FROM ItemVideo
WHERE ItemVideo_ID = @ItemVideo_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemVideo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemVideo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemVideo] TO [IRMAReportsRole]
    AS [dbo];

