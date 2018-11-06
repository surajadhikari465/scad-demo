CREATE PROCEDURE dbo.GetRegionCustomersAsDC
AS 
BEGIN

    SET NOCOUNT ON
    
    SELECT DISTINCT
		Vendor_ID, 
		Vendor_Key, 
		CompanyName,
		S.Mega_Store, 
		S.WFM_Store, 
		S.Distribution_Center, 
		S.Manufacturer
    FROM 
		Vendor WITH (NOLOCK) 
	INNER JOIN Store S  WITH (NOLOCK)
		ON Vendor.Store_No = S.Store_No
    WHERE 
    	dbo.fn_GetCustomerType(S.Store_No, S.Internal, S.BusinessUnit_ID) = 3 AND S.Distribution_Center = 1
    ORDER BY 
		Vendor.CompanyName
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegionCustomersAsDC] TO [IRMAClientRole]
    AS [dbo];

