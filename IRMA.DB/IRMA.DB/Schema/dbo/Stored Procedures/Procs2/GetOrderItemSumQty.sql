CREATE PROCEDURE dbo.GetOrderItemSumQty 
    (@OrderHeader_ID int)
	/*

	Written by Rick Kelleher, Dec 2007
	Used for updating 3rd Party Freight values 

	EXEC dbo.GetOrderItemSumQty 67

	grant exec on dbo.GetOrderItemSumQty to IRMAClientRole
	grant exec on dbo.GetOrderItemSumQty to IRMAReportsRole
	grant exec on dbo.GetOrderItemSumQty to public

	*/
AS
BEGIN
    SET NOCOUNT ON
	
	select sum(QuantityOrdered) as SumQtyOrdered,
	       sum(QuantityReceived) as SumQtyReceived
	FROM OrderItem (nolock)
	where OrderHeader_ID =  @OrderHeader_ID
    
	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSumQty] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSumQty] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSumQty] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSumQty] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSumQty] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSumQty] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSumQty] TO [IRMAPromoRole]
    AS [dbo];

