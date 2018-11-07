CREATE PROCEDURE dbo.GetOrderItemQueueCount
	@PurchasingVendor_ID int,
	@ToSubTeam_No int 
AS 
BEGIN

	SET NOCOUNT ON
		
	DECLARE @Store_No int
	IF @PurchasingVendor_ID > 0 BEGIN SELECT @Store_No = Store_No FROM Vendor WHERE Vendor_ID = @PurchasingVendor_ID END
	
	SELECT 
		COUNT(OrderItemQueue_ID) AS ItemCount 
	FROM 
		OrderItemQueue 
	WHERE 
		Store_No = @Store_No 
		AND 
		TransferToSubTeam_No = @ToSubTeam_No
	
	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemQueueCount] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemQueueCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemQueueCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemQueueCount] TO [IRMAReportsRole]
    AS [dbo];

