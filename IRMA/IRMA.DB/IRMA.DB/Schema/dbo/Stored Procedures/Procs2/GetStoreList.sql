CREATE PROCEDURE dbo.GetStoreList 
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT Vendor.Vendor_ID,
           Store.Store_No,
	       Store_Name
    FROM Store (NOLOCK)
    LEFT JOIN 
		Vendor (nolock) 
		ON Store.Store_No = Vendor.Store_No
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreList] TO [IRMAReportsRole]
    AS [dbo];

