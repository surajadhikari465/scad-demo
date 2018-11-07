CREATE PROCEDURE [dbo].[GetPriceBatchSign]
    @ItemList varchar(max),
    @ItemListSeparator char(1),
    @PriceBatchHeaderID int,
	@StartLabelPosition AS INT
AS
BEGIN
    SET NOCOUNT ON

	SELECT TOP (@StartLabelPosition - 1)
			NULL AS Origin_Name, 
			NULL AS Brand_Name, 
			'' AS Identifier, 
			'' AS CheckDigit,
			'' AS Sign_Description, 
			'' AS Ingredients,
			0 AS Multiple, 
			0 AS Price, 
			0 AS MSRPPrice, 
			0 AS Sale_Multiple, 
			0 AS Sale_Price, 
			NULL AS Sale_Start_Date, 
			NULL AS Sale_End_Date, 
			'' AS Retail_Unit_Abbr, 
			'' AS Retail_Unit_Full, 
			0 AS Sold_By_weight, 
			0 AS Organic, 
			0 AS Package_Desc2, 
			'' AS Package_Unit, 
			'' AS SubTeam_Name, 
			NULL AS PricingMethod_ID, 
			0 AS Package_Desc1, 
			0 AS Case_Price, 
			0 AS Vendor_ID,
			'' AS Vendor_Key,
			'' AS VendorItemID,
			0 AS Item_Key,
			NULL AS PLU,
			0 AS LabelType_ID,
			0 AS CurrentPrice, 
			0 AS POSPrice,
			0 AS POSSale_Price,
			0 as category_ID
	FROM ITEM

	UNION ALL 

    SELECT 
        Origin_Name, 
        Brand_Name, 
        PBD.Identifier, 
        II.CheckDigit,
        PBD.Sign_Description, 
        CASE 
            WHEN PBD.Ingredients <> '0' 
                THEN PBD.Ingredients 
                ELSE '' 
            END As Ingredients,
        Multiple, 
        Price, 
        MSRPPrice, 
        Sale_Multiple, 
        Sale_Price, 
        PBD.StartDate As Sale_Start_Date, 
        Sale_End_Date, 
        Retail_Unit_Abbr, 
        Retail_Unit_Full, 
        Sold_By_weight, 
        PBD.Organic, 
        PBD.Package_Desc2, 
        Package_Unit, 
        SubTeam.SubTeam_Name, 
        PricingMethod_ID, 
		dbo.fn_GetCurrentVendorPackage_Desc1(PBD.Item_Key, PBD.Store_No) AS Package_Desc1,
        Case_Price,
        PBD.Vendor_ID,
        V.Vendor_Key,
		IV.Item_ID AS VendorItemID,
        PBD.Item_Key,
        CASE 
            WHEN (LEFT(PBD.Identifier, 1) = '2') AND (LEN(PBD.Identifier) = 11) 
                THEN SUBSTRING(PBD.Identifier, 2, 5) 
                ELSE NULL 
            END AS PLU,
        I.LabelType_ID,
        dbo.fn_PricingMethodMoney(PBD.PriceChgTypeID, PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price) as CurrentPrice, 
        PBD.POSPrice,
        PBD.POSSale_Price,
		Category_ID
    FROM (
        SELECT 
            D.Item_Key, 
            D.Store_No, 
            D.ItemChgTypeID, 
            D.PriceChgTypeID, 
            PriceBatchHeaderID, 
            StartDate, 
            D.Sale_End_Date,
            --D.Tax_Table_A, D.Tax_Table_B, D.Tax_Table_C, D.Tax_Table_D,
            CASE 
                WHEN 1 = dbo.fn_OnSale(D.PriceChgTypeID)  
                        AND ISNULL(ItemChgTypeID, 0) <> 1 
                    THEN Price.Multiple
                    ELSE D.Multiple 
            END As Multiple,
            CASE 
                WHEN 1 = dbo.fn_OnSale(D.PriceChgTypeID)  
                        AND ISNULL(ItemChgTypeID, 0) <> 1 
                    THEN Price.Price
                	ELSE D.Price 
                END As Price,
            D.PricingMethod_ID, 
            D.Sale_Multiple, 
            D.Sale_Price,
            D.MSRPMultiple, 
            D.MSRPPrice, 
            POS_Description, 
            ScaleDesc1, 
            ScaleDesc2, 
            D.Ingredients, 
            Retail_Unit_Abbr, 
            D.Package_Desc1, 
            D.Package_Desc2, 
            Package_Unit,
            Sold_By_Weight, 
            D.Restricted_Hours, 
            Quantity_Required, 
            Price_Required, 
            Retail_Sale,
            D.Discountable, 
            Food_Stamps, 
            ItemType_ID, 
            D.SubTeam_No, 
            D.IBM_Discount,
            Origin_Name, 
            Brand_Name, 
            Identifier, 
            D.Sign_Description, 
            Retail_Unit_Full, 
            D.Organic, 
            Case_Price, 
            Vendor_ID,
            D.POSPrice,
            D.POSSale_Price 
        FROM PriceBatchDetail D (nolock)
            INNER JOIN Price (nolock)
                ON D.Item_Key = Price.Item_Key 
                    AND D.Store_No = Price.Store_No
        ) PBD 
        INNER JOIN PriceBatchHeader PBH (nolock)
            ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
        INNER JOIN (
            SELECT 
                Item_Key, 
                D.Store_No, 
                MAX(ISNULL(D.StartDate, H.StartDate)) As StartDate
            FROM PriceBatchDetail D (nolock)
                INNER JOIN PriceBatchHeader H
                    ON D.PriceBatchHeaderID = H.PriceBatchHeaderID
            WHERE D.PriceBatchHeaderID = @PriceBatchHeaderID
            GROUP BY Item_Key, D.Store_No
            ) G
                ON PBD.Item_Key = G.Item_Key 
                    AND PBD.Store_No = G.Store_No 
                    AND ISNULL(PBD.StartDate, PBH.StartDate) = G.StartDate
        INNER JOIN SubTeam (nolock)
            ON (PBD.SubTeam_No = SubTeam.SubTeam_No)
        INNER JOIN fn_Parse_List(@ItemList, @ItemListSeparator) IL
            ON IL.Key_Value = PBD.Item_Key 
        INNER JOIN Item I (nolock)
            ON I.Item_Key = IL.Key_Value
        LEFT JOIN Vendor V (nolock)
            ON PBD.Vendor_ID = V.Vendor_ID
        LEFT JOIN ItemVendor IV (nolock) ON IV.Item_Key = I.Item_Key AND IV.Vendor_ID = PBD.Vendor_ID
        INNER JOIN ItemIdentifier II (nolock)
            ON PBD.Item_Key = II.Item_Key
        WHERE PBD.PriceBatchHeaderID = @PriceBatchHeaderID
			AND II.Default_Identifier = 1
        
	Order by subteam_name, category_ID, Sign_Description

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchSign] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchSign] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchSign] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchSign] TO [IRMAReportsRole]
    AS [dbo];

