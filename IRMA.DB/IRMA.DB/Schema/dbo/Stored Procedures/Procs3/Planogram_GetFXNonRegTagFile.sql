CREATE PROCEDURE [dbo].[Planogram_GetFXNonRegTagFile]
    @ItemList varchar(max),
    @ItemListSeparator char(1),
    @Store_No int,
	@StartDate DateTime
AS
BEGIN

	BEGIN TRY

			declare @NOW datetime
			
			select @NOW = (getdate())
	DECLARE @UseExempt BIT, @UseEST BIT
	SELECT @UseExempt = dbo.fn_InstanceDataValue('ExemptShelfTags', @Store_No), 
		@UseEST = dbo.fn_InstanceDataValue('ElectronicShelfTagRule', @Store_No)

	--DStacey - Dynamically set reg type id
	DECLARE @RegPriceChgTypeID as int
	select top 1 @RegPriceChgTypeID = PriceChgTypeID from dbo.PriceChgType where On_Sale = 0

	DECLARE @tblPBD TABLE (PBDID INT)
	DECLARE  @Item_Key INT, @message varchar(700)
	DECLARE pbd_cursor CURSOR FOR 
	select Key_Value FROM fn_Parse_List(@ItemList, @ItemListSeparator) IL GROUP BY Key_Value

OPEN pbd_cursor
FETCH NEXT FROM pbd_cursor INTO @Item_Key

WHILE @@FETCH_STATUS = 0
BEGIN

INSERT @tblPBD

SELECT TOP 1 pbd.PriceBatchDetailID 
FROM dbo.Pricebatchdetail pbd (nolock)
	JOIN dbo.PriceChgType pct (nolock) on pct.PriceChgTypeID = pbd.PriceChgTypeID
WHERE Item_Key = @Item_Key AND store_no = @Store_No
	AND @StartDate BETWEEN pbd.startdate and pbd.sale_end_date
	AND pbd.PriceChgTypeID <> @RegPriceChgTypeID
ORDER BY pct.priority DESC, pbd.startdate

FETCH NEXT FROM pbd_cursor INTO @Item_Key

END

CLOSE pbd_cursor
DEALLOCATE pbd_cursor


    SELECT DISTINCT
		PBD.Store_No,
		ST.StoreAbbr As Store_Name,
		'MISC_' + TAG.ShelfTag_Ext As tagExt,
		LTRIM(RTRIM(STA.ShelfTagAttrDesc)) As LabelTypeDesc,
		dbo.fn_OnSale(PBD.PriceChgTypeID) As On_Sale,
		SB.SubTeam_Name As SubTeam, 
		II.Identifier, 
		SB.SubTeam_No,
		I.Category_ID As CategoryId,
		B.Brand_Name,
		LTRIM(RTRIM(I.Item_Description)) As Item_Desc,
		CAST(I.Package_Desc1 AS INT) As PackSize,
		I.Package_Desc2 As Item_Size,
		RU.Unit_Abbreviation As Package_Unit_Abbr,
		ISNULL(IV.Item_ID, 0) As Vendor_Item_ID,
		dbo.fn_GET_DRLUOM_for_item(PBD.Item_Key,PBD.Store_No) as DRLUOM,
		ISNULL(V.Vendor_key, '') As Vendor_Key,
		ST.EXEWarehouse As Warehouse,
		CAST (VCH.Package_Desc1 AS INT) As CaseSize,
		[Price] =		-- logic based on [Replenishment_POSPush_GetPriceBatchSent] field [MultipleWithPOSPrice]
			CONVERT(varchar(3),PBD.Multiple) + '/' + CONVERT(varchar(6),PBD.POSPrice), -- always the Base Price for the Item
		[CurrPrice] =	
			CONVERT(varchar(3), dbo.fn_PricingMethodInt(PBD.PriceChgTypeID, PBD.PricingMethod_ID, PBD.Multiple, PBD.Sale_Multiple))
			+ '/' + 
			CONVERT(varchar(6), dbo.fn_PricingMethodMoney(PBD.PriceChgTypeID, PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price)),
		  CONVERT(varchar(3),dbo.fn_PricingMethodInt(PBD.PriceChgTypeID, PBD.PricingMethod_ID, PBD.Multiple, PBD.Sale_Multiple))
		  + '/' +
		  CONVERT(varchar(10),dbo.fn_PricingMethodMoney(PBD.PriceChgTypeID, PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price))
		  As SaleMultipleWithPOSSalePrice,		
		  CAST(MONTH(PBD.StartDate) AS VARCHAR(2)) + '/' +
                CAST(DAY(PBD.StartDate) AS VARCHAR(2))+ '/'+
				CAST(YEAR(PBD.StartDate) AS VARCHAR(4))
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
		0 As PriceBatchID,
		IA.Text_1 As Prod_Nutrition,
		LTRIM(RTRIM(V.CompanyName)) As  Vendor_Name,
		0 as PriceBatchHeaderID,
		PBD.Item_Key,
		PBD.PriceBatchDetailID,
		TAG.Exempt_ShelfTag_Type As ExemptTagTypeID,
		'MISC_' + STA2.ShelfTag_Ext As ExemptTagExt,
		STA2.ShelfTagAttrDesc As ExemptTagDesc,
		PL.ProductGroup,
		PL.ShelfIdentifier,
		PL.ProductPlacement,
		PL.MaxUnits,
		PL.ProductPlanogramCode 
      FROM 
		dbo.PriceBatchDetail PBD (nolock) 
		JOIN @tblPBD IL ON IL.PBDID = PBD.PriceBatchDetailID
		JOIN dbo.Store ST (nolock) ON PBD.Store_No = ST.Store_No AND PBD.Store_No = @Store_No
		JOIN dbo.Price P (nolock) ON PBD.Item_Key = P.Item_Key AND PBD.Store_No = P.Store_No
        JOIN dbo.Item I (nolock) ON I.Item_Key = PBD.Item_Key
		JOIN dbo.SubTeam SB (nolock) ON (I.SubTeam_No = SB.SubTeam_No)
        JOIN dbo.ItemIdentifier II (nolock) ON PBD.Item_Key = II.Item_Key AND II.Default_Identifier = 1
		JOIN dbo.Planogram PL (nolock) ON PBD.Item_Key = PL.Item_Key AND  PL.Store_No = ST.PSI_Store_No
		JOIN dbo.ItemBrand B (nolock) ON I.Brand_ID = B.Brand_ID
		JOIN dbo.ItemUnit as RU (nolock) ON I.Package_Unit_ID = RU.Unit_ID
		JOIN dbo.StoreItem SI (nolock)
			ON ST.Store_no=SI.Store_No AND I.Item_Key=SI.Item_Key 
		CROSS APPLY (Select ShelfTag_Type, Exempt_ShelfTag_Type, ShelfTag_Ext, ShelfTag_Ext2 from dbo.fn_getShelfTagType (PBD.Item_Key,I.LabelType_ID,ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID),NULL,I.SubTeam_No,st.TaxJurisdictionID,st.Store_No, St.Zone_ID, @UseExempt, @UseEST)) AS TAG
		JOIN dbo.ShelfTagAttribute STA (nolock) ON TAG.ShelfTag_Type = STA.ShelfTag_Type
		LEFT JOIN dbo.ShelfTagAttribute STA2 (nolock) ON TAG.Exempt_ShelfTag_Type = STA2.ShelfTag_Type 
		JOIN dbo.StoreItemVendor SIV (nolock) ON ST.Store_no = SIV.Store_No AND I.Item_Key = SIV.Item_Key
        JOIN dbo.Vendor V (nolock) ON SIV.Vendor_ID = V.Vendor_ID 
		LEFT OUTER JOIN dbo.VendorCostHistory VCH (nolock) ON SIV.StoreItemVendorID=VCH.StoreItemVendorID
		LEFT OUTER JOIN dbo.ItemVendor IV (nolock) ON PBD.Item_Key = IV.Item_Key AND PBD.Vendor_ID = IV.Vendor_ID 
		LEFT OUTER JOIN dbo.ItemAttribute IA (nolock) ON IA.Item_Key = PBD.Item_Key
		WHERE SIV.PrimaryVendor = 1 AND SI.Authorized = 1
			AND VCH.VendorCostHistoryID = (Select Top 1 VCH2.VendorCostHistoryID 
					FROM dbo.VendorCostHistory VCH2 (nolock) 
					WHERE SIV.StoreItemVendorID = VCH2.StoreItemVendorID 
						AND @NOW BETWEEN VCH2.startdate AND VCH2.enddate
					ORDER BY VCH2.VendorCostHistoryID Desc) 
		ORDER BY 
			tagExt,
			ExemptTagExt,
			PL.ProductPlanogramCode, 
			PL.ProductGroup, 
			PL.ShelfIdentifier,
			PL.ProductPlacement

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR ('Planogram_GetFXNonRegTagFile failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Planogram_GetFXNonRegTagFile] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Planogram_GetFXNonRegTagFile] TO [IRMAClientRole]
    AS [dbo];

