CREATE PROCEDURE dbo.OrderInvoice_3PartyFreightInvoice_Get
	@OrderHeader_ID		int
AS
-- ****************************************************************************************************************
-- Procedure: OrderInvoice_3PartyFreightInvoice_Get
--    Author: n/a
--      Date: n/a
--
-- Description: Get a record from the OrderInvoice_Freight3Party table.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/26	KM		3744	Added update history template; coding standards;
-- ****************************************************************************************************************
BEGIN
	SELECT 
		OrderHeader_ID,
		InvoiceNumber,
		InvoiceDate,
		InvoiceCost,
		f3p.Vendor_ID,
		V.Vendor_Key,
		V.CompanyName,
		UploadedDate,
		MatchingValidationCode,
		MatchingUser_ID,
		MatchingDate
	FROM 
		dbo.OrderInvoice_Freight3Party		(nolock)	f3p
		INNER JOIN dbo.Vendor				(nolock)	v	ON	f3p.Vendor_ID = V.Vendor_ID
	WHERE 
		f3p.OrderHeader_ID = @OrderHeader_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_3PartyFreightInvoice_Get] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_3PartyFreightInvoice_Get] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_3PartyFreightInvoice_Get] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_3PartyFreightInvoice_Get] TO [IRMAReportsRole]
    AS [dbo];

