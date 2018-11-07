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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorCost] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorCost] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorCost] TO [IRMAAVCIRole]
    AS [dbo];

