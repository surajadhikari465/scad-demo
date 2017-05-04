CREATE PROCEDURE dbo.UnlockCategory 
@Category_ID int 
AS 

UPDATE ItemCategory 
SET User_ID = NULL 
WHERE Category_ID = @Category_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockCategory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockCategory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockCategory] TO [IRMAReportsRole]
    AS [dbo];

