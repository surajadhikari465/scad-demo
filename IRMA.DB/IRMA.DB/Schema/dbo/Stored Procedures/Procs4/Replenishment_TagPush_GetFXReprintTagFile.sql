
CREATE PROCEDURE [dbo].[Replenishment_TagPush_GetFXReprintTagFile]
    @ItemList varchar(max),
	@Store_No int,
    @ItemListSeparator char(1),
	@StartLabelPosition int
AS
BEGIN
    SET QUOTED_IDENTIFIER OFF

    DECLARE @OmitShelfTagTypeFields BIT
    DECLARE @SQL     NVARCHAR(MAX)

	DECLARE @UseExempt BIT, @UseEST BIT
	SELECT @UseExempt = dbo.fn_InstanceDataValue('ExemptShelfTags', @Store_No), 
			@UseEST = dbo.fn_InstanceDataValue('ElectronicShelfTagRule', @Store_No)

    SET @OmitShelfTagTypeFields = (SELECT FlagValue 
                                   FROM   InstanceDataFlags
                                   WHERE  FlagKey = 'OmitShelfTagTypeFields')
-- DaveStacey - 20070718 - Merge forward changes from 2.4.0/1 - 
	-- Case Size - changed to a top 1 from vendor cost history b/c this was crashing for us once there is more than one cost history
	-- Price value updated according to fixes required from post go-live
	-- Similar Fix to CurrPrice
	-- Added SaleMultipleWithPOSSalePrice 
	-- Brought back 0 value for PriceBatchDetailID and item_Key which had been dropped from the query
	-- Added flag check for multiple id returns to match the PrintBatch query
	-- Branched STA3 out and renamed to STA to retain sorting
	-- 20070815 - Added BatchDescription from NA080207a1 - Planogram Limit

    SET @SQL =    

    '
       SET QUOTED_IDENTIFIER OFF 

    
       SELECT
        SQ.Store_No                         AS Store_No,
        LTRIM(RTRIM(Store.StoreAbbr))       AS Store_Name,
		SQ.item_Key							AS item_Key,
        0									AS PriceBatchDetailID,
        dbo.fn_OnSale(SQ.PriceChgTypeID)    AS On_Sale,
        SubTeam.SubTeam_Name                AS SubTeam, 
        II.Identifier                       AS Identifier, 
        SubTeam.SubTeam_No                  AS SubTeam_No,
        I.Category_ID                       AS Category_Id,
        SQ.Brand_Name                        AS Brand_Name,
		REPLACE(LTRIM(RTRIM(SQ.Item_Description)), ",", " ") AS Item_Desc,
        CAST(SQ.Package_Desc1 AS INT)        AS PackSize,
        SQ.Package_Desc2                     AS Item_Size,
        SQ.Package_Unit                AS Package_Unit_Abbr,
        IV.Item_ID                          AS Vendor_Item_ID,
        V.Vendor_key                        AS Vendor_Key,
        Store.EXEWarehouse                  AS Warehouse,
		CAST ((Select top 1  Package_Desc1 from VendorCostHistory where StoreItemVendorID = SIV.StoreItemVendorID
			ORDER BY InsertDate DESC) AS INT)     AS CaseSize,
        CONVERT(varchar(3),SQ.Multiple) + "/" + CONVERT(varchar(6),SQ.POSPrice)
						                    AS Price,
        CONVERT(varchar(3), (CASE WHEN dbo.fn_OnSale(SQ.PriceChgTypeID) = 1 
									THEN CASE SQ.PricingMethod_ID 
										WHEN 0 THEN SQ.Sale_Multiple
										WHEN 1 THEN SQ.Sale_Multiple
										WHEN 2 THEN SQ.Multiple 
										WHEN 4 THEN SQ.Multiple END
									ELSE SQ.Multiple END))
			+ "/" + 
			CONVERT(varchar(6), dbo.fn_PricingMethodMoney(SQ.PriceChgTypeID, SQ.PricingMethod_ID, SQ.POSPrice, SQ.POSSale_Price))
                                            AS CurrPrice, 
		CASE WHEN dbo.fn_OnSale(SQ.PriceChgTypeID) = 1
			THEN CASE SQ.PricingMethod_ID 
				WHEN 0 THEN CONVERT(varchar(3), SQ.Sale_Multiple) + "/" + CONVERT(varchar(10),SQ.POSSale_Price) 
				WHEN 1 THEN CONVERT(varchar(3), SQ.Sale_Multiple) + "/" + CONVERT(varchar(10),SQ.POSSale_Price)
				WHEN 2 THEN CONVERT(varchar(3), SQ.Multiple) + "/" + CONVERT(varchar(10),SQ.POSPrice)
				WHEN 4 THEN CONVERT(varchar(3), SQ.Multiple) + "/" + CONVERT(varchar(10),SQ.POSPrice) END
			ELSE CONVERT(varchar(3),SQ.Multiple) + "/" + CONVERT(varchar(10),SQ.POSPrice) END As SaleMultipleWithPOSSalePrice,
		CASE WHEN SQ.Sale_Start_Date IS NOT NULL THEN
                CAST(MONTH(SQ.Sale_Start_Date) AS VARCHAR(2)) + "/" +
                CAST(DAY  (SQ.Sale_Start_Date) AS VARCHAR(2)) + "/"+
                CAST(YEAR (SQ.Sale_Start_Date) AS VARCHAR(4))
            ELSE
                ""
        END                                 AS Sale_Start_Date,		
		CASE WHEN SQ.Sale_End_Date IS NOT NULL THEN
                CAST(MONTH(SQ.Sale_End_Date) AS VARCHAR(2)) + "/" +
                CAST(DAY  (SQ.Sale_End_Date) AS VARCHAR(2)) + "/"+
		        CAST(YEAR (SQ.Sale_End_Date) AS VARCHAR(4)) 
            ELSE
                ""
        END                                 AS Sale_End_Date,
        PL.ProductFacings                   AS PlanoFacings,
        IA.Text_2                           AS Promo_Desc1,
        IA.Text_3                           AS Promo_Desc2,
        IA.Text_4                           AS Promo_Desc3, 
        0                                   AS PriceBatchID,
        IA.Text_1                           AS Prod_Nutrition,
        LTRIM(RTRIM(V.CompanyName))         AS Vendor_Name,
        ""                                  AS PriceBatchHeaderID,
        SQ.Item_Key                         AS Item_Key,
        0                                   AS PriceBatchDetailID,
        ""                                  AS Batch,
        ""                                  AS AbbreviatedBatchDesc,
        ISNULL(CONVERT(VARCHAR(10), SQ.Sale_Start_Date, 101), "12/31/2099") AS EventStartDate,
		CASE 
		    WHEN dbo.fn_OnSale(SQ.PriceChgTypeID) = 1 THEN
		        ISNULL(CONVERT(VARCHAR(10), SQ.Sale_End_Date, 101), "12/31/2099")
		    ELSE
		        "12/31/2099"
        END                               AS EventEndDate,
        IC.Category_Name                    AS CategoryName,
        PHL3.ProdHierarchyLevel3_ID         AS Level3_ID,
        PHL3.Description                    AS Level3_Desc,
        PHL4.ProdHierarchyLevel4_ID         AS Level4_ID,
        PHL4.Description                    AS Level4_Desc,
        CASE
            WHEN II.IdentifierType = "B" OR II.IdentifierType = "P" OR II.IdentifierType = "O"
                THEN II.Identifier
            ELSE
                ""   
        END                                 AS UPC_PLU,
        CASE
            WHEN II.IdentifierType = "S" AND II.Default_Identifier = 0
                THEN II.Identifier
            ELSE
                ""   
        END                                 AS SKU,
        SQ.Vendor_ID                        AS VendorID,
        ISNULL(CAST(dbo.fn_GetVendorPackSize(SQ.Item_Key, 
                                             SQ.Vendor_Id, 
                                             SQ.Store_No, 
                                             CAST(GETDATE() AS SMALLDATETIME)) 
               AS VARCHAR), "")             AS UnitsPerCase,
        dbo.fn_SellingUnitWeight(SQ.Package_Desc1, 
                                 SQ.Package_Desc2) AS SellingUnitWeight,
        dbo.fn_UnitPrice(SQ.PriceChgTypeID, 
                         SQ.Multiple, 
                         SQ.Sale_Multiple, 
                         SQ.POSPrice, 
                         SQ.POSSale_Price, 
                         SQ.Package_Desc1, 
                         SQ.Package_Desc2, 
                         TJ.TaxJurisdictionDesc, 
                         SQ.Retail_Unit_Abbr, 
                         IA.Text_6)                 AS UnitPrice,
        dbo.fn_EDLP_UnitPrice(SQ.PriceChgTypeID, 
                         SQ.Multiple, 
                         SQ.Sale_Multiple, 
                         SQ.POSPrice, 
                         SQ.POSSale_Price, 
                         SQ.Package_Desc1, 
                         SQ.Package_Desc2, 
                         TJ.TaxJurisdictionDesc, 
                         SQ.Retail_Unit_Abbr, 
                         IA.Text_6,
                         PCT.MSRP_Required)         AS EDLP_UnitPrice,
        CASE 
            WHEN LTRIM(RTRIM(TJ.TaxJurisdictionDesc)) = "NJ" AND
                 IA.Item_Key IS NOT NULL AND 
                 IA.Text_6   IS NOT NULL AND
                 IA.Text_6   <> "Not Applicable"
                THEN RTRIM(IA.Text_6)
            ELSE
		        RTRIM(SQ.Retail_Unit_Full)                      
        END                                            AS Package_Unit_Name,
        --ISNULL(SQ.Multiple, "")                        AS UnitsPerPrice,
        SQ.Multiple                        AS UnitsPerPrice,
        --dbo.fn_UnitsPerPrice(SQ.PriceChgTypeID,
        --                     SQ.Multiple,
        --                     SQ.Sale_Multiple)         AS UnitsPerPrice,
        dbo.fn_RegularCurrentPrice(SQ.POSPrice)        AS RegularCurrentPrice,
        dbo.fn_SaleItemUnitsPerPrice(SQ.PriceChgTypeID, 
                                     SQ.Sale_Multiple) AS SaleItem_UnitsPerPrice,
        dbo.fn_SalePrice(SQ.PriceChgTypeID, 
                         SQ.POSSale_Price) AS SalePrice,
        dbo.fn_EDLP_UnitsPerPrice(SQ.PriceChgTypeID,
                                  SQ.Multiple,
                                  SQ.Sale_Multiple,
                                  PCT.MSRP_Required) AS EDLP_UnitsPerPrice,
        dbo.fn_EDLP_Price(SQ.PriceChgTypeID,
                          SQ.POSPrice,
                          SQ.POSSale_Price,
                          PCT.MSRP_Required)         AS EDLP_Price,
        dbo.fn_MSRP_UnitsPerPrice(SQ.MSRPMultiple)   AS MSRP_UnitsPerPrice,
        dbo.fn_MSRP_Price(SQ.MSRPPrice)              AS MSRP_Price,
        CASE
            WHEN PL.ProductPlanogramCode IS NULL
                THEN "999"
            ELSE
                SUBSTRING(PL.ProductPlanogramCode, 5, 3)
        END                                 AS Planogram_Code,
        ISNULL(PL.ShelfIdentifier,"99")     AS Planogram_ShelfIdentifer,
        ISNULL(PL.ProductPlacement,"999")   AS Planogram_ProductPlacement,
        ""                                  AS FileMakerPro_DownloadReason,
        RIGHT("0000" + CAST(ROW_NUMBER() OVER(ORDER BY SQ.Item_Key) AS VARCHAR(5)), 5) AS RowNumber,
        II.Default_Identifier               AS SKUFlag,
        "DVO"                               AS DVO_Vendor_Indicator, 
		"Reprint Tags"						AS BatchDescription,
		SQ.LocalItem						AS LocalItem,
		[ElectronicShelfTag] = P.ElectronicShelfTag'

        IF @OmitShelfTagTypeFields = 0
        BEGIN
            SET @SQL = @SQL + ',
                dbo.fn_GET_DRLUOM_for_item(SQ.Item_Key,SQ.Store_No) AS DRLUOM,
				STA.ShelfTag_Ext As tagExt,
				LTRIM(RTRIM(STA.ShelfTagAttrDesc)) As LabelTypeDesc,
				CAST(STA.ShelfTag_Type as INT) As ShelfTagTypeID,
				CAST(STA2.ShelfTag_Type as INT) As ExemptTagTypeID,
				STA2.ShelfTag_Ext As ExemptTagExt,
				STA2.ShelfTagAttrDesc As ExemptTagDesc, '
        END
        ELSE
            BEGIN
                SET @SQL = @SQL + ',
                    ""                                  AS DRLUOM,
                    STA.ShelfTag_Ext                    AS tagExt,
		            LTRIM(RTRIM(STA.ShelfTagAttrDesc))  AS LabelTypeDesc,
		            1                                   AS ShelfTagTypeID,
		            2                                   AS ExemptTagTypeID,
		            ""                                  AS ExemptTagExt,
		            ""                                  AS ExemptTagDesc '
            END

    SET @SQL = @SQL +        
      'FROM SignQueue SQ                    (nolock)
		INNER JOIN Store                    (nolock) ON SQ.Store_No                 = Store.Store_No
        INNER JOIN SubTeam                  (nolock) ON SQ.SubTeam_No               = SubTeam.SubTeam_No
        INNER JOIN fn_Parse_List(@Item_List, @Item_List_Separator) IL
                                                     ON IL.Key_Value                = SQ.Item_Key         
        INNER JOIN Item I                   (nolock) ON I.Item_Key                  = IL.Key_Value
		INNER JOIN Price P					(nolock) ON P.Item_Key					= I.Item_Key
														AND P.Store_No				= Store.Store_No
        INNER JOIN ItemCategory IC          (nolock) ON I.Category_ID               = IC.Category_ID
        INNER JOIN ProdHierarchyLevel4 PHL4 (nolock) ON I.ProdHierarchyLevel4_ID    = PHL4.ProdHierarchyLevel4_ID
        INNER JOIN ProdHierarchyLevel3 PHL3 (nolock) ON PHL4.ProdHierarchyLevel3_ID = PHL3.ProdHierarchyLevel3_ID
        INNER JOIN PriceChgType PCT         (nolock) ON SQ.PriceChgTypeID           = PCT.PriceChgTypeID
        LEFT  JOIN Vendor V                 (nolock) ON SQ.Vendor_ID                = V.Vendor_ID
		LEFT  JOIN StoreItemVendor SIV      (nolock) ON store.Store_no              = SIV.Store_No AND 
                                                        I.Item_Key                  = SIV.Item_Key AND 
                                                        V.Vendor_ID                 = SIV.Vendor_ID
        INNER JOIN dbo.fn_GetItemIdentifiers() II       ON SQ.Item_Key                 = II.Item_Key
		INNER JOIN ItemVendor IV            (nolock) ON IL.Key_Value                = IV.Item_Key  AND
                                                        SQ.Vendor_ID                = IV.Vendor_ID
		LEFT  JOIN ItemAttribute IA         (nolock) ON IA.Item_Key                 = IL.Key_Value
		LEFT  JOIN Planogram PL             (nolock) ON Store.Store_No              = PL.Store_No  AND 
                                                        IL.Key_Value                = PL.Item_Key
        INNER JOIN TaxJurisdiction TJ       (nolock) ON Store.TaxJurisdictionID     = TJ.TaxJurisdictionID '

        IF @OmitShelfTagTypeFields = 0
        BEGIN
            SET @SQL = @SQL + 
		    ' CROSS APPLY (Select ShelfTag_Type, Exempt_ShelfTag_Type from dbo.fn_getShelfTagType (SQ.Item_Key,I.LabelType_ID,SQ.PriceChgTypeID,NULL,I.SubTeam_No,st.TaxJurisdictionID,st.Store_No, St.Zone_ID, @UseExempt, @UseEST)) AS TAG
				INNER JOIN dbo.ShelfTagAttribute STA (nolock)
						ON TAG.ShelfTag_Type = STA.ShelfTag_Type
				LEFT JOIN dbo.ShelfTagAttribute STA2 (nolock)
						ON TAG.Exempt_ShelfTag_Type = STA2.ShelfTag_Type     '
        END
        ELSE
            BEGIN
                SET @SQL = @SQL + ' INNER JOIN ShelfTagAttribute STA   (nolock) ON I.LabelType_ID              = STA.LabelTypeID '
            END

        SET @SQL = @SQL + 'WHERE SQ.Item_Key = IL.Key_Value AND SQ.store_No = @StoreNo '

        -- Note: the following code is MA-specific.  MA has only two LabelTypes: NONE=1 and TAG=2.
        IF @OmitShelfTagTypeFields = 1
        BEGIN
            SET @SQL = @SQL + 'AND I.LabelType_ID = 2 '
        END

        SET @SQL = @SQL + 'ORDER BY STA.ShelfTag_Type, SubTeam.SubTeam_Name, SQ.Brand_Name, II.Identifier '

    EXEC sp_executesql 
        @stmt = @SQL,
        @params = N'@Item_List AS VARCHAR(max), @Item_List_Separator AS CHAR(1), @StoreNo AS INT, @Start_Label_Position AS INT',
        @Item_List            = @ItemList,
        @Item_List_Separator  = @ItemListSeparator,
	    @StoreNo              = @Store_No,
        @Start_Label_Position = @StartLabelPosition;                  
           

END

SET QUOTED_IDENTIFIER ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TagPush_GetFXReprintTagFile] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TagPush_GetFXReprintTagFile] TO [IRMAClientRole]
    AS [dbo];

