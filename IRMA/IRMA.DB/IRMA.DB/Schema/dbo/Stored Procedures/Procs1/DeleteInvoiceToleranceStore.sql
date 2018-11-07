CREATE PROCEDURE dbo.DeleteInvoiceToleranceStore

        (
        @Store_No int
        )

AS
BEGIN
        SET NOCOUNT ON

/* -- Description:
   -- This procedure deletes records in the Store Tolerance table
   --
   -- Modification History:
   -- Date        Init Comment
   -- 11/11/2009  AZ  Creation Date
*/

        DELETE    InvoiceMatchingTolerance_StoreOverride
        WHERE     (Store_No = @Store_No)
        SET NOCOUNT Off
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteInvoiceToleranceStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteInvoiceToleranceStore] TO [IRMAClientRole]
    AS [dbo];

