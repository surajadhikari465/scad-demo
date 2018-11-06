CREATE Procedure dbo.PurchasesToSalesCompSubTeamDetail
    @Team_No int,
    @SubTeam_List varchar(max),
    @RetailSubTeams bit,
    @OrderType_ID int,
    @CurrDate datetime
AS

-- **************************************************************************
-- Procedure: PurchasesToSalesCompSubTeamDetail()
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
--								Verified LineItemReceivedCost is calculated correctly
-- 12/22/2011  BAS		3744	updated aggregation of Line Item Received Cost
--								to consume the new column OrderHeader.AdjustedReceivedCost
-- **************************************************************************

BEGIN

SET NOCOUNT ON

DECLARE
		@StartDate datetime,
		@EndDate datetime,
		@Subteam_No int

	SELECT
		@StartDate = CONVERT(datetime, CONVERT(varchar(255), DATEADD(day, 1 - DATEPART(dw, ISNULL(@CurrDate, GETDATE())) - 6, ISNULL(@CurrDate, GETDATE())), 101)),
		@EndDate = @StartDate + 6 --CONVERT(datetime, CONVERT(varchar(255), DATEADD(day, 2 - DATEPART(dw, ISNULL(@CurrDate, GETDATE())), ISNULL(@CurrDate, GETDATE())), 101))


	DECLARE @Store table (store_no int, store_name varchar(50), BusinessUnit_ID int)		
	INSERT INTO @Store
		SELECT
			Store_No,
			Store_Name,
			BusinessUnit_ID
			
		FROM
			Store (nolock) 
		
		WHERE	
			Mega_Store = 1 
			OR WFM_Store = 1
      
      
	DECLARE @Subteam table (Subteam_No int, Subteam_Name varchar(100))
	INSERT INTO @Subteam 
		SELECT
			Subteam_No,
			Subteam_Name
		FROM
			Subteam	
			INNER JOIN fn_Parse_List(@SubTeam_List,',') sl on Subteam.Subteam_No = sl.Key_Value


	DECLARE 
		@Orders TABLE	(
							Store_No int,
							BusinessUnit_ID int,
							Store_Name varchar(50), 
							OrderHeader_ID int,
							SubTeam_Name varchar(255),
							OrderType_ID int,
							Return_Order int,
							POCost money,
							Subteam_No int
						)
	INSERT INTO @Orders (Store_No, BusinessUnit_ID, Store_Name, OrderHeader_ID, SubTeam_Name, OrderType_ID, Return_Order, POCost, Subteam_No)
		SELECT      
			s.Store_No,
			BusinessUnit_ID,
			s.Store_Name,
			oi.OrderHeader_ID,
			st.Subteam_Name,
			OrderType_ID,
			Return_Order,
			
			ISNULL((oh.AdjustedReceivedCost + SUM(oi.ReceivedItemFreight) 
						*	(	CASE 
									WHEN oh.Return_Order = 0 THEN 
										1 
									ELSE 
										-1 
								END
							)
					),0),
			ST.Subteam_No
			
		FROM 
			OrderItem				(nolock)	oi
			INNER JOIN OrderHeader	(nolock)	oh	ON	oi.OrderHeader_ID	= oh.OrderHeader_ID
			INNER JOIN @Subteam					st	ON	st.Subteam_No		= oh.Transfer_To_SubTeam
			INNER JOIN Vendor		(nolock)	vr	ON	vr.Vendor_ID		= oh.ReceiveLocation_ID
			INNER JOIN StoreSubTeam (nolock)	sst	ON	sst.Store_No		= vr.Store_No 
																			AND sst.SubTeam_No = oh.Transfer_To_SubTeam
			INNER JOIN @Store					s	ON	s.Store_No			= sst.Store_No
		
		WHERE 
			sst.Team_No			= ISNULL(@Team_No, sst.Team_No)
			AND sst.SubTeam_No	= ISNULL(@SubTeam_No, sst.SubTeam_No)
			AND OrderType_ID	= ISNULL(@OrderType_ID, OrderType_ID)
			AND CloseDate		>= @StartDate AND CloseDate < DATEADD(D,1, @EndDate)
			AND EXISTS (	
							SELECT * 
							FROM
								SubTeam (nolock) 
							WHERE 
								SubTeam_No = OH.Transfer_To_SubTeam
								AND ((SubTeamType_ID IN (1,3) AND @RetailSubTeams = 1) 
								OR (SubTeamType_ID NOT IN (1,3) AND @RetailSubTeams = 0)
								OR (@RetailSubTeams IS NULL))
						)
		GROUP BY 
			s.Store_No, 
			BusinessUnit_ID,
			s.Store_Name,
			oi.OrderHeader_ID,
			st.Subteam_Name,
			OrderType_ID,
			Return_Order,
			st.Subteam_No,
			oh.AdjustedReceivedCost


	DECLARE @NoPurchase table (Store_No int, BusinessUnit_ID int, Store_Name varchar(255), SubTeam_Name varchar(255), Subteam_No int)
	INSERT INTO @NoPurchase
	SELECT
		S.Store_No,
		BusinessUnit_ID,
		Store_Name,
		Subteam_Name,
		Subteam_No
	FROM 
		@Subteam ST,
		@Store S
	WHERE
		Store_No NOT IN	(
							SELECT DISTINCT Store_No 
							FROM @Orders
						)


	INSERT INTO @Orders
	SELECT
		Store_No,
		BusinessUnit_ID,
		Store_Name,
		NULL,
		Subteam_Name,
		NULL,
		NULL,
		0,
		Subteam_No
		
	FROM @NoPurchase


	INSERT INTO @Orders (Store_No, BusinessUnit_ID, Store_Name, OrderHeader_ID, SubTeam_Name, OrderType_ID, Return_Order, POCost, Subteam_No)
		SELECT
			s.Store_No,
			BusinessUnit_ID,
			s.Store_Name,
			oi.OrderHeader_ID,
			st.Subteam_Name,
			OrderType_ID,
			Return_Order,
			- ISNULL((oh.AdjustedReceivedCost + SUM(oi.ReceivedItemFreight)), 0),
			Subteam_No
			
		FROM
			OrderHeader					(nolock)	oh
			INNER JOIN OrderItem		(nolock)	oi	ON	oi.OrderHeader_ID	= oh.OrderHeader_ID
			INNER JOIN Vendor			(nolock)	v	ON	v.Vendor_ID			= oh.Vendor_ID
			INNER JOIN @Store 						s	ON	s.Store_No			= v.Store_No
			INNER JOIN SubTeam			(nolock)	st	ON	st.Subteam_No		= oh.Transfer_SubTeam
			  
		WHERE
			oh.Transfer_SubTeam  = @SubTeam_No
			AND oi.DateReceived >= @startDate
			AND oi.DateReceived < DATEADD(day, 1, @EndDate)
			
		GROUP BY 
			s.Store_No,
			BusinessUnit_ID,
			s.Store_Name,
			oi.OrderHeader_ID,
			st.Subteam_Name,
			OrderType_ID,
			Return_Order,
			Subteam_No,
			oh.AdjustedReceivedCost


	UPDATE @Orders
		SET POCost = (InvoiceCost + InvoiceFreight) * CASE WHEN O.Return_Order = 0 THEN 1 ELSE -1 END
		FROM
			@Orders O
			JOIN OrderInvoice (nolock) oiv	ON O.OrderHeader_ID = oiv.OrderHeader_ID


	SELECT
		Store_Name,
		SubTeam_Name, 
		[OrderType]			=	CASE OrderType_Id 
									WHEN 1 THEN 'Purchase'
									WHEN 2 THEN 'Distribution'
									WHEN 3 THEN 'Transfer'
								END,
		[PO Number]			= OrderHeader_ID,
		[Purchase Amount]	= POCOST
	
	FROM 
		@Orders
		
	ORDER BY
		Store_Name, 
		SubTeam_Name, 
		'OrderType'

SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurchasesToSalesCompSubTeamDetail] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurchasesToSalesCompSubTeamDetail] TO [IRMAReportsRole]
    AS [dbo];

