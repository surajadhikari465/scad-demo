CREATE PROCEDURE dbo.GLTransfersReport
@BeginDate varchar(20),
@EndDate varchar(20), 
@Store_No int,
@SubTeam_No int

AS 

-- **************************************************************************
-- Procedure: GLTransfersReport()
--    Author: n/a
--      Date: n/a
--
-- Description: This procedure is called from a single RDL file and generates 
--				a report consumed by SSRS procedures.
--
-- Modification History:
-- Date			Init	TFS		Comment
-- 11/06/2009	BBB				update existing SP to specifically declare table source 
--								for BusinessUnit_ID column to prevent ambiguity between
--								Store and Vendor table
-- 12/21/2011	BAS		3744	coding standards;
--								validated Line Item Received Cost Calculation
-- 12/22/2011	BAS		3744	updated aggregation of OrderItem.ReceivedItemCost
--								to consume new column OrderHeader.AdjustedReceivedCost
--								and removed Group By statments
-- **************************************************************************


BEGIN
SET NOCOUNT ON

	SELECT
		[Unit]			= sv.BusinessUnit_ID,
		[Ledger]		= 'ACTUALS',
		[Account]		= '500000',
		sst.Team_No,
		st.SubTeam_No,
		[Aff]			= '',
		[Proj]			= '',
		[Curr]			= 'USD',
		[Descr]			= 'Transfers',
		[Amount]		= -1 * CAST(oh.AdjustedReceivedCost AS MONEY),
		[Trans_Date]	= @EndDate,
		st.SubTeam_Name
		
	FROM
		OrderHeader				(nolock)	oh
		INNER JOIN OrderItem	(nolock)	oi	ON	oh.OrderHeader_ID	= oi.OrderHeader_ID
		INNER JOIN SubTeam		(nolock)	st	ON	oh.Transfer_SubTeam	= st.SubTeam_No
		INNER JOIN Vendor		(nolock)	v	ON	oh.Vendor_ID		= v.Vendor_ID 
		INNER JOIN Store		(nolock)	sv	ON	v.Store_No			= sv.Store_No
		INNER JOIN StoreSubTeam	(nolock)	sst	ON	st.SubTeam_No		= sst.SubTeam_No
													AND sv.Store_No			= sst.Store_No
		
	WHERE 
		oi.DateReceived		>= @Begindate 
		AND oi.DateReceived < DATEADD(d,1,@Enddate) 
		AND sv.Store_No		= ISNULL(@Store_No, sv.Store_No) 
		AND st.SubTeam_No	= ISNULL(@SubTeam_No, st.SubTeam_No)
		AND oh.OrderType_ID = 3	-- Transfer order type
		
	UNION ALL
	
	SELECT
		[Unit]			= sr.BusinessUnit_ID,
		[Ledger]		= 'ACTUALS',
		[Account]		= '500000',
		sst.Team_No,
		sst.SubTeam_No,
		[Aff]			= '',
		[Proj]			= '',
		[Curr]			= 'USD',
		[Descr]			= 'Transfers',
		[Amount]		= CAST(oh.AdjustedReceivedCost AS MONEY),
		[Trans_Date]	= @EndDate,
		st.SubTeam_Name
		
	FROM
		OrderHeader				(nolock)	oh 
		INNER JOIN OrderItem	(nolock)	oi	ON	oh.OrderHeader_ID	= oi.OrderHeader_ID
		INNER JOIN SubTeam		(nolock)	st	ON	st.SubTeam_No		= oh.Transfer_To_SubTeam 
		INNER JOIN Vendor		(nolock)	vr	ON	vr.Vendor_ID		= oh.ReceiveLocation_ID
		INNER JOIN Store		(nolock)	sr	ON	sr.Store_No			= vr.Store_No
		INNER JOIN StoreSubTeam (nolock)	sst	ON	sst.SubTeam_No		= st.SubTeam_No AND sst.Store_No = sr.Store_No
		INNER JOIN Vendor		(nolock)	v	ON	v.Vendor_ID			= oh.Vendor_ID
		INNER JOIN Store		(nolock)	sv	ON	sv.Store_No			= v.Store_No
	
	WHERE 
		oi.DateReceived		>= @Begindate 
		AND oi.DateReceived < DATEADD(d,1,@Enddate) 
		AND sr.Store_No		= ISNULL(@Store_No, sr.Store_No) 
		AND st.SubTeam_No	= ISNULL(@SubTeam_No, st.SubTeam_No)
		AND oh.OrderType_ID = 3	-- Transfer order type
		
	ORDER BY
		sv.BusinessUnit_ID,
		sst.Team_No,
		st.SubTeam_No
	
	SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLTransfersReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLTransfersReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLTransfersReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLTransfersReport] TO [IRMAReportsRole]
    AS [dbo];

