CREATE PROCEDURE dbo.ReceiveOrderItem2
    @OrderItem_ID int,
    @DateReceived DateTime, 
    @User_ID int
AS

BEGIN
    SET NOCOUNT ON

    EXEC ReceiveOrderItem @OrderItem_ID,
                          @DateReceived,
                          NULL,
                          NULL,
                          @User_ID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem2] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem2] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem2] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem2] TO [IRMAReportsRole]
    AS [dbo];

