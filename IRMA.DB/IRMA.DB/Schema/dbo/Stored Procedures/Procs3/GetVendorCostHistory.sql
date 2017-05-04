CREATE PROCEDURE dbo.GetVendorCostHistory
    @VendorCostHistoryID int
AS

BEGIN
    SET NOCOUNT ON

    SELECT Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate, Promotional, FromVendor, MSRP
    FROM VendorCostHistory (rowlock)
    WHERE VendorCostHistoryID = @VendorCostHistoryID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorCostHistory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorCostHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorCostHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorCostHistory] TO [IRMAReportsRole]
    AS [dbo];

