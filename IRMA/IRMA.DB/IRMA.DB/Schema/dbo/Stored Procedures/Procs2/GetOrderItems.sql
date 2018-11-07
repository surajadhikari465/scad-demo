CREATE PROCEDURE dbo.GetOrderItems
    @OrderHeader_ID		int

AS

-- *************************************************************************************************
-- Procedure: GetOrderItems
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/23	KM		3744	Added update history template; extension change;
--								coding standards
-- 2012/01/26   FA      3007    Merged code from v4.3.4. Added 'OrderUOMAbbr' 
-- 2012/10/30	BS		2752	Added 'eInvoiceQuantity', 'eInvoiceWeight', 'InvoiceQuantityUnitName'
--                              'ReceivingDiscrepancyReasonID' &  (for handheld enhancements)
-- 2012.12.05	BBB		9287	Changed IJ on IV to LJ for interstore transfer where OH.Vendor_Id is store
-- 2013-01-02	KM		9685	Make Unit_Abbreviation the default value for InvoiceQuantityUnit so that it
--								matches the ordered UOM;
-- 2013-03-12	KM		11535	Add a join to ItemOverride in order to pick up alternate jurisdiction Item_Description and CostedByWeight;
-- *************************************************************************************************

BEGIN
    SET NOCOUNT ON

    SELECT 
		oi.OrderItem_ID,   
        oi.Item_Key,   
        II.Identifier,   
        oi.Package_Desc1,  
        oi.Package_Desc2,  
        oi.Package_Unit_ID,  
        QuantityUnit,  
        QuantityOrdered,  
		QuantityReceived,  
		Total_Weight,  
		IsReceivedWeightRequired	= ISNULL(iov.CostedByWeight, i.CostedByWeight), 
		ItemUnit					= NULL, 
		VendorItemID				= ISNULL(IV.Item_ID, ''),  
		Item_Description			= ISNULL(iov.Item_Description, i.Item_Description), 
		OrderUnit					= NULL,  
		oi.LineItemCost,  
		oi.LineItemFreight,  
		UOMUnitCost					= dbo.fn_CostConversion(oi.Cost, oi.CostUnit, oi.QuantityUnit, oi.Package_Desc1, oi.Package_Desc2, oi.Package_Unit_ID),  
		LN.LineNumber,             
		ItemAllowanceDiscountAmount = ABS(oi.QuantityOrdered * (oi.LandedCost - dbo.fn_CostConversion (oi.Freight, oi.FreightUnit, oi.QuantityUnit, oi.Package_Desc1, oi.Package_Desc2, oi.Package_Unit_ID) - dbo.fn_CostConversion (oi.Cost, oi.CostUnit, oi.QuantityUnit, oi.Package_Desc1, oi.Package_Desc2, oi.Package_Unit_ID))),  
		oi.QuantityDiscount,  
		oi.DiscountType,  
		oi.CostUnit,  
		UOMAdjustedUnitCost			= dbo.fn_CostConversion(oi.AdjustedCost, oi.CostUnit, oi.QuantityUnit, oi.Package_Desc1, oi.Package_Desc2, oi.Package_Unit_ID)  ,
		oi.QuantityShipped,
		i.CatchweightRequired,
		PackageUnitAbbr				= iup.Unit_Abbreviation,
		OrderUOMAbbr                = iuq.Unit_Abbreviation,
		oi.eInvoiceQuantity,
		oi.eInvoiceWeight,
		InvoiceQuantityUnitName		=	CASE
											WHEN eii.alt_ordering_uom IS NOT NULL AND iuq.EDISysCode = 'CA' THEN 
												'CA'
											WHEN eii.alt_ordering_uom IS NOT NULL AND iuq.EDISysCode = 'LB' THEN 
												'LB'
											ELSE
												iuq.Unit_Abbreviation
										END,
		oi.ReceivingDiscrepancyReasonCodeID
    
	FROM
		OrderItem					(nolock) oi
		INNER JOIN ItemIdentifier	(nolock) ii		ON	oi.Item_Key				= ii.Item_Key 
													AND Default_Identifier		= 1  
		INNER JOIN OrderHeader		(nolock) oh		ON	oi.OrderHeader_ID		= OH.OrderHeader_ID 
		INNER JOIN Vendor			(nolock) v		ON	oh.ReceiveLocation_ID	= v.Vendor_ID
		INNER JOIN Store			(nolock) s		ON  v.Store_no				= s.Store_No
		INNER JOIN Item				(nolock) i		ON	oi.Item_Key				= i.Item_Key  
		LEFT JOIN  ItemOverride		(nolock) iov	ON  i.Item_Key				= iov.Item_Key
													AND s.StoreJurisdictionID	= iov.StoreJurisdictionID
		LEFT JOIN  ItemVendor		(nolock) iv		ON	oi.Item_Key				= IV.Item_Key 
													AND OH.Vendor_ID			= IV.Vendor_ID  
		LEFT JOIN  EInvoicing_Item	(nolock) eii	ON	oh.eInvoice_id			= eii.eInvoice_id
													AND i.Item_Key				= eii.Item_Key
		INNER JOIN ItemUnit			(nolock) iup	ON	oi.Package_Unit_ID		= iup.Unit_ID  
		INNER JOIN ItemUnit			(nolock) iuq	ON	oi.QuantityUnit			= iuq.Unit_ID
		LEFT  JOIN ItemUnit 		(nolock) iiu	ON	oi.InvoiceQuantityUnit	= iiu.Unit_ID 
		INNER JOIN (	SELECT 
							ROW_NUMBER() 
						OVER 
							(ORDER BY 
								OI.OrderItem_ID) As LineNumber, OI.OrderItem_ID 
						FROM 
							OrderItem OI 
						WHERE
							OI.OrderHeader_ID = @OrderHeader_ID) LN ON oi.OrderItem_ID = LN.OrderItem_ID 
    
	WHERE 
		oi.OrderHeader_ID = @OrderHeader_ID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItems] TO [IRMAReportsRole]
    AS [dbo];

