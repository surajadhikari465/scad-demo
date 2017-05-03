if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteInvoiceToleranceStore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteInvoiceToleranceStore]
GO
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
