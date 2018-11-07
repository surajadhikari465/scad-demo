SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderItemReceivedList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderItemReceivedList]
GO

CREATE PROCEDURE dbo.GetOrderItemReceivedList 
    @OrderHeader_ID		int,
    @Item_ID			bit
AS 
-- **************************************************************************
-- Procedure: GetOrderItemReceivedList
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
    
    SELECT 
		oi.OrderItem_ID, 
		Identifier					=	(CASE
											WHEN ISNULL(iv.Item_ID,'') > '' THEN iv.Item_ID 
											ELSE Identifier 
										END), 
       Item_Description, 
       oi.QuantityReceived, 
       oi.QuantityDiscount, 
       oi.DiscountType, 
       oi.ReceivedItemCost, 
       EstItemFreight				=	ReceivedItemFreight,
       oi.ReceivedItemHandling, 
       iuq.Unit_Name,
       oi.Package_Desc1,
       oi.Package_Desc2,
       Package_Unit					=	ISNULL(iup.Unit_Name, 'Unit'),
       oi.Cost,
       st.SubTeam_Name,
       EstUnitFreight				=	ReceivedFreight,
       oi.UnitsReceived,
       st.SubTeam_No,
       st.Team_No,
       Category_Name,
       Brand_Name, 
       ir.Origin_Name,
       ip.Origin_Name Proc_Name,
       oi.Lot_no
    FROM
		OrderHeader					(nolock)	oh
		INNER JOIN OrderItem		(nolock)	oi	ON	oh.OrderHeader_ID = oi.OrderHeader_ID
		INNER JOIN Item				(nolock)	i	ON	oi.Item_Key = i.Item_Key
		INNER JOIN Vendor			(nolock)	sv	ON	(CASE
															WHEN (OrderType_ID = 1) OR (NoDistMarkup = 1) THEN oh.ReceiveLocation_ID 
															ELSE oh.Vendor_ID 
														END = sv.Vendor_ID)
		INNER JOIN ItemUnit			(nolock)	iuq	ON	oi.QuantityUnit = iuq.Unit_ID
		INNER JOIN ItemIdentifier	(nolock)	ii	ON	ii.Item_Key = i.Item_Key AND ii.Default_Identifier = 1
		INNER JOIN SubTeam			(nolock)	st	ON	st.SubTeam_No = Transfer_To_SubTeam
		LEFT  JOIN ItemBrand		(nolock)	ib	ON	i.Brand_ID = ib.Brand_ID
		LEFT  JOIN ItemOrigin		(nolock)	ip	ON	ISNULL(oi.CountryProc_ID, i.Origin_ID) = ip.Origin_ID
		LEFT  JOIN ItemOrigin		(nolock)	ir	ON	ir.Origin_ID = ISNULL(oi.Origin_ID, i.Origin_ID)
		LEFT  JOIN ItemUnit			(nolock)	iup	ON	oi.Package_Unit_ID = iup.Unit_ID
		LEFT  JOIN ItemVendor		(nolock)	iv	ON	@Item_ID = 1 AND iv.Item_Key = oi.Item_Key AND iv.Vendor_ID = oh.Vendor_ID
		LEFT  JOIN ItemCategory		(nolock)	ic	ON	i.Category_ID = ic.Category_ID
    WHERE
		oh.OrderHeader_ID = @OrderHeader_ID
    ORDER BY
		oi.OrderItem_ID
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO