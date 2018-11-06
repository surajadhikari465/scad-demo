/****** Object:  StoredProcedure [dbo].[GLDistributionCheckFromSubReport]    Script Date: 02/08/2012 12:28:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLDistributionCheckFromSubReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GLDistributionCheckFromSubReport]
GO

/****** Object:  StoredProcedure [dbo].[GLDistributionCheckFromSubReport]    Script Date: 02/08/2012 12:28:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GLDistributionCheckFromSubReport]
@StartDate varchar(20), 
@EndDate varchar(20), 
@DistributionStore_No int,
@ReceiveStore_No int,
@Transfer_SubTeam int,
@Transfer_To_SubTeam int

AS

   -- **************************************************************************
   -- Procedure: GLDistributionCheckReport()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	TFS		Comment
   -- 01/11/2011  BBB	1011	Added Credit column to output for consumption in RDL
   -- 12/19/2011  BAS   3744	Coding standards/formatting. Verified that LineItemReceivedCost is calculated correctly.
   -- 12/22/2011  BAS	3744	updated aggregation of OrderItem.ReceivedItemCost and
   --							changed it to new column OrderHeader.AdjustedReceivedCost
   -- 02/08/2012  td	4784	put orderitem.receiveditemcost back in since this reports by sales account changed right join to left join					
   -- 09/13/2013  MZ    13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
   -- **************************************************************************

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON
    
    SELECT
		[Team_No]					=	ISNULL(sst.Team_No, st.Team_No),	
		st.SubTeam_No, 
		st.SubTeam_Name, 
		[Receiver_Account]			=	ISNULL((SELECT
													CONVERT(varchar(255), Zone.GLMarketingExpenseAcct)
													FROM Zone
													WHERE Zone.Zone_ID = sd.Zone_ID AND sr.Regional = 1), '537000'
												),  
		[ReceiverName]				=	sr.Store_Name, 
		[Receiver_BusinessUnit_ID]	=	sr.BusinessUnit_ID, 
		[Vendor_Account]			=	MAX(CASE
												WHEN (sr.Mega_Store = 1 OR sr.WFM_Store = 1) AND (sd.Mega_Store = 1 OR sd.WFM_Store = 1) THEN
													'537000'
												ELSE 
													'450000'
											END),  
		[Vendor_BusinessUnit_ID]	=	sd.BusinessUnit_ID, 
		[VendorName]				=	sd.Store_Name, 
		oh.Return_Order,
		[Amount]					=	CAST(	(	
													CASE 
														WHEN oh.Return_Order = 1 THEN
															-1
														ELSE
															1
													END
												) 
											* ROUND(SUM(oi.ReceivedItemCost + oi.ReceivedItemFreight),4) AS MONEY)
				
	FROM
		OrderHeader				(nolock)	oh
		INNER JOIN OrderItem	(nolock)	oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Item			(nolock)	i	ON	oi.Item_Key				= i.Item_Key
		INNER JOIN SubTeam		(nolock)	st	ON	oh.Transfer_SubTeam		= st.SubTeam_No
		INNER JOIN Vendor		(nolock)	vr	ON	oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor		(nolock)	vd	ON	oh.Vendor_ID			= vd.Vendor_ID
		INNER JOIN Store		(nolock)	sd	ON	vd.Store_no				= sd.Store_No
		INNER JOIN Store		(nolock)	sr	ON	vr.Store_no				= sr.Store_No
		LEFT JOIN StoreSubTeam	(nolock)	sst	ON	oh.Transfer_SubTeam		= sst.SubTeam_No
												AND	sd.Store_No				= sst.Store_No
         
    WHERE 
		oi.DateReceived				>= @StartDate 
		AND oi.DateReceived			< DATEADD("day", 1, @EndDate) 
		AND sd.BusinessUnit_ID		<> sr.BusinessUnit_ID 
		AND dbo.fn_GetCustomerType(sr.Store_No, sr.Internal, sr.BusinessUnit_ID) <> 1 
		AND dbo.fn_VendorType(vd.PS_Vendor_ID, vd.WFM, vd.Store_No, sd.Internal) = 3 
		AND vr.Store_No				= ISNULL(@ReceiveStore_No, vr.Store_No) 
		AND vd.Store_No				= ISNULL(@DistributionStore_No, vd.Store_No) 
		AND oh.OrderType_ID			= 2	-- Distribution order type
		AND oh.Transfer_SubTeam		= ISNULL(@Transfer_SubTeam, oh.Transfer_SubTeam)
		AND oh.Transfer_To_SubTeam	= ISNULL(@Transfer_To_SubTeam, oh.Transfer_To_SubTeam)
		
    GROUP BY 
		ISNULL(sst.Team_No, st.Team_No), 
		st.SubTeam_No, 
		st.SubTeam_Name, 
		sr.Store_Name, 
		sr.Regional, 
		sd.Store_Name, 
		sd.Zone_ID, 
		sr.Store_No, 
		sd.BusinessUnit_ID, 
		sr.BusinessUnit_ID, 
		oh.Return_Order
             
    SET NOCOUNT OFF
END
GO


