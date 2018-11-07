SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPrimVendItmCnt]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPrimVendItmCnt]
GO


CREATE PROCEDURE dbo.GetPrimVendItmCnt
@VendorID int
AS
BEGIN
    SET NOCOUNT ON


    SELECT COUNT(DISTINCT source.item_key)  ItmCnt
    FROM StoreItemVendor source
        inner join
            StoreItemVendor Target
            on Source.Item_key = Target.Item_Key and Source.Store_no = Target.Store_No
    WHERE Source.vendor_id = @VendorID and Source.primaryvendor = 1 and Source.Vendor_ID <> Target.Vendor_ID

   
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


