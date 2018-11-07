CREATE PROCEDURE dbo.GetShelfLifeInfoLast
AS 

SELECT ShelfLife_Name, ShelfLife_ID, User_ID 
FROM ItemShelfLife
WHERE ShelfLife_ID = (SELECT MAX(ShelfLife_ID) FROM ItemShelfLife)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfoLast] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfoLast] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfoLast] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeInfoLast] TO [IRMAReportsRole]
    AS [dbo];

