SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetItemUnitInfo') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetItemUnitInfo
GO


CREATE PROCEDURE dbo.GetItemUnitInfo
    @Item_Key int
AS

BEGIN
    SET NOCOUNT ON

    SELECT Package_Desc1, Package_Desc2, Package_Unit_ID, 
           PU.Unit_Name As PU_Name
    FROM Item WITH (nolock)
    INNER JOIN
        ItemUnit PU WITH (nolock)
        ON PU.Unit_ID = Item.Package_Unit_ID    
    WHERE Item_Key = @Item_Key

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

