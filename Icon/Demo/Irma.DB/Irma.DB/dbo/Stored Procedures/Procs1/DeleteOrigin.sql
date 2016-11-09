CREATE PROCEDURE dbo.DeleteOrigin
@Origin_ID int 
AS 

DELETE 
FROM ItemOrigin 
WHERE Origin_ID = @Origin_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrigin] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrigin] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrigin] TO [IRMAReportsRole]
    AS [dbo];

