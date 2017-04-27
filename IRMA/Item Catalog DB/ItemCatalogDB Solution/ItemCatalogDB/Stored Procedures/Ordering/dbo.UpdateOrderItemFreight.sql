IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateOrderItemFreight')
	DROP  Procedure  dbo.UpdateOrderItemFreight
GO

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

