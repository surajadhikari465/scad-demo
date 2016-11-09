CREATE PROCEDURE [dbo].[WFMM_GetItemBilledQuantity]
	@Store_No				int,
    @Subteam_No				int,
	@Identifier				varchar(13)
AS
-- **************************************************************************
-- Procedure: WFMM_GetItemBilledQuantity()
--    Author: Hui Kou
--      Date: 09.19.12
--
-- Description:
-- This procedure is called from the WFM Mobile app to return item invoice quantity
-- 
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 09.19.12		Hk   	7427	Creation
-- **************************************************************************
BEGIN
    SET NOCOUNT ON 

	select oh.OrderDate orderDate, eh.invoice_num invNumber, oi.QuantityOrdered orderQty, isNull(ei.qty_shipped,0) invoiceQty
	from orderheader oh (nolock) 
	inner join orderitem oi (nolock) on oi.OrderHeader_ID = oh.OrderHeader_ID
	inner join Vendor (nolock)	rl	ON	oh.ReceiveLocation_ID   = rl.Vendor_ID 
	inner join itemidentifier ii (nolock) on ii.item_key = oi.item_key and deleted_identifier = 0 and remove_identifier = 0
	inner join einvoicing_header eh (nolock) on eh.einvoice_id = oh.einvoice_id
	left join EInvoicing_Item ei (nolock) on eh.Einvoice_id = ei.EInvoice_id and ei.item_key = oi.item_key
	where ii.identifier = @Identifier 
	and oh.Transfer_To_SubTeam = @Subteam_No
	and rl.Store_no = @Store_No
	and oi.datereceived is null
	and oh.closedate is null

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WFMM_GetItemBilledQuantity] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WFMM_GetItemBilledQuantity] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WFMM_GetItemBilledQuantity] TO [IRMAReportsRole]
    AS [dbo];

