CREATE PROCEDURE dbo.CountReceivedOrderItems

@OrderHeader_ID int 

AS 
-- ****************************************************************************************************************
-- Procedure: CountReceivedOrderItems()
--    Author: Faisal Ahmed
--      Date: 02/01/2013
--
-- Description:
-- Returns the count of the number of items on an order that does not have a zero or null received quantity.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 02/01/2013	FA		9805	Initial Version
-- ****************************************************************************************************************

SELECT 
	COUNT(*) AS ReceivedOrderItemCount 
FROM 
	OrderItem 
WHERE 
	OrderHeader_ID = @OrderHeader_ID AND QuantityReceived IS NOT NULL AND QuantityReceived <> 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountReceivedOrderItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountReceivedOrderItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountReceivedOrderItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountReceivedOrderItems] TO [IRMAReportsRole]
    AS [dbo];

