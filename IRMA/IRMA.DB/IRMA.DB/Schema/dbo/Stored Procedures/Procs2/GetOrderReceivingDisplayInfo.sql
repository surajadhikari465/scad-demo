CREATE PROCEDURE dbo.GetOrderReceivingDisplayInfo
	@OrderHeader_ID int

AS
   -- **************************************************************************
   -- Procedure: GetOrderReceivingDisplayInfo()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from ReceivingListDAO.vb
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 12/06/2011	BBB		3744	coding standards; new columns; 
   -- 01.18.2012	BBB		4376	added IsNull trap;
   -- **************************************************************************
BEGIN
	SET NOCOUNT ON

	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT 
		oh.OrderHeader_ID, 
		st.Subteam_Name, 
		s.Store_Name, 
		[OrderedCost] = ISNULL(oh.OrderedCost, 0), 
		oh.Expected_Date,
		oh.QtyShippedProvided, 
		oh.EInvoice_Id,
		OpenPO		=	(CASE WHEN oh.SentDate is not null and oh.CloseDate is null THEN 1 ELSE 0 END),
		oh.OrderType_ID,
		oh.Return_Order,
		ohe.PastReceiptDate
	FROM 
		OrderHeader			(nolock) oh
		INNER JOIN Subteam	(nolock) st ON	oh.Transfer_To_Subteam	= st.Subteam_No
		INNER JOIN Vendor	(nolock) vs ON	oh.PurchaseLocation_ID	= vs.Vendor_ID
		INNER JOIN Store	(nolock) s	ON	vs.Store_No				= s.Store_No
         LEFT JOIN OrderHeaderExtended (nolock) ohe 
										ON ohe.OrderHeader_ID = oh.OrderHeader_ID  
	WHERE 
		oh.OrderHeader_ID = @OrderHeader_ID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderReceivingDisplayInfo] TO [IRMAClientRole]
    AS [dbo];

