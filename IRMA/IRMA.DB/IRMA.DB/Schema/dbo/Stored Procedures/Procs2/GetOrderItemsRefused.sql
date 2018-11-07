CREATE PROCEDURE [dbo].[GetOrderItemsRefused]
    @OrderHeader_ID		int

AS

-- *************************************************************************************************
-- Procedure: GetOrderItemsRefused
--    Author: Faisal Ahmed
--      Date: 03/05/2013
--
-- Description: This procedure returns the list of refused order items
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/05/2013	FA		8325	Initial Code
-- 03/28/2013	FA		8325	Added eInvoice_Id column to track entries from eInvoice Exceptions
-- *************************************************************************************************

BEGIN
    SET NOCOUNT ON

    SELECT 
		oir.OrderItemRefusedID,
		oir.Identifier,
		ISNULL(iv.Item_ID, ISNULL(oir.VendorItemNumber, '')) as VendorItemNumber,
		oir.[Description],
		oir.Unit,
		oi.QuantityOrdered,
		oi.QuantityReceived,
		oi.eInvoiceQuantity,
		oi.InvoiceCost as eInvoiceCost,
		oir.InvoiceQuantity,
		oir.InvoiceCost,
		oir.RefusedQuantity,
		oir.DiscrepancyCodeID,
		oir.UserAddedEntry,
		CASE WHEN oir.eInvoice_Id > 0 THEN 1 ELSE 0 END AS eInvoiceItem,
		CASE WHEN (oir.eInvoice_Id > 0 AND oir.OrderItem_ID IS NULL) THEN 1 ELSE 0 END AS eInvoiceException
 
    FROM
		OrderItemRefused			(nolock) oir
		INNER JOIN	OrderHeader		(nolock) oh		ON	oir.OrderHeader_ID	= oh.OrderHeader_ID 
		LEFT JOIN	OrderItem		(nolock) oi     ON	oi.OrderItem_ID		= oir.OrderItem_ID	
		LEFT JOIN	Item			(nolock) i		ON	oi.Item_Key			= i.Item_Key  
		LEFT JOIN	EInvoicing_Item	(nolock) eii	ON	oh.eInvoice_id		= eii.eInvoice_id
													AND i.Item_Key			= eii.Item_Key
		LEFT  JOIN ItemVendor		(nolock) iv		ON	i.Item_Key			= iv.Item_Key
													AND oh.Vendor_Id		= iv.Vendor_Id
    WHERE 
		oir.OrderHeader_ID = @OrderHeader_ID

	ORDER BY 
		oir.eInvoice_Id DESC, UserAddedEntry ASC 

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemsRefused] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemsRefused] TO [IRMAReportsRole]
    AS [dbo];

