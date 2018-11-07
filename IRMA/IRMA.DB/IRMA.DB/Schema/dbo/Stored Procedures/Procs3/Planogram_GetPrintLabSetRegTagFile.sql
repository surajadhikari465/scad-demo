CREATE PROCEDURE [dbo].[Planogram_GetPrintLabSetRegTagFile]
    @ItemList varchar(max),
    @ItemListSeparator char(1),
	@Store_No int ,
	@StartDate DateTime
AS
-- ****************************************************************************************************************
-- Procedure: Planogram_GetPrintLabSetRegTagFile()
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-06-14   MZ      12642   Modified the order by clause by removing LabelTypeDesc and ExemptTagDesc so that the 
--                              output will be sorted by planogram order.
-- 2013-06-20   MZ      12578   Added CROSS APPLY to join to the ShelfTagRule table to limit retrieved ShelfTag_Type 
--                              to a store's tax jurisdiction if the tax jurisdiction on ShelfTag_Type is specified.
-- 2013-07-12	MZ		12642	Rollback 12642 as the Order By changes were causing problems for NA;
-- ****************************************************************************************************************
BEGIN
	BEGIN TRY

	DECLARE @NOW datetime
			
	SELECT @NOW = (getdate())
	--DStacey - Dynamically set reg type id
	DECLARE @PriceChgTypeID as int
	SELECT TOP 1 @PriceChgTypeID = PriceChgTypeID FROM dbo.PriceChgType (nolock) WHERE On_Sale = 0

	DECLARE @UseExempt BIT, @UseEST BIT
	SELECT @UseExempt = dbo.fn_InstanceDataValue('ExemptShelfTags', @Store_No), 
		@UseEST = dbo.fn_InstanceDataValue('ElectronicShelfTagRule', @Store_No)

    SELECT DISTINCT
		P.Store_No,
		ST.StoreAbbr As Store_Name,
		PL.Item_Key,
        0 As PriceBatchDetailID,
		'SET_' + TAG.ShelfTag_Ext As tagExt,
		LTRIM(RTRIM(STA.ShelfTagAttrDesc)) As LabelTypeDesc,
		0 As On_Sale,
		SB.SubTeam_Name As SubTeam, 
		II.Identifier, 
		SB.SubTeam_No,
		I.Category_ID As CategoryId,
		B.Brand_Name,
		LTRIM(RTRIM(I.Item_Description)) As Item_Desc,
		CAST(I.Package_Desc1 AS INT) As PackSize,
		I.Package_Desc2 As Item_Size,
		RU.Unit_Abbreviation As Package_Unit_Abbr, --RU.Unit_Name As Package_Unit_Abbr,
		ISNULL(IV.Item_ID, 0) As Vendor_Item_ID,
		dbo.fn_GET_DRLUOM_for_item(PL.Item_Key,P.Store_No) as DRLUOM,
		ISNULL(V.Vendor_key, '') As Vendor_Key,
		ST.EXEWarehouse As Warehouse,
		CAST (VCH.Package_Desc1 AS INT) As CaseSize,
		CAST( CAST (P.MSRPMultiple AS varchar(3)) + '/'+ CAST(P.MSRPPrice As Varchar(6)) As VARCHAR(10)) As Price,
		[CurrPrice] =	-- Hard-coded to reg
		CONVERT(varchar(3),P.Multiple) + '/' + CONVERT(varchar(10),P.POSPrice),
		CONVERT(varchar(3),P.Multiple) + '/' + CONVERT(varchar(10),P.POSPrice) As SaleMultipleWithPOSSalePrice,  -- the current Price for the Item 
		PCT.PriceChgTypeDesc As PriceChangeType,
		CAST(MONTH(P.Sale_Start_Date) AS VARCHAR(2)) + '/' +
                CAST(DAY(P.Sale_Start_Date) AS VARCHAR(2))+ '/'+
				CAST(YEAR(P.Sale_Start_Date) AS VARCHAR(4))
		 As Sale_Start_Date,		
		CAST(MONTH(P.Sale_End_Date) AS VARCHAR(2)) + '/' +
        CAST(DAY(P.Sale_End_Date) AS VARCHAR(2))+ '/'+
		CAST(YEAR(P.Sale_End_Date) AS VARCHAR(4)) 
		 As Sale_End_Date,
		PL.ProductFacings As PlanoFacings,
		IA.Text_2 As Promo_Desc1,
		IA.Text_3 As Promo_Desc2,
		IA.Text_4 As Promo_Desc3, 
		TAG.ShelfTag_Type As ShelfTagTypeID,
		CASE STA2.ShelfTag_Ext 
			WHEN 'EST_LG_REG' 
			THEN NULL 
			WHEN 'EST_SM_REG' 
			THEN NULL 
			ELSE TAG.Exempt_ShelfTag_Type END As ExemptTagTypeID,
		CASE STA2.ShelfTag_Ext 
			WHEN 'EST_LG_REG' 
			THEN NULL 
			WHEN 'EST_SM_REG' 
			THEN NULL 
			ELSE 'SET_' + STA2.ShelfTag_Ext END As ExemptTagExt,
		CASE STA2.ShelfTag_Ext 
			WHEN 'EST_LG_REG' 
			THEN NULL 
			WHEN 'EST_SM_REG' 
			THEN NULL 
			ELSE STA2.ShelfTagAttrDesc END As ExemptTagDesc,
		0 As PriceBatchID,
		IA.Text_1 As Prod_Nutrition,
		ISNULL(V.CompanyName, '') As Vendor_Name,
		PL.ProductGroup,
		PL.ShelfIdentifier,
		PL.ProductPlacement,
		PL.MaxUnits,
		PL.ProductPlanogramCode 
      FROM 
		dbo.Price P (nolock)
        INNER JOIN (select Key_Value FROM fn_Parse_List(@ItemList, @ItemListSeparator) IL GROUP BY Key_Value) IL
            ON IL.Key_Value = P.Item_Key 
        INNER JOIN dbo.Item I (nolock)
            ON I.Item_Key = IL.Key_Value
		INNER JOIN dbo.Store ST (nolock)
				ON P.Store_No = ST.Store_No
		INNER JOIN dbo.Planogram PL (nolock)
				ON P.Item_Key = PL.Item_Key
				AND PL.Store_No = ST.PSI_Store_No
		JOIN dbo.StoreItem SI (nolock) ON ST.Store_no=SI.Store_No AND I.Item_Key=SI.Item_Key 
        INNER JOIN dbo.SubTeam SB (nolock)
            ON (I.SubTeam_No = SB.SubTeam_No)
        INNER JOIN dbo.ItemIdentifier II (nolock)
            ON I.Item_Key = II.Item_Key
			AND II.Default_Identifier = 1
		INNER JOIN dbo.PriceChgType PCT (nolock) 
			ON P.PriceChgTypeID = PCT.PriceChgTypeID
		INNER JOIN dbo.ItemBrand B (nolock)
			ON I.Brand_ID = B.Brand_ID
		INNER JOIN dbo.ItemUnit as RU (nolock)
			ON I.Package_Unit_ID = RU.Unit_ID
		CROSS APPLY (Select ShelfTag_Type, Exempt_ShelfTag_Type, ShelfTag_Ext, ShelfTag_Ext2 from dbo.fn_getShelfTagType (P.Item_Key,I.LabelType_ID,@PriceChgTypeID,NULL,I.SubTeam_No,st.TaxJurisdictionID,st.Store_No, St.Zone_ID, @UseExempt, @UseEST)) AS TAG
		INNER JOIN dbo.ShelfTagAttribute STA (nolock)
				ON TAG.ShelfTag_Type = STA.ShelfTag_Type
		LEFT JOIN dbo.ShelfTagAttribute STA2 (nolock)
				ON TAG.Exempt_ShelfTag_Type = STA2.ShelfTag_Type 
		INNER JOIN dbo.StoreItemVendor SIV (nolock)
			ON ST.Store_no=SIV.Store_No AND I.Item_Key=SIV.Item_Key 
			AND SIV.PrimaryVendor = 1
        INNER JOIN dbo.Vendor V (nolock)
            ON SIV.Vendor_ID = V.Vendor_ID
		LEFT JOIN dbo.VendorCostHistory VCH (nolock)
			ON SIV.StoreItemVendorID=VCH.StoreItemVendorID
		LEFT JOIN dbo.ItemVendor IV (nolock)
			ON IL.Key_Value = IV.Item_Key and
			   V.Vendor_ID = IV.Vendor_ID
		LEFT JOIN dbo.ItemAttribute IA (nolock)
			ON IA.Item_Key = IL.Key_Value
        WHERE 
			PL.Item_Key = IL.Key_Value
			AND ST.store_No = @Store_No
			AND P.PriceChgTypeID = @PriceChgTypeID AND SI.Authorized = 1
			AND ((TAG.ShelfTag_Ext NOT IN ('LG_REG', 'SM_REG')) OR  (TAG.ShelfTag_Ext IN ('LG_REG', 'SM_REG') and TAG.ShelfTag_Ext2 IN ('EX_REG')))
			AND VCH.VendorCostHistoryID = (Select Top 1 VCH2.VendorCostHistoryID 
					FROM dbo.VendorCostHistory VCH2 (nolock) 
					WHERE SIV.StoreItemVendorID = VCH2.StoreItemVendorID 
						AND @NOW BETWEEN VCH2.startdate AND VCH2.enddate
					ORDER BY VCH2.VendorCostHistoryID Desc) 
