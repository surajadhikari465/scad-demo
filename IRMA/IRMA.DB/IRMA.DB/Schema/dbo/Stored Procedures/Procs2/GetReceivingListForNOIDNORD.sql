CREATE PROCEDURE [dbo].[GetReceivingListForNOIDNORD] 
    @OrderHeader_ID INT

AS 

/*********************************************************************************************************************************************
CHANGE LOG
DEV				DATE				TASK		Description
----------------------------------------------------------------------------------------------
?				Nov 06, 2009		?			Script/creation date.
MYounes			Jan 13, 2011		731			Added validations to correctly show Catch Weight Items in Receiving List screen/ show corrected Cost calculation
Tom Lux			Jan 24, 2011		759			Changed @Now var to @CostDate var, added pull of vendor ID from OrderHeader, and updated to include any vendor lead-time in the date used to pull vendor cost attributes.
Hui Kou			Nov. 5, 2012		8329		Add unmatched UPC/VIN from invoice against order combo exception
Hui Kou			Nov. 9, 2012		6247		Reverse the change
Kyle Milner		2012/12/19			8747		Check ItemOverride table for Brand, CostedByWeight, Not_Available, and Not_AvailableNote values where appropriate;
Ben Sims		2013/01/07			8755		Changed Item.Discontinue_Item to SIV.DiscontinueItem due to schema change
Min Zhao        Jan 23, 2013        9882        Retrieve the brand name from e-invoice if it exists	
**********************************************************************************************************************************************/

