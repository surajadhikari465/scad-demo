create function [dbo].[fn_GetCurrentCostForVendor]
(
	@Item_Key int
	,@Store_No int
	,@Vendor_ID int
)
returns smallmoney 
as
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/18/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.
										Changed variable from @NOW to @costDate.
*/
begin
	declare
		@CURRENTCOST smallmoney
		,@costDate datetime

	-- Build target cost date that includes any lead-time for the vendor.
	select @costDate = (getdate() + dbo.fn_GetLeadTimeDays(@Vendor_ID))
  
	select top 1
		@CURRENTCOST = unitcost
	from 
		vendorcosthistory vch (nolock)
	where
		vch.storeitemvendorid =
		(
			SELECT StoreItemVendorID 
			FROM StoreItemVendor 
			WHERE
				Item_Key = @Item_Key 
				AND Store_No = @Store_No 
				AND Vendor_ID = @Vendor_ID
		)
		and startdate <= @costDate
		and enddate >= @costDate
	order by
		VendorCostHistoryID DESC

	--------------------------------------------------------------------------

	return @CURRENTCOST
end