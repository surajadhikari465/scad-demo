-- =========================================================================
-- Author:		Sekhara
-- Create date: 12/07/2007
-- To fetch the data required for Short report.
-- =========================================================================

CREATE PROCEDURE [dbo].[ShortsReport]
	(
		@Warehouse int,
		@SubTeam int,
        @BeginDate DateTime = null,
		@EndDate DateTime = null
	)
	/*
		grant exec on dbo.ShortsReport to IRMAClientRole
		grant exec on dbo.ShortsReport to IRMAReportsRole
		grant exec on dbo.ShortsReport to IRMAExcelRole
	*/
AS
BEGIN

select (CASE WHEN ii.Identifier IS NOT NULL THEN RIGHT('000000000000'+ii.Identifier, 12) END) as SKU-- Pad the SKUs to 12 characters
	,	I.Item_Description as Description
	,	IsNULL(dbo.fn_GetCurrentOnHand(oi.Item_Key,V.Store_no,oh.Transfer_SubTeam),0.0000) as CasesOnHand
	,	SUM(isnull(oi.QuantityOrdered,0)) as QtyOrdered
	,	SUM(isnull(QuantityAllocated,0)) as QtyAllocated
	,   (SUM(isnull(oi.QuantityOrdered,0)) - SUM(isnull(QuantityAllocated,0))) as QtyShorted   
	,	LandedCost * (SUM(isnull(oi.QuantityOrdered,0)) - SUM(isnull(QuantityAllocated,0))) as DollarValueLost
	,	V.Store_no as Storeno
	,	Store_Name as StoreName
	,	ST.SUBTEAM_NAME as SubTeam
	FROM  OrderHeader oh (NOLOCK)
		INNER join OrderItem oi (NOLOCK) on oh.OrderHeader_ID = oi.OrderHeader_ID
		INNER JOIN Vendor V (NOLOCK) ON V.Vendor_ID = OH.ReceiveLocation_ID 
		INNER join Item I (NOLOCK) on I.Item_Key = oi.Item_Key
		INNER join ItemIdentifier ii (NOLOCK) on I.Item_Key = ii.Item_Key  AND DELETED_IDENTIFIER = 0 and default_identifier = 1--always 
		INNER JOIN STORE ON STORE.Store_No = V.Store_No AND Distribution_center<>1  --  all stores related to ware house.
		INNER JOIN SUBTEAM ST (NOLOCK) ON ST.SUBTEAM_NO = OH.Transfer_SubTeam
		where 
		-- filter for Vendor input criteria
			--DATEDiff(day,Sentdate,getDate()) = 0--DN 
			Sentdate Between @BeginDate and @EndDate
			and
			oh.Vendor_ID in 
				(
					SELECT Vendor_ID FROM Vendor
					where store_no=@Warehouse  -- fetching orders from WareHouse.
				)
			and Transfer_SubTeam = IsNull(@SubTeam,Transfer_SubTeam) 	-- filter for "store transfers"
            And OH.CloseDate IS NULL--DN taken out in final version
            AND OH.Return_order = 0 -- POs only, no credits (returns)
            and OH.OrderType_ID=2
            and V.Store_No IS NOT NULL
			
			GROUP BY OH.Vendor_ID, OI.Item_Key, V.Store_No, OH.Transfer_SubTeam,ii.Identifier,I.Item_Description,
			I.Category_ID,Store_Name,SUBTEAM_NAME,I.CostedByWeight,OI.Package_Desc1,OI.Package_Desc2,OI.Package_Unit_ID, I.SubTeam_No
			,LandedCost
			HAVING (SUM(isnull(QuantityAllocated,0)) <  SUM(isnull(oi.QuantityOrdered,0)))

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShortsReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShortsReport] TO [IRMAReportsRole]
    AS [dbo];

