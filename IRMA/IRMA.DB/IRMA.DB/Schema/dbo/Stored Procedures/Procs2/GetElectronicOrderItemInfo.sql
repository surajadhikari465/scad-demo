CREATE PROCEDURE dbo.GetElectronicOrderItemInfo
    @OrderHeader_ID		int

AS

	-- **************************************************************************
	-- Procedure: GetElectronicOrderItemInfo()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is utilized by the Orders.vb to select a list of order
	-- items specific to this order type.
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 07/14/2009	BBB		Added left joins to ItemOverride table from OrderItem in
	--						all SQL calls; added IsNull on column values that should
	--						pull from override table if value available
	-- 2011/12/23	KM		Extension change; code readability
	-- 2012/10/08   RDE		Add CreditReason_Id and Lot_No for POFlip credit support
	-- 2012/01/04	KM		Search ItemOverride for Brand value;
	-- **************************************************************************

BEGIN
    SET NOCOUNT ON

    SELECT 
		UPC					=	II.Identifier, 
		VID					=	IV.Item_ID, 
		Case_Pack			=	dbo.fn_GetCurrentVendorPackInfo(OI.Item_Key,rv.Store_No,OH.Vendor_ID,1), 
		Qty					=	OI.QuantityOrdered,
		Unit_Cost			=	CASE
									WHEN oi.AdjustedCost > 0 THEN oi.AdjustedCost -	CASE oi.DiscountType 
																						WHEN 1 THEN oi.QuantityDiscount
																						WHEN 2 THEN ( oi.QuantityDiscount/100 * oi.AdjustedCost )
																						ELSE 0
																					END  
	                                WHEN oi.AdjustedCost = 0 AND oi.LineItemCost = 0 THEN 0 -- this happens with 100% header discounts.
									ELSE oi.Cost -	CASE oi.DiscountType 
														WHEN 1 THEN oi.QuantityDiscount
														WHEN 2 THEN ( oi.QuantityDiscount/100 * oi.Cost )
														ELSE 0
													END  
								END,
		Pack_Size			=	ISNULL(ior.Package_Desc2, I.Package_Desc2),
		Item_UOM			=	IUR.Unit_Name, 
		Description			=	ISNULL(IV.VendorItemDescription, ISNULL(ior.Item_Description, I.Item_Description)), 
		POS_Dept			=	I.SubTeam_No,
		Brand				=	ISNULL(ibo.Brand_Name, ib.Brand_Name), 
		Case_UOM			=	iuq.Unit_Name,
		CreditReason_Id		=	oi.CreditReason_ID,
		Lot_No				=   oi.Lot_No,
		UOM_IsCase			=	CASE iuq.Unit_Name 
									WHEN 'CASE' THEN 1 
									WHEN 'BOX' THEN 1 
									ELSE 0 
								END 
    
	FROM 
		OrderHeader					(nolock) oh
		JOIN OrderItem				(nolock) oi		ON	OH.OrderHeader_ID		= OI.OrderHeader_ID 
		JOIN ItemIdentifier			(nolock) ii		ON	OI.Item_Key				= II.Item_Key 
													AND II.Default_Identifier	= 1
		JOIN Item					(nolock) i		ON	OI.Item_Key				= I.Item_Key 
		JOIN Vendor					(nolock) rv		ON	OH.ReceiveLocation_ID	= rv.Vendor_ID 
		JOIN ItemVendor				(nolock) iv		ON	OH.Vendor_ID			= IV.Vendor_ID 
													AND OI.Item_Key				= IV.Item_Key 
		JOIN ItemUnit				(nolock) iuq	ON	OI.QuantityUnit			= iuq.Unit_ID 
		JOIN ItemBrand				(nolock) ib		ON	I.Brand_ID				= IB.Brand_ID 
		LEFT JOIN ItemOverride		(nolock) ior	ON	I.Item_Key				= ior.Item_Key 
													AND ior.StoreJurisdictionID =	(SELECT 
																						StoreJurisdictionID 
																					FROM 
																						Store		(nolock) s
																						JOIN Vendor (nolock) v	ON	s.Store_No = v.Store_No 
																					WHERE
																						v.Vendor_ID = OH.PurchaseLocation_ID)
		LEFT JOIN ItemBrand			(nolock) ibo	ON	ior.Brand_ID			= ibo.Brand_ID
		JOIN ItemUnit IUR ON IUR.Unit_ID = ISNULL(ior.Package_Unit_ID,I.Package_Unit_ID)
    
	WHERE
		OH.OrderHeader_ID = @OrderHeader_ID 
		AND IV.DeleteDate IS NULL 

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetElectronicOrderItemInfo] TO [IRMAClientRole]
    AS [dbo];

