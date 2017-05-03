CREATE PROCEDURE dbo.UnlockShelfLife 
@ShelfLife_ID int 
AS 

UPDATE ItemShelfLife 
SET User_ID = NULL 
WHERE ShelfLife_ID = @ShelfLife_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockShelfLife] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockShelfLife] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockShelfLife] TO [IRMAReportsRole]
    AS [dbo];

