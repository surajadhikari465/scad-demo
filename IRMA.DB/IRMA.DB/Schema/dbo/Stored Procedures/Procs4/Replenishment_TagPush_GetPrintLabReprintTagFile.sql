
CREATE PROCEDURE [dbo].[Replenishment_TagPush_GetPrintLabReprintTagFile]
    @ItemList varchar(max),
	@Store_No int,
    @ItemListSeparator char(1),
	@StartLabelPosition int,
	@SortTags as bit
AS
BEGIN

	BEGIN TRY
		DECLARE @UseExempt BIT, @UseEST BIT
		SELECT @UseExempt = dbo.fn_InstanceDataValue('ExemptShelfTags', @Store_No), 
			@UseEST = dbo.fn_InstanceDataValue('ElectronicShelfTagRule', @Store_No)


		IF @SortTags = 1
			BEGIN
				SELECT 
					SQ.Store_No,
					LTRIM(RTRIM(st.StoreAbbr)) As Store_Name,
					SQ.item_Key,
					0 As PriceBatchDetailID,
					STA.ShelfTag_Ext As tagExt,
					LTRIM(RTRIM(STA.ShelfTagAttrDesc)) As LabelTypeDesc,
					dbo.fn_OnSale(SQ.PriceChgTypeID) As On_Sale,
					SB.SubTeam_Name As SubTeam, 
					II.Identifier, 
					SB.SubTeam_No,
					I.Category_ID As CategoryId,
					ISNULL(SQ.Brand_Name, B.Brand_Name) AS Brand_Name,
					LTRIM(RTRIM(I.Item_Description)) As Item_Desc,
					CAST(SQ.Package_Desc1 AS INT) As PackSize,
					SQ.Package_Desc2 As Item_Size,
					RU.Unit_Abbreviation As Package_Unit_Abbr,
					IV.Item_ID As Vendor_Item_ID,
					dbo.fn_GET_DRLUOM_for_item(SQ.Item_Key,SQ.Store_No) as DRLUOM,
					V.Vendor_key As Vendor_Key,
					st.EXEWarehouse As Warehouse,
					CAST ((Select top 1  Package_Desc1 from VendorCostHistory where StoreItemVendorID = SIV.StoreItemVendorID
						ORDER BY InsertDate DESC) AS INT) AS CAseSIZE,
					[Price] =		-- logic based on [Replenishment_POSPush_GetPriceBatchSent] field [MultipleWithPOSPrice]
						CONVERT(varchar(3),SQ.Multiple) + '/' + CONVERT(varchar(6),SQ.POSPrice), -- always the Base Price for the Item
					[CurrPrice] =	
						CONVERT(varchar(3), dbo.fn_PricingMethodInt(SQ.PriceChgTypeID, SQ.PricingMethod_ID, SQ.Multiple, SQ.Sale_Multiple))
						+ '/' + 
						CONVERT(varchar(6), dbo.fn_PricingMethodMoney(SQ.PriceChgTypeID, SQ.PricingMethod_ID, SQ.POSPrice, SQ.POSSale_Price)),
					CONVERT(varchar(3), dbo.fn_PricingMethodInt(SQ.PriceChgTypeID, SQ.PricingMethod_ID, SQ.Multiple, SQ.Sale_Multiple))
					+ '/' + 
					CONVERT(varchar(10), dbo.fn_PricingMethodMoney(SQ.PriceChgTypeID, SQ.PricingMethod_ID, SQ.POSPrice, SQ.POSSale_Price))
							As SaleMultipleWithPOSSalePrice,  -- the current Price for the Item 
					CAST(MONTH(SQ.Sale_Start_Date) AS VARCHAR(2)) + '/' +
							CAST(DAY(SQ.Sale_Start_Date) AS VARCHAR(2))+ '/'+
							CAST(YEAR(SQ.Sale_Start_Date) AS VARCHAR(4))
					 As Sale_Start_Date,		
					CAST(MONTH(SQ.Sale_End_Date) AS VARCHAR(2)) + '/' +
					CAST(DAY(SQ.Sale_End_Date) AS VARCHAR(2))+ '/'+
					CAST(YEAR(SQ.Sale_End_Date) AS VARCHAR(4)) 
					 As Sale_End_Date,
					PL.ProductFacings As PlanoFacings,
					IA.Text_2 As Promo_Desc1,
					IA.Text_3 As Promo_Desc2,
					IA.Text_4 As Promo_Desc3, 
					CAST(STA.ShelfTag_Type as INT) As ShelfTagTypeID,
					CAST(STA2.ShelfTag_Type as INT) As ExemptTagTypeID,
					0 As PriceBatchID,
					IA.Text_1 As Prod_Nutrition,
					LTRIM(RTRIM(V.CompanyName)) As Vendor_Name,
					STA2.ShelfTag_Ext As ExemptTagExt,
					STA2.ShelfTagAttrDesc As ExemptTagDesc,
					'Reprint Tags' As BatchDescription,
					SQ.LocalItem AS LocalItem,
					[ElectronicShelfTag] = P.ElectronicShelfTag
				FROM 
					dbo.SignQueue SQ (nolock)
					INNER JOIN dbo.Store st (nolock)
							ON SQ.Store_No = st.Store_No
					INNER JOIN dbo.SubTeam SB (nolock)
						ON (SQ.SubTeam_No = SB.SubTeam_No)
					INNER JOIN dbo.fn_Parse_List(@ItemList, @ItemListSeparator) IL
						ON IL.Key_Value = SQ.Item_Key 
					INNER JOIN dbo.Item I (nolock)
						ON I.Item_Key = IL.Key_Value
					INNER JOIN dbo.Price P (nolock)
						ON P.Item_Key = I.Item_Key AND P.Store_No = st.Store_No
					LEFT JOIN dbo.Vendor V (nolock)
						ON SQ.Vendor_ID = V.Vendor_ID
					LEFT JOIN dbo.StoreItemVendor SIV (nolock)
						ON st.Store_no=SIV.Store_No AND I.Item_Key=SIV.Item_Key AND V.Vendor_ID=SIV.Vendor_ID
					JOIN dbo.StoreItem SI (nolock)
						ON ST.Store_no=SI.Store_No AND I.Item_Key=SI.Item_Key 
					INNER JOIN dbo.fn_GetItemIdentifiers() II 
						ON SQ.Item_Key = II.Item_Key
					INNER JOIN dbo.ItemVendor IV (nolock)
						ON IL.Key_Value = IV.Item_Key and
						   SQ.Vendor_ID = IV.Vendor_ID
					LEFT JOIN dbo.ItemAttribute IA (nolock)
						ON IA.Item_Key = IL.Key_Value
					LEFT JOIN (SELECT Item_Key, Store_No, ProductFacings FROM dbo.Planogram PL(nolock) GROUP BY Store_No, Item_Key, ProductFacings) PL ON st.Store_No = PL.Store_No and IL.Key_Value= PL.Item_Key  
					INNER JOIN dbo.ItemBrand B (nolock)
						ON I.Brand_ID = B.Brand_ID
					INNER JOIN dbo.ItemUnit RU (nolock)
						ON I.Package_Unit_ID = RU.Unit_ID
					CROSS APPLY (Select ShelfTag_Type, Exempt_ShelfTag_Type from dbo.fn_getShelfTagType (SQ.Item_Key,I.LabelType_ID,SQ.PriceChgTypeID,NULL,I.SubTeam_No,st.TaxJurisdictionID,st.Store_No, St.Zone_ID, @UseExempt, @UseEST)) AS TAG
					INNER JOIN dbo.ShelfTagAttribute STA (nolock)
							ON TAG.ShelfTag_Type = STA.ShelfTag_Type
					LEFT JOIN dbo.ShelfTagAttribute STA2 (nolock)
							ON TAG.Exempt_ShelfTag_Type = STA2.ShelfTag_Type        
				WHERE SQ.Item_Key = IL.Key_Value
						AND SQ.store_No = @Store_No 
						AND II.Default_Identifier = 1 AND SI.Authorized = 1
				ORDER BY LabelTypeDesc, STA2.ShelfTagAttrDesc, SB.SubTeam_Name, Brand_Name, II.Identifier
			END
		ELSE
			BEGIN
				SELECT 
					SQ.Store_No,
					LTRIM(RTRIM(st.StoreAbbr)) As Store_Name,
					SQ.item_Key,
					0 As PriceBatchDetailID,
					STA.ShelfTag_Ext As tagExt,
					LTRIM(RTRIM(STA.ShelfTagAttrDesc)) As LabelTypeDesc,
					dbo.fn_OnSale(SQ.PriceChgTypeID) As On_Sale,
					SB.SubTeam_Name As SubTeam, 
					II.Identifier, 
					SB.SubTeam_No,
					I.Category_ID As CategoryId,
					ISNULL(SQ.Brand_Name, B.Brand_Name) AS Brand_Name,
					LTRIM(RTRIM(I.Item_Description)) As Item_Desc,
					CAST(SQ.Package_Desc1 AS INT) As PackSize,
					SQ.Package_Desc2 As Item_Size,
					RU.Unit_Abbreviation As Package_Unit_Abbr,
					IV.Item_ID As Vendor_Item_ID,
					dbo.fn_GET_DRLUOM_for_item(SQ.Item_Key,SQ.Store_No) as DRLUOM,
					V.Vendor_key As Vendor_Key,
					st.EXEWarehouse As Warehouse,
					CAST ((Select top 1  Package_Desc1 from VendorCostHistory where StoreItemVendorID = SIV.StoreItemVendorID
						ORDER BY InsertDate DESC) AS INT) AS CAseSIZE,
					[Price] =		-- logic based on [Replenishment_POSPush_GetPriceBatchSent] field [MultipleWithPOSPrice]
						CONVERT(varchar(3),SQ.Multiple) + '/' + CONVERT(varchar(6),SQ.POSPrice), -- always the Base Price for the Item
					[CurrPrice] =	-- logic based on [Replenishment_POSPush_GetPriceBatchSent] field [CurrMultiple]
						CONVERT(varchar(3), (CASE WHEN dbo.fn_OnSale(SQ.PriceChgTypeID) = 1 
												THEN CASE SQ.PricingMethod_ID 
													WHEN 0 THEN SQ.Sale_Multiple
													WHEN 1 THEN SQ.Sale_Multiple
													WHEN 2 THEN SQ.Multiple 
													WHEN 4 THEN SQ.Multiple END
												ELSE SQ.Multiple END))
						+ '/' + 
						CONVERT(varchar(6), dbo.fn_PricingMethodMoney(SQ.PriceChgTypeID, SQ.PricingMethod_ID, SQ.POSPrice, SQ.POSSale_Price)),
					CASE WHEN dbo.fn_OnSale(SQ.PriceChgTypeID) = 1
						THEN CASE SQ.PricingMethod_ID 
							WHEN 0 THEN CONVERT(varchar(3), SQ.Sale_Multiple) + '/' + CONVERT(varchar(10),SQ.POSSale_Price) 
							WHEN 1 THEN CONVERT(varchar(3), SQ.Sale_Multiple) + '/' + CONVERT(varchar(10),SQ.POSSale_Price)
							WHEN 2 THEN CONVERT(varchar(3), SQ.Multiple) + '/' + CONVERT(varchar(10),SQ.POSPrice)
							WHEN 4 THEN CONVERT(varchar(3), SQ.Multiple) + '/' + CONVERT(varchar(10),SQ.POSPrice) END
						ELSE CONVERT(varchar(3),SQ.Multiple) + '/' + CONVERT(varchar(10),SQ.POSPrice) END As SaleMultipleWithPOSSalePrice,  -- the current Price for the Item 
					CAST(MONTH(SQ.Sale_Start_Date) AS VARCHAR(2)) + '/' +
							CAST(DAY(SQ.Sale_Start_Date) AS VARCHAR(2))+ '/'+
							CAST(YEAR(SQ.Sale_Start_Date) AS VARCHAR(4))
					 As Sale_Start_Date,		
					CAST(MONTH(SQ.Sale_End_Date) AS VARCHAR(2)) + '/' +
					CAST(DAY(SQ.Sale_End_Date) AS VARCHAR(2))+ '/'+
					CAST(YEAR(SQ.Sale_End_Date) AS VARCHAR(4)) 
					 As Sale_End_Date,
					PL.ProductFacings As PlanoFacings,
					IA.Text_2 As Promo_Desc1,
					IA.Text_3 As Promo_Desc2,
					IA.Text_4 As Promo_Desc3, 
					CAST(STA.ShelfTag_Type as INT) As ShelfTagTypeID,
					CAST(STA2.ShelfTag_Type as INT) As ExemptTagTypeID,
					0 As PriceBatchID,
					IA.Text_1 As Prod_Nutrition,
					LTRIM(RTRIM(V.CompanyName)) As Vendor_Name,
					STA2.ShelfTag_Ext As ExemptTagExt,
					STA2.ShelfTagAttrDesc As ExemptTagDesc,
					'Reprint Tags' As BatchDescription,
					SQ.LocalItem,
					[ElectronicShelfTag] = P.ElectronicShelfTag
				FROM 
					dbo.SignQueue SQ (nolock)
					INNER JOIN dbo.Store st (nolock)
							ON SQ.Store_No = st.Store_No
					INNER JOIN dbo.SubTeam SB (nolock)
						ON (SQ.SubTeam_No = SB.SubTeam_No)
					INNER JOIN dbo.fn_Parse_List(@ItemList, @ItemListSeparator) IL
						ON IL.Key_Value = SQ.Item_Key 
					INNER JOIN dbo.Item I (nolock)
						ON I.Item_Key = IL.Key_Value
					INNER JOIN dbo.Price P (nolock)
						ON P.Item_Key = I.Item_Key AND P.Store_No = st.Store_No
					LEFT JOIN dbo.Vendor V (nolock)
						ON SQ.Vendor_ID = V.Vendor_ID
					LEFT JOIN dbo.StoreItemVendor SIV (nolock)
						ON st.Store_no=SIV.Store_No AND I.Item_Key=SIV.Item_Key AND V.Vendor_ID=SIV.Vendor_ID
					JOIN dbo.StoreItem SI (nolock)
						ON ST.Store_no=SI.Store_No AND I.Item_Key=SI.Item_Key 
					INNER JOIN dbo.fn_GetItemIdentifiers() II
						ON SQ.Item_Key = II.Item_Key
					INNER JOIN dbo.ItemVendor IV (nolock)
						ON IL.Key_Value = IV.Item_Key and
						   SQ.Vendor_ID = IV.Vendor_ID
					LEFT JOIN dbo.ItemAttribute IA (nolock)
						ON IA.Item_Key = IL.Key_Value
					LEFT JOIN (SELECT Item_Key, Store_No, ProductFacings FROM dbo.Planogram PL(nolock) GROUP BY Store_No, Item_Key, ProductFacings) PL ON st.Store_No = PL.Store_No and IL.Key_Value= PL.Item_Key  
					INNER JOIN dbo.ItemBrand B (nolock)
						ON I.Brand_ID = B.Brand_ID
					INNER JOIN dbo.ItemUnit RU (nolock)
						ON I.Package_Unit_ID = RU.Unit_ID
					CROSS APPLY (Select ShelfTag_Type, Exempt_ShelfTag_Type from dbo.fn_getShelfTagType (SQ.Item_Key,I.LabelType_ID,SQ.PriceChgTypeID,NULL,I.SubTeam_No,st.TaxJurisdictionID,st.Store_No, St.Zone_ID, @UseExempt, @UseEST)) AS TAG
					INNER JOIN dbo.ShelfTagAttribute STA (nolock)
							ON TAG.ShelfTag_Type = STA.ShelfTag_Type
					LEFT JOIN dbo.ShelfTagAttribute STA2 (nolock)
							ON TAG.Exempt_ShelfTag_Type = STA2.ShelfTag_Type        
				WHERE SQ.Item_Key = IL.Key_Value
						AND SQ.store_No = @Store_No 
						AND II.Default_Identifier = 1 AND SI.Authorized = 1
				ORDER BY IL.RowID
			END		
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR ('Replenishment_TagPush_GetPrintLabReprintTagFile failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TagPush_GetPrintLabReprintTagFile] TO [IRMAClientRole]
    AS [dbo];

