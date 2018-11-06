IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetP2PUnsentInvoiceDiscrepancies]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetP2PUnsentInvoiceDiscrepancies]
GO
/****** Object:  StoredProcedure [dbo].[GetP2PUnsentInvoiceDiscrepancies]    Script Date: 06/22/2009 15:08:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[GetP2PUnsentInvoiceDiscrepancies]

AS

-- Used in the Procurement to Payment (P2P) portion of IRMA.  Called
-- from the ItemCatalogLib.Order.GetP2PUnsentInvoiceDiscrepancies procedure.	 

SET NOCOUNT ON 
	-- Set the "we're processing invoice discrepancies" flag
	UPDATE OrderHeader
	SET    InvoiceProcessingDiscrepancy = 1
	WHERE  InvoiceDiscrepancy = 1
	  AND  InvoiceDiscrepancySentDate IS NULL
	  AND  UploadedDate IS NOT NULL

SET NOCOUNT OFF  

	-- Retrieve the records for processing	 
	SELECT DISTINCT OH.Vendor_ID as VendorID,
		   Vendor.Email as VendorEmail
	FROM   OrderHeader (nolock) OH
	INNER JOIN Vendor (nolock) ON Vendor.Vendor_ID = OH.Vendor_ID
	WHERE  InvoiceProcessingDiscrepancy = 1
Go
