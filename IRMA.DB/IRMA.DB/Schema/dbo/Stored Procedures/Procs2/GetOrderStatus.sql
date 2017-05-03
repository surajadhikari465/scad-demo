CREATE PROCEDURE dbo.GetOrderStatus 
@OrderHeader_ID int 

AS 

	-- **************************************************************************
	-- Procedure: GetOrderStatus()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:  This is called by frmOrderStatus
	--
	-- Modification History:
	-- Date			Init	TFS			Comment
	-- 1/18/2012	BAS		Bug 4317	Added ReceivingDiscrepancyItemCount subquery;
	--									Renamed File Extension; Coding Standards
	--									Updated this SP since frmOrderStatus already called it.
	-- 02/07/2012	BBB		3388		Added logic to return discrepancy counts
	-- 02/08/2012   MZ      4317        Corrected the ReceivingDiscrepancyItemCount subquery to 
	--                                  handle the case when ordered item is missing from the e-invoice.
	-- 02/22/2012   MZ      4736        Added oh.Return_Order in the query because it's needed to 
	--                                  determine whether it's a credit order
	-- 02/08/2013   FA      10824       Added new column DSDOrder in the SELECT query
	-- 03/12/2013   FA      8325		Added new column TotalRefused in the SELECT query	
	-- **************************************************************************

BEGIN
	SET NOCOUNT ON

	--**************************************************************************
	-- Main SQL
	--**************************************************************************

	SELECT
		InvoiceNumber,
		InvoiceDate,
		CloseDate,
		UploadedDate,
		AccountingUploadDate,
		ApprovedDate,
		vr.Store_No, 
		oh.Vendor_ID,
		ReceiveLocation_ID,
		PurchaseLocation_ID,
		Transfer_SubTeam,
		Transfer_To_SubTeam,
		v.PS_Vendor_ID,
		v.PS_Location_Code,
		v.PS_Address_Sequence,
		oh.QuantityDiscount,
		oh.DiscountType,
		VendorDoc_ID,
		VendorDocDate,
		v.WFM,
		PayByAgreedCost,
		oh.CurrencyID,
		oh.RefuseReceivingReasonID,
		v.EinvoiceRequired,
		Einvoice_ID,
		[ReceivingDiscrepancyItemCount] =	ISNULL((	
												SELECT
													COUNT(*)
												FROM
													OrderItem (nolock) oi INNER JOIN OrderHeader (nolock) oh ON oi.OrderHeader_ID = oh.OrderHeader_ID
												WHERE	
													oi.OrderHeader_Id		= @OrderHeader_ID
													AND oh.eInvoice_Id	IS NOT NULL  
													AND ISNULL(oi.QuantityReceived, 0) <> ISNULL(oi.eInvoiceQuantity,0) 
													AND oi.ReceivingDiscrepancyReasonCodeID IS NULL
													AND (SELECT SUM(ISNULL(QuantityReceived, 0)) FROM OrderItem WHERE OrderHeader_ID = @OrderHeader_ID) > 0
											), 0),
		oh.Return_Order,
		oh.DSDOrder,
		oh.TotalRefused												
    
	FROM
		OrderHeader			(nolock) oh
		INNER JOIN Vendor	(nolock) v	ON	oh.Vendor_ID = v.Vendor_ID
		INNER JOIN Vendor	(nolock) vr	ON	oh.ReceiveLocation_ID = vr.Vendor_ID

	WHERE
		oh.OrderHeader_ID = @OrderHeader_ID
		
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderStatus] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderStatus] TO [IRMAReportsRole]
    AS [dbo];

