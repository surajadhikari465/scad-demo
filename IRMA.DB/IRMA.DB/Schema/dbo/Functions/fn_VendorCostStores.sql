CREATE FUNCTION dbo.fn_VendorCostStores 
	(@Item_Key int, @Vendor_ID int, @Date smalldatetime)
RETURNS TABLE
AS
RETURN (SELECT Store_No
			,VendorCostHistoryID
			,UnitCost
			,UnitFreight
			,Package_Desc1
			,MSRP
			,StartDate
			,EndDate
			,Promotional
			,FromVendor
			,NetDiscount
			,NetCost
			,CostUnit_ID
			,FreightUnit_ID
			,Currency
			,InsertDate
        FROM
            fn_VendorCostStoresValidation(
					@Item_Key,
					@Vendor_ID,
					@Date,
					
				-- pass in null for @EndDate and
				-- -1 for @VendorDealHistory_ID since we are not
				-- calling to validate a user updated vendor deal   
				NULL,
				-1,
				0 -- @NotStackable can only be 1 when validating a nonstackable deal
			)
       )
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostStores] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostStores] TO [IRMAAVCIRole]
    AS [dbo];

