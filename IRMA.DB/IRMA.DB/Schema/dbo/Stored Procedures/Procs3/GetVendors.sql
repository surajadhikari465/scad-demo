CREATE PROCEDURE dbo.GetVendors
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Vendor.Vendor_ID, CompanyName, PS_Vendor_ID
    FROM Vendor (NoLock)
    WHERE PS_Vendor_Id IS NOT NULL OR WFM = 1 OR Store_No IN 
    	(SELECT Store_No FROM Store (NoLock) WHERE Mega_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1 OR WFM_Store = 1)
    ORDER BY CompanyName
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendors] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendors] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendors] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendors] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendors] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendors] TO [IRMASLIMRole]
    AS [dbo];

