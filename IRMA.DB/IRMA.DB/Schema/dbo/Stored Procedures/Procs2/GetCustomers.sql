CREATE PROCEDURE dbo.GetCustomers
    @Regional bit
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT Vendor_ID, Vendor_Key, CompanyName
    FROM Vendor (nolock) LEFT JOIN( 
           Store (nolock) INNER JOIN Zone (nolock) ON (Zone.Zone_Id = Store.Zone_Id)
         ) ON (Vendor.Store_No = Store.Store_No)
    WHERE 
        (Vendor.Store_No IS NOT NULL ) -- Any customer type - Regional, WFM, External - customer if Store record exists
        AND Regional = CASE WHEN @Regional = 0 THEN 0 ELSE Regional END
    ORDER BY Mega_Store DESC, WFM_Store DESC, Distribution_Center DESC, Manufacturer DESC, CompanyName
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomers] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomers] TO [IRMAReportsRole]
    AS [dbo];

