if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[BRC_GetOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[BRC_GetOrders]
GO

CREATE PROCEDURE dbo.BRC_GetOrders
	@Vendor_ID	int,
	@Subteam_No int,
	@StartDate	datetime,
	@EndDate	datetime
AS
	-- **************************************************************************
	-- Procedure: BRC_GetOrders()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is called from BatchReceiveCloseDAO
	--
	-- Modification History:
	-- Date			Init	TFS		Comment
	-- 03/30/2010	BBB		xxxx	Changed WHERE clause on @Orders from Transfer_Subteam to
	--								Transfer_To_Subteam;
	-- 12/05/2011	BBB		3744	applied coding standards; removed deprecated fn call;
	--								added consumption of new OrderedCost col;
	-- **************************************************************************
BEGIN
	SELECT 
		oh.OrderHeader_ID, 
		st.Subteam_Name, 
		s.Store_Name, 
		oh.OrderedCost, 
		oh.Expected_Date
	FROM 
		OrderHeader			(nolock) oh
		INNER JOIN Subteam	(nolock) st	ON oh.Transfer_To_Subteam	= st.Subteam_No
		INNER JOIN Vendor	(nolock) v	ON oh.PurchaseLocation_ID	= v.Vendor_ID 
		INNER JOIN Store	(nolock) s	on v.Store_No				= s.Store_No
	WHERE
		Sent = 1
		AND oh.CloseDate			IS NULL
		AND oh.Vendor_ID			=	@Vendor_ID
		AND oh.Transfer_To_Subteam	=	@Subteam_No
		AND oh.Expected_Date		>=	@StartDate
		AND oh.Expected_Date		<	DATEADD(d,1,@EndDate)
END
GO