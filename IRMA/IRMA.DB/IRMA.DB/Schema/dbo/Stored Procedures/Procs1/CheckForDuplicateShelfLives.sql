CREATE PROCEDURE dbo.CheckForDuplicateShelfLives 
@ShelfLife_ID int, 
@ShelfLife_Name varchar(25) 
AS 

SELECT COUNT(*) AS ShelfLifeCount 
FROM ItemShelfLife 
WHERE ShelfLife_Name = @ShelfLife_Name AND ShelfLife_ID <> @ShelfLife_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateShelfLives] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateShelfLives] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateShelfLives] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateShelfLives] TO [IRMAReportsRole]
    AS [dbo];

