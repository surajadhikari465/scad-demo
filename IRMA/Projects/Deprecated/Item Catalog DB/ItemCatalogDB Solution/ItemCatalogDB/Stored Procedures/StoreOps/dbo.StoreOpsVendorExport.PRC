/****** Object:  StoredProcedure [dbo].[StoreOpsVendorExport]    Script Date: 03/25/2009 09:18:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreOpsVendorExport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[StoreOpsVendorExport]
GO
/****** Object:  StoredProcedure [dbo].[StoreOpsVendorExport]    Script Date: 03/25/2009 09:18:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreOpsVendorExport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



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


' 
END
GO
GRANT EXECUTE ON [dbo].[StoreOpsVendorExport] TO [IRMAClientRole]
GO
GRANT EXECUTE ON [dbo].[StoreOpsVendorExport] TO [IRMAReportsRole]
GO
GRANT EXECUTE ON [dbo].[StoreOpsVendorExport] TO [IRMASchedJobsRole]
GO
GRANT EXECUTE ON [dbo].[StoreOpsVendorExport] TO [IRMASupportRole]
GO
