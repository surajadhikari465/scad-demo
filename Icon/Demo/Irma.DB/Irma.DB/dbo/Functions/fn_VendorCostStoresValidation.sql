CREATE FUNCTION [dbo].[fn_VendorCostStoresValidation] 
(
	@Item_Key int,
	@Vendor_ID int,
	@StartOrCurrentDate smalldatetime,
	
	-- The following params are added to support the use of this
	-- function for validating the creation or the updating
	-- of existing deals.
	@EndDate smalldatetime,       -- null if not for validation
	
	@VendorDealHistory_ID int,    -- null or <= 0 if not for validation
								  -- or if validating a new deal
								  
	@NotStackable bit			  -- 1 only if for validating a new or update
	                              -- to an existing nonstackable deal
)
RETURNS @CostData TABLE (
	Store_No int
	,VendorCostHistoryID int
	,UnitCost smallmoney
	,UnitFreight smallmoney
	,Package_Desc1 decimal(9, 4)
	,MSRP smallmoney
	,StartDate smalldatetime
	,EndDate smalldatetime
	,Promotional bit
	,FromVendor bit
	,NetDiscount smallmoney
	,NetCost smallmoney
	,CostUnit_ID int
	,FreightUnit_ID int
	,Currency int
	,InsertDate datetime
)
AS
	----------------------------------------------------------------------
	-- This function is "overloaded" by the fn_VendorCostStoresValidation
	-- function, which passes in a -1 for the @VendorDealHistory_ID and
	-- a null for the @EndDate.
	--
	-- The reason for this overloading is to allow the calculation
	-- of an item's net discount for a given vendor and store
	-- to be done for the following:
	--
	--  * for the purpose of the fn_VendorCostStores overload of
	--    getting the elemsnts of cost for a given date
	--
	--  * for validating that a new or update of an existing deal doesn't
	--    result in a zero or negative net cost for the provided date
	--    range. Note: @VendorDealHistory_ID will be a valid ID value
	--    and @EndDate non null when the validation is done for an
    --    existing deal update. @NotStackable will only be 1 if this
    --    function is called for validation and the deal, new or existing,
    --    being validated is nonstackable.
	----------------------------------------------------------------------

/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	12/08/2010	DBS				13636	Removed the stack which had most recent promo cost overriding most recent cost. (manually merged by Tom)
	01/19/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.  Reformatted.
										Had to update RETURNS clause after adding line in body of FN that updates the @Date variable (syntax requirement).
										So, we insert into table var and then just return.
	03/18/2011 Faisal Ahmed     1681    @EndDate should be passed to fn_ItemNetDiscountValidation as NULL if not for validation. 
*/
BEGIN

	-- Update target cost date that includes any lead-time for the vendor.
	SELECT @StartOrCurrentDate = @StartOrCurrentDate + dbo.fn_GetLeadTimeDays(@Vendor_ID)

	INSERT into @CostData
		SELECT
			MVCH.Store_No
			,VendorCostHistoryID
			,UnitCost
			,UnitFreight = ISNULL(UnitFreight, 0)
			,VCH.Package_Desc1
			,MSRP
			,StartDate
			,EndDate
			,Promotional
			,FromVendor
			,NetDiscount = ISNULL(dbo.fn_ItemNetDiscountValidation(MVCH.Store_No, @Item_Key, @Vendor_ID, ISNULL(UnitCost, 0), @StartOrCurrentDate, @EndDate, @VendorDealHistory_ID, @NotStackable), 0)
			--NET COST = REG COST - NET DISCOUNT + Freight
			,NetCost =
				ISNULL(UnitCost, 0) 
				- ISNULL(dbo.fn_ItemNetDiscountValidation(MVCH.Store_No, @Item_Key, @Vendor_ID, ISNULL(UnitCost, 0), @StartOrCurrentDate, @EndDate, @VendorDealHistory_ID, @NotStackable), 0)
				+ ISNULL(UnitFreight, 0)
			,CostUnit_ID
			,FreightUnit_ID
			,Currency
			,vch.InsertDate
		FROM
			VendorCostHistory VCH (nolock)
			INNER JOIN
			(
				SELECT
					Store_No
					,MaxVCHID = (
						SELECT TOP 1
							VendorCostHistoryID
						FROM
							VendorCostHistory (nolock)
							INNER JOIN StoreItemVendor (nolock)
								ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
						WHERE
							Store_No = SIV.Store_No AND Item_Key = @Item_Key AND Vendor_ID = @Vendor_ID
							AND ((@StartOrCurrentDate >= StartDate) AND (ISNULL(@EndDate, @StartOrCurrentDate) <= EndDate))
							AND @StartOrCurrentDate < ISNULL(DeleteDate, DATEADD(day, 1, @StartOrCurrentDate))
						ORDER BY
							VendorCostHistoryID DESC
					) -- End of MaxVCHID.
				FROM
					StoreItemVendor SIV (nolock)
				WHERE
					Item_Key = @Item_Key 
					AND Vendor_ID = @Vendor_ID
				GROUP BY
					Store_No
			) MVCH
				ON MVCH.MaxVCHID = VCH.VendorCostHistoryID

	return
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostStoresValidation] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostStoresValidation] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostStoresValidation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostStoresValidation] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostStoresValidation] TO [IRMAReportsRole]
    AS [dbo];

