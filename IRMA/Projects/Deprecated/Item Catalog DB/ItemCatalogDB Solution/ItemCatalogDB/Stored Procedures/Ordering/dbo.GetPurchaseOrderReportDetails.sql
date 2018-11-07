SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPurchaseOrderReportDetails]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPurchaseOrderReportDetails]
GO 

CREATE PROCEDURE [dbo].[GetPurchaseOrderReportDetails] 
    @OrderHeader_ID		int,
    @Item_ID			bit,
	@SortType			tinyint,
	@GroupByCat			bit 

AS

-- ****************************************************************************************************************
-- Procedure: GetPurchaseOrderReportDetails
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from a single RDL file and generates a report consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2010/04/21	DS		12561	Incorrect number in Identifier column, took out VIN as Identifier
-- 2010/08/18	DS		2546	Added Header Level Discount to Line Item Discount as a sum in the select clause
-- 2011/12/23	KM		3744	Added update history template; extension change; coding standards;
-- 2013/01/06	KM		9251	Check ItemOverride for new 4.8 override values (Brand, Origin, CountryProc);
-- ****************************************************************************************************************

BEGIN
	SET NOCOUNT ON
    
	SELECT 
		oi.OrderItem_ID,
		VIN							=	CASE 
											WHEN ISNULL(iv.Item_ID,'') > '' THEN RTRIM(iv.Item_ID) 
											ELSE RTRIM(Identifier) 
										END, 
		ii.Identifier,
		Item_Description			=	ISNULL(ior.Item_Description, i.Item_Description),
		oi.QuantityOrdered,
		oi.QuantityReceived,
		oi.Total_Weight,
		QuantityDiscount			=	oi.QuantityDiscount + oh.QuantityDiscount,
		oi.DiscountType,
		oi.UnitCost,
		oi.UnitExtCost,
		oi.LineItemCost,
		oi.LineItemFreight,
		oi.LineItemHandling,
		HandlingCharge				=	ISNULL(oi.HandlingCharge, 0),
		iuq.Unit_Name,
		oi.Package_Desc1,
		oi.Package_Desc2,
		Package_Unit				=	ISNULL(iup.Unit_Name, 'Unit'),
		st.SubTeam_Name,
		i.SubTeam_No,
		Category_Name,
		Brand_Name,
		ig.Origin_Name,
		iop.Origin_Name Proc_Name,
		SustainabilityRankingAbbr	=	sr.RankingAbbr,
		SeafoodMissingCountryInfo	=	CAST(	CASE 
													WHEN i.SubTeam_No = 2800 AND ((oi.Origin_ID IS NULL) OR (oi.CountryProc_ID IS NULL)) THEN 1
													ELSE 0
												END AS bit),
		Pre_Order,
		i.EXEDistributed,
		oi.Lot_No,
		oi.eInvoiceQuantity,
		ActualCost					=	CASE 
											WHEN oi.AdjustedCost > 0 THEN oi.AdjustedCost
											ELSE oi.MarkUpCost
										END,
		LineItemCostDividedByQuantity = oi.LineItemCost / oi.QuantityOrdered,
		oh.CloseDate
	FROM
		OrderHeader							(NOLOCK) oh
		INNER JOIN OrderItem				(NOLOCK) oi		ON	oh.OrderHeader_ID			= oi.OrderHeader_ID
		INNER JOIN ItemUnit					(NOLOCK) iuq	ON	oi.QuantityUnit				= iuq.Unit_ID
		INNER JOIN Vendor					(NOLOCK) rv		ON  oh.ReceiveLocation_ID		= rv.Vendor_ID
		LEFT  JOIN ItemUnit					(NOLOCK) iup	ON	oi.Package_Unit_ID			= iup.Unit_ID
		LEFT  JOIN Item						(NOLOCK) i		ON	oi.Item_Key					= i.Item_Key
		LEFT  JOIN SustainabilityRanking	(NOLOCK) sr		ON	oi.SustainabilityRankingID	= sr.ID
		LEFT  JOIN Store					(NOLOCK) s		ON	rv.Store_no					= s.Store_No 
		LEFT  JOIN ItemIdentifier			(NOLOCK) ii		ON	i.Item_Key					= ii.Item_Key 
															AND ii.Default_Identifier		= 1
		LEFT  JOIN SubTeam					(NOLOCK) st		ON	Transfer_To_SubTeam			= st.SubTeam_No 
		LEFT  JOIN ItemVendor				(NOLOCK) iv		ON	@Item_ID					= 1
															AND oi.Item_Key					= iv.Item_Key 
															AND oh.Vendor_ID				= iv.Vendor_ID 
		LEFT  JOIN ItemOverride				(NOLOCK) ior	ON	i.Item_Key					= ior.Item_Key 
															AND ior.StoreJurisdictionID		= s.StoreJurisdictionID
		LEFT  JOIN ItemBrand				(NOLOCK) ib		ON	ISNULL(ior.Brand_ID, i.Brand_ID) = ib.Brand_ID
		LEFT  JOIN ItemOrigin				(NOLOCK) ig		ON	ISNULL(oi.Origin_ID, ISNULL(ior.Origin_ID, i.Origin_ID))				= ig.Origin_ID
		LEFT  JOIN ItemOrigin				(NOLOCK) iop	ON	ISNULL(oi.CountryProc_ID, ISNULL(ior.CountryProc_ID, i.CountryProc_ID))	= iop.Origin_ID 
		LEFT  JOIN ItemCategory				(NOLOCK) ic		ON	i.Category_ID				= ic.Category_ID
		
	WHERE
		oh.OrderHeader_ID		=	@OrderHeader_ID
		AND oi.QuantityOrdered	<>	0
	ORDER BY
		CASE @SortType
			WHEN 2 THEN ABS(oi.LineItemCost + oi.LineItemFreight)
			WHEN 3 THEN ii.Identifier
			WHEN 4 THEN		CASE
								WHEN iv.Item_ID IS NOT NULL	THEN RIGHT(REPLICATE('0',20)+ RTRIM(iv.Item_ID),20)
								ELSE ii.Identifier					
							END
			ELSE oi.OrderItem_ID
		END
		
    SET NOCOUNT OFF    
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO