SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GLTransferDetailsReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GLTransferDetailsReport]
GO


CREATE PROCEDURE dbo.GLTransferDetailsReport
@BeginDate varchar(20),
@EndDate varchar(20), 
@Store_No int,
@SubTeam_No int

AS 

-- **************************************************************************
-- Procedure: GLTransferDetailsReport()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from a single RDL file and generates a report consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init		TFS		Comment
-- 12/21/2011  BAS		3744	Coding standards/formatting.
--								Verified Line Item Received Cost is calculated correctly
-- **************************************************************************

BEGIN

SET NOCOUNT ON

	SELECT
		[Unit]			= vs.BusinessUnit_ID,
		[Team_No]		= ISNULL(sst.Team_No,990),
		[SubTeam_No]	= ISNULL(st.SubTeam_No,9941),
		[Amount]		= -1 * CAST(ReceivedItemCost AS MONEY),
		i.Item_Key,
		i.Item_Description,
		Identifier, 
		[Quantity]		= -1 * (UnitsReceived), 
		st.SubTeam_Name,
		oi.OrderHeader_ID,
		[DateStamp]		= oi.DateReceived
	    
	FROM
		OrderHeader					(nolock)	oh 
		INNER JOIN OrderItem		(nolock)	oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Item				(nolock)	i	ON	i.Item_Key				= oi.Item_Key
		INNER JOIN ItemIdentifier	(nolock)	ii	ON	i.Item_Key				=	ii.Item_Key 
													AND ii.Default_Identifier	= 1
		INNER JOIN SubTeam			(nolock)	st	ON	oh.Transfer_SubTeam		= st.SubTeam_No
		INNER JOIN Vendor			(nolock)	v	ON	oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Store			(nolock)	vs	ON	v.Store_No				= vs.Store_No
		INNER JOIN StoreSubTeam		(nolock)	sst	ON	st.SubTeam_No			= sst.SubTeam_No
													AND vs.Store_No				= sst.Store_No
													
	WHERE 
		oi.DateReceived		>= @Begindate 
		AND oi.DateReceived < DATEADD(d,1,@Enddate) 
		AND vs.Store_No		= ISNULL(@Store_No, vs.Store_No) 
		AND st.SubTeam_No	= ISNULL(@SubTeam_No, st.SubTeam_No)
		AND oh.OrderType_ID = 3	-- Transfer order type

	UNION ALL

	SELECT 
		[Unit]			= sr.BusinessUnit_ID, 
		[Team_No]		= ISNULL(sst.Team_No,990),
		[SubTeam_No]	= ISNULL(st.SubTeam_No,9941),
		[Amount]		= CAST(ReceivedItemCost AS MONEY),
		i.Item_Key,
		i.Item_Description,
		Identifier, 
		[Quantity]		= UnitsReceived, 
		st.SubTeam_Name,
		oi.OrderHeader_ID,
		[DateStamp]		= oi.DateReceived
	    
	FROM
		OrderHeader					(nolock)	oh
		INNER JOIN OrderItem		(nolock)	oi		ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Item				(nolock)	i		ON	oi.Item_Key				= i.Item_Key
		INNER JOIN ItemIdentifier	(nolock)	ii		ON	i.Item_Key				= ii.Item_Key
														AND ii.Default_Identifier	= 1
		INNER JOIN SubTeam			(nolock)	st		ON	st.SubTeam_No			= ISNULL(oh.Transfer_To_SubTeam, oh.Transfer_SubTeam)
		INNER JOIN Vendor			(nolock)	vr		ON	oh.ReceiveLocation_ID	= vr.Vendor_ID 
		INNER JOIN Store			(nolock)	sr		ON	vr.Store_no				= sr.Store_No 
		INNER JOIN StoreSubTeam		(nolock)	sst		ON	st.SubTeam_No			= sst.SubTeam_No
														AND sr.Store_No				= sst.Store_No
		INNER JOIN Vendor			(nolock)	v		ON	oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Store			(nolock)	vs		ON	v.Store_No				= vs.Store_No
	
	WHERE 
		oi.DateReceived		>= @Begindate 
		AND oi.DateReceived < DATEADD(d,1,@Enddate) 
		AND sr.Store_No		= ISNULL(@Store_No, sr.Store_No) 
		AND st.SubTeam_No	= ISNULL(@SubTeam_No, st.SubTeam_No)
		AND oh.OrderType_ID = 3	-- Transfer order type

	ORDER BY
		Unit,
		Team_No,
		SubTeam_No
		
	SET NOCOUNT OFF

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


