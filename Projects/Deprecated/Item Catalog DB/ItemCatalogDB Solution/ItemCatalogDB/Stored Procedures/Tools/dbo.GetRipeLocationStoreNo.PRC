SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRipeLocationStoreNo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRipeLocationStoreNo]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


