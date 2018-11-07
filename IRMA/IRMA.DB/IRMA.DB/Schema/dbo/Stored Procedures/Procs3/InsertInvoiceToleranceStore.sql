CREATE PROCEDURE dbo.InsertInvoiceToleranceStore

        (
        @Store_No int,
        @Vendor_Tolerance Decimal(5,2),
        @Vendor_Tolerance_Amount smallmoney,
        @User_ID int
        )

AS
BEGIN
        SET NOCOUNT ON

         /* -- Description:
   -- This procedure inserts a store override setting
   --
   -- Modification History:
   -- Date        Init Comment
   -- 11/11/2009  AZ  Creation Date
    */


INSERT INTO InvoiceMatchingTolerance_StoreOverride
                                                ([Store_No], [Vendor_Tolerance], [Vendor_Tolerance_Amount], [User_ID])
VALUES                                  (@Store_No,@Vendor_Tolerance,@Vendor_Tolerance_Amount,@User_ID)

        SET NOCOUNT Off
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertInvoiceToleranceStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertInvoiceToleranceStore] TO [IRMAClientRole]
    AS [dbo];

