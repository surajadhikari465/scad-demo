CREATE PROCEDURE dbo.UpdateOrderVendor
    @OrderHeader_ID int,
    @Vendor_ID int
AS

BEGIN
    SET NOCOUNT ON

    UPDATE OrderHeader
    SET Vendor_ID = @Vendor_ID
    WHERE OrderHeader_ID = @OrderHeader_ID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderVendor] TO [IRMAReportsRole]
    AS [dbo];

