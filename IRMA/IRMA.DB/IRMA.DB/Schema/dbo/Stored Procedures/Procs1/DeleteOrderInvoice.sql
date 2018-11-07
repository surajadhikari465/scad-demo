CREATE PROCEDURE dbo.DeleteOrderInvoice 
@OrderHeader_ID int
AS

DELETE 
FROM OrderInvoice
WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderInvoice] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderInvoice] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderInvoice] TO [IRMAReportsRole]
    AS [dbo];

