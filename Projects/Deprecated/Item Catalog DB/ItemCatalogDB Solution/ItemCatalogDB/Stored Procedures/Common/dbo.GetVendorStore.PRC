SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendorStore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendorStore]
GO


CREATE PROCEDURE dbo.GetVendorStore 
@Vendor_ID int 
AS 

SELECT Vendor.Store_No, Distribution_Center, Manufacturer, Mega_Store, WFM_Store
FROM Vendor (NOLOCK)
INNER JOIN 
    Store (NOLOCK) ON 
    Store.Store_No = Vendor.Store_No
WHERE Vendor_ID = @Vendor_ID 
AND ((Distribution_Center = 1) OR (Manufacturer = 1) OR (Mega_Store = 1) OR (WFM_Store = 1))
AND dbo.fn_VendorType(Vendor.PS_Vendor_ID, Vendor.WFM, Vendor.Store_No, Store.Internal) = 3 -- regional

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

