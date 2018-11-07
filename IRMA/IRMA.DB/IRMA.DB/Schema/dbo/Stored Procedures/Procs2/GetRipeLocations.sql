CREATE PROCEDURE dbo.GetRipeLocations 
@UserName varchar(25)

AS 
BEGIN
    SET NOCOUNT ON
        

    SELECT DISTINCT Recipe.dbo.Location.LocationName, Recipe.dbo.Location.LocationID
    FROM Recipe.dbo.Location
    INNER JOIN
        Recipe.dbo.UserLocation ON
        Recipe.dbo.Location.LocationID = Recipe.dbo.UserLocation.LocationID
    INNER JOIN
        Recipe.dbo.Users ON
        Recipe.dbo.Users.UserID = Recipe.dbo.UserLocation.UserID
    WHERE upper(Recipe.dbo.Users.UserName) = upper(isnull(@UserName, Recipe.dbo.Users.UserName))
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeLocations] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeLocations] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeLocations] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeLocations] TO [IRMAReportsRole]
    AS [dbo];

