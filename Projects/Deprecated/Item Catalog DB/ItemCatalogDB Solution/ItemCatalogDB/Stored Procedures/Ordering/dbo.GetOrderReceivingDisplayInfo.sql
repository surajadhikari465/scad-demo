SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderReceivingDisplayInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetOrderReceivingDisplayInfo]
GO

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
		[OpenPO]		=	(CASE WHEN oh.SentDate is not null and oh.CloseDate is null THEN 1 ELSE 0 END)
	FROM 
		OrderHeader			(nolock) oh
		INNER JOIN Subteam	(nolock) st ON	oh.Transfer_To_Subteam	= st.Subteam_No
		INNER JOIN Vendor	(nolock) vs ON	oh.PurchaseLocation_ID	= vs.Vendor_ID
		INNER JOIN Store	(nolock) s	ON	vs.Store_No				= s.Store_No
	WHERE 
		oh.OrderHeader_ID = @OrderHeader_ID

END
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO