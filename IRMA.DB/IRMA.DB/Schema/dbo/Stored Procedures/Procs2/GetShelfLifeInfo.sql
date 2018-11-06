CREATE PROCEDURE dbo.GetShelfLifeInfo
@ShelfLife_ID int
AS 

SELECT ShelfLife_Name, ShelfLife_ID, User_ID 
FROM ItemShelfLife
WHERE ShelfLife_ID = @ShelfLife_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfo] TO [IRMAReportsRole]
    AS [dbo];

