CREATE PROCEDURE dbo.InsertVendor
    @CompanyName varchar(50),
    @PS_Vendor_ID varchar(10),
    @PS_Export_Vendor_ID varchar(10),
    @PS_Address_Sequence varchar(2),
    @VendorKey varchar(10)
AS 
BEGIN
    SET NOCOUNT ON
    
    INSERT INTO Vendor (CompanyName, PS_Vendor_ID, PS_Export_Vendor_ID, PS_Address_Sequence, AddVendor, Vendor_Key)
    VALUES (@CompanyName, @PS_Vendor_ID, @PS_Export_Vendor_ID, @PS_Address_Sequence, 1, @VendorKey)
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendor] TO [IRMASLIMRole]
    AS [dbo];

