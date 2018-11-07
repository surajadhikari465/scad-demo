CREATE PROCEDURE dbo.GetShelfLifeInfoFirst
AS 

SELECT ShelfLife_Name, ShelfLife_ID, User_ID 
FROM ItemShelfLife
WHERE ShelfLife_ID = (SELECT MIN(ShelfLife_ID) FROM ItemShelfLife)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

