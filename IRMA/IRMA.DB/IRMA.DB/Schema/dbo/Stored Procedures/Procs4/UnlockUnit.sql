CREATE PROCEDURE dbo.UnlockUnit 
@Unit_ID int 
AS 

UPDATE ItemUnit 
SET User_ID = NULL 
WHERE Unit_ID = @Unit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockUnit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockUnit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockUnit] TO [IRMAReportsRole]
    AS [dbo];

