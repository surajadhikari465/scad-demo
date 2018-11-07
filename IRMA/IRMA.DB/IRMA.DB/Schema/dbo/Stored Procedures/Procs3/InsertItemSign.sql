CREATE PROCEDURE dbo.InsertItemSign
@Item_Key int,
@Store_No int,
@Sign_ID int
AS 

INSERT INTO ItemSign (Item_Key, Store_No, Sign_ID)
VALUES (@Item_Key, @Store_No, @Sign_ID)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemSign] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemSign] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemSign] TO [IRMAReportsRole]
    AS [dbo];

