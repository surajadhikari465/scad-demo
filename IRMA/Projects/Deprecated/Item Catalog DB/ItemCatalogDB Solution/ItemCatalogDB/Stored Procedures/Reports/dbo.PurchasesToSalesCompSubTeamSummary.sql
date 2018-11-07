SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'PurchasesToSalesCompSubTeamSummary')
	BEGIN
		DROP  Procedure  dbo.PurchasesToSalesCompSubTeamSummary
	END

GO

CREATE Procedure dbo.PurchasesToSalesCompSubTeamSummary
         @Team_No int,
         @SubTeam_List varchar(max),
         @RetailSubTeams bit,
         @OrderType_ID int,
         @CurrDate datetime
AS

-- **************************************************************************
-- Procedure: PurchasesToSalesCompSubTeamSummary()
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
-- 12/22/2011  BAS		3744	changed aggregation of OrderItem.ReceivedItemCost
--								to the new column OrderHeader.AdjustedReceivedCost
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
			Subteam (nolock)
			INNER JOIN fn_Parse_List(@SubTeam_List, ',')	sl	ON	Subteam.Subteam_No = sl.Key_Value 


	DECLARE @Orders TABLE	(
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
	INSERT INTO @Orders		(
								Store_No,
								BusinessUnit_ID,
								Store_Name,
								OrderHeader_ID,
								SubTeam_Name,
								OrderType_ID,
								Return_Order,
								POCost,
								Subteam_No
							)
		SELECT      
			S.Store_No,
			BusinessUnit_ID,
			S.Store_Name,
			OI.OrderHeader_ID,
			ST.Subteam_Name,
			OrderType_ID,
			Return_Order,
			ISNULL(((oh.AdjustedReceivedCost + SUM(oi.ReceivedItemFreight))
					*	CASE
							WHEN OH.Return_Order = 0 THEN
								1
							ELSE 
								-1
						END),0),
			ST.Subteam_No
			
		FROM 
			OrderItem				(nolock)	OI
			INNER JOIN OrderHeader	(nolock)	OH			ON OI.OrderHeader_ID	= OH.OrderHeader_ID
			INNER JOIN @Subteam ST							ON ST.Subteam_No		= OH.Transfer_To_SubTeam
			INNER JOIN [Vendor]		(nolock)	RecvVend	ON RecvVend.Vendor_ID	= OH.ReceiveLocation_ID
			INNER JOIN StoreSubTeam	(nolock)	SSTI		ON SSTI.Store_No		= RecvVend.Store_No AND SSTI.SubTeam_No = OH.Transfer_To_SubTeam
			INNER JOIN @Store					S			ON S.Store_No			= SSTI.Store_No
			
		WHERE 
			SSTI.Team_No		= ISNULL(@Team_No, SSTI.Team_No)
			AND SSTI.SubTeam_No = ISNULL(@SubTeam_No, SSTI.SubTeam_No)
			AND OrderType_ID	= ISNULL(@OrderType_ID, OrderType_ID)
			AND CloseDate		>= @StartDate AND CloseDate < DATEADD(D,1, @EndDate)
			AND EXISTS	(	SELECT 1 
							FROM SubTeam (nolock) 
							WHERE
								SubTeam_No = OH.Transfer_To_SubTeam
								AND((SubTeamType_ID IN (1,3) AND @RetailSubTeams = 1) 
								OR (SubTeamType_ID NOT IN (1,3) AND @RetailSubTeams = 0)
								OR (@RetailSubTeams IS NULL))
						)
		GROUP BY 
			S.Store_No,
			BusinessUnit_ID,
			S.Store_Name,
			OI.OrderHeader_ID,
			st.Subteam_Name,
			OrderType_ID,
			Return_Order,
			ST.Subteam_No,
			oh.AdjustedReceivedCost
	    
	    
	DECLARE @NoPurchase table	(
									Store_No int,
									BusinessUnit_ID int,
									Store_Name varchar(255),
									SubTeam_Name varchar(255),
									Subteam_No int
								)
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
		ORDER BY Subteam_Name

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
		FROM
			@NoPurchase

	DECLARE @TransfersOut table (store_no int,  transfers numeric(9,4))
	INSERT INTO @TransfersOut
		SELECT 
			S.Store_No 
			,ISNULL((oh.AdjustedReceivedCost + SUM(oi.ReceivedItemFreight)),0)
		FROM
			OrderHeader				(nolock)	oh
			INNER JOIN OrderItem	(nolock)	oi	ON	oh.OrderHeader_ID	= oi.OrderHeader_ID
			INNER JOIN Vendor		(nolock)	v	ON	oh.Vendor_ID		= v.Vendor_ID
			INNER JOIN @Store					s	ON	s.Store_No			= v.Store_No
		WHERE
			oh.Transfer_SubTeam	<> oh.Transfer_To_Subteam
			AND oi.DateReceived	>= @startDate
			AND oi.DateReceived < DATEADD(day, 1, @EndDate)
		GROUP BY
			S.Store_no,
			oh.AdjustedReceivedCost


	DECLARE @OrdersTotal TABLE (Store_No int, BusinessUnit_ID int, Store_Name varchar(50), SubTeam_Name varchar(255), Cost money, Subteam_No int)
	INSERT INTO @OrdersTotal
		SELECT
			Store_No,
			BusinessUnit_ID,
			Store_Name,
			SubTeam_Name,
			[Cost] = SUM(	CASE 
								WHEN OI.OrderHeader_ID IS NOT NULL THEN
									(ISNULL(InvoiceCost, 0) + ISNULL(InvoiceFreight, 0)) *	CASE
																								WHEN Return_Order = 0 THEN
																									1
																								ELSE
																									-1
																							END
								ELSE
									POCost
							END),
			O.Subteam_No
		FROM
			@Orders O
			LEFT JOIN OrderInvoice (nolock) OI ON O.OrderHeader_ID = OI.OrderHeader_ID
		GROUP BY
			Store_No,Store_Name,
			BusinessUnit_ID,
			SubTeam_Name,
			O.Subteam_No 


	UPDATE @OrdersTotal
		  SET Cost = Cost - T.Transfers
	FROM @OrdersTotal OT
	JOIN @TransfersOut T ON T.Store_No = OT.Store_No


	DECLARE @sales TABLE	(
								store_no int,
								sales numeric(18,4),
								Subteam_Name varchar(100)
							)
	INSERT INTO @sales
		SELECT
			SSBI.Store_No, 
			[SalesAmount] = SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount),
			O.Subteam_Name
		FROM
			Sales_SumByItem SSBI(nolock)
			INNER JOIN @OrdersTotal	O	ON	SSBI.Subteam_No		= O.Subteam_No 
											AND SSBI.Store_No	= O.Store_No 
	WHERE
		Date_Key		>=  @StartDate
		AND Date_Key	<= @EndDate
	GROUP BY
		SSBI.Store_No,
		O.Subteam_Name


	SELECT
		Store_Name,
		OT.SubTeam_Name,
		Sales,
		Cost
	FROM 
		@OrdersTotal  OT 
		RIGHT JOIN @Sales	S	ON	S.Store_No			= OT.Store_No
									AND S.Subteam_Name	= OT.Subteam_Name
	ORDER BY
		BusinessUnit_ID

	SET NOCOUNT OFF
END	
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

