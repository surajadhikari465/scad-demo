CREATE PROCEDURE dbo.UpdateShelfLifeInfo
@ShelfLife_ID int,
@ShelfLife_Name varchar(50)
AS 

UPDATE ItemShelfLife
SET User_ID = NULL,
    ShelfLife_Name = @ShelfLife_Name
WHERE ShelfLife_ID = @ShelfLife_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateShelfLifeInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateShelfLifeInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateShelfLifeInfo] TO [IRMAReportsRole]
    AS [dbo];

