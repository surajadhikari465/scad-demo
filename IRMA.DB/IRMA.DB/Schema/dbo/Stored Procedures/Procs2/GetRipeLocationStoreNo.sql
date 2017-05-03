CREATE PROCEDURE dbo.GetRipeLocationStoreNo 
@LocationID int

AS 
BEGIN
    SET NOCOUNT ON

    SELECT DISTINCT Recipe.dbo.Location.LocationName, Recipe.dbo.Location.LocationID, Recipe.dbo.Location.Store_No
    FROM Recipe.dbo.Location 
    WHERE Recipe.dbo.Location.LocationID = @LocationID
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeLocationStoreNo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeLocationStoreNo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeLocationStoreNo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeLocationStoreNo] TO [IRMAReportsRole]
    AS [dbo];

