SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAvgCostVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAvgCostVendor]
GO

CREATE PROCEDURE dbo.GetAvgCostVendor

AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Vendor_ID, CompanyName
    FROM Store
    INNER JOIN
        Zone
        ON Zone.Zone_ID = Store.Zone_ID
    INNER JOIN
        Vendor
        ON Vendor.Store_No = Store.Store_No

    WHERE 
		(dbo.fn_GetCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) = 3)
		AND ((Distribution_Center = 1) OR (Manufacturer = 1))
		AND EXISTS (SELECT *
                FROM Price
                INNER JOIN
                    Item
                    ON Item.Item_Key = Price.Item_Key
                WHERE Price.Store_No = Store.Store_No
                AND NoDistMarkup = 1
                AND Deleted_Item = 0 AND Remove_Item = 0)
                
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

