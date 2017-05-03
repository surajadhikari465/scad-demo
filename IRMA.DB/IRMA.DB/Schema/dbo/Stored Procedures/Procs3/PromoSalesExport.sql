CREATE PROCEDURE dbo.PromoSalesExport
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @BeginDate smalldatetime,
    @EndDate smalldatetime,
    @Brand_ID int,
    @FamilyCode varchar(13),
    @VendorID int,
    @SubTeamNo int,
    @CatID int
WITH RECOMPILE
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @StoreItem TABLE (Store_No int, Item_Key int, Sale_Price money, PRIMARY KEY (Store_No, Item_Key))

    IF @FamilyCode IS NOT NULL
        INSERT INTO @StoreItem (Store_No, Item_Key)
        SELECT Store.Key_Value, Item_Key
        FROM fn_Parse_List(@StoreList, @StoreListSeparator) Store, 
             (SELECT Item.Item_Key
              FROM 
                  Item (nolock)
                  INNER JOIN
                      ItemIdentifier II (nolock)
                      ON Item.Item_Key = II.Item_Key AND Default_Identifier = 1
                  INNER JOIN
                      ItemVendor IV (nolock)
                      ON IV.Item_key = Item.Item_Key
              WHERE Item.SubTeam_No = ISNULL(@SubTeamNo, Item.SubTeam_no)
                    AND ISNULL(Item.Category_ID, 0) = ISNULL(@CatID, ISNULL(Item.Category_ID, 0))   
                    AND IV.Vendor_ID = isnull(@VendorID, IV.Vendor_ID)
                    AND Deleted_Item = 0 
                    AND ISNULL(Brand_ID, 0) = ISNULL(@Brand_ID, ISNULL(Brand_ID, 0))
                    AND Identifier LIKE ISNULL(@FamilyCode + '%', Identifier)
              GROUP BY Item.Item_Key) I
    ELSE
        INSERT INTO @StoreItem (Store_No, Item_Key)
        SELECT store.Key_Value, Item_Key
        FROM fn_Parse_List(@StoreList, @StoreListSeparator) Store, 
             (SELECT Item.Item_Key
              FROM Item (nolock)
              INNER JOIN
                  ItemVendor IV (nolock)
                  ON IV.Item_key = Item.Item_Key
              WHERE Item.SubTeam_No = ISNULL(@SubTeamNo, Item.SubTeam_no)
                    AND ISNULL(Item.Category_ID, 0) = ISNULL(@CatID, ISNULL(Item.Category_ID, 0))   
                    AND IV.Vendor_ID = isnull(@VendorID, IV.Vendor_ID)
                    AND Deleted_Item = 0 
                    AND ISNULL(Brand_ID, 0) = ISNULL(@Brand_ID, ISNULL(Brand_ID, 0))
              GROUP BY Item.Item_Key) I            

    UPDATE @StoreItem
    SET Sale_Price = PH.Sale_Price
    FROM @StoreItem SI
    INNER JOIN
        (SELECT PriceHistory.Item_Key, PriceHistory.Store_No, MAX(PriceHistoryID) As MaxPriceHistoryID
         FROM PriceHistory (nolock)
         INNER JOIN
            @StoreItem SI2
            ON SI2.Item_Key = PriceHistory.Item_Key AND SI2.Store_No = PriceHistory.Store_No
         WHERE Effective_Date <= @EndDate
         GROUP BY PriceHistory.Item_Key, PriceHistory.Store_No) As MPH
        ON MPH.Item_Key = SI.Item_Key AND MPH.Store_No = SI.Store_No
    INNER JOIN
        PriceHistory PH (nolock)
        ON MPH.MaxPriceHistoryID = PH.PriceHistoryID

    SELECT
        Identifier,
        Item_Description As [Description],
        Brand_Name As Brand,
        MAX(Price) As Reg_Price,
        ISNULL(MAX(SI.Sale_Price), 0) As Sale_Price,
        StoreAbbr, 
        ISNULL(sum(dbo.Fn_ItemSalesQty(II.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)), 0) As Qty,
        ISNULL(SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount), 0) As Sales
    FROM
        @StoreItem SI
        INNER JOIN
            Store (nolock)
            ON SI.Store_No = Store.Store_No
        INNER JOIN
            Item (nolock)
            ON SI.Item_Key = Item.Item_Key
        INNER JOIN
            ItemIdentifier II (nolock)
            ON SI.Item_Key = II.Item_Key AND Default_Identifier = 1
        left JOIN 
            ItemUnit
            ON Item.Retail_Unit_ID = ItemUnit.Unit_ID
        INNER JOIN
            Price (nolock)
            ON Price.Item_Key = SI.Item_Key AND Price.Store_No = SI.Store_No
        LEFT JOIN
            ItemBrand (nolock)
            ON Item.Brand_ID = ItemBrand.Brand_ID
        LEFT JOIN
            Sales_SumByItem (nolock)
            ON SI.Store_No = Sales_SumByItem.Store_No AND SI.Item_Key = Sales_SumByItem.Item_Key
            AND Date_Key >= @BeginDate AND Date_Key <= @EndDate
    GROUP BY SI.Item_Key, Identifier, Item_Description, Brand_Name, SI.Store_No, StoreAbbr
    ORDER BY Identifier, StoreAbbr

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PromoSalesExport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PromoSalesExport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PromoSalesExport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PromoSalesExport] TO [IRMAReportsRole]
    AS [dbo];

