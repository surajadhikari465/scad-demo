CREATE PROCEDURE dbo.GetVendorLinks 
@Vendor_ID int 
AS 

SELECT Vendor_ID FROM OrderHeader WHERE Vendor_ID = @Vendor_ID 
UNION 
SELECT PurchaseLocation_ID AS Vendor_ID FROM OrderHeader WHERE PurchaseLocation_ID = @Vendor_ID 
UNION 
SELECT ReceiveLocation_ID AS Vendor_ID FROM OrderHeader WHERE ReceiveLocation_ID = @Vendor_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorLinks] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorLinks] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorLinks] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorLinks] TO [IRMAReportsRole]
    AS [dbo];