UNION
    SELECT DISTINCT
		P.Store_No,
		ST.StoreAbbr As Store_Name,
		PL.Item_Key,
        0 As PriceBatchDetailID,
		'SET_' + TAG.ShelfTag_Ext As tagExt,
		LTRIM(RTRIM(TAG.ShelfTagAttrDesc)) As LabelTypeDesc,
		0 As On_Sale,
		SB.SubTeam_Name As SubTeam, 
		II.Identifier, 
		SB.SubTeam_No,
		I.Category_ID As CategoryId,
		B.Brand_Name,
		LTRIM(RTRIM(I.Item_Description)) As Item_Desc,
		CAST(I.Package_Desc1 AS INT) As PackSize,
		I.Package_Desc2 As Item_Size,
		RU.Unit_Abbreviation As Package_Unit_Abbr, --RU.Unit_Name As Package_Unit_Abbr,
		ISNULL(IV.Item_ID, 0) As Vendor_Item_ID,
		dbo.fn_GET_DRLUOM_for_item(PL.Item_Key,P.Store_No) as DRLUOM,
		ISNULL(V.Vendor_key, '') As Vendor_Key,
		ST.EXEWarehouse As Warehouse,
		CAST (VCH.Package_Desc1 AS INT) As CaseSize,
			CONVERT(varchar(3),P.Multiple) + '/' + CONVERT(varchar(6),P.POSPrice), -- always the Base Price for the Item
		[CurrPrice] =	-- logic based on [Replenishment_POSPush_GetPriceBatchSent] field [CurrMultiple]
			CONVERT(varchar(3),P.Multiple) + '/' + CONVERT(varchar(10),P.POSPrice),
		CONVERT(varchar(3),P.Multiple) + '/' + CONVERT(varchar(10),P.POSPrice) As SaleMultipleWithPOSSalePrice,  -- the current Price for the Item 
		PCT.PriceChgTypeDesc As PriceChangeType,
		CAST(MONTH(P.Sale_Start_Date) AS VARCHAR(2)) + '/' +
                CAST(DAY(P.Sale_Start_Date) AS VARCHAR(2))+ '/'+
				CAST(YEAR(P.Sale_Start_Date) AS VARCHAR(4))
		 As Sale_Start_Date,		
		CAST(MONTH(P.Sale_End_Date) AS VARCHAR(2)) + '/' +
        CAST(DAY(P.Sale_End_Date) AS VARCHAR(2))+ '/'+
		CAST(YEAR(P.Sale_End_Date) AS VARCHAR(4)) 
		 As Sale_End_Date,
		PL.ProductFacings As PlanoFacings,
		IA.Text_2 As Promo_Desc1,
		IA.Text_3 As Promo_Desc2,
		IA.Text_4 As Promo_Desc3, 
		TAG.ShelfTag_Type As ShelfTagTypeID,
		CASE TAG2.ShelfTag_Ext2 WHEN 'EX_REG' THEN NULL ELSE TAG2.Exempt_ShelfTag_Type END As ExemptTagTypeID,
		CASE TAG2.ShelfTag_Ext2 WHEN 'EX_REG' THEN NULL ELSE 'SET_' + TAG2.ShelfTag_Ext2 END As ExemptTagExt,
		CASE TAG2.ShelfTag_Ext2 WHEN 'EX_REG' THEN NULL ELSE STA2.ShelfTagAttrDesc END As ExemptTagDesc,
		0 As PriceBatchID,
		IA.Text_1 As Prod_Nutrition,
		ISNULL(V.CompanyName, '') As Vendor_Name,
		PL.ProductGroup,
		PL.ShelfIdentifier,
		PL.ProductPlacement,
		PL.MaxUnits,
		PL.ProductPlanogramCode 
      FROM 
		dbo.Price P (nolock)
        JOIN (select Key_Value FROM fn_Parse_List(@ItemList, @ItemListSeparator) IL GROUP BY Key_Value) IL
            ON IL.Key_Value = P.Item_Key 
        JOIN dbo.Item I (nolock) ON I.Item_Key = IL.Key_Value
		JOIN dbo.Store ST (nolock) ON P.Store_No = ST.Store_No 
		JOIN dbo.Planogram PL (nolock) ON P.Item_Key = PL.Item_Key AND PL.Store_No = ST.PSI_Store_No 
		JOIN dbo.StoreItem SI (nolock) ON ST.Store_no=SI.Store_No AND I.Item_Key=SI.Item_Key 
        JOIN dbo.SubTeam SB (nolock) ON (I.SubTeam_No = SB.SubTeam_No)
        JOIN dbo.ItemIdentifier II (nolock) ON I.Item_Key = II.Item_Key AND II.Default_Identifier = 1
		JOIN dbo.PriceChgType PCT (nolock) ON P.PriceChgTypeID = PCT.PriceChgTypeID 
		JOIN ItemBrand B (nolock) ON I.Brand_ID = B.Brand_ID 
		JOIN ItemUnit as RU (nolock) ON I.Package_Unit_ID = RU.Unit_ID
		JOIN dbo.StoreItemVendor SIV (nolock) ON ST.Store_no=SIV.Store_No AND I.Item_Key=SIV.Item_Key
        JOIN dbo.Vendor V (nolock) ON SIV.Vendor_ID = V.Vendor_ID
        CROSS APPLY (select MIN(SFTR1.RulePriority) as RulePriority, TAG1.ShelfTag_Ext, TAG1.ShelfTagAttrDesc
					   from dbo.ShelfTagAttribute TAG1 (nolock) 
					   JOIN dbo.ShelfTagRule SFTR1 (nolock) ON SFTR1.ShelfTag_Type = TAG1.ShelfTag_Type 
						AND (SFTR1.TaxJurisdictionID = ST.TaxJurisdictionID OR SFTR1.TaxJurisdictionID IS NULL)
					  WHERE TAG1.LabelTypeID = I.LabelType_ID 			
				   group by TAG1.ShelfTag_Ext, TAG1.ShelfTagAttrDesc) AS UTAG
		JOIN dbo.ShelfTagRule SFTR (nolock) ON SFTR.RulePriority = UTAG.RulePriority 
		JOIN dbo.ShelfTagAttribute TAG (nolock) ON TAG.LabelTypeID = I.LabelType_ID AND TAG.ShelfTag_Type = SFTR.ShelfTag_Type
		CROSS APPLY (Select ShelfTag_Ext2, Exempt_ShelfTag_Type from dbo.fn_getShelfTagType (P.Item_Key,I.LabelType_ID,@PriceChgTypeID,NULL,I.SubTeam_No,st.TaxJurisdictionID,st.Store_No, St.Zone_ID, @UseExempt, @UseEST)) AS TAG2
		LEFT JOIN dbo.ShelfTagAttribute STA2 (nolock) ON TAG2.Exempt_ShelfTag_Type = STA2.ShelfTag_Type 
		LEFT JOIN dbo.VendorCostHistory VCH (nolock) ON SIV.StoreItemVendorID=VCH.StoreItemVendorID
		LEFT JOIN ItemVendor IV (nolock) ON IL.Key_Value = IV.Item_Key and V.Vendor_ID = IV.Vendor_ID
		LEFT JOIN dbo.ItemAttribute IA (nolock) ON IA.Item_Key = IL.Key_Value
        WHERE 
			PL.Item_Key = IL.Key_Value
			AND ST.store_No = @Store_No  AND SI.Authorized = 1
			AND ((TAG.ShelfTag_Ext IN ('LG_REG', 'SM_REG')) OR (@UseEST <> 0 and TAG2.ShelfTag_Ext2 NOT IN ('EST_LG_REG', 'EST_SM_REG')))
 			AND SIV.PrimaryVendor = 1
			AND VCH.VendorCostHistoryID = (Select Top 1 VCH2.VendorCostHistoryID 
					FROM dbo.VendorCostHistory VCH2 (nolock) 
					WHERE SIV.StoreItemVendorID = VCH2.StoreItemVendorID 
						AND @NOW BETWEEN VCH2.startdate AND VCH2.enddate
					ORDER BY VCH2.VendorCostHistoryID Desc) 
		ORDER BY 
			LabelTypeDesc,
			ExemptTagDesc,
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

		RAISERROR ('Planogram_GetPrintLabSetRegTagFile failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Planogram_GetPrintLabSetRegTagFile] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Planogram_GetPrintLabSetRegTagFile] TO [IRMAClientRole]
    AS [dbo];

