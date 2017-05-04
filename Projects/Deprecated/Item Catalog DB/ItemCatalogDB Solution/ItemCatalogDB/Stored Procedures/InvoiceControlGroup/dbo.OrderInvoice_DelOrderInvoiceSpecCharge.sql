
/****** Object:  StoredProcedure [dbo].[OrderInvoice_DelOrderInvoiceSpecCharge]    Script Date: 08/07/2008 12:54:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderInvoice_DelOrderInvoiceSpecCharge]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrderInvoice_DelOrderInvoiceSpecCharge]
GO

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