CREATE PROCEDURE dbo.GetVendorElectronic_Transfer 
@Vendor_ID int
AS 

SELECT Electronic_Transfer
FROM Vendor
WHERE Vendor_ID = @Vendor_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorElectronic_Transfer] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorElectronic_Transfer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorElectronic_Transfer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorElectronic_Transfer] TO [IRMAReportsRole]
    AS [dbo];

