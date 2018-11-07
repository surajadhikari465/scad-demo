CREATE PROCEDURE dbo.GetCurrentPrices
    @Item_Key int
AS
/*
    #############################################################################
		Procedure:	GetCurrentPrices
   
		Modification History:
		Date        Init	Comment
		
		07/26/2010	MU		incorporated vendor pack into calculation for MarginPercentCurrCost (bug 13051)
		
		07/08/2010  RDE		modified calculations for TFS 12816. Added Avg Cost and 
   							Current Cost calculations for Margin. RegCost and NetCost
   							are now divided by packsize (unit cost)
   					
		11/06/2009  BBB		update existing SP to specifically declare table source 
   							for BusinessUnit_ID column to prevent ambiguity between
   							Store and Vendor table

		02/26/2011  Faisal Ahmed - Modified the cost calculations for the items that are
		            bought by weight but sold as each

		12/15/2012  Faisal Ahmed - Added Currency in the SELECT
							bought by weight but sold as each

		02/27/2012	BAS		TFS 1561	Added filter in WHERE clause to only include
										WFM_Stores or MegaStores and replaced
										GetCustomerType function
    #############################################################################
*/
BEGIN
    SET NOCOUNT ON
	   
   		SELECT	Price.Store_No, 
				Store_Name, 
				Multiple, 
				Price, 
				MSRPPrice, -- Added to resolve bug 3125	
				POSPrice,
				ROUND(	dbo.fn_Price	(	
											Price.PriceChgTypeID, 
											Multiple, 
											Price, 
											PricingMethod_ID, 
											Sale_Multiple, 
											Sale_Price
										) * CasePriceDiscount * Item.Package_Desc1, 2) As Case_Price,
				Sale_Multiple, 
				Sale_Price,
				POSSale_Price,
				Sale_Start_Date, 
				Sale_End_Date, 
				CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(Item.Item_key) = 1 THEN
    					ISNULL(	dbo.fn_AvgCostHistory(Item.Item_Key, Price.Store_No, Item.SubTeam_No, GETDATE()), 0) * dbo.fn_GetAverageUnitWeightByItemKey(Item.Item_Key)
					ELSE
    					ISNULL(	dbo.fn_AvgCostHistory(Item.Item_Key, Price.Store_No, Item.SubTeam_No, GETDATE()), 0) 
    				END AS AvgCost, 
    		   
    		    --NET COST = (REG COST - NET DISCOUNT + FREIGHT)
    			CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(Item.Item_key) = 1 THEN
						CAST(
							ROUND(
								dbo.fn_GetCurrentCost(											-- Curent Cost
														@Item_Key, 
														Store.Store_No
													   )
								/ ISNULL(	dbo.fn_GetCurrentVendorPackInfo	(					-- divide CurrentCost by packsize.
																				Item.Item_Key, 
																				Price.Store_No,
																				SIV.Vendor_Id
																				,1
																			),1) , 4)  AS decimal(10,4)) * dbo.fn_GetAverageUnitWeightByItemKey(Item.Item_Key)
    				ELSE
						CAST(
							ROUND(
								dbo.fn_GetCurrentCost(											-- Curent Cost
														@Item_Key, 
														Store.Store_No
													   )
								/ ISNULL(	dbo.fn_GetCurrentVendorPackInfo	(					-- divide CurrentCost by packsize.
																				Item.Item_Key, 
																				Price.Store_No,
																				SIV.Vendor_Id
																				,1
																			),1) , 4)  AS decimal(10,4)) 
					END AS CurrentRegCost,				-- cast as decimal

    			-- NET UNIT COST
    			CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(Item.Item_key) = 1 THEN
    			    	CAST(ROUND(ISNULL(dbo.fn_GetCurrentNetCost(Item.Item_Key, Price.Store_No), 0) 
    					/ ISNULL(dbo.fn_GetCurrentVendorPackInfo(Item.Item_Key, Price.Store_No,SIV.Vendor_Id,1),1),4) as decimal(10,4)) * dbo.fn_GetAverageUnitWeightByItemKey(Item.Item_Key)
    				ELSE
    					CAST(ROUND(ISNULL(dbo.fn_GetCurrentNetCost(Item.Item_Key, Price.Store_No), 0) 
    					/ ISNULL(dbo.fn_GetCurrentVendorPackInfo(Item.Item_Key, Price.Store_No,SIV.Vendor_Id,1),1),4) as decimal(10,4)) 
    				END As NetUnitCost,							

				-- MARGIN% AVERAGE COST
    			( 100 * (( ISNULL(POSPrice,0) - 
    						CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(Item.Item_key) = 1 THEN
    								ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, Price.Store_No, Item.SubTeam_No, GETDATE()), 0) * dbo.fn_GetAverageUnitWeightByItemKey(Item.Item_Key)
								ELSE
    								ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, Price.Store_No, Item.SubTeam_No, GETDATE()), 0)
    							END
    						) / case POSPrice when 0 then 1 else isnull(POSPrice,1) end )) 
    			as MarginPercentAvgCost,
    				
				-- MARGIN% CURRENT COST
    			CAST(round( 100 * 
    						((ISNULL(POSPrice,0) -  
    								CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(Item.Item_key) = 1 THEN
    								((ISNULL(dbo.fn_GetCurrentNetCost(@Item_Key, Store.Store_No ), 0))
    									/ ISNULL(dbo.fn_GetCurrentVendorPackInfo (Item.Item_Key, Price.Store_No, SIV.Vendor_Id ,1),1)) * dbo.fn_GetAverageUnitWeightByItemKey(Item.Item_Key)
    								ELSE
    								((ISNULL(dbo.fn_GetCurrentNetCost(@Item_Key, Store.Store_No ), 0))
    									/ ISNULL(dbo.fn_GetCurrentVendorPackInfo (Item.Item_Key, Price.Store_No, SIV.Vendor_Id ,1),1))
    								END
    						 ) -- divide CurrentCost by packsize.
    						 / case POSPrice when 0 then 1 else isnull(POSPrice,1) end )
    					,2) as decimal(10,2)) 
    					as MarginPercentCurrCost,
    			
				(select taxClassDesc from TaxClass where TaxClassId = Item.TaxClassId)  As TaxTable,
				Restricted_Hours, 
				IBM_Discount, 
				NotAuthorizedForSale, 
				CASE	WHEN (	Brand_ID IN (SELECT Brand_ID FROM ItemBrand WHERE Brand_Name LIKE '%365%')) 
								OR PCT.MSRP_Required = 1 
						THEN 1 
						ELSE 0 
					END As EDLP_365,
				(WFM_Store | Mega_Store) AS IsRetailStore,
				CompFlag,
				PCT.MSRP_Required,
				CASE	WHEN	Price.ExceptionSubteam_No = SST.SubTeam_No 
						THEN 1
						ELSE 0
					END AS SubTeamValueMatch,
				ISNULL(Price.ExceptionSubteam_No, SST.SubTeam_No) as SubTeam_No,
			   (	SELECT	Subteam_Name 
					FROM	SubTeam 
					WHERE	SubTeam_No = ISNULL(Price.ExceptionSubteam_No, SST.SubTeam_No)
				) as SubTeam_Name,
			   PCT.PriceChgTypeDesc,
			   ISNULL(dbo.fn_GetCurrentVendorPackInfo(	Item.Item_Key, 
														Price.Store_No,
														SIV.Vendor_Id,
														1
													),1) as VendorPackSize,
				C.CurrencyCode
		FROM	Price									(nolock) 
				INNER JOIN		Store					(nolock)	ON	Price.Store_No				= Store.Store_No
				INNER JOIN 		Zone					(nolock)	ON	Store.Zone_Id				= Zone.Zone_Id
				INNER JOIN		Item					(nolock)	ON	Item.Item_Key				= Price.Item_Key
				INNER JOIN		StoreJurisdiction SJ	(nolock)	ON	Store.StoreJurisdictionID	= SJ.StoreJurisdictionID
				INNER JOIN		Currency C				(nolock)	ON	C.CurrencyID				= SJ.CurrencyID 
				INNER JOIN		StoreSubTeam SST		(nolock)	ON	SST.Store_No				= Price.Store_No 
																	AND SST.SubTeam_No				= Item.SubTeam_No
				LEFT JOIN		StoreItemVendor SIV		(nolock)	ON	SIV.Store_No				= Price.Store_No
																	AND SIV.Item_Key				= Item.Item_Key
																	AND SIV.PrimaryVendor			= 1
				LEFT JOIN		PriceChgType PCT					ON	PCT.PriceChgTypeID			= Price.PriceChgTypeID	            
		WHERE	(Internal = 1 AND BusinessUnit_ID IS NOT NULL)
				AND (Price.Item_Key = @Item_Key)
				AND (WFM_Store = 1 OR Mega_Store = 1)
		ORDER BY	IsRetailStore DESC, 
					Store_Name
		
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentPrices] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentPrices] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentPrices] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentPrices] TO [IRMAReportsRole]
    AS [dbo];

