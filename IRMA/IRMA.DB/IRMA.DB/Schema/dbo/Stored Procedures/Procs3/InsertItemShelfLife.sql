CREATE PROCEDURE dbo.InsertItemShelfLife
@ShelfLife_Name varchar(50)
AS 

INSERT INTO ItemShelfLife (ShelfLife_Name)
VALUES (@ShelfLife_Name)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemShelfLife] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemShelfLife] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemShelfLife] TO [IRMAReportsRole]
    AS [dbo];

