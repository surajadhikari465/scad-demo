if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertInvoiceToleranceVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertInvoiceToleranceVendor]
GO
CREATE PROCEDURE dbo.InsertInvoiceToleranceVendor

        (
        @Vendor_ID int,
        @Vendor_Tolerance Decimal(5,2),
        @Vendor_Tolerance_Amount smallmoney,
        @User_ID int
        )

AS
BEGIN
        SET NOCOUNT ON

        /* -- Description:
   -- This procedure inserts a Vendor Tolerance Setting
   --
   -- Modification History:
   -- Date        Init Comment
   -- 11/11/2009  AZ  Creation Date
   */

INSERT INTO InvoiceMatchingTolerance_VendorOverride
                                                ([Vendor_ID], [Vendor_Tolerance], [Vendor_Tolerance_Amount], [User_ID])
VALUES                                  (@Vendor_ID,@Vendor_Tolerance,@Vendor_Tolerance_Amount,@User_ID)

        SET NOCOUNT Off
END
GO
