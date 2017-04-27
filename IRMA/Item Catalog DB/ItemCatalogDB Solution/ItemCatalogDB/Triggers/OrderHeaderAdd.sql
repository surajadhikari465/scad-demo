 if object_id ('OrderHeaderAdd','TR') IS NOT NULL
	DROP TRIGGER OrderHeaderAdd
GO

CREATE TRIGGER OrderHeaderAdd
ON [dbo].[OrderHeader]
AFTER INSERT
AS

/*
	This trigger will populate Orderheader.PayByAgreedCost with the correct value for every order that is created.
	Added for bug 10586
*/

BEGIN
	UPDATE 
		OrderHeader	
	SET 
		PayByAgreedCost = dbo.fn_IsPayByAgreedCostStoreVendor(vs.Store_No, oh.Vendor_ID, GETDATE())
	FROM
		OrderHeader		oh
		JOIN Vendor		vs	ON	oh.PurchaseLocation_ID	= vs.Vendor_ID
		JOIN Inserted	i	ON	oh.OrderHeader_ID		= i.OrderHeader_ID

END

GO

	