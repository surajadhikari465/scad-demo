if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteInvoiceToleranceVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteInvoiceToleranceVendor]
GO
CREATE PROCEDURE dbo.DeleteInvoiceToleranceVendor

        (
        @Vendor_ID int
        )

AS
BEGIN
                SET NOCOUNT ON

/*
 -- Description:
   -- This procedure deletes records in the Vendor Tolerance table
   --
   -- Modification History:
   -- Date        Init Comment
   -- 11/11/2009  AZ  Creation Date
*/


DELETE    InvoiceMatchingTolerance_VendorOverride
WHERE     (Vendor_ID = @Vendor_ID)

                SET NOCOUNT Off
END

GO
