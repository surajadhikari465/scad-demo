CREATE PROCEDURE dbo.GetVendorDealHistoryStackableConflicts
	@VendorDealHistoryID int, --CAN BE NULL FOR NEW DEALS
	@Item_Key int,
    @Vendor_ID int,
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
	@StartDate smalldatetime,
	@EndDate smalldatetime
AS
BEGIN
    SET NOCOUNT ON

	SELECT COUNT(1) AS NumConflicts
	FROM VendorDealHistory VDH
	INNER JOIN
		StoreItemVendor SIV
		ON SIV.StoreItemVendorID = VDH.StoreItemVendorID
	INNER JOIN
		fn_Parse_List(@StoreList, @StoreListSeparator) Store
		ON Store.Key_Value = SIV.Store_No
	WHERE SIV.Item_Key = @Item_Key
		AND SIV.Vendor_ID = @Vendor_ID
		AND (@VendorDealHistoryID IS NULL 
				--DO NOT INCLUDE DATA FOR THE DEAL THAT IS BEING EDITED IN CONFLICT SEARCH
				OR (@VendorDealHistoryID IS NOT NULL AND VDH.VendorDealHistoryID <> @VendorDealHistoryID))
		AND NotStackable = 1
		AND ((StartDate <= @StartDate AND EndDate >= @StartDate AND EndDate <= @EndDate) --StartDate is between existing deal
				OR (StartDate <= @StartDate AND EndDate <= @EndDate AND @StartDate <= EndDate) --new deal is consumed by existing deal
				OR (StartDate >= @StartDate AND EndDate <= @EndDate) --new deal consumes existing deal
				OR (StartDate >= @StartDate AND StartDate <= @EndDate AND EndDate >= @EndDate)) --EndDate is between existing deal

    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorDealHistoryStackableConflicts] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorDealHistoryStackableConflicts] TO [IRMAClientRole]
    AS [dbo];

