CREATE PROCEDURE dbo.UnlockOrderHeader 
@OrderHeader_ID int 
AS 

UPDATE OrderHeader 
SET User_ID = NULL 
WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockOrderHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockOrderHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockOrderHeader] TO [IRMAReportsRole]
    AS [dbo];

