CREATE PROCEDURE [dbo].[AddOrderItemRefusedForEInvoiceExceptionItems]
	@OrderHeader_ID int,
	@EInvoice_Id	int

AS 

-- **************************************************************************
-- Procedure: AddOrderItemRefusedForEInvoiceExceptionItems
--    Author: Faisal Ahmed
--      Date: 03/28/2013
--
-- Description:
-- This procedure inserts a record into OrderItemRefused table for any einvoice 
-- exception item where refused quantity is updated by the user.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/29/2013	FA   	8325	Initial Code
-- 2013-04-05	KM		11841	Insert the correct values into OrderItemRefused;
-- **************************************************************************

BEGIN
	IF @OrderHeader_ID IS NULL OR @EInvoice_Id IS NULL RETURN
			
	DECLARE @EInvoiceExceptionItems TABLE 
	(	
		UPC					VARCHAR(255),
		VIN					VARCHAR(255),
		Item_Description	VARCHAR(255),
		Unit				VARCHAR(25),
		Cost				MONEY,
		eInvoiceQuantity	DECIMAL(18,4)
	)	

	DECLARE @CostDate smalldatetime
	SELECT @CostDate = GETDATE() + dbo.fn_GetLeadTimeDays(oh.Vendor_ID)
	FROM OrderHeader oh (nolock)
	WHERE OrderHeader_ID = @OrderHeader_ID

	-- Get Case Unit ID
	DECLARE @CaseUnitID AS int		
	SELECT @CaseUnitID = (SELECT Unit_ID FROM ItemUnit WHERE EDISysCode = 'CA')

	INSERT INTO @EInvoiceExceptionItems	
		SELECT 
			ISNULL(eii.upc,Identifier) as UPC,   
			ISNULL(eii.vendor_item_num,IV.Item_ID) as VIN,
			ISNULL(eii.descrip,Item.Item_Description) as Item_Description,
			IUnit.Unit_Abbreviation as Unit,
			Cost = 
				CASE
					WHEN eii.alt_ordering_uom IN ('CA','CS') THEN  (eii.unit_cost * eii.Qty_Shipped) 
					ELSE eii.unit_cost 									   
				END,	
			eInvoiceQuantity =
				CASE 
					-- if alt_ordering_uom it means it is not a Random Weight Item
					WHEN eii.alt_ordering_uom IS NULL THEN eii.Qty_Shipped
					-- otherwise validate type of item hosting
					WHEN eii.alt_ordering_uom IN ('CA','CS') THEN
						 CASE WHEN OI.Package_Unit_ID = @CaseUnitID THEN eii.alt_ordering_qty ELSE eii.Qty_Shipped END
					WHEN eii.alt_ordering_uom IN ('LB','EA') THEN
						 CASE WHEN OI.Package_Unit_ID = @CaseUnitID THEN eii.Qty_Shipped ELSE eii.alt_ordering_qty END			   
				END
		FROM OrderHeader (nolock)
			INNER JOIN
				Einvoicing_Header eInvHeader (NOLOCK)
				ON eInvHeader.Einvoice_id = OrderHeader.EInvoice_ID 
			INNER JOIN
				eInvoicing_Item eii (NOLOCK)
				ON eii.EInvoice_ID = eInvHeader.Einvoice_id
			LEFT JOIN
				OrderItem OI (nolock) 
				ON OrderHeader.OrderHeader_ID = OI.OrderHeader_ID AND eii.Item_Key = OI.Item_Key  
			INNER JOIN  
				Item (nolock)   
				ON eii.Item_Key = Item.Item_Key  
			INNER JOIN  
				ItemIdentifier (nolock)  
				ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1  
			LEFT JOIN
				ItemVendor IV (nolock)
				ON Item.Item_Key = IV.Item_Key
				AND OrderHeader.Vendor_Id = IV.Vendor_Id
			INNER JOIN 
				ItemUnit IUnit (nolock)
				ON IUnit.Unit_ID = Item.Package_Unit_ID
			INNER JOIN 
				Vendor V (nolock)
				ON V.Vendor_Id = OrderHeader.Vendor_ID
			INNER JOIN VENDOR RV (nolock)
				ON RV.Vendor_ID = Orderheader.ReceiveLocation_ID
			LEFT JOIN storeitemvendor SIV
				ON SIV.Store_No = RV.Store_No
				AND SIV.Vendor_ID = OrderHeader.Vendor_ID
				AND SIV.Item_Key = eii.Item_Key
			LEFT OUTER JOIN ItemOverride (nolock) ON     
				Item.Item_Key = ItemOverride.Item_Key 
				AND ItemOverride.StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store WHERE Store_No = RV.Store_No)  
		
		WHERE 
			OrderHeader.OrderHeader_ID = @OrderHeader_ID AND eii.IsNotOrdered = 1

		UNION

		SELECT 
			eii.upc as Identifier,   
			eii.vendor_item_num as VIN,
			eii.descrip as Item_Description,
			NULL as Unit,
			eii.unit_cost as Cost,
			eii.qty_shipped as eInvoiceQuantity
		FROM OrderHeader (nolock)
			INNER JOIN
				Einvoicing_Header eInvHeader (NOLOCK)
				ON eInvHeader.Einvoice_id = OrderHeader.EInvoice_ID 
			INNER JOIN
				eInvoicing_Item eii (NOLOCK)
				ON eii.EInvoice_ID = eInvHeader.Einvoice_id
			INNER JOIN 
				Vendor V (nolock)
				ON V.Vendor_Id = OrderHeader.Vendor_ID
			INNER JOIN VENDOR RV (nolock)
				ON RV.Vendor_ID = Orderheader.ReceiveLocation_ID
		WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID AND eii.IsNotIdentifiable = 1
		
	
	-- Delete any previous exceptions to prevent them from being added multiple times during a reparse.  Because an exception will
	-- not match to an OrderItem_ID, we can use that field to determine which items are exceptions.
	DELETE FROM OrderItemRefused WHERE 
		OrderHeader_ID = @OrderHeader_ID AND UserAddedEntry = 0 AND OrderItem_ID IS NULL
		
	INSERT INTO OrderItemRefused 
			(OrderHeader_ID, OrderItem_ID, Identifier, VendorItemNumber, Description, Unit, InvoiceQuantity, InvoiceCost, RefusedQuantity, DiscrepancyCodeID, UserAddedEntry)
		
		SELECT 
			@OrderHeader_ID,
			NULL,
			ei.UPC				AS	Identifier,
			ei.VIN				AS	VendorItemNumber,
			ei.Item_Description	AS	[Description],
			ei.Unit				AS	Unit,
			ei.eInvoiceQuantity	AS	InvoiceQuantity,
			ei.Cost				AS	InvoiceCost,
			ei.eInvoiceQuantity AS	RefusedQuantity,
			11					AS  DiscrepancyCodeID, -- 11 is the ID for reason code II - Refused item-Incorrect Item Delivered.
			0
			--@EInvoice_Id		AS	eInvoice_Id   
		
		FROM 
			@EInvoiceExceptionItems ei
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrderItemRefusedForEInvoiceExceptionItems] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrderItemRefusedForEInvoiceExceptionItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrderItemRefusedForEInvoiceExceptionItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrderItemRefusedForEInvoiceExceptionItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrderItemRefusedForEInvoiceExceptionItems] TO [IRMAReportsRole]
    AS [dbo];

