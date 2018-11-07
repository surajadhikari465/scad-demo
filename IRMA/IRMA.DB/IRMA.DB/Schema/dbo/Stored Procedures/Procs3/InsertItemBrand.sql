CREATE PROCEDURE dbo.InsertItemBrand
@Brand_Name varchar(50)
AS 

INSERT INTO ItemBrand (Brand_Name)
VALUES (@Brand_Name)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemBrand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemBrand] TO [IRMAReportsRole]
    AS [dbo];

