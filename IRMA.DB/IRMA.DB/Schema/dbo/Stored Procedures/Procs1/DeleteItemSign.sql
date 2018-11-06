CREATE PROCEDURE dbo.DeleteItemSign
@Store_No int,
@ItemKey int, 
@Sign_ID int 
AS 

DELETE 
FROM ItemSign
WHERE Item_Key = @ItemKey AND Store_No = @Store_No AND Sign_ID = @Sign_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemSign] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemSign] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemSign] TO [IRMAReportsRole]
    AS [dbo];

