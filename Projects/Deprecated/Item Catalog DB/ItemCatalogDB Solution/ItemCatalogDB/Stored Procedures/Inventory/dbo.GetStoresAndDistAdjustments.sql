SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoresAndDistAdjustments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoresAndDistAdjustments]
GO


CREATE PROCEDURE dbo.GetStoresAndDistAdjustments
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT Zone.Zone_id, Zone_Name, Store_Name, Mega_Store, WFM_Store, Region_id, Store.Store_No, Vendor.State
    FROM Zone 
    INNER JOIN Store on Zone.Zone_Id = Store.Zone_Id
    INNER JOIN Vendor on Store.Store_No = Vendor.Store_No
    WHERE (Mega_Store = 1 OR WFM_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1)
    GROUP BY Zone.Zone_Id, Store.Store_No, Zone_Name, Store_Name, Region_Id, Mega_Store, WFM_Store, Vendor.State
    ORDER BY Store_Name, Zone.Zone_Id, Store.Store_No, Zone_Name, Region_Id, Mega_Store, WFM_Store, Vendor.State
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


