CREATE PROCEDURE dbo.GetAvgCostDist
	@Vendor_ID			int,
	@Transfer_SubTeam	int,
   	@StoreList			varchar(8000),
	@StoreListSeparator char(1),    
	@StartRecvDate		datetime,
	@EndRecvDate		datetime
AS

-- **************************************************************************
-- Procedure: GetAvgCostDist
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/21	KM		3744	added update history template; code formatting; extension change
-- **************************************************************************

BEGIN
    SET NOCOUNT ON

    DECLARE @Orders TABLE (OrderHeader_ID int)

    INSERT INTO
		@Orders
    SELECT
		OrderHeader_ID
    FROM
		OrderHeader				(nolock)	oh
		INNER JOIN Vendor		(nolock)	v	ON oh.Vendor_ID				= v.Vendor_ID 
		INNER JOIN Store		(nolock)	sv	ON v.Store_No				= sv.Store_No
		INNER JOIN Vendor		(nolock)	pv  ON oh.PurchaseLocation_ID	= pv.Vendor_ID
		INNER JOIN Store		(nolock)	ps	ON pv.Store_No				= ps.Store_No
		INNER JOIN fn_Parse_List(@StoreList, @StoreListSeparator) Store
												ON pv.Vendor_ID				= Store.Key_Value
    WHERE 
		(oh.Vendor_ID					= @Vendor_ID
			AND oh.Transfer_SubTeam		= ISNULL(@Transfer_SubTeam, oh.Transfer_SubTeam)
			AND oh.Transfer_To_Subteam	= oh.Transfer_Subteam
			AND Return_Order			= 0)
		AND ((CloseDate > @StartRecvDate)
			AND (CloseDate < DATEADD(day, 1, @EndRecvDate)))
    
    SELECT 
		ps.Store_Name, 
        oh.OrderHeader_ID, 
        Identifier, 
        Item_Description, 
	    ExtCost					=	ISNULL(dbo.fn_AvgCostHistory(oi.Item_Key, sv.Store_No, @Transfer_SubTeam, DateReceived), 0), 
        TotQuantity				=	UnitsReceived, 
        oi.QuantityReceived,
        QuantityUnit_Abbr		=	Unit_Abbreviation, 
        zs.Distribution_Markup, 
        UnitExtCost				=	CASE
										WHEN UnitsReceived > 0 THEN (ReceivedItemCost + ReceivedItemFreight) / UnitsReceived 
										ELSE 0 
									END, 
	    Retail					=	dbo.fn_PriceHistoryRegPrice(oi.Item_Key, ps.Store_No, oi.DateReceived),
        oi.Package_Desc1,
        i.Package_Desc2,
        i.CostedByWeight
    
    FROM
		OrderHeader					(nolock)	oh
		INNER JOIN @Orders						O	ON	oh.OrderHeader_ID			= O.OrderHeader_ID 
		INNER JOIN Vendor			(nolock)	v	ON	oh.Vendor_ID				= v.Vendor_ID
		INNER JOIN Store			(nolock)	sv	ON	v.Store_No					= sv.Store_No
		INNER JOIN Vendor			(nolock)	pv	ON	oh.PurchaseLocation_ID		= pv.Vendor_ID 
		INNER JOIN Store			(nolock)	ps	ON	pv.Store_No					= ps.Store_No 
		INNER JOIN fn_Parse_List(@StoreList, @StoreListSeparator) Store
													ON  pv.Vendor_ID				= Store.Key_Value
		INNER JOIN ZoneSupply		(nolock)	zs	ON	sv.Zone_ID					= zs.FromZone_ID
													AND ps.Zone_ID					= zs.ToZone_ID 
													AND oh.Transfer_SubTeam			= zs.SubTeam_No 
		INNER JOIN OrderItem		(nolock)	oi	ON	oh.OrderHeader_ID			= oi.OrderHeader_ID 
		INNER JOIN Item				(nolock)	i	ON	oi.Item_Key					= i.Item_Key 
		INNER JOIN ItemIdentifier	(nolock)	ii	ON	i.Item_Key					= ii.Item_Key 
													AND Default_Identifier			= 1
		INNER JOIN ItemUnit			(nolock)	iuq	ON	oi.QuantityUnit				= iuq.Unit_ID 
    
    WHERE 
    	Deleted_Item = 0 AND Remove_Item = 0

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostDist] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostDist] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostDist] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostDist] TO [IRMAReportsRole]
    AS [dbo];

