CREATE PROCEDURE [dbo].[DeleteVendorCostHistory]
    @VendorCostHistoryID int
AS

BEGIN
    SET NOCOUNT ON
	
	Update OrderItem Set VendorCostHistoryID = Null Where VendorCostHistoryID = @VendorCostHistoryID
    DELETE VendorCostHistory FROM VendorCostHistory (rowlock) WHERE VendorCostHistoryID = @VendorCostHistoryID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteVendorCostHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteVendorCostHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteVendorCostHistory] TO [IRMAReportsRole]
    AS [dbo];

