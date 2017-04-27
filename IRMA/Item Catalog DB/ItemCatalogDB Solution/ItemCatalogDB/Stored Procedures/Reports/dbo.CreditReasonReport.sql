SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreditReasonReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreditReasonReport]
GO

CREATE PROCEDURE [dbo].[CreditReasonReport]
    @StartDate			smalldatetime, 
    @EndDate			smalldatetime, 
    @Vendor_ID			int,
    @ReceiveLocation_ID int = NULL,
    @Zone				int = NULL,	
    @CostOption			tinyint
AS

   -- **************************************************************************
   -- Procedure: CreditReasonReport
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
   -- 12/15/2011  BAS	3744	Coding standards/formatting. Verified LineItemReceivedCost is calculated correctly
   -- **************************************************************************

BEGIN

	SET NOCOUNT ON

	DECLARE @TotalDistribution AS MONEY

	SELECT @TotalDistribution = (	SELECT
										SUM(TotalDis.ReceivedItemCostTotal) 
									FROM
										(
											SELECT
												[ReceivedItemCosttotal] = (	CASE
																				WHEN @CostOption = 1 THEN
																					ISNULL(dbo.fn_AvgCostHistory(oi2.Item_Key, s2.Store_No, oh2.Transfer_To_SubTeam, oh2.CloseDate), 0) * UnitsReceived 
																				ELSE
																					oi2.ReceivedItemCost
																			END)
																		
											FROM
												OrderHeader				(NOLOCK)	oh2
												INNER JOIN OrderItem	(NOLOCK)	oi2	ON oh2.OrderHeader_ID		= oi2.OrderHeader_ID
												INNER JOIN Vendor		(NOLOCK)	vr2	ON oh2.ReceiveLocation_ID	= vr2.Vendor_ID
												INNER JOIN Vendor		(NOLOCK)	v2	ON oh2.Vendor_id			= v2.Vendor_ID
												INNER JOIN Store		(NOLOCK)	s2	ON vr2.Store_No				= s2.Store_No
												INNER JOIN Zone			(NOLOCK)	z2	ON s2.Zone_ID				= z2.Zone_ID
											
										   WHERE
												CloseDate								>= CONVERT(DATETIME, @StartDate, 102)
												AND CloseDate							< DATEADD(d, 1, CONVERT(DATETIME, @EndDate, 102))
												AND ISNULL(@Vendor_ID, oh2.Vendor_ID)	= oh2.Vendor_ID 
												AND CreditReason_ID IS NULL
										) TotalDis
								)
						     
	SELECT
		CloseDate,
		Item_Key, 
		[PO_Number]				= oh.OrderHeader_ID, 
		[ReceivedItemCost]		= (	CASE
										WHEN @CostOption = 1 THEN
											ISNULL(dbo.fn_AvgCostHistory(oi.Item_Key, s.Store_No, oh.Transfer_To_SubTeam, oh.CloseDate), 0) * UnitsReceived 
										ELSE
											ReceivedItemCost 
									END), 
		cr.CreditReason_ID, 
		CreditReason, 
		[ReceiveLocation]		= vr.CompanyName, 
		[Vendor]				= v.CompanyName, 
		z.Zone_Name, 
		oh.ReceiveLocation_ID, 
		[ReceivedItemCostTotal] = ISNULL(T2.ReceivedItemCostTotal, 0), 
		[TotalDistribution]		= @TotalDistribution, 
		[OrgOrderHeader_ID]		= rol.OrderHeader_ID
	
	FROM OrderHeader						(NOLOCK) oh
		INNER JOIN	OrderItem				(NOLOCK) oi		ON oh.OrderHeader_ID		= oi.OrderHeader_ID
		LEFT JOIN	ReturnOrderList			(NOLOCK) rol	ON oi.OrderHeader_ID		= rol.ReturnOrderHeader_ID
		INNER JOIN	CreditReasons			(NOLOCK) cr		ON oi.CreditReason_ID		= cr.CreditReason_ID
		INNER JOIN	Vendor					(NOLOCK) vr		ON oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN	Vendor					(NOLOCK) v		ON oh.Vendor_id				= v.Vendor_ID
		INNER JOIN	Store					(NOLOCK) s		ON vr.Store_No				= s.Store_No
		INNER JOIN	Zone					(NOLOCK) z		ON s.Zone_ID				= z.Zone_ID
		LEFT JOIN 
					(	SELECT 
							[ReceivedItemCostTotal] = SUM(T1.ReceivedItemCostTotal), 
							T1.Zone_ID 
						FROM 
							(	SELECT
									z3.Zone_ID, 
									[ReceivedItemCostTotal] =	(	CASE
																		WHEN @CostOption = 1 THEN 
																			ISNULL(dbo.fn_AvgCostHistory(oi3.Item_Key, s3.Store_No, oh3.Transfer_To_SubTeam, oh3.CloseDate), 0) * UnitsReceived
																		ELSE
																			ReceivedItemCost 
																	END)
															
								FROM
									OrderHeader				(NOLOCK)	oh3
									INNER JOIN OrderItem	(NOLOCK)	oi3	ON oh3.OrderHeader_ID		= oi3.OrderHeader_ID
									INNER JOIN Vendor		(NOLOCK)	vr3	ON oh3.ReceiveLocation_ID	= vr3.Vendor_ID
									INNER JOIN Vendor		(NOLOCK)	v3	ON oh3.Vendor_id			= v3.Vendor_ID 
									INNER JOIN Store		(NOLOCK)	s3	ON vr3.Store_No				= s3.Store_No
									INNER JOIN Zone			(NOLOCK)	z3	ON s3.Zone_ID				= z3.Zone_ID 
								
								WHERE
									CloseDate								>= CONVERT(DATETIME, @StartDate, 102)
									AND CloseDate							< DATEADD(d, 1, CONVERT(DATETIME, @EndDate, 102)) 
									AND ISNULL(@Vendor_ID, oh3.Vendor_ID)	= oh3.Vendor_ID 
									AND CreditReason_ID IS NULL
							) T1
								
						GROUP BY
							T1.Zone_ID
					) T2
					ON	(T2.Zone_ID	= s.Zone_ID)

	WHERE
		CloseDate												>= CONVERT(DATETIME, @StartDate, 102)
		AND CloseDate											< DATEADD(d, 1,CONVERT(DATETIME, @EndDate, 102))
		AND ISNULL(@Vendor_ID, oh.Vendor_ID)					= oh.Vendor_ID
		AND ISNULL(@ReceiveLocation_ID, oh.ReceiveLocation_ID)	= oh.ReceiveLocation_ID
		AND ISNULL(@Zone, z.Zone_ID)							= z.Zone_ID

	SET NOCOUNT OFF

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO