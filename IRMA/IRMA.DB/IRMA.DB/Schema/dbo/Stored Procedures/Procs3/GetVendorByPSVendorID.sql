CREATE PROCEDURE dbo.GetVendorByPSVendorID
    @PS_Vendor_ID varchar(255),
    @Vendor_ID bigint OUTPUT
AS

BEGIN
    SET NOCOUNT ON

    SELECT @Vendor_ID = Vendor_ID FROM Vendor WHERE PS_Vendor_ID = cast(@PS_Vendor_ID as int)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorByPSVendorID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorByPSVendorID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorByPSVendorID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorByPSVendorID] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorByPSVendorID] TO [IRMAExcelRole]
    AS [dbo];

