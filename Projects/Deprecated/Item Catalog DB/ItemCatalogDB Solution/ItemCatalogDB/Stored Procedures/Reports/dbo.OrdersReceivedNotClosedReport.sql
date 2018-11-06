SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[OrdersReceivedNotClosedReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[OrdersReceivedNotClosedReport]
GO

CREATE PROCEDURE dbo.OrdersReceivedNotClosedReport

	@Store_No	int,
	@SubTeam_No int,
	@BeginDate	smalldatetime,
	@EndDate	smalldatetime

AS

   -- **************************************************************************
   -- Procedure: OrdersReceivedNotClosedReport
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 12/22/2011	BAS		3744	renamed file extension to .sql. Coding standards/formatting;
   --								verified LineItemReceivedCost is calculated correctly;
   --								removed second SELECT statement within the WHERE clause;
   --								updated aggregation to consume new column
   --								OrderHeader.AdjustedReceivedCost
   -- 03/07/2013	KM		11476	Remove the hardcoded date formatting in the selection of LastReceived;
   -- 09/12/2013    MZ		13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
   -- **************************************************************************

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	SELECT
		v.CompanyName, 
		oh.OrderHeader_ID,
		[LastReceived]	= CONVERT(date, oi.DateReceived),
		[TotalCost]		= oh.AdjustedReceivedCost + SUM(oi.ReceivedItemFreight)
	
	FROM
		OrderHeader				(nolock)	oh
		INNER JOIN OrderItem	(nolock)	oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor		(nolock)	v	ON	oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Item			(nolock)	i	ON	oi.Item_Key				= i.Item_Key
		INNER JOIN Vendor		(nolock)	vr	ON	oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Store		(nolock)	s	ON	vr.Store_no				= s.Store_No
    
	WHERE												             
		oh.CloseDate IS NULL
		AND (CONVERT(Varchar(10), DateReceived, 101)	>= CONVERT(Varchar(10), @BeginDate, 101))
		AND (CONVERT(Varchar(10), DateReceived, 101)	<= CONVERT(Varchar(10), @EndDate, 101))
		AND i.SubTeam_No = ISNULL(@SubTeam_No, i.SubTeam_No)
		AND s.Store_No = @Store_No

	GROUP BY 
		v.CompanyName,
		oh.OrderHeader_ID,
		oh.CloseDate,
		oi.DateReceived,
		oh.AdjustedReceivedCost

	SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO