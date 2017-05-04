IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_TagPush_GetPrintLabBatchTagFile]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_TagPush_GetPrintLabBatchTagFile]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Replenishment_TagPush_GetPrintLabBatchTagFile]
    @ItemList varchar(max),
    @ItemListSeparator char(1),
    @PriceBatchHeaderID int,
	@StartLabelPosition AS INT
AS
BEGIN

BEGIN TRY
DECLARE @UseExempt BIT, @UseEST BIT, @Store_No INT
SELECT Top 1 @Store_No = PBD.Store_No from PriceBatchDetail PBD (nolock) WHERE PBD.PriceBatchHeaderID = @PriceBatchHeaderID
	SELECT @UseExempt = dbo.fn_InstanceDataValue('ExemptShelfTags', @Store_No), 
		@UseEST = dbo.fn_InstanceDataValue('ElectronicShelfTagRule', @Store_No)

        SELECT DISTINCT
		PBD.Store_No,
		LTRIM(RTRIM(ST.StoreAbbr)) As Store_Name,
		TAG.ShelfTag_Ext As tagExt,
		LTRIM(RTRIM(STA.ShelfTagAttrDesc)) As LabelTypeDesc,
		dbo.fn_OnSale(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID)) As On_Sale,
		SB.SubTeam_Name As SubTeam, 
		II.Identifier, 
		SB.SubTeam_No,
		I.Category_ID As CategoryId,
		PBD.Brand_Name,
		LTRIM(RTRIM(I.Item_Description)) As Item_Desc,
		CAST(PBD.Package_Desc1 AS INT) As PackSize,
		PBD.Package_Desc2 As Item_Size,
		RU.Unit_Abbreviation As Package_Unit_Abbr,
		IV.Item_ID As Vendor_Item_ID,
		dbo.fn_GET_DRLUOM_for_item(PBD.Item_Key,PBD.Store_No) as DRLUOM,
		V.Vendor_key As Vendor_Key,
		ST.EXEWarehouse As Warehouse,
		CAST ((Select top 1  Package_Desc1 from VendorCostHistory where StoreItemVendorID = SIV.StoreItemVendorID
			ORDER BY InsertDate DESC) AS INT) AS CaseSize,
		[Price] =		-- logic based on [Replenishment_POSPush_GetPriceBatchSent] field [MultipleWithPOSPrice]
			CONVERT(varchar(3),PBD.Multiple) + '/' + CONVERT(varchar(6),PBD.POSPrice), -- always the Base Price for the Item
		[CurrPrice] =	
			CONVERT(varchar(3), dbo.fn_PricingMethodInt(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.Multiple, PBD.Sale_Multiple))
			+ '/' + 
			CONVERT(varchar(6), dbo.fn_PricingMethodMoney(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price)),
		CONVERT(varchar(3), dbo.fn_PricingMethodInt(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.Multiple, PBD.Sale_Multiple)),
		+ '/' + 
		CONVERT(varchar(10), dbo.fn_PricingMethodMoney(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price))
				As SaleMultipleWithPOSSalePrice,  -- the current Price for the Item 
		CAST(MONTH(ISNULL(PBD.StartDate, Price.Sale_Start_Date)) AS VARCHAR(2)) + '/' +
                CAST(DAY(ISNULL(PBD.StartDate, Price.Sale_Start_Date)) AS VARCHAR(2))+ '/'+
				CAST(YEAR(ISNULL(PBD.StartDate, Price.Sale_Start_Date)) AS VARCHAR(4))
		 As Sale_Start_Date,		
		CAST(MONTH(PBD.Sale_End_Date) AS VARCHAR(2)) + '/' +
        CAST(DAY(PBD.Sale_End_Date) AS VARCHAR(2))+ '/'+
		CAST(YEAR(PBD.Sale_End_Date) AS VARCHAR(4)) 
		 As Sale_End_Date,
		PL.ProductFacings As PlanoFacings,
		IA.Text_2 As Promo_Desc1,
		IA.Text_3 As Promo_Desc2,
		IA.Text_4 As Promo_Desc3, 
		TAG.ShelfTag_Type As ShelfTagTypeID,
		PBH.POSBatchID As PriceBatchID,
		IA.Text_1 As Prod_Nutrition,
		LTRIM(RTRIM(V.CompanyName)) As  Vendor_Name,
		PBD.PriceBatchHeaderID,
		PBD.Item_Key,
		PBD.PriceBatchDetailID,
		TAG.Exempt_ShelfTag_Type As ExemptTagTypeID,
		TAG.ShelfTag_Ext2 As ExemptTagExt,
		STA2.ShelfTagAttrDesc As ExemptTagDesc,
		PBH.BatchDescription,
		PBD.LocalItem,
		[ElectronicShelfTag] = Price.ElectronicShelfTag
      FROM 
		dbo.PriceBatchDetail PBD (nolock)
		INNER JOIN dbo.PriceBatchHeader PBH (nolock)
				ON PBD.PriceBatchHeaderID=PBH.PriceBatchHeaderID
		INNER JOIN dbo.Store ST (nolock)
				ON PBD.Store_No = ST.Store_No
		INNER JOIN dbo.Price (nolock)
				ON PBD.Item_Key = Price.Item_Key
				AND PBD.Store_No = Price.Store_No
        INNER JOIN dbo.SubTeam SB (nolock)
            ON (PBD.SubTeam_No = SB.SubTeam_No)
        INNER JOIN (select Key_Value FROM fn_Parse_List(@ItemList, @ItemListSeparator) IL GROUP BY Key_Value) IL
            ON IL.Key_Value = PBD.Item_Key 
        INNER JOIN dbo.Item I (nolock)
            ON I.Item_Key = IL.Key_Value
        LEFT JOIN dbo.Vendor V (nolock)
            ON PBD.Vendor_ID = V.Vendor_ID
		JOIN dbo.StoreItem SI (nolock)
			ON ST.Store_no=SI.Store_No AND I.Item_Key=SI.Item_Key 
		LEFT JOIN dbo.StoreItemVendor SIV (nolock)
			ON ST.Store_no=SIV.Store_No AND I.Item_Key=SIV.Item_Key AND V.Vendor_ID=SIV.Vendor_ID
        INNER JOIN dbo.fn_GetItemIdentifiers() II 
            ON PBD.Item_Key = II.Item_Key
		INNER JOIN dbo.ItemVendor IV (nolock)
			ON IL.Key_Value = IV.Item_Key and
			   PBD.Vendor_ID = IV.Vendor_ID
		LEFT JOIN dbo.ItemAttribute IA (nolock)
			ON IA.Item_Key = IL.Key_Value
		LEFT JOIN dbo.Planogram PL(nolock)
			ON ST.Store_No = PL.Store_No and IL.Key_Value= PL.Item_Key
		INNER JOIN dbo.ItemUnit RU (nolock)
			ON I.Package_Unit_ID = RU.Unit_ID
		CROSS APPLY (Select ShelfTag_Type, Exempt_ShelfTag_Type, ShelfTag_Ext, ShelfTag_Ext2 from dbo.fn_getShelfTagType (PBD.Item_Key,I.LabelType_ID,IsNull(PBD.PriceChgTypeID,Price.PriceChgTypeID),PBD.ItemChgTypeID,I.SubTeam_No,st.TaxJurisdictionID,st.Store_No, St.Zone_ID, @UseExempt, @UseEST)) AS TAG
		INNER JOIN dbo.ShelfTagAttribute STA (nolock)
				ON TAG.ShelfTag_Type = STA.ShelfTag_Type
		LEFT JOIN dbo.ShelfTagAttribute STA2 (nolock)
				ON TAG.Exempt_ShelfTag_Type = STA2.ShelfTag_Type  
        WHERE PBD.PriceBatchHeaderID = @PriceBatchHeaderID AND SI.Authorized = 1
			AND II.Default_Identifier = 1
		ORDER BY LabelTypeDesc, STA2.ShelfTagAttrDesc, SB.SubTeam_Name, Brand_Name, II.Identifier

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR ('Replenishment_TagPush_GetPrintLabBatchTagFile failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
END
GO