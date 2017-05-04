CREATE PROCEDURE [dbo].[GetOrderItemInfoPrevious]
@OrderItem_ID int,
@OrderHeader_ID int
AS 

BEGIN
    SET NOCOUNT ON
    
    -- UPDATED FOR V3 RELEASE TO CALL GetOrderItemInfo, MAKING THE MAINTENANCE EASIER BECAUSE
    -- THE UPDATES ONLY HAVE TO BE MADE IN ONE PLACE INSTEAD OF FOUR
    SELECT @OrderItem_ID = MAX(OrderItem_ID) 
    FROM OrderItem (NOLOCK) 
    WHERE OrderItem_ID < @OrderItem_ID  AND OrderHeader_ID = @OrderHeader_ID
    
	EXEC dbo.GetOrderItemInfo @OrderItem_ID, @OrderHeader_ID
    
    SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemInfoPrevious] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemInfoPrevious] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemInfoPrevious] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemInfoPrevious] TO [IRMAReportsRole]
    AS [dbo];

