CREATE PROCEDURE dbo.Replenishment_POSPush_UpdateVendorAddsProcessed
    @Vendor_ID int    
AS

BEGIN
    UPDATE Vendor 
    SET AddVendor=0
    WHERE Vendor_ID = @Vendor_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdateVendorAddsProcessed] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdateVendorAddsProcessed] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdateVendorAddsProcessed] TO [IRMASchedJobsRole]
    AS [dbo];

