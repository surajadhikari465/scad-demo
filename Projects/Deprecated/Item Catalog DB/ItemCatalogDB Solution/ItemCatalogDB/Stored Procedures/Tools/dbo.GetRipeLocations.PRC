SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRipeLocations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRipeLocations]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



