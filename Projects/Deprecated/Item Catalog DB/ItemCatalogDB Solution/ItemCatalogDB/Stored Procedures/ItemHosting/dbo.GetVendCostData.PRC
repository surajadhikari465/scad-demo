SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetVendCostData') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetVendCostData
GO



CREATE PROCEDURE dbo.GetVendCostData
    @Item_Key int,
    @Vendor_ID int,
    @Store_No int,
    @Date datetime    
AS

BEGIN
    SET NOCOUNT ON

    -- Make sure the input @Date does not have a non-zero time
    SELECT @Date = CONVERT(datetime, CONVERT(varchar(255), @Date, 101))

    SELECT VC.UnitCost, ISNULL(VC.UnitFreight, 0) as UnitFreight, VC.Package_Desc1, ISNULL(VC.MSRP, 0) as MSRP
    FROM fn_VendorCostByPromo(@Item_Key, @Vendor_ID, @Store_No, @Date, 0) VC


    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


