IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetCurrentCost]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetCurrentCost]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [dbo].[fn_GetCurrentCost]
(
	@ItemKey int
	,@Store_No int
)
returns smallmoney 
as
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	10/09/2006	?				?		Scripted/created.
	01/18/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.
										Changed variable from @NOW to @costDate.
										Pulled in logic from fn_GetStoreItemVendorID(), which was being used here, so that the vendor ID, needed to determine lead-time
										could be pulled at the same time we retrieve the SIV ID.
*/
begin
	declare
		@CURRENTCOST smallmoney
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
		siv.item_key = @ItemKey
		and siv.store_no = @Store_No
		and siv.primaryvendor = 1

	-- Build target cost date that includes any lead-time for the vendor.
	select @costDate = (getdate() + dbo.fn_GetLeadTimeDays(@vendorID))

	select top 1
		@CURRENTCOST = unitcost
	from 
		vendorcosthistory vch (nolock)
	where
		vch.storeitemvendorid = @sivID
		and startdate <= @costDate
		and enddate >= @costDate
	order by
		VendorCostHistoryID DESC

	-------------------------------------------------------------------

	return @CURRENTCOST
end
GO
