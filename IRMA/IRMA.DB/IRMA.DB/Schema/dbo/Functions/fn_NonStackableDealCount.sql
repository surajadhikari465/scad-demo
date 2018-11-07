Create FUNCTION [dbo].[fn_NonStackableDealCount]
(
	@Store_No int,
	@Item_Key int,
	@Vendor_ID int,
	@StartOrCurrentDate smalldatetime,	
	@EndDate smalldatetime,
	@VendorDealHistory_ID int -- null or <= 0 if validating a new deal
)
RETURNS int
AS
BEGIN

	DECLARE @NotStackableCount int
	
	SET @NotStackableCount = 0
				
	SELECT @NotStackableCount = COUNT(1)
	FROM VendorDealHistory VDH (nolock)
	INNER JOIN
		StoreItemVendor SIV (nolock)
		ON SIV.StoreItemVendorID = VDH.StoreItemVendorID
	INNER JOIN
		VendorDealType VDT (nolock)
		ON VDT.VendorDealTypeID = VDH.VendorDealTypeID
	WHERE

		-- If a valid vendor deal id is passed in then we are validating an update
		-- to an existing vendor deal. This being so, we must ignore
		-- the original deal when validating.
		VDH.VendorDealHistoryID <> @VendorDealHistory_ID
		
		-- Catch any overlap including:
		--   * deal start and end dates fully within provided date range
		--   * OR either a deal start or end date fully within provided date range
		--
		-- This logic continues to work if the provided range start and end dates
		-- are equal. 
		AND
		(
			(
				StartDate <= DateAdd(dd,0, DateDiff(dd, 0, @StartOrCurrentDate)) --REMOVES TIME FROM CurrentDate
				AND EndDate >= DateAdd(dd,0, DateDiff(dd, 0, @EndDate))          --REMOVES TIME FROM CurrentDate
			)
			OR
			(
				StartDate BETWEEN @StartOrCurrentDate AND @EndDate 
				OR EndDate BETWEEN @StartOrCurrentDate AND @EndDate
			)
		)

		AND SIV.Item_Key = @Item_Key
		AND SIV.Vendor_ID = @Vendor_ID

		AND Store_No = @Store_No
		AND NotStackable = 1

	RETURN @NotStackableCount
END