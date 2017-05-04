CREATE Procedure dbo.PurchasesSummaryReport
(
	@Store_No			int,
	@Team_No			int,
	@SubTeam_No			int,
	@RetailSubTeams		bit,
	@OrderType_ID		int,
	@StartDate			smalldatetime,
	@EndDate			smalldatetime
)
AS
-- **************************************************************************
-- Procedure: PurchasesSummaryReport
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/21	KM		3744	Added update history template; coding standards; extension change;
-- **************************************************************************
BEGIN
	DECLARE @Orders TABLE 
	(
		OrderHeader_ID	int, 
		SubTeam_Name	varchar(255), 
		OrderType_ID	int, 
		Return_Order	int, 
		POCost			smallmoney
	)

	INSERT INTO @Orders
	(
		OrderHeader_ID, 
		SubTeam_Name, 
		OrderType_ID, 
		Return_Order, 
		POCost
	)
	
	SELECT
		OI.OrderHeader_ID,
        Subteam_Name,
        OrderType_ID,
        Return_Order,
		SUM((ReceivedItemCost + ReceivedItemFreight) * CASE WHEN OH.Return_Order = 0 THEN 1 ELSE -1 END)
	FROM 
		OrderItem				(nolock) oi
		INNER JOIN OrderHeader	(nolock) oh		ON	OI.OrderHeader_ID = OH.OrderHeader_ID
		INNER JOIN SubTeam		(nolock) st		ON	OH.Transfer_To_SubTeam = ST.Subteam_No 
		INNER JOIN [Vendor]		(nolock) rv		ON	OH.ReceiveLocation_ID = rv.Vendor_ID 
		INNER JOIN StoreSubTeam (nolock) sst	ON	rv.Store_No = SST.Store_No 
												AND OH.Transfer_To_SubTeam = SST.SubTeam_No 
	WHERE	
		rv.Store_No			=	@Store_No
		AND	SST.Team_No		=	ISNULL(@Team_No, SST.Team_No)
		AND SST.SubTeam_No	=	ISNULL(@SubTeam_No, SST.SubTeam_No)
		AND OrderType_ID	=	ISNULL(@OrderType_ID, OrderType_ID)
		AND CloseDate		>=	@StartDate AND CloseDate < DATEADD(D,1, @EndDate)
		AND EXISTS (SELECT * 
					FROM
						SubTeam (nolock) 
					WHERE 
						SubTeam_No = OH.Transfer_To_SubTeam
						AND ((SubTeamType_ID IN (1,3) AND @RetailSubTeams = 1) 
						OR (SubTeamType_ID NOT IN (1,3) AND @RetailSubTeams = 0)
						OR (@RetailSubTeams IS NULL)))
	GROUP BY 
		OI.OrderHeader_ID,
		Subteam_Name,
		OrderType_ID,
		Return_Order

	SELECT
		SubTeam_Name, 
		OrderType, 
		Cost = SUM(Cost)
	FROM 
		(SELECT 
			SubTeam_Name,
			OrderType	=	CASE OrderType_ID 
								WHEN 1 THEN 'Purchases' 
								WHEN 2 THEN 'Distributions' 
								WHEN 3 THEN 'Transfers' 
							END,
           Cost			=	SUM(	CASE 
										WHEN ov.OrderHeader_ID IS NOT NULL THEN (ISNULL(InvoiceCost, 0) + ISNULL(InvoiceFreight, 0)) *	CASE 
																																			WHEN Return_Order = 0 THEN 1 
																																			ELSE -1 
																																		END
										ELSE POCost 
									END)
		FROM
			@Orders O
			LEFT JOIN OrderInvoice		(nolock) ov		ON O.OrderHeader_ID = ov.OrderHeader_ID
		GROUP BY
			SubTeam_Name,
			OrderType_ID
    
		UNION
	    
		SELECT
			SubTeam_Name, 
			OrderType		= 'Purchases',
			Cost			= 0
		FROM
			@Orders O
			
		UNION
	    
		SELECT 
			SubTeam_Name, 
			OrderType		= 'Distributions', 
			Cost			= 0
		FROM
			@Orders O
	    
		UNION
	    
		SELECT
			SubTeam_Name, 
			OrderType		= 'Transfers', 
			Cost			= 0
		FROM
			@Orders O) T
	
	GROUP BY
		SubTeam_Name, OrderType
	ORDER BY
		SubTeam_Name, OrderType
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurchasesSummaryReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurchasesSummaryReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurchasesSummaryReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurchasesSummaryReport] TO [IRMAReportsRole]
    AS [dbo];

