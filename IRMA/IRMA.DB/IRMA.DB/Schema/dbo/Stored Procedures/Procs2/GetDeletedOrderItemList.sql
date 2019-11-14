CREATE PROCEDURE [dbo].[GetDeletedOrderItemList] 
	@OrderHeader_ID int,
	@Item_ID		bit,
	@SortType		tinyint = NULL

AS

	-- ***************************************************************************************************************************
	-- Procedure: GetDeletedOrderItemList
	--    Author: AlexB
	--      Date: n/a
	--
	-- Description: n/a
	--
	-- Modification History:
	-- Date       	Init  	TFS   	Comment
	--11/07/2012	AB		8312    Copied GetOrderItemList to work with deleted OrderItems
	--2013/01/04	KM		9251	Check ItemOverride for new 4.8 override values (Brand, Origin, CountryProc);
	-- ***************************************************************************************************************************

BEGIN  
    SET NOCOUNT ON
      
    SELECT 
		i.item_key,
		oi.OrderItem_ID,       
		Identifier					=	CASE
											WHEN @Item_ID = 1 THEN	CASE 
																		WHEN ISNULL(iv.Item_ID,'') > '' THEN iv.Item_ID 
																		ELSE Identifier 
																	END 
											ELSE Identifier 
										END,
           
		Item_Description			= ISNULL(ior.Item_Description, i.Item_Description),      
		oi.QuantityOrdered,
		oi.QuantityReceived,
		oi.Total_Weight,
		oi.QuantityDiscount,
		oi.DiscountType,
		oi.UnitCost,
		oi.UnitExtCost,
		[LineItemCost]				=	CASE i.CatchWeightRequired
											WHEN 1 THEN
												oi.LineItemCost
											ELSE
												ROUND(oi.LineItemCost, 2)
										END,
		oi.LineItemFreight,       
		oi.LineItemHandling,  
		HandlingCharge				= ISNULL(oi.HandlingCharge, 0),
		iuq.Unit_Name,
		oi.Package_Desc1,
		oi.Package_Desc2,
		Package_Unit				= ISNULL(iup.Unit_Name, 'Unit'),
		st.SubTeam_Name,
		i.SubTeam_No,
		Category_Name, 
		Brand_Name					= ISNULL(ibo.Brand_Name, ib.Brand_Name),
		ir.Origin_Name,
		irp.Origin_Name Proc_Name,
		SeafoodMissingCountryInfo	= Cast(0 as BIT), --The field is absoleted in IRMA. Return default in case if there's a reference to this field somewhere.
		Pre_Order,
		i.EXEDistributed,
		oi.Lot_No,
		oi.eInvoiceQuantity,
		ActualCost					=	CASE
											WHEN oi.AdjustedCost > 0 THEN 
												ROUND(oi.AdjustedCost, 2)
											ELSE 
												oi.MarkUpCost
										END,  
		iv.VendorItemDescription,
		[CurrentVendorCost]			=	CASE 
											WHEN (oh.OrderType_ID = 2 AND sv.Distribution_Center = 1) OR oh.OrderType_ID = 3 THEN 
												dbo.fn_AvgCostHistory(i.Item_Key, v.Store_no, oh.Transfer_SubTeam, GETDATE()) * oi.Package_Desc1
											ELSE 
												ISNULL(VCA.UnitCost, ISNULL(VCA2.UnitCost, 0))
										END
	
	FROM
		DeletedOrder					(nolock) oh
		INNER JOIN	DeletedOrderItem	(nolock) oi		ON	oh.OrderHeader_ID					= oi.OrderHeader_ID
		INNER JOIN	ItemUnit			(nolock) iuq	ON	oi.QuantityUnit						= iuq.Unit_ID
		INNER JOIN	Item				(nolock) i		ON	oi.Item_Key							= i.Item_Key
		INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key							= ii.Item_Key			
														AND ii.Default_Identifier				= 1
		INNER JOIN	SubTeam				(nolock) st		ON	Transfer_To_SubTeam					= st.SubTeam_No
		INNER JOIN  Vendor				(nolock) v		ON	oh.Vendor_ID						= v.Vendor_ID
		INNER JOIN	Vendor				(nolock) rv		ON	oh.ReceiveLocation_ID				= rv.Vendor_ID
		INNER JOIN	Store				(nolock) sr		ON  rv.Store_no 						= sr.Store_No	
		LEFT JOIN	Store				(nolock) sv		ON	v.Store_No 							= sv.Store_No
		LEFT JOIN	ItemBrand			(nolock) ib		ON	i.Brand_ID							= ib.Brand_ID
		LEFT JOIN	ItemUnit			(nolock) iup	ON	oi.Package_Unit_ID					= iup.Unit_ID
		LEFT JOIN	ItemVendor			(nolock) iv		ON	oi.Item_Key							= iv.Item_Key			
														AND oh.Vendor_ID						= iv.Vendor_ID 
		LEFT JOIN	ItemCategory		(nolock) ic		ON	i.Category_ID						= ic.Category_ID
		LEFT JOIN	ItemOverride		(nolock) ior	ON	i.Item_Key							= ior.Item_Key		
														AND sr.StoreJurisdictionID				= ior.StoreJurisdictionID
		LEFT JOIN	ItemOrigin			(nolock) ir		ON	ISNULL(oi.Origin_ID, ISNULL(ior.Origin_ID, i.Origin_ID))		= ir.Origin_ID		
		LEFT JOIN	ItemOrigin			(nolock) irp	ON	ISNULL(oi.CountryProc_ID, ISNULL(ior.Origin_ID, i.Origin_ID))	= irp.Origin_ID		
		LEFT JOIN	ItemBrand			(nolock) ibo	ON	ior.Brand_ID						= ibo.Brand_ID
		OUTER APPLY
			dbo.fn_VendorCost(i.Item_Key, oh.Vendor_ID, sr.Store_No, ISNULL(oh.SentDate, GETDATE()) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID)) AS vca
		OUTER APPLY
			dbo.fn_VendorCost(i.Item_Key, oh.Vendor_ID, sr.Store_No, ISNULL(oh.OrderDate-1, GETDATE()) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID)) AS vca2
    
	WHERE
		oh.OrderHeader_ID = @OrderHeader_ID      
    
	ORDER BY       
		CASE @SortType      
			WHEN 2 THEN ABS(oi.LineItemCost + oi.LineItemFreight)  
			WHEN 3 THEN Identifier      
			WHEN 4 THEN		CASE 
								WHEN iv.Item_ID IS NULL THEN iv.Item_ID 
								ELSE Identifier 
							END      
			ELSE oi.OrderItem_ID 
		END

	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDeletedOrderItemList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDeletedOrderItemList] TO [IRMASchedJobsRole]
    AS [dbo];

