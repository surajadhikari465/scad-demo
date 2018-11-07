SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetDistAndMfg]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetDistAndMfg]
GO


CREATE PROCEDURE dbo.GetDistAndMfg
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT Zone.Zone_id, Zone_Name, Store_Name, Region_id, Store.Store_No, State 
    FROM 
        Zone (nolock)
        INNER JOIN 
            Store (nolock)
            ON Zone.Zone_Id = Store.Zone_Id
        LEFT JOIN
            Vendor (nolock)
            ON Vendor.Store_No = Store.Store_No
    WHERE (Distribution_Center = 1 OR Manufacturer = 1)
    GROUP BY Zone.Zone_Id, Store.Store_No, Zone_Name, Store_Name, Region_Id, State
    ORDER BY Zone.Zone_Id, Store.Store_No, Zone_Name, Region_Id 
     
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



