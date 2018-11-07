CREATE PROCEDURE dbo.CheckForDuplicateVendors 
    @Vendor_ID int,
    @CompanyName varchar(255),
    @PS_Vendor_ID varchar(255),
    @PS_Address_Sequence varchar(255)
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT COUNT(*) AS VendorCount 
    FROM Vendor
    WHERE 
		(
			RTRIM(CompanyName) = RTRIM(@CompanyName)
		OR 
			dbo.fn_TrimLeadingZeros(PS_Vendor_ID) = dbo.fn_TrimLeadingZeros(@PS_Vendor_ID)
		)		                  
    AND Vendor_Id <> @Vendor_ID
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateVendors] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateVendors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateVendors] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateVendors] TO [IRMAReportsRole]
    AS [dbo];

