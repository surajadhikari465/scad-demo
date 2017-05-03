SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetRegionStates') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetRegionStates
GO


CREATE PROCEDURE dbo.GetRegionStates

AS

BEGIN
    SET NOCOUNT ON

    SELECT DISTINCT State 
    FROM Vendor
    INNER JOIN Store ON Store.Store_No = Vendor.Store_No
    INNER JOIN Zone ON Zone.Zone_ID = Store.Zone_ID
    WHERE Region_ID IS NOT NULL
    AND LEN(RTRIM(ISNULL(State, ''))) > 0
    ORDER BY State

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


