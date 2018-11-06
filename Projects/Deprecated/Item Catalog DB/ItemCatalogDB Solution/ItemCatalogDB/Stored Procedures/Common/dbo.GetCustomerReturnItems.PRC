IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'GetCustomerReturnItems' 
	   AND 	  type = 'P')
    DROP PROCEDURE GetCustomerReturnItems
GO

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

