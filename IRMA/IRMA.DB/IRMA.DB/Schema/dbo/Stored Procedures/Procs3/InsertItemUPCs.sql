CREATE PROCEDURE dbo.InsertItemUPCs
@Item_Key int,
@UPC varchar(20)
AS 

INSERT INTO ItemUPC (Item_Key, UPC, UPC_Changed, UPC_Number)
VALUES (@Item_Key, @UPC, 1, CAST(@UPC AS decimal(18,0)))
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemUPCs] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemUPCs] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemUPCs] TO [IRMAReportsRole]
    AS [dbo];

