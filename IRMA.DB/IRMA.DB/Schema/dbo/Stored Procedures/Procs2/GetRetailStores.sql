CREATE PROCEDURE dbo.GetRetailStores 
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT DISTINCT Zone.Zone_id
		,Zone_Name
		,Store.Store_No
		,Store.StoreAbbr
		,Store_Name
		,Mega_Store
		,WFM_Store
		,ISNULL(State, '') As State
	    ,dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) as CustomerType -- 3 = Regional
	    ,BusinessUnit_ID
		,fn_InstanceDataValue('GlobalPriceManagement', Store_No) AS IsGpmStore
    FROM Zone (nolock)
    INNER JOIN 
        Store (nolock)
        ON Zone.Zone_Id = Store.Zone_Id
    LEFT JOIN
        Vendor (nolock)
        ON Store.Store_No = Vendor.Store_No
    WHERE Mega_Store = 1 OR WFM_Store = 1
    ORDER BY Store_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStores] TO [IRMAReportsRole]
    AS [dbo];