BEGIN

	DECLARE @CostDate SMALLDATETIME
	SELECT 
		@CostDate = GETDATE() + dbo.fn_GetLeadTimeDays(oh.Vendor_ID)
	FROM 
		OrderHeader oh (NOLOCK)
	WHERE 
		OrderHeader_ID = @OrderHeader_ID

	DECLARE @CaseUnitID AS INT		
	SELECT @CaseUnitID = (SELECT Unit_ID FROM ItemUnit WHERE EDISysCode = 'CA')

	
	-- Main Select		
	SELECT 
		ISNULL(eii.upc,Identifier) AS Identifier,   
		ISNULL(eii.descrip,Item.Item_Description) AS Item_Description,
		ISNULL(ItemOverride.CostedByWeight, Item.CostedByWeight) AS CostedByWeight,  
		ISNULL(eii.vendor_item_num,IV.Item_ID) AS VendorItemID,
		ISNULL(eii.brand, ib.Brand_Name) AS Brand,
		Cost = 
			CASE
				WHEN eii.alt_ordering_uom IN ('CA','CS') THEN  (eii.unit_cost * eii.Qty_Shipped) 
				ELSE eii.unit_cost 									   
			END,	
		ISNULL(ItemOverride.Not_Available, Item.Not_Available) AS Not_Available, 
		ISNULL(ItemOverride.Not_AvailableNote, Item.Not_AvailableNote) AS Not_AvailableNote,    
		ISNULL(VCA.Package_Desc1, 0) AS Package_Desc1,
		ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2) AS Package_Desc2,
		IUnit.Unit_Abbreviation as Package_Unit,
		InvoiceQuantityUnitName =    
			CASE 
				-- if alt_ordering_uom it means it is not a Random Weight Item
				WHEN eii.alt_ordering_uom IS NULL THEN eii.case_uom
				-- otherwise validate type of item hosting
				WHEN eii.alt_ordering_uom IN ('CA','CS') THEN
					 CASE WHEN OI.Package_Unit_ID = @CaseUnitID THEN eii.alt_ordering_uom ELSE eii.case_uom END
				WHEN eii.alt_ordering_uom IN ('LB','EA') THEN
					 CASE WHEN OI.Package_Unit_ID = @CaseUnitID THEN eii.case_uom ELSE eii.alt_ordering_uom END			   
			END,
		OrderHeader.EInvoice_ID, 
		eii.IsNotOrdered,--NORD WILL HAVE AN ITEM KEY 
		(CASE WHEN eii.IsNotOrdered = 1 AND SI.Authorized = 0 THEN 'Not Authorized: item is on file in IRMA, but is not authorized for the ordering store' ELSE
			CASE WHEN eii.IsNotOrdered = 1 AND StoreItemVendorID IS NULL THEN 'Incorrect Vendor: item is on file in IRMA, but does not have a item-vendor-store association for the vendor specified on the PO' ELSE
				CASE WHEN eii.IsNotOrdered = 1 AND SIV.DiscontinueItem = 1 THEN 'Discontinued: item is on file in IRMA, but has been marked as discontinued'
				END
			END
		END) as eInvoiceItemException,
		V.CompanyName as VendorName,--DN added belwo 3 fields for the GetReceivingListForNOIDNORD report
		(SELECT top 1 Store_Name FROM Store WHERE Store_No = RV.Store_No) as Store,
		(SELECT top 1 SubTeam_Name FROM Subteam WHERE SubTeam_No = Item.SubTeam_No) as SubTeam,
		ISNULL(OI.QuantityOrdered, 0) AS QuantityOrdered,
		ISNULL(OI.Total_Weight, 0) as [Weight],
	    eInvoiceQuantity =
			CASE 
				-- if alt_ordering_uom it means it is not a Random Weight Item
				WHEN eii.alt_ordering_uom IS NULL THEN eii.Qty_Shipped
				-- otherwise validate type of item hosting
				WHEN eii.alt_ordering_uom IN ('CA','CS') THEN
					 CASE WHEN OI.Package_Unit_ID = @CaseUnitID THEN eii.alt_ordering_qty ELSE eii.Qty_Shipped END
				WHEN eii.alt_ordering_uom IN ('LB','EA') THEN
					 CASE WHEN OI.Package_Unit_ID = @CaseUnitID THEN eii.Qty_Shipped ELSE eii.alt_ordering_qty END			   
			END,
		eInvoiceWeight =   
			CASE 
				-- if alt_ordering_uom it means it is not a Random Weight Item
				WHEN eii.alt_ordering_uom IS NULL THEN 0
				-- otherwise validate type of item hosting
				WHEN eii.alt_ordering_uom IN ('CA','CS') THEN
					 CASE WHEN OI.Package_Unit_ID = @CaseUnitID THEN eii.Qty_Shipped ELSE 0 END
				WHEN eii.alt_ordering_uom IN ('LB','EA') THEN
					 CASE WHEN OI.Package_Unit_ID = @CaseUnitID THEN eii.alt_ordering_qty ELSE 0 END			   
			END,			
		eii.case_pack AS eInvoiceCase_Pack,
		eii.case_pack AS eInvoiceCase_Uom,
		eii.IsNotIdentifiable

	FROM 
		OrderHeader (NOLOCK)
		INNER JOIN	Einvoicing_Header eInvHeader		(NOLOCK)	ON	eInvHeader.Einvoice_id				= OrderHeader.EInvoice_ID 
		INNER JOIN	eInvoicing_Item eii					(NOLOCK)	ON	eii.EInvoice_ID						= eInvHeader.Einvoice_id
		LEFT JOIN	OrderItem OI						(NOLOCK)	ON	OrderHeader.OrderHeader_ID			= OI.OrderHeader_ID 
																	AND eii.Item_Key						= OI.Item_Key  
		INNER JOIN	Item								(NOLOCK)	ON	eii.Item_Key						= Item.Item_Key  
		INNER JOIN	ItemIdentifier						(NOLOCK)	ON	ItemIdentifier.Item_Key				= Item.Item_Key 
																	AND ItemIdentifier.Default_Identifier	= 1  
		LEFT JOIN	ItemVendor IV						(NOLOCK)	ON	Item.Item_Key						= IV.Item_Key
																	AND OrderHeader.Vendor_Id				= IV.Vendor_Id
		INNER JOIN	ItemUnit IUnit						(NOLOCK)	ON	IUnit.Unit_ID						= Item.Package_Unit_ID
		INNER JOIN 	Vendor V							(NOLOCK)	ON	V.Vendor_Id							= OrderHeader.Vendor_ID
		INNER JOIN	Vendor RV							(NOLOCK)	ON	RV.Vendor_ID						= Orderheader.ReceiveLocation_ID
		LEFT JOIN	StoreItemVendor SIV					(NOLOCK)	ON	SIV.Store_No						= RV.Store_No
																	AND	SIV.Vendor_ID						= OrderHeader.Vendor_ID
																	AND SIV.Item_Key						= eii.Item_Key
		LEFT JOIN	dbo.fn_VendorCostAll(@CostDate) VCA				ON	VCA.Item_Key						= Item.Item_Key 
																	AND	VCA.Store_No						= RV.Store_No 
																	AND	VCA.Vendor_ID						= OrderHeader.Vendor_Id
		LEFT JOIN	StoreItem SI						(NOLOCK)	ON	SI.Store_No							= RV.Store_No 
																	AND SI.Item_Key							= Item.Item_Key
		LEFT JOIN	ItemOverride						(NOLOCK)	ON	Item.Item_Key						= ItemOverride.Item_Key 
																	AND	ItemOverride.StoreJurisdictionID	= (SELECT StoreJurisdictionID FROM Store WHERE Store_No = RV.Store_No) 
		INNER JOIN	ItemBrand IB						(NOLOCK)	ON Item.Brand_Id						= IB.Brand_Id
		LEFT JOIN	ItemBrand ibo						(NOLOCK)	ON ItemOverride.Brand_ID				= ibo.Brand_ID	 
	
	WHERE 
		OrderHeader.OrderHeader_ID = @OrderHeader_ID   
		AND eii.IsNotOrdered = 1

UNION

	SELECT 
		eii.upc as Identifier,   
		eii.descrip as Item_Description,
		NULL,  
		eii.vendor_item_num as VendorItemID,
		eii.brand as Brand,
		eii.unit_cost as Cost,
		NULL,    
		NULL,   
		NULL,
		NULL as Package_Desc2,
		NULL as Package_Unit,
		eii.case_uom as InvoiceQuantityUnitName,
		eInvHeader.Einvoice_id, 
		eii.IsNotOrdered,--NORD WILL HAVE AN ITEM KEY 
		'Not On File: item is not on file in IRMA database' as eInvoiceItemException,
		V.CompanyName as VendorName,--DN added belwo 3 fields for the GetReceivingListForNOIDNORD report
		(SELECT top 1 Store_Name FROM Store WHERE Store_No = RV.Store_No) as Store,
		NULL as SubTeam,
		0,
		0 as Weight,
		eii.qty_shipped as eInvoiceQuantity,
		eii.alt_ordering_qty as eInvoiceWeight,
		eii.case_pack as eInvoiceCase_Pack,
		eii.case_pack as eInvoiceCase_Uom,
		eii.IsNotIdentifiable
	
	FROM 
		OrderHeader									(NOLOCK)
		INNER JOIN	Einvoicing_Header eInvHeader	(NOLOCK)	ON eInvHeader.Einvoice_id	= OrderHeader.EInvoice_ID 
		INNER JOIN	eInvoicing_Item eii				(NOLOCK)	ON eii.EInvoice_ID			= eInvHeader.Einvoice_id
		INNER JOIN  Vendor V						(NOLOCK)	ON V.Vendor_Id				= OrderHeader.Vendor_ID
		INNER JOIN	Vendor RV						(NOLOCK)	ON RV.Vendor_ID				= Orderheader.ReceiveLocation_ID
	
	WHERE 
		OrderHeader.OrderHeader_ID	= @OrderHeader_ID   
		AND eii.IsNotIdentifiable	= 1

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingListForNOIDNORD] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingListForNOIDNORD] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingListForNOIDNORD] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingListForNOIDNORD] TO [IRMAReportsRole]
    AS [dbo];

