CREATE PROCEDURE dbo.DeleteVendorDealHistory
    @VendorDealHistoryID int
AS

BEGIN
    SET NOCOUNT ON
    
    DELETE VendorDealHistory 		
    FROM VendorDealHistory
	WHERE VendorDealHistoryID = @VendorDealHistoryID			
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteVendorDealHistory] TO [IRMAClientRole]
    AS [dbo];

