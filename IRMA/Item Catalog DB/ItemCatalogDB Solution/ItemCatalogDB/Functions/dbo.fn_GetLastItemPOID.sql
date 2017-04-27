IF EXISTS ( SELECT  *
            FROM    SYSOBJECTS
            WHERE   NAME = 'fn_GetLastItemPOID' ) 
    DROP FUNCTION dbo.fn_GetLastItemPOID
    GO

CREATE FUNCTION [dbo].[fn_GetLastItemPOID]
    (@Item_Key int, @StoreVendorId int)
RETURNS INT

AS 

-- **********************************************************************************************************************
-- Function: fn_GetLastItemPOID
-- Author: n/a
-- Date: n/a
--
-- Description:
-- This procedure is called from AutomaticOrderItemInfo() on the Orders.vb page to add items to an order.
--
-- Modification History:
-- Date			Init	Comment
-- 01.14.2011	DBS		TFS 12793 Effective Cost Ordering modifications
--						Modified lookup to prioritize prior vendor store's receiving of item from 
--						primary vendor first, followed by non-primary vendor on type 1 or 2 orders
-- 04.01.2011	DBS		TFS 1743, 1607 Match cost to last 'OnHand' cost instead of 'System Cost'
-- 06.29.2011	DBS		TFS 2286, Make sure the item on the last PO has been received.
-- 08.09.2011	DBS		TFS 2286, Make sure the item on the last PO is not suspended.
-- 12.19.2012   MZ		TFS 9470, Added one more criteria that the Quantity Received is greater than zero.
-- 2013/09/27	KM		TFS 13367, Rewrite the join logic to be more efficient.
-- **********************************************************************************************************************

BEGIN
	DECLARE @OrderHeaderID int 
	
    SELECT 
		@OrderHeaderID = MAX(oi.OrderHeader_ID)
                
	FROM    
		Item					(nolock) i
		INNER JOIN OrderItem	(NOLOCK) oi ON oi.item_key				= i.Item_Key
		INNER JOIN OrderHeader	(NOLOCK) oh ON oh.OrderHeader_ID		= oi.OrderHeader_ID
		
	WHERE   
		oi.Item_Key = @Item_Key
		AND oh.Return_Order = 0
        AND oh.ReceiveLocation_ID = @StoreVendorId
        AND oh.OrderType_ID <> 3
        AND oh.ApprovedDate IS NOT NULL
		AND oi.QuantityReceived > 0
        
    RETURN @OrderHeaderID
END
GO