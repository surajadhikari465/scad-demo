CREATE PROCEDURE dbo.CountOrderItems 

@OrderHeader_ID int 

AS 
-- ****************************************************************************************************************
-- Procedure: CountOrderItems()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Returns the count of the number of items on an order that have a null received quantity.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012/12/27	KM				Add update history template; Modify logic to count only order items which have
--								a received quantity of null;
-- ****************************************************************************************************************

SELECT 
	COUNT(*) AS OrderItemCount 
FROM 
	OrderItem 
WHERE 
	OrderHeader_ID = @OrderHeader_ID AND QuantityReceived IS NULL
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountOrderItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountOrderItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountOrderItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountOrderItems] TO [IRMAReportsRole]
    AS [dbo];

