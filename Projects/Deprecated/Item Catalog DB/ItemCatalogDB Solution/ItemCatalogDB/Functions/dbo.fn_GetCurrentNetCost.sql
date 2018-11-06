IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetCurrentNetCost]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetCurrentNetCost]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fn_GetCurrentNetCost]
(
	@Item_Key int
	,@Store_No int
)
RETURNS decimal(10,4)

AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	02/21/2007	?				?		Scripted/created.
	01/20/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.
										Changed variable from @CurrDate to @costDate.
										Pulled in logic from fn_GetStoreItemVendorID(), which was being used here, so that the vendor ID, needed to determine lead-time,
										could be pulled at the same time we retrieve the SIV ID.
										Removed unnecessary join to StoreItemVendor.
										Removed outer grouping/dataset named "CurrentCosts" and extra value being selected there (last SELECT before return).
										Moved security grants at bottom of this script to dedicated security script (most already existed there).
   05/04/2011	Min Zhao		1685	When fn_ItemNetDiscount is called, pass @vendorID instead of @sivID (StoreItemVendorId) as one of the params, so that 
										Item Net Discoount can be calculated correctly.
   02/11/2014	Faisal Ahmed			returns 0 if the value is null
*/
BEGIN
	DECLARE
		@CurrentNetCost decimal(10,4)
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

	-- SELECTS THE CURRENT NET COST (REG COST - NET DISCOUNT + FREIGHT) FROM VendorCostHistory FOR THE PRIMARY VENDOR
	SELECT top 1
		@CurrentNetCost = (
			ISNULL(UnitCost, 0)
			-
			ISNULL(dbo.fn_ItemNetDiscount(@Store_No, @Item_Key, @vendorID, ISNULL(UnitCost, 0), @costDate), 0)
			+
			ISNULL(UnitFreight, 0)
		)
		FROM
			VendorCostHistory VCH (nolock)
		WHERE
			VCH.StoreItemVendorId = @sivID
			AND StartDate <= @costDate
			AND EndDate >= @costDate
		ORDER BY
			VendorCostHistoryID DESC

	------------------------------------------------------------------

	RETURN isnull(@CurrentNetCost, 0)
END
GO
