CREATE PROCEDURE dbo.GetIsPayAgreedCostVendorAllStores
    @Vendor_ID int
    
	/*
	Created by Tom Lux
	2009.06.29
	IRMA v3.5
	
	EXEC dbo.[GetIsPayAgreedCostVendorAllStores] 1534 -- FL, UNFI
	*/

AS
BEGIN
    SET NOCOUNT ON

	select 
		[Index] = row_number() over(order by s.store_no) - 1 -- Turning into zero-based index for use in ultra-grid and ultra-datasource controls.
		,Store_No = s.Store_No
		,Store_Name = s.Store_Name
		,Vendor_ID = @vendor_id
		,IsPayAgreedCost = case when poc.store_no is null then 0 else 1 end
		,EffectiveDate = poc.beginDate
	from
		store s
		left join payorderedcost poc
			on s.store_no = poc.store_no and poc.vendor_id = @vendor_id
	where
		(mega_store = 1 or wfm_store = 1 or distribution_center = 1 or manufacturer = 1)
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIsPayAgreedCostVendorAllStores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIsPayAgreedCostVendorAllStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIsPayAgreedCostVendorAllStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIsPayAgreedCostVendorAllStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIsPayAgreedCostVendorAllStores] TO [IRMAReportsRole]
    AS [dbo];

