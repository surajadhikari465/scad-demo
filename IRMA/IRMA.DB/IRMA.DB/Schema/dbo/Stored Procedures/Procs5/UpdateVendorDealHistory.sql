CREATE PROCEDURE dbo.UpdateVendorDealHistory
    @VendorDealHistoryID int,
    @CaseQty int,
    @CaseAmt smallmoney,
    @StartDate smalldatetime,
    @EndDate smalldatetime,
    @NotStackable bit    
AS

BEGIN
    SET NOCOUNT ON

	--UPDATE ONLY THE RECORD FOR THE @VendorDealHistoryID PASSED IN;
	--A TRIGGER ON THE VendorDealHistory TABLE WILL UPDATE ALL ASSOCIATED RECORDS TO THIS @VendorDealHistoryID
	UPDATE VendorDealHistory   
    	SET CaseQty = @CaseQty, 			 
			CaseAmt = @CaseAmt, 
			StartDate = @StartDate,
			EndDate = @EndDate,
			NotStackable = @NotStackable			
	WHERE VendorDealHistoryID = @VendorDealHistoryID
		    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVendorDealHistory] TO [IRMAClientRole]
    AS [dbo];

