CREATE PROCEDURE dbo.UnlockItem 
@Item_Key int 
AS 

UPDATE Item 
SET User_ID = NULL, User_ID_Date = NULL
FROM Item (rowlock)
WHERE Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockItem] TO [IRMAReportsRole]
    AS [dbo];

