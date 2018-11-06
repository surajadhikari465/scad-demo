



/****** Object:  Stored Procedure dbo.StoreOpsVendorExport    Script Date: 12/20/2005 9:47:04 AM ******/
CREATE PROCEDURE [dbo].[StoreOpsVendorExport]

AS

SELECT MAX(Vendor.CompanyName) AS VendorName, 
       LTRIM(RTRIM(Vendor.PS_Vendor_ID)) As PS_Vendor_ID, 
       Vendor.PS_Export_Vendor_ID,
	   Vendor.Vendor_Key,
	   PSV.Old_PS_Vendor_ID
FROM Vendor 
INNER JOIN
    (SELECT LTRIM(RTRIM(PS_Vendor_ID)) As PS_Vendor_ID, Old_PS_Vendor_ID
     FROM Vendor
     INNER JOIN
		 tmpVendExport T
         ON T.Vendor_ID = Vendor.Vendor_ID) PSV
    ON LTRIM(RTRIM(Vendor.PS_Vendor_ID)) = PSV.PS_Vendor_ID 

GROUP BY Vendor.PS_Vendor_ID, Vendor.PS_Export_Vendor_ID, Vendor.Vendor_Key, PSV.Old_PS_Vendor_ID



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOpsVendorExport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOpsVendorExport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOpsVendorExport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOpsVendorExport] TO [IRMAReportsRole]
    AS [dbo];

