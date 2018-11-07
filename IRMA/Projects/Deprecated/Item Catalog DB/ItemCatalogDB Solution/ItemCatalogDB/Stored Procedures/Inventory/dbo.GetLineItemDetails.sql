SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLineItemDetails]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GetLineItemDetails]
GO

CREATE PROCEDURE [dbo].[GetLineItemDetails] 
	@PONUmber as int
AS
	-- **************************************************************************
	-- Procedure: GetLineItemDetails()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is called from a LineItemsData.vb
	--
	-- Modification History:
	-- Date       	Init  	TFS   	Comment
	-- 12/14/2011	BBB   	3744	coding standards;
	-- 07/02/2013	BAS		12757	Changed INNER JOIN to ItemUnit for oi.CostUnit
	--								to LEFT JOIN to handle NULL values
	-- **************************************************************************
BEGIN
	SET NOCOUNT ON;

	SELECT		
		oi.OrderHeader_ID,
		oi.OrderItem_ID,
		ii.Identifier,
		st.SubTeam_Name,
		v.Vendor_ID,
		v.CompanyName,
		ib.Brand_Name,
		i.Item_Description,
		[VIN]					=	iv.Item_ID,
		[Case_Pack]				=	vc.Package_Desc1,
		[Effective_Case_Cost]	=	oi.Cost,						
		[Effective_Unit_Cost]	=	oi.UnitCost,					
		[From_Vendor_Flag]		=	vc.FromVendor,
		[Cost_Effective_Date]	=	vc.StartDate,
		[Cost_Insert_Date]		=	vc.InsertDate,	
		[EInvoice_Cost]			=	oi.InvoiceCost,
		[ReceivedItemCost]		=	oi.ReceivedItemCost,
		[InvoiceExtendedCost]	=	oi.InvoiceExtendedCost,
		[POEffectiveCost]		=	CASE WHEN oi.AdjustedCost > 0 THEN oi.AdjustedCost ELSE oi.MarkUpCost END,
		[PO_Adjusted_Cost]		=	oi.AdjustedCost,
		[PO_Cost]				=	oi.Cost,
		[IRMACurrentCost]		=	dbo.fn_GetCurrentNetCost(oi.Item_Key, vs.Store_No),
		[Vendor_Order_UOM]		=	iuv.Unit_Name,
		[Cost_UOM]				=	iuc.Unit_Name,
		[eInvoice_UOM]			=	iue.Unit_Name,
		[cbW]					=	i.CostedByWeight,
		[Ordered_Quantity]		=	ISNULL(oi.QuantityOrdered, 0),
		[Received_Quantity]		=	ISNULL(oi.QuantityReceived, 0),
		[eInvoice_Quantity]		=	ISNULL(oi.eInvoiceQuantity, 0),
		[POExtendedCost]        =   oi.UnitExtCost * vc.Package_Desc1,
		[Einvoice_Case_Pack]	=	eii.case_pack,
		[IsEInvoicedOrder]		=	CASE 
										WHEN oh.Einvoice_Id IS NULL THEN 
											0 
										ELSE 
											1 
									END,
		[Adjusted_Cost_Reason]	=	CASE WHEN rci.ReasonCodeDesc IS NOT NULL THEN rci.ReasonCodeDesc ELSE rch.ReasonCodeDesc END,
		[ReceivingReasonCode]	=	rcr.ReasonCodeDesc,
		i.Item_Key,
		s.Store_No,
		case when oh.Einvoice_Id IS NULL then 0 else 1 end as IsEInvoicedOrder,
		oh.InvoiceNumber,
		oh.Einvoice_Id,
		oi.InvoiceExtendedCost,
		[DiscountType]			=	CASE WHEN oi.DiscountType > 0 THEN oi.DiscountType ELSE oh.DiscountType END,
		[QuantityDiscount]		=	CASE WHEN oi.QuantityDiscount > 0 THEN oi.QuantityDiscount ELSE oh.QuantityDiscount END,
		oi.Freight,
		oi.AdminNotes,
		oi.ResolutionCodeID,
		oi.PaymentTypeID,
		oi.ApprovedDate,
		oi.ApprovedByUserId,
		oi.LineItemSuspended
	FROM 
		OrderItem							(nolock) oi     
		INNER JOIN	OrderHeader				(nolock) oh		ON	oi.OrderHeader_ID					= oh.OrderHeader_ID
		INNER JOIN	Item					(nolock) i		ON	oi.Item_Key							= i.Item_Key
		INNER JOIN	ItemIdentifier			(nolock) ii		ON	i.Item_Key							= ii.Item_Key 
															AND ii.Default_Identifier				= 1
		INNER JOIN	Vendor					(nolock) v		ON	oh.Vendor_ID						= v.Vendor_ID
		INNER JOIN	Vendor					(nolock) vs		ON	oh.PurchaseLocation_ID				= vs.Vendor_ID
		INNER JOIN	Store					(nolock) s		ON	vs.Store_no							= s.Store_No
		INNER JOIN	SubTeam					(nolock) st		ON	i.SubTeam_No						= st.SubTeam_No
		INNER JOIN	ItemBrand				(nolock) ib		ON	i.Brand_ID							= ib.Brand_ID
		INNER JOIN	ItemUnit				(nolock) iuv	ON	i.Vendor_Unit_ID					= iuv.Unit_ID
		LEFT JOIN	ItemUnit				(nolock) iuc	ON	oi.CostUnit							= iuc.Unit_ID
		LEFT JOIN	ItemVendor				(nolock) iv		ON	i.Item_Key							= iv.Item_Key 
															AND v.Vendor_ID							= iv.Vendor_ID
		LEFT JOIN	VendorCostHistory		(nolock) vc		ON	oi.VendorCostHistoryID				= vc.VendorCostHistoryID
		LEFT JOIN	EInvoicing_Item			(nolock) eii	ON	oi.Item_Key							= eii.Item_Key
															AND oh.eInvoice_Id						= eii.EInvoice_id
		LEFT JOIN	ItemUnit				(nolock) iue	ON	eii.case_uom						= iue.Unit_Abbreviation
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) rch	ON	oh.ReasonCodeDetailID				= rch.ReasonCodeDetailID
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) rci	ON	oi.ReasonCodeDetailID				= rci.ReasonCodeDetailID
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) rcr	ON	oi.ReceivingDiscrepancyReasonCodeID	= rcr.ReasonCodeDetailID
	WHERE
		oh.OrderHeader_ID = @PONUmber
END
GO