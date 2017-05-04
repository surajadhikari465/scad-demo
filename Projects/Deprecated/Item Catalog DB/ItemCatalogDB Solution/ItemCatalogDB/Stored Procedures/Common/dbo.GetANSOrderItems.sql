SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetANSOrderItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetANSOrderItems]
GO

CREATE PROCEDURE dbo.GetANSOrderItems
	@OrderHeader_ID int

AS

	-- **************************************************************************
	-- Procedure: GetANSOrderItems()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is utilized by the SendOrdersBO.vb to select a list of order
	-- items specific to this order type.
	--
	-- Modification History:
	-- Date       	Init  	TFS   	Comment
	-- 07/14/2009	BBB				Added left joins to ItemOverride table from OrderItem in
	--								all SQL calls; added IsNull on column values that should
	--								pull from override table if value available
	-- 12/13/2011	BBB		3744	coding standards;
	-- 2013/01/06	KM		9251	Check ItemOverride for new 4.8 override values (Brand, Origin, CountryProc);
	-- 2013/07/03	BS		12727	Changed VendorOrderUOMName to iuv.Unit_Name and removed
	--								unused join to ItemUnit due to this change, and changed iuv JOIN to
	--								use oi.CostUnit so it's pulling the unit name from the actual PO
	-- 09/12/2013   MZ      13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- **************************************************************************

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON
    
	SELECT
		vendItemId						= iv.item_id,
		vendorPartNum					= RIGHT(ii.Identifier, 6),
		WFMSKU							= ii.Identifier,
		posDept							= i.SubTeam_No / 10,
		productName						= ISNULL(ior.Item_Description, i.Item_Description),
		casePack						= oi.Package_Desc1,												-- This is # units in vendor case.
		packSize						= oi.Package_Desc2,												-- This is # UOM-units in i.  EX: 500ML bottle of oil.
		UOM								= ISNULL(ivp.Unit_Abbreviation, iup.Unit_Abbreviation),			-- For oi.Package_Unit_ID.
		USPrice							= oi.UnitCost,
		quantity						= oi.QuantityOrdered,
		comment							= oi.Comments,
		BrandName						= ib.Brand_Name,
		ItemCasePack					= ISNULL(ior.Package_Desc1,i.Package_Desc1),					-- This is # units in an item-pack.
		Origin							= ig.Origin_Name,												-- Origin
		CountryOfProcessing				= ip.Origin_Name,												-- Country of Processing
		ItemUOMName						= ISNULL(ivp.Unit_Name, iup.Unit_Name),							-- For oi.Package_Unit_ID.
		VendorOrderUOMName				= iuv.Unit_Name,												-- For oi.CostUnit.
		SumAllowances					= dbo.fn_GetCurrentSumAllowances(oi.Item_Key, vp.Store_No),		-- Comes from VendorDealHistory.
		SumDiscounts					= dbo.fn_GetCurrentSumDiscounts(oi.Item_Key, vp.Store_No),		-- Comes from VendorDealHistory.
		oi.QuantityDiscount,																			-- Comes from line item order screen in IRMA.
		DiscountType					= vdt.CaseAmtType + ' ' + vdt.Description,
		oi.AdjustedCost,																				-- Comes from 'Adjusted Cost' field in line item order screen in IRMA (cost can be manually entered).
		VendorOrderUOMCost				= oi.Cost,														-- This is usually case cost, unless vendor supplies in eaches or other UOM.
		NetLineItemCost					= oi.LineItemCost												-- Qty ordered * (UOM cost - allowances/discounts).
	
	FROM 
		OrderItem					(nolock) oi
		INNER JOIN	OrderHeader		(nolock) oh		ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
		INNER JOIN	ItemIdentifier	(nolock) ii		ON	oi.Item_Key				= ii.Item_Key
													AND ii.Default_Identifier	= 1
		INNER JOIN	Item			(nolock) i		ON	oi.Item_Key				= i.Item_Key
		INNER JOIN	ItemVendor		(nolock) iv		ON	oi.Item_Key				= iv.Item_Key
													AND oh.Vendor_ID			= iv.Vendor_ID
		INNER JOIN	Vendor			(nolock) vp		ON	oh.PurchaseLocation_ID	= vp.Vendor_ID													
		INNER JOIN	Store			(nolock) s		ON	vp.Store_No				= s.Store_No
		LEFT JOIN	ItemUnit		(nolock) iup	ON	oi.Package_Unit_ID		= iup.Unit_ID
		LEFT JOIN	ItemUnit		(nolock) iuv	ON	oi.CostUnit				= iuv.Unit_ID
		LEFT JOIN	VendorDealType	(nolock) vdt	ON	oi.DiscountType			= vdt.VendorDealTypeID
		LEFT JOIN 	ItemOverride	(nolock) ior	ON  ior.Item_Key			= i.Item_Key
													AND s.StoreJurisdictionID	= ior.StoreJurisdictionID
		LEFT JOIN	ItemBrand		(nolock) ib		ON	ib.Brand_ID				= ISNULL(ior.Brand_ID, i.Brand_ID)
		LEFT JOIN	ItemOrigin		(nolock) ig		ON	ig.Origin_ID			= ISNULL(oi.Origin_ID, ISNULL(ior.Origin_ID, i.Origin_ID))
		LEFT JOIN	ItemOrigin		(nolock) ip		ON	ip.Origin_ID			= ISNULL(oi.CountryProc_ID, ISNULL(ior.CountryProc_ID, i.CountryProc_ID))
		LEFT JOIN	ItemUnit		(nolock) ivp	ON	ior.Package_Unit_ID		= ivp.Unit_ID
	
	WHERE
		oi.OrderHeader_ID = @OrderHeader_ID

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO