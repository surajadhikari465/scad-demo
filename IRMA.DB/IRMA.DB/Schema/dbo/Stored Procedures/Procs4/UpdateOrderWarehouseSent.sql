CREATE PROCEDURE dbo.UpdateOrderWarehouseSent
    @OrderHeader_ID int
AS

BEGIN
    SET NOCOUNT ON

    UPDATE OrderHeader
    SET WarehouseSentDate = GETDATE()
    WHERE OrderHeader_ID = @OrderHeader_ID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderWarehouseSent] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderWarehouseSent] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderWarehouseSent] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderWarehouseSent] TO [IRMAReportsRole]
    AS [dbo];

