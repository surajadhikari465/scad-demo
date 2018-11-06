IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UpdateP2PInvoiceDiscrepancySentDate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateP2PInvoiceDiscrepancySentDate]
GO
/****** Object:  StoredProcedure [dbo].[UpdateP2PInvoiceDiscrepancySentDate]    Script Date: 06/22/2009 15:08:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[UpdateP2PInvoiceDiscrepancySentDate]
  @Vendor_ID int

AS

-- Used in the Procurement to Payment (P2P) portion of IRMA.  Called
-- from the ItemCatalogLib.Order.UpdateP2PInvoiceDiscrepancySentDate procedure.
	  
UPDATE OrderHeader
SET    InvoiceDiscrepancySentDate = GETDATE(),
       InvoiceProcessingDiscrepancy = 0
WHERE  Vendor_ID = @Vendor_ID
  AND  InvoiceDiscrepancy = 1
  AND  InvoiceDiscrepancySentDate IS NULL
  AND  UploadedDate IS NOT NULL
  AND InvoiceProcessingDiscrepancy = 1

Go


GRANT EXECUTE ON [dbo].[UpdateP2PInvoiceDiscrepancySentDate] TO [IRMAClientRole]
GO