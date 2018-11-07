if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_GetVendorPack]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_GetVendorPack]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE FUNCTION [dbo].[fn_GetVendorPack]
(
	@identifier varchar(13)
	,@vendor_ID int
)
RETURNS decimal(9,4)
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/20/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.  Reformatted.
										Changed variable from @CurrDate to @costDate.
										Removed unnecessary join to StoreItemVendor.
										Removed outer grouping/dataset named "CurrentCosts" and extra value being selected there (last SELECT before return).
*/
BEGIN
	DECLARE
		@CurrentVendorPackage_Desc1 decimal(10,4)
		,@costDate datetime
		,@Item_key int
		,@Store_No Int
    
	-- Build target cost date that includes any lead-time for the vendor (W/O TIME).
	SELECT @costDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE() + dbo.fn_GetLeadTimeDays(@vendor_ID), 101))

    select @Item_key=item_key from itemidentifier where identifiertype='S' and identifier=@identifier

	-- NOTE: This means this FN will only work for internal vendors.  Normal/external vendors don't have a store #.
    select @Store_No=Store_no from vendor where vendor_id=@vendor_ID

	--SELECTS THE CURRENT Package_Desc1 FROM VendorCostHistory FOR THE PRIMARY VENDOR
	SELECT TOP 1
		@CurrentVendorPackage_Desc1 = Package_Desc1
	FROM
		VendorCostHistory VCH (nolock)
	WHERE
		VCH.StoreItemVendorId = dbo.fn_GetStoreItemVendorID(@Item_Key, @Store_No)
		AND StartDate <= @costDate
		AND EndDate >= @costDate
	ORDER BY
		VendorCostHistoryID DESC

	RETURN @CurrentVendorPackage_Desc1
END 
GO
