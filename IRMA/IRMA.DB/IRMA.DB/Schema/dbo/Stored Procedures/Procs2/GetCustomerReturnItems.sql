CREATE PROCEDURE dbo.GetCustomerReturnItems 
	@ReturnID int
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT ReturnItemID,CustomerReturnItem.Item_Key, 
           Quantity, Weight, Amount, CustomerReturnItem.CustReturnReasonID, CustReturnReason
    FROM CustomerReturnItem
    INNER JOIN
        CustomerReturnReason ON CustomerReturnReason.CustReturnReasonID = CustomerReturnItem.CustReturnReasonID
    WHERE ReturnID = @ReturnID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerReturnItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerReturnItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerReturnItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerReturnItems] TO [IRMAReportsRole]
    AS [dbo];

