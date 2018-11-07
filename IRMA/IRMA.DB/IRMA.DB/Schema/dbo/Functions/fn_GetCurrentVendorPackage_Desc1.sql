CREATE FUNCTION [dbo].[fn_GetCurrentVendorPackage_Desc1]
(
	@Item_Key int
	,@Store_No int
)
RETURNS decimal(9,4)
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	02/18/2010	?				?		Date last scripted.
	01/19/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.  Reformatted.
										Removed dynamic SQL being used to define/create the function.
										Changed variable from @CurrDate to @costDate.
										Pulled in logic from fn_GetStoreItemVendorID(), which was being used here, so that the vendor ID, needed to determine lead-time,
										could be pulled at the same time we retrieve the SIV ID.
										Removed unnecessary join to StoreItemVendor.
										Removed outer grouping/dataset named "CurrentCosts" and extra value being selected there (last SELECT before return).
										Moved security grants at bottom of this script to dedicated security script (most already existed there).
*/
BEGIN
	DECLARE
		@CurrentVendorPackage_Desc1 decimal(10,4)
		,@costDate datetime
		,@sivID int
		,@vendorID int

	-- Get Store-Item-Vendor ID so we can pull VCH entry and get vendor ID so we can determine the lead-time.
	select top 1
		@sivID = siv.storeitemvendorid
		,@vendorID = siv.vendor_id
	from
		storeitemvendor siv (nolock)
	where
		siv.item_key = @Item_Key
		and siv.store_no = @Store_No
		and siv.primaryvendor = 1

	-- Build target cost date that includes any lead-time for the vendor (W/O TIME).
	SELECT @costDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE() + dbo.fn_GetLeadTimeDays(@vendorID), 101))

	--SELECTS THE CURRENT Package_Desc1 FROM VendorCostHistory FOR THE PRIMARY VENDOR
	SELECT TOP 1
		@CurrentVendorPackage_Desc1 = Package_Desc1
	FROM
		VendorCostHistory VCH (nolock)
	WHERE
		VCH.StoreItemVendorId = @sivID
		AND StartDate <= @costDate
		AND EndDate >= @costDate
	ORDER BY
		VendorCostHistoryID DESC

	RETURN @CurrentVendorPackage_Desc1
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentVendorPackage_Desc1] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentVendorPackage_Desc1] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentVendorPackage_Desc1] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentVendorPackage_Desc1] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentVendorPackage_Desc1] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentVendorPackage_Desc1] TO [IRMASLIMRole]
    AS [dbo];

