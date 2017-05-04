CREATE FUNCTION [dbo].[fn_GetNetCost]
(
	@Item_Key int
	,@Store_No int
	,@VendorId int
	,@StartDate datetime
)
RETURNS @Results TABLE
(
	Item_Key int,
	CostUnit_Id int,
	NetCost money
)
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/20/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.  Reformatted.
										Changed variable from @CurrDate to @costDate.
										Removed unused table var @temp.
										Removed unnecessary join to StoreItemVendor.
										Removed outer grouping/dataset named "CurrentCosts".
*/
BEGIN
	
	DECLARE
		@CurrentNetCost decimal(10,4)
		,@costDate datetime
		,@VCH_Id int
		,@SIV_ID int

	select top 1
		@SIV_ID = siv.storeitemvendorid  
	from  
		storeitemvendor siv (nolock)  
	where  
		siv.item_key = @Item_Key  
		and siv.store_no = @Store_No  
		and siv.vendor_id = @VendorId

	--GET CURRENT DATE W/O TIME
	SELECT @costDate = CONVERT(datetime, CONVERT(varchar(255), @StartDate + dbo.fn_GetLeadTimeDays(@VendorID), 101))

	--SELECTS THE CURRENT NET COST (REG COST - NET DISCOUNT + FREIGHT) FROM VendorCostHistory FOR THE PRIMARY VENDOR
	SELECT TOP 1 @VCH_Id = VCH.VendorCostHistoryid
	FROM	VendorCostHistory VCH (nolock) 
	WHERE	VCH.StoreItemVendorId = @SIV_ID
		AND StartDate <= @costDate
		AND EndDate >= @costDate
	ORDER BY VendorCostHistoryID DESC

	Insert Into @Results (Item_Key, CostUnit_Id, NetCost)
	SELECT 
			@Item_Key as Item_Key,
			CostUnit_Id,
			ISNULL(UnitCost, 0) - ISNULL(dbo.fn_ItemNetDiscount(@Store_No, @Item_Key,@VendorId, ISNULL(UnitCost, 0), @costDate),0) + ISNULL(UnitFreight, 0) AS NetCost
	FROM 
			VendorCostHistory VCH (nolock)
	WHERE   VendorCostHistoryId = @VCH_Id

	RETURN 
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetNetCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetNetCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetNetCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetNetCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetNetCost] TO [IRMAReportsRole]
    AS [dbo];

