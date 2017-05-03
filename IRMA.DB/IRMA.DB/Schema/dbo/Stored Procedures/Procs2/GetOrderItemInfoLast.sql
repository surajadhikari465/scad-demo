CREATE PROCEDURE [dbo].[GetOrderItemInfoLast]
@OrderHeader_ID int
AS 

BEGIN
    SET NOCOUNT ON
    
    -- UPDATED FOR V3 RELEASE TO CALL GetOrderItemInfo, MAKING THE MAINTENANCE EASIER BECAUSE
    -- THE UPDATES ONLY HAVE TO BE MADE IN ONE PLACE INSTEAD OF FOUR
    DECLARE @OrderItem_ID int
    SELECT @OrderItem_ID = MAX(OrderItem_ID) 
    FROM dbo.OrderItem (NOLOCK) 
    WHERE OrderHeader_ID = @OrderHeader_ID
    
	EXEC dbo.GetOrderItemInfo @OrderItem_ID, @OrderHeader_ID
    
    SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemInfoLast] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemInfoLast] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemInfoLast] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemInfoLast] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemInfoLast] TO [IRMAReportsRole]
    AS [dbo];

