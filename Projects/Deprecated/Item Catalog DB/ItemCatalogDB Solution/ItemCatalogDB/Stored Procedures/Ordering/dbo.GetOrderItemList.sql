SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderItemList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderItemList]
GO 

CREATE PROCEDURE [dbo].[GetOrderItemList] 
	@OrderHeader_ID int,  
    @Item_ID		bit,  
	@SortType		tinyint = NULL  

AS

-- ***************************************************************************************************************************
-- Procedure: GetOrderItemList
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/29	KM		3744	Added update history template; coding standards; extension change;
-- 2011/12/29	BJL		3751	Added CurrentVendorCost in return, from GetOrderItemInfo
-- 2012/02/03	DBS		4683	Added second call to fn_GetVendorCost with the date backdated to orderdate-1 so it 
--								won't throw an error in the case of the item becoming deauthed;
-- 2012.02.07	BBB		4623	Added rounding logic for non-catchweight items for discounts applied;
-- 2012/02/07	DBS		4687	Added wrapped ISNULL around second call to fn_GetVendorCost setting to 0 so form doesn't bomb;
-- 2013/01/06	KM		9251	Check ItemOverride for new 4.8 override values (Brand, Origin, CountryProc); 
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
		Brand_Name,       
		ir.Origin_Name,      
		irp.Origin_Name Proc_Name,      
		SeafoodMissingCountryInfo	= CAST(	CASE 
												WHEN i.SubTeam_No = 2800 AND ((oi.Origin_ID IS NULL) OR (oi.CountryProc_ID IS NULL)) THEN 1      
												ELSE 0      
											END AS bit),      
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
		[CurrentVendorCost]				=	CASE 
												WHEN (oh.OrderType_ID = 2 AND sv.Distribution_Center = 1) OR oh.OrderType_ID = 3 THEN 
													dbo.fn_AvgCostHistory(i.Item_Key, v.Store_no, oh.Transfer_SubTeam, GETDATE()) * oi.Package_Desc1
												ELSE 
													ISNULL(VCA.UnitCost, ISNULL(VCA2.UnitCost, 0))
											END
	FROM
		OrderHeader						(nolock) oh
		INNER JOIN	OrderItem			(nolock) oi		ON	oh.OrderHeader_ID					= oi.OrderHeader_ID
		INNER JOIN	ItemUnit			(nolock) iuq	ON	oi.QuantityUnit						= iuq.Unit_ID
		INNER JOIN	Item				(nolock) i		ON	oi.Item_Key							= i.Item_Key
		INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key							= ii.Item_Key			
														AND ii.Default_Identifier				= 1
		INNER JOIN	SubTeam				(nolock) st		ON	Transfer_To_SubTeam					= st.SubTeam_No
		INNER JOIN  Vendor				(nolock) v		ON	oh.Vendor_ID						= v.Vendor_ID
		INNER JOIN	Vendor				(nolock) rv		ON	oh.ReceiveLocation_ID				= rv.Vendor_ID
		INNER JOIN	Store				(nolock) sr		ON  rv.Store_no 						= sr.Store_No	
		LEFT JOIN	Store				(nolock) sv		ON	v.Store_No 							= sv.Store_No
		LEFT JOIN	ItemUnit			(nolock) iup	ON	oi.Package_Unit_ID					= iup.Unit_ID
		LEFT JOIN	ItemVendor			(nolock) iv		ON	oi.Item_Key							= iv.Item_Key			
														AND oh.Vendor_ID						= iv.Vendor_ID 
		LEFT JOIN	ItemCategory		(nolock) ic		ON	i.Category_ID						= ic.Category_ID
		LEFT JOIN	ItemOverride		(nolock) ior	ON	i.Item_Key							= ior.Item_Key		
														AND sr.StoreJurisdictionID				= ior.StoreJurisdictionID
		LEFT JOIN	ItemBrand			(nolock) ib		ON	ISNULL(ior.Brand_ID, i.Brand_ID)	= ib.Brand_ID
		LEFT JOIN	ItemOrigin			(nolock) ir		ON	ISNULL(oi.Origin_ID, ISNULL(ior.Origin_ID, i.Origin_ID))		= ir.Origin_ID		
		LEFT JOIN	ItemOrigin			(nolock) irp	ON	ISNULL(oi.CountryProc_ID, ISNULL(ior.Origin_ID, i.Origin_ID))	= irp.Origin_ID		
		OUTER APPLY
			dbo.fn_VendorCost(i.Item_Key, oh.Vendor_ID, sr.Store_No, ISNULL(oh.SentDate, GETDATE()) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID)) AS vca
		OUTER APPLY
			dbo.fn_VendorCost(i.Item_Key, oh.Vendor_ID, sr.Store_No, ISNULL(oh.OrderDate - 1, GETDATE()) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID)) AS vca2
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO