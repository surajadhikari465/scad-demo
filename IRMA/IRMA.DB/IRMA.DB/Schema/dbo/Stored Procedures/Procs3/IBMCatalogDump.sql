CREATE PROCEDURE dbo.IBMCatalogDump
    @Store_No int
AS 

    SELECT LEFT(Identifier,12) AS Identifier,
           CASE WHEN ISNULL(RU.Weight_Unit, 0) = 1 AND dbo.fn_IsScaleItem(Identifier) = 0 THEN 1 ELSE 0 END As Sold_By_Weight, 
           Restricted_Hours, Quantity_Required, Price_Required, Retail_Sale, 
           --Tax_Table_A, Tax_Table_B, Tax_Table_C, Tax_Table_D, 
           Price.Discountable, Food_Stamps, ItemType_ID, Item.SubTeam_No, IBM_Discount, 
           CONVERT(smallmoney, ISNULL(ROUND(dbo.fn_Price(Price.PriceChgTypeID, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price) * CasePriceDiscount * Package_Desc1, 2),0)) AS Case_Price,
           Multiple, Price, dbo.fn_IsScaleItem(Identifier) as IsScaleItem, 
           PricingMethod_ID, Sale_Start_Date, Sale_End_Date, Sale_Multiple, Sale_Price, Sale_Earned_Disc1, 
           Sale_Earned_Disc2, Sale_Earned_Disc3, 
           LEFT(REPLACE(POS_Description,',',' '),18) AS Item_Desc,
           PCT.On_Sale
    FROM Item (nolock)
    INNER JOIN
        Price (nolock)
        ON (Item.Item_Key = Price.Item_Key AND Price.Store_No = @Store_No) 
    INNER JOIN
        ItemIdentifier (nolock)
        ON (ItemIdentifier.Item_Key = Item.Item_Key AND Add_Identifier = 0)
    INNER JOIN
        StoreSubTeam SST (nolock)
        ON SST.Store_No = Price.Store_No AND SST.SubTeam_No = Item.SubTeam_No
    LEFT JOIN
        ItemUnit RU
        ON RU.Unit_ID = Item.Retail_Unit_ID
    LEFT JOIN
		PriceChgType PCT
		ON Price.PriceChgTypeID = PCT.PriceChgTypeID
    WHERE Deleted_Item = 0 AND Remove_Item = 0 AND Retail_Sale = 1
        AND NOT EXISTS (SELECT * 
                        FROM PriceBatchDetail D
                        LEFT JOIN
                            PriceBatchHeader H
                            ON D.PriceBatchHeaderID = H.PriceBatchHeaderID 
                        WHERE D.ItemChgTypeID = 1 AND ISNULL(PriceBatchStatusID,0) < 6
                            AND D.Item_Key = Price.Item_Key AND D.Store_No = Price.Store_No)
    ORDER BY Identifier
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IBMCatalogDump] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IBMCatalogDump] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IBMCatalogDump] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IBMCatalogDump] TO [IRMAReportsRole]
    AS [dbo];

