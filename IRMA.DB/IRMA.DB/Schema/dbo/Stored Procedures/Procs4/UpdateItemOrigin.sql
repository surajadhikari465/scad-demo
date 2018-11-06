CREATE PROCEDURE dbo.UpdateItemOrigin
@Item_Key int, 
@Origin_ID int
AS 

UPDATE Item
SET Origin_ID = @Origin_ID
WHERE Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemOrigin] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemOrigin] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemOrigin] TO [IRMAReportsRole]
    AS [dbo];

