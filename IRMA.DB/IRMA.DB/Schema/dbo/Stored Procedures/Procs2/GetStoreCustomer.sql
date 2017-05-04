CREATE PROCEDURE [dbo].[GetStoreCustomer] 
	@Store_No int,
	@Vendor_ID int -- Not needed if @Store_No has a value; otherwise, should be null
AS 
	-- **************************************************************************
	-- Procedure: GetStoreCustomer()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is called from Global.vb to load values across the code base.
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 11/17/2009	BBB		converted calls to Store/Zone/ZoneSubTeam to Left Join;
	--						added OR clause to Vendor.Store_No; reformatted for readability
	-- 04/06/2010	BBB		Removed lookups needed for GL Enhancements and leave values set 
	--						in initial query as is
	-- **************************************************************************
BEGIN
    SET NOCOUNT ON
    
	SELECT DISTINCT 
		Vendor_ID,
		CompanyName,
		Mega_Store,
		WFM_Store,
		Zone.Zone_ID,
		Vendor.State,
		dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) as CustomerType, -- 3 = Regional
		Store.Store_No
	FROM 
		Vendor					(NOLOCK)
		INNER JOIN Store		(NOLOCK) ON Store.Store_No = Vendor.Store_No
		INNER JOIN Zone			(NOLOCK) ON Store.Zone_Id = Zone.Zone_Id
		INNER JOIN ZoneSubTeam	(NOLOCK) ON ZoneSubTeam.Supplier_Store_No = ISNULL(@Store_No, (SELECT Store_No FROM Vendor WHERE Vendor_ID = @Vendor_ID))
										 AND ZoneSubTeam.Zone_ID = Store.Zone_ID
	WHERE 
		Vendor.Customer = 1 
		-- Don't return itself in the list
		AND
		Vendor.Store_No <> ISNULL(@Store_No, (SELECT Store_No FROM Vendor WHERE Vendor_ID = @Vendor_ID))
	ORDER BY
		Vendor.CompanyName
    
SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreCustomer] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreCustomer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreCustomer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreCustomer] TO [IRMAReportsRole]
    AS [dbo];

