CREATE PROCEDURE dbo.GetShelfLifeLockStatus 
@ShelfLife_ID int 
AS 

SELECT User_ID 

FROM ItemShelfLife 

WHERE ShelfLife_ID = @ShelfLife_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeLockStatus] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeLockStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeLockStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShelfLifeLockStatus] TO [IRMAReportsRole]
    AS [dbo];

