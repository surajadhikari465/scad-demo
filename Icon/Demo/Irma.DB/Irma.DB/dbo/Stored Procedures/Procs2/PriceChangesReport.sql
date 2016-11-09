CREATE PROCEDURE dbo.PriceChangesReport
    @Begin_Date varchar(20),
    @End_Date varchar(20),
    @Store_No int, 
    @SubTeam_No int, 
    @WFM_Item int,
    @Price_Change int,
    @Item_Change int,
    @Promo_Change int,
    @New_Item int
AS

BEGIN

    SET NOCOUNT ON
 
    SELECT Item.Item_Key,
           ItemIdentifier.Identifier, 
           Item.Item_Description,
           Item.Package_Desc1, Item.Package_Desc2, ItemUnit.Unit_Abbreviation, 
           (CASE WHEN dbo.fn_OnSale(PBD.PriceChgTypeID) = 1 THEN Sale_Multiple ELSE Multiple END) AS Multiple,
           (CASE WHEN dbo.fn_OnSale(PBD.PriceChgTypeID) = 1 THEN Sale_Price ELSE Price END) AS Price,
	   ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @Store_No, Item.SubTeam_No, PBD.StartDate), 0) AS AvgCost, 
           SubTeam.SubTeam_Name,
           PBD.StartDate, PBD.Sale_End_Date,
           ISNULL(ItemChgTypeDesc, 'Price') As ItemChgTypeDesc, 
            PriceChgTypeDesc,
           (SELECT TOP 1 (CASE WHEN dbo.fn_OnSale(PBD.PriceChgTypeID) = 1 THEN Sale_Multiple ELSE Multiple END) AS Multiple
            FROM PriceHistory (nolock) 
            WHERE Effective_Date < @Begin_Date AND Store_No = @Store_No AND Item_Key = Item.Item_Key
            ORDER BY Effective_Date DESC) AS OldMultiple,
           (SELECT TOP 1 (CASE WHEN dbo.fn_OnSale(PBD.PriceChgTypeID) = 1 THEN Sale_Price ELSE Price END) AS Price
            FROM PriceHistory (nolock)
            WHERE Effective_Date < @Begin_Date AND Store_No = @Store_No AND Item_Key = Item.Item_Key
            ORDER BY Effective_Date DESC) AS OldPrice
    FROM 
       (SELECT Store_No, Item_Key, MAX(D.StartDate) As StartDate
        FROM PriceBatchDetail D (nolock)
        INNER JOIN
           PriceBatchHeader H (nolock)
           ON D.PriceBatchHeaderID = H.PriceBatchHeaderID AND PriceBatchStatusID = 6
        WHERE D.StartDate >= @Begin_Date AND D.StartDate <= @End_Date AND 
             ((@Price_Change = 1 AND dbo.fn_OnSale(D.PriceChgTypeID) = 0) OR 
              (@Item_Change = 1 AND ISNULL(D.ItemChgTypeID, 0) = 2) OR 
              (@Promo_Change = 1 AND dbo.fn_OnSale(D.PriceChgTypeID) = 1)) AND
             CASE WHEN ISNULL(D.ItemChgTypeID, 0) = 1 THEN 1 ELSE @New_Item END = @New_Item AND
             D.Store_No = @Store_No
        GROUP BY Store_No, Item_Key) T1
    INNER JOIN
        PriceBatchDetail PBD (nolock)
        ON PBD.Item_Key = T1.Item_Key AND PBD.Store_No = T1.Store_No AND PBD.StartDate = T1.StartDate
    INNER JOIN
        PriceBatchHeader PBH (nolock)
        ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = T1.Item_Key
    INNER JOIN
        ItemIdentifier (nolock)
        ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
    INNER JOIN
        SubTeam (nolock)
        ON SubTeam.SubTeam_No = Item.SubTeam_No
    INNER JOIN
        ItemUnit (nolock) 
        ON ItemUnit.Unit_ID = Item.Package_Unit_ID
    LEFT JOIN
        ItemChgType (nolock)
        ON PBH.ItemChgTypeID = ItemChgType.ItemChgTypeID
    INNER JOIN
        PriceChgType (nolock)
        ON PBH.PriceChgTypeID = PriceChgType.PriceChgTypeID
    WHERE SubTeam.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No) AND
          Item.WFM_Item >= @WFM_Item AND
          Item.Deleted_Item = 0

    SET NOCOUNT OFF

END