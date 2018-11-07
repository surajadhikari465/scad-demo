CREATE PROCEDURE dbo.GetReceivingList 
	@OrderHeader_ID	INT

AS 

-- **************************************************************************
-- Procedure: GetReceivingList()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- Called by ReceivingList.vb to return list of items for a PO to be received.
--
-- Modification History:
-- Date			Init	TFS		Comment
-- 12/13/2010	BBB		13334	added Left Join to ItemOverride and ISNULL call
--								for Item_Description
-- 01/24/2011	TTL		759		Changed @Now var to @CostDate var, added pull of vendor ID from OrderHeader, and updated to include any vendor lead-time in the date used to pull vendor cost attributes.
-- 02/21/2011	MY		1427    Changed to use EInvoiceWeight from OrderItem table for eInvoiceWeight
-- 02/23/2011	MY		1428    Populate Weight with eInvoiceWeight from OrderItem table
-- 03/09/2011	MY		1582    Added checking if it's 0 make it null
-- 06/29/2011	MD		2456   	Added CatchweightRequired field 
-- 07/25/2011	MD		2459    Returning ItemUnit EDISysCode instead of description
-- 07/26/2011	MD		2459    Added Receiving Discrepancy Reason Code ID
-- 10/13/2011	RDE		3269	Undoing a change made on 2/23/2011 (1428). Einvoicing values should ONLY populate einvoicing fields.  
-- 11/04/2011	MD		3455    Fixing the issue introduced by 3269, it was only meant for CatchWeightRequired items.
-- 11/05/2011	RDE		3455    If an item has already been received show total_weight if not, show einvoice weight if it exists. for CBW items only. 
--								NOTE: this is because we are using the weight field for more than one reason and the logic used in displaying values BEFORE the
--								item has been received does not work after the item has been received. THIS IS NOT GOOD and needs to be addressed in the future.
--							     This bandaid is required for a production breakfix.
-- 2011/12/16	KM		3744	Deprecated unused local variable; implemented Outer Apply on vca fn call; added join to Store; coding standards
-- 02/06/2012	BAS		4685	Bug Fix 4685: Updated InvoiceQuantityUnitName to have a CASE statement to check if the alt_ordering_uom field is not null and then match the
--								oi.QuantityUnit.  This bug was related to catchweight items for eInvoice orders.
-- 02.06.2012	BBB		4685	Added missiong condition on the join to EInvoicing Item;
-- 02.07.2012	BSR		4685	Fixed Billy's 'fixed' join
-- 11.02.2012	HK		6247	Add vin match in where clause
-- 11.09.2012	HK		6247	Reverse the previous change
-- 2012/12/19	KM		8747	Check ItemOverride table for Brand, CostedByWeight, Not_Available, and Not_AvailableNote values where appropriate;
-- 2013/01/23	KM		9820	Use an ISNULL in the iup join to pick up the override value if it exists;
-- 03/01/2013   FA		8325	Added code to add record to OrderItemRefused table
-- 03/11/2013	FA		8325	Added a new column to return Refused Quantity 
-- **************************************************************************

SELECT 
	oi.OrderItem_ID,   
	ii.Identifier,   
	Item_Description		=	ISNULL(ior.Item_Description, i.Item_Description),   
	oi.QuantityOrdered,   
	oi.QuantityReceived,   
	oi.QuantityDiscount,   
	oi.DiscountType,  
	QuantityUnitName		=	iuq.EDISysCode,  
	oi.eInvoiceQuantity,		
	Weight					=	CASE
									WHEN oi.quantityreceived IS NULL THEN 
										CASE
											WHEN (oi.eInvoiceWeight <> 0 AND i.CatchweightRequired = 0) THEN oi.eInvoiceWeight
											ELSE Total_Weight 
										END
									ELSE total_weight
								END,
	IsWeightRequired		=	CASE
									WHEN ISNULL(ior.CostedByWeight, i.CostedByWeight) = 1 AND iuq.IsPackageUnit = 1 THEN 1
									ELSE 0
								END,
	WeightPerPackage		=	oi.Package_Desc1 * oi.Package_Desc2,
	[CostedByWeight]		=	ISNULL(ior.CostedByWeight, i.CostedByWeight),
	i.CatchweightRequired,
	iuq.isPackageUnit,
	VendorItemID			=	iv.Item_ID,
	[Brand]					=	ISNULL(ibo.Brand_Name, ib.Brand_Name),
	Cost					=	oi.LandedCost,
	[Not_Available]			=	ISNULL(ior.Not_Available, i.Not_Available),    
	[Not_AvailableNote]		=	ISNULL(ior.Not_AvailableNote, i.Not_AvailableNote),   
	Package_Desc1			=	vca.Package_Desc1,
	Package_Desc2			=	ISNULL(ior.Package_Desc2, i.Package_Desc2),
	Package_Unit			=	iup.Unit_Abbreviation,
	eInvoiceWeight			=	CASE
									WHEN oi.eInvoiceWeight = 0 THEN NULL
									ELSE oi.eInvoiceWeight
								END,                    
	InvoiceQuantityUnitName =	CASE
									WHEN eii.alt_ordering_uom IS NOT NULL AND iuq.EDISysCode = 'CA' THEN 
										'CA'
									WHEN eii.alt_ordering_uom IS NOT NULL AND iuq.EDISysCode = 'LB' THEN 
										'LB'
									ELSE
										iiu.EDISysCode
								END,
	oi.QuantityShipped,
	oi.WeightShipped,
	eInvoiceCost			=	oi.InvoiceCost,
	iv.VendorItemDescription,
	oi.ReceivingDiscrepancyReasonCodeID,
	eii.vendor_item_num,
	oir.RefusedQuantity
FROM
	OrderItem						(nolock) oi
	INNER JOIN	Item				(nolock) i		ON	oi.Item_Key				= i.Item_Key  
	INNER JOIN	OrderHeader			(nolock) oh		ON	oi.OrderHeader_ID		= oh.OrderHeader_ID  
	INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key				= ii.Item_Key
													AND ii.Default_Identifier	= 1  
	INNER JOIN	Vendor				(nolock) pv		ON	oh.PurchaseLocation_ID	= pv.Vendor_Id
	LEFT JOIN	EInvoicing_Item		(nolock) eii	ON	oh.eInvoice_id			= eii.eInvoice_id
													AND i.Item_Key				= eii.Item_Key
	INNER JOIN	ItemUnit			(nolock) iuq	ON	oi.QuantityUnit			= iuq.Unit_ID
	LEFT  JOIN	ItemUnit 			(nolock) iiu	ON	oi.InvoiceQuantityUnit	= iiu.Unit_ID
	LEFT  JOIN	Store				(nolock) s		ON	pv.Store_no				= s.Store_No
	LEFT  JOIN	ItemVendor			(nolock) iv		ON	i.Item_Key				= iv.Item_Key
													AND oh.Vendor_Id			= iv.Vendor_Id
	LEFT  JOIN	ItemOverride		(nolock) ior	ON	i.Item_Key				= ior.Item_Key
													AND ior.StoreJurisdictionID = s.StoreJurisdictionID
	INNER JOIN	ItemBrand			(nolock) ib		ON	i.Brand_Id				= ib.Brand_Id
	LEFT JOIN	ItemBrand			(nolock) ibo	ON	ior.Brand_ID			= ibo.Brand_ID
	LEFT  JOIN	ItemUnit			(nolock) iup	ON	ISNULL(ior.Package_Unit_ID, i.Package_Unit_ID) = iup.Unit_ID
    LEFT JOIN OrderItemRefused (nolock) oir ON oh.OrderHeader_ID = oir.OrderHeader_ID
                                            AND oi.OrderItem_ID = oir.OrderItem_ID
	OUTER APPLY	(
					SELECT * 
					FROM
						dbo.fn_VendorCostAll(CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID))
					WHERE 
						Item_Key				= i.Item_Key 
						AND Vendor_ID			= iv.Vendor_ID 
						AND Store_No			= s.Store_No
				)	AS vca
			
WHERE
	oh.OrderHeader_ID = @OrderHeader_ID  
ORDER BY
	oi.OrderItem_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingList] TO [IRMAReportsRole]
    AS [dbo];

