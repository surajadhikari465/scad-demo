CREATE PROCEDURE dbo.GetShelfLifeAndID 
AS 

SELECT ShelfLife_ID, ShelfLife_Name 
FROM ItemShelfLife 
ORDER BY ShelfLife_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeAndID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeAndID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeAndID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeAndID] TO [IRMAReportsRole]
    AS [dbo];

