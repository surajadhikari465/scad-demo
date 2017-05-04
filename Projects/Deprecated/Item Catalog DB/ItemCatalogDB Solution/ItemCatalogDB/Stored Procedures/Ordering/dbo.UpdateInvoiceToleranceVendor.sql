if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateInvoiceToleranceVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateInvoiceToleranceVendor]
GO
CREATE PROCEDURE dbo.UpdateInvoiceToleranceVendor

        (
        @Vendor_ID int,
        @Vendor_Tolerance Decimal(5,2),
        @Vendor_Tolerance_Amount smallmoney,
        @User_ID int
        )

AS
BEGIN
                SET NOCOUNT ON

/*
 -- Description:
   -- This procedure updates the Vendor Tolerance
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 11/11/2009  AZ	Creation Date
   -- 01/17/2012  BJL	TFS 4314 - Use DEFAULT value for UpdateDate & Deleted. Also, check for Deleted IS NULL.
*/


UPDATE    InvoiceMatchingTolerance_VendorOverride
SET                                     [Vendor_Tolerance] = @Vendor_Tolerance,
                                        [Vendor_Tolerance_Amount] = @Vendor_Tolerance_Amount,
                                        [User_ID] = @User_ID,
										[UpdateDate] = DEFAULT,
										[Deleted] = DEFAULT
WHERE     (Vendor_ID = @Vendor_ID) AND (Deleted IS NULL OR Deleted <> 1)

                SET NOCOUNT Off
END

GO
