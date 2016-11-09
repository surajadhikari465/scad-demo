CREATE PROCEDURE dbo.GetStores 
AS 
   -- **************************************************************************
   -- Procedure: GetStores
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 11/06/2009  BBB	update existing SP to specifically declare table source 
   --					for BusinessUnit_ID column to prevent ambiguity between
   --					Store and Vendor table
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON
    
    SELECT Store_Name,
		StoreAbbr,
		Store.Store_No,
		Mega_Store,
		WFM_Store,
		Zone.Zone_ID,
		Vendor.State,
		dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) as CustomerType, -- 3 = Regional
		POSSystemTypes.POSSystemId,
		POSSystemTypes.POSSystemType,
		SRM.Region_Code, 
		Store.BusinessUnit_Id
    FROM Store (NOLOCK) 
	INNER JOIN 
		Zone (NOLOCK) 
		ON Store.Zone_Id = Zone.Zone_Id
    LEFT JOIN 
		Vendor (nolock) 
		ON Store.Store_No = Vendor.Store_No    
	LEFT JOIN
		POSSystemTypes (nolock)
		ON POSSystemTypes.POSSystemId = Store.POSSystemId
	LEFT JOIN 
		StoreRegionMapping SRM (nolock)
		ON store.store_no = SRM.store_No
    WHERE (Mega_Store = 1 OR WFM_Store = 1)
		AND dbo.fn_GetCustomerType(Store.Store_No, Internal, Store.BusinessUnit_ID) = 3 -- Regional    
    ORDER BY Mega_Store DESC, WFM_Store DESC, Store_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStores] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStores] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStores] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStores] TO [IRMARSTRole]
    AS [dbo];

