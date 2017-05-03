CREATE PROCEDURE dbo.GLDistributionsReport
@StartDate varchar(20), 
@EndDate varchar(20), 
@Store_No int,
@SubTeam_No int

AS

-- ******************************************************************************************
-- Procedure: GLDistributionsReport()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from a single RDL file and generates a report consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init		TFS		Comment
-- 12/21/2011  BAS		3744	Coding standards/formatting. Verified LineItemReceivedCost is calculated correctly
-- 12/22/2011  BAS		3744	updated aggregation of OrderItem.ReceivedItemCost and
 --								changed it to consume new column OrderHeader.AdjustedReceivedCost
-- ******************************************************************************************

BEGIN

	SET NOCOUNT ON

	SELECT 
		[Unit]			= s.BusinessUnit_ID, 
		[Ledger]		= 'ACTUALS', 
		[Account]		=	CASE
								WHEN oh.Return_Order = 1 THEN
									'537000'
								ELSE 
									'450000'
							END, 
		st.Team_No, 
		st.SubTeam_No, 
		st.SubTeam_Name,
		[Aff]			= '', 
		[Proj]			= '', 
		[Curr]			= 'USD', 
		[Descr]			= 'Distributions', 
		[Amount]		= -1 * CAST(ROUND(oh.AdjustedReceivedCost + SUM(oi.ReceivedItemFreight), 2) AS MONEY),
		[Trans_Date]	= @EndDate
		
	FROM 
		OrderHeader				(nolock)	oh
		INNER JOIN OrderItem	(nolock)	oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Item			(nolock)	i	ON	oi.Item_Key				= i.Item_Key
		INNER JOIN SubTeam		(nolock)	st	ON	i.SubTeam_No			= st.SubTeam_No
		INNER JOIN Vendor		(nolock)	vr	ON	oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor		(nolock)	vd	ON	oh.Vendor_ID			= vd.Vendor_ID
		INNER JOIN Store		(nolock)	s	ON	vd.Store_no				= s.Store_No
		   
	WHERE 
		oi.DateReceived			>= @StartDate 
		AND oi.DateReceived		<= @EndDate 
		AND vd.Vendor_ID		<> vr.Vendor_ID 
		AND	vd.Vendor_ID		NOT IN (1396,1397,3361,3635,3737,4895,5118,5250) 
		AND vr.Vendor_ID		NOT IN (1396,1397,3361,3635,3737,4895,5118,5250) 
		AND vd.CompanyName		IS NOT NULL 
		AND (NOT oi.DateReceived IS NULL) 
		AND (vd.Store_No IS NOT NULL) 
		AND (vr.Store_No IS NOT NULL) 
		AND	s.Store_No			= ISNULL(@Store_No, s.Store_No) 
		AND	st.SubTeam_No		= ISNULL(@SubTeam_No, st.SubTeam_No)
		AND oh.OrderType_ID		= 2	-- Distribution Order Type
		
	GROUP BY
		s.BusinessUnit_ID, 
		st.Team_No, 
		st.SubTeam_No, 
		st.SubTeam_Name, 
		oh.Return_Order,
		oh.AdjustedReceivedCost

	UNION ALL

	SELECT 
		[Unit]			= s.BusinessUnit_ID, 
		[Ledger]		= 'ACTUALS', 
		[Account]		=	CASE
								WHEN oh.Return_Order = 0 THEN
									'537000'
								ELSE
									'450000'
							END, 
		st.Team_No, 
		st.SubTeam_No, 
		st.SubTeam_Name,
		[Aff]			= '', 
		[Proj]			= '', 
		[Curr]			= 'USD', 
		[Descr]			= 'Distributions', 
		[Amount]		= CAST(ROUND(oh.AdjustedReceivedCost + SUM(oi.ReceivedItemFreight), 2) AS MONEY),
		[Trans_Date]	= @EndDate
		
	FROM 
		OrderHeader				(nolock)	oh
		INNER JOIN OrderItem	(nolock)	oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Item			(nolock)	i	ON	oi.Item_Key				= i.Item_Key
		INNER JOIN SubTeam		(nolock)	st	ON	i.SubTeam_No			= st.SubTeam_No
		INNER JOIN Vendor		(nolock)	vr	ON	oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor		(nolock)	vd	ON	oh.Vendor_ID			= vd.Vendor_ID
		INNER JOIN Store		(nolock)	s	ON	vr.Store_no				= s.Store_No
			     
	WHERE 
		oi.DateReceived		>= @StartDate 
		AND oi.DateReceived <= @EndDate 
		AND vd.Vendor_ID	<> vr.Vendor_ID 
		AND	vd.Vendor_ID	NOT IN (1396,1397,3361,3635,3737,4895,5118,5250) 
		AND vr.Vendor_ID	NOT IN (1396,1397,3361,3635,3737,4895,5118,5250) 
		AND vd.CompanyName	IS NOT NULL 
		AND	(NOT oi.DateReceived IS NULL) 
		AND (vd.Store_No IS NOT NULL) 
		AND (vr.Store_No IS NOT NULL) 
		AND	s.Store_No		= ISNULL(@Store_No, s.Store_No) 
		AND	st.SubTeam_No	= ISNULL(@SubTeam_No, st.SubTeam_No)
		AND oh.OrderType_ID = 2	-- Distribution Order Type
		
	GROUP BY 
		s.BusinessUnit_ID, 
		st.Team_No, 
		st.SubTeam_No, 
		st.SubTeam_Name, 
		oh.Return_Order,
		oh.AdjustedReceivedCost
		
	ORDER BY 
		s.BusinessUnit_ID, 
		st.Team_No, 
		st.SubTeam_No, 
		st.SubTeam_Name

	SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLDistributionsReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLDistributionsReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLDistributionsReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLDistributionsReport] TO [IRMAReportsRole]
    AS [dbo];

