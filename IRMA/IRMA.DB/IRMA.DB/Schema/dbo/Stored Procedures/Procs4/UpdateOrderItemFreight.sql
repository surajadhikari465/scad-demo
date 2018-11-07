CREATE PROCEDURE dbo.UpdateOrderItemFreight
    @OrderHeader_ID int,
    @AvgFreight money	
	/*

	EXEC dbo.UpdateOrderItemFreight 67, 6.0

	*/
AS
BEGIN
    SET NOCOUNT ON
	UPDATE OrderItem 
	SET Freight = @AvgFreight,
		LineItemFreight = (@AvgFreight * QuantityOrdered)
	WHERE OrderHeader_ID = @OrderHeader_ID
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemFreight] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemFreight] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemFreight] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemFreight] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemFreight] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemFreight] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemFreight] TO [IRMAPromoRole]
    AS [dbo];

