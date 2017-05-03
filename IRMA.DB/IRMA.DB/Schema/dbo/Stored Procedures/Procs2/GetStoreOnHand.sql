CREATE PROCEDURE [dbo].[GetStoreOnHand]  
	@Item_Key	int,
	@Store_No	int,
	@SubTeam_No int
AS

-- **************************************************************************
-- Procedure: GetStoreOnHand
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/19	KM		3744	Added update history template; code formatting.
-- **************************************************************************

SELECT
	Store_No		=	@Store_No,
	SubTeam_Name,
	AvgWeight		=	ISNULL(	CASE 
									WHEN Weight = 0 OR Quantity = 0 THEN 0
									ELSE Weight / Quantity
								END, 0),
	OnHand			=	ISNULL(Quantity, 0),
	Weight			=	ISNULL(Weight, 0),
	AvgCost			=	ISNULL((SELECT TOP 1
									AvgCost
								FROM
									AvgCostHistory (nolock)
								WHERE
									Item_Key = @Item_Key
										AND Store_No		=	@Store_No
										AND SubTeam_No		=	ISNULL(oa.SubTeam_No, i.SubTeam_No)
										AND Effective_Date	<=	GETDATE()
								ORDER BY
									Effective_Date DESC), 0),
	LastOrderedCost	=	ISNULL((SELECT TOP 1
									(ReceivedItemCost + ReceivedItemFreight) / UnitsReceived
								FROM
									OrderItem				(nolock)	oi
									INNER JOIN	OrderHeader	(nolock)	oh	ON	oi.OrderHeader_ID = oh.OrderHeader_ID
									INNER JOIN	Vendor		(nolock)	sv	ON	oh.ReceiveLocation_ID = sv.Vendor_ID
								WHERE
									oi.Item_Key					= @Item_Key 
									AND sv.Store_No				= @Store_No 
									AND oh.Transfer_To_SubTeam	= ISNULL(oa.SubTeam_No, i.SubTeam_No)
									AND Return_Order			= 0
									AND UnitsReceived			> 0
								ORDER BY
									DateReceived DESC), 0),
	LastCostUnit	=	(SELECT
							Unit_Abbreviation 
						FROM
							ItemUnit
						WHERE
							Unit_ID =	(SELECT TOP 1
											CostUnit
										FROM
											OrderItem				(nolock)	oi
											INNER JOIN OrderHeader	(nolock)	oh	ON	oi.OrderHeader_ID = oh.OrderHeader_ID
											INNER JOIN Vendor		(nolock)	sv	ON	oh.ReceiveLocation_ID = sv.Vendor_ID
										WHERE
											oi.Item_Key					= @Item_Key
											AND sv.Store_No				= @Store_No
											AND oh.Transfer_To_SubTeam	= ISNULL(oa.SubTeam_No, i.SubTeam_No)
											AND Return_Order			= 0
											AND UnitsReceived			> 0
										ORDER BY 
											DateReceived DESC)) 
FROM
	Item				(nolock)	i
	LEFT JOIN	OnHand	(nolock)	oa	ON	i.Item_Key							= oa.Item_Key
										AND @Store_No							= oa.Store_No
	INNER JOIN	SubTeam (nolock)	st	ON  ISNULL(oa.SubTeam_No, i.SubTeam_No) = st.SubTeam_No
WHERE
	i.Item_Key			= @Item_Key
	AND oa.Store_No		= @Store_No
	AND oa.SubTeam_No	= @SubTeam_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreOnHand] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreOnHand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreOnHand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreOnHand] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreOnHand] TO [IRMAExcelRole]
    AS [dbo];

