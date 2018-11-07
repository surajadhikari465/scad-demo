CREATE PROCEDURE dbo.UnlockOrigin 
@Origin_ID int 
AS 

UPDATE ItemOrigin 
SET User_ID = NULL 
WHERE Origin_ID = @Origin_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockOrigin] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockOrigin] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockOrigin] TO [IRMAReportsRole]
    AS [dbo];

