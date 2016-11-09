CREATE PROCEDURE [dbo].[OrderInvoice_DelOrderInvoiceSpecCharge]
	@Charge_Id	INT
AS
-- ****************************************************************************************************************
-- Procedure: OrderInvoice_DelOrderInvoiceSpecCharge
--    Author: Dave Stacey
--      Date: 2008/08/07
--
-- Description: This query deletes a special charge from an invoice.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/27	KM		3744	Added update history template; usage review;
-- ****************************************************************************************************************
BEGIN
	DELETE	dbo.OrderInvoiceCharges 
	WHERE	Charge_ID = @Charge_Id
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_DelOrderInvoiceSpecCharge] TO [IRMAClientRole]
    AS [dbo];

