CREATE Procedure dbo.UpdateOrderHeaderFreight3Party
    @OrderHeader_ID int,
    @Freight3Party money	
	/*

	Written by Rick Kelleher, Dec 2007
	Used for updating 3rd Party Freight values 

	EXEC dbo.UpdateOrderHeaderFreight3Party 67, 6.0

	grant exec on dbo.UpdateOrderHeaderFreight3Party to IRMAClientRole
	grant exec on dbo.UpdateOrderHeaderFreight3Party to IRMAReportsRole

	*/
AS
BEGIN
    SET NOCOUNT ON

	UPDATE dbo.OrderHeader
	SET Freight3Party_OrderCost = ROUND(@Freight3Party, 4)
	WHERE OrderHeader_ID = @OrderHeader_ID
    SET NOCOUNT OFF
END
 
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderHeaderFreight3Party] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderHeaderFreight3Party] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderHeaderFreight3Party] TO [IRMAReportsRole]
    AS [dbo];

