SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetVendorCost') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetVendorCost
GO


CREATE PROCEDURE dbo.GetVendorCost
    @Item_Key int,
    @Vendor_ID int,
    @Store_No int,
    @Promotional bit,
    @Date datetime,
    @UnitCost money OUTPUT, 
    @UnitFreight money OUTPUT, 
    @Package_Desc1 decimal(9,4) OUTPUT, 
    @MSRP money OUTPUT
    
AS

BEGIN
    SET NOCOUNT ON

    -- Make sure the input @Date does not have a non-zero time
    SELECT @Date = CONVERT(datetime, CONVERT(varchar(255), @Date, 101))

    SELECT @UnitCost = VC.UnitCost, @UnitFreight = ISNULL(VC.UnitFreight, 0), @Package_Desc1 = VC.Package_Desc1, @MSRP = ISNULL(VC.MSRP, 0)

    FROM fn_VendorCostByPromo(@Item_Key, @Vendor_ID, @Store_No, @Date, @Promotional) VC


    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

