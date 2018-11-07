CREATE PROCEDURE dbo.[VendorMovement]
    @BeginDate varchar(10),
    @EndDate varchar(10),
    @Top int,
    @Zone_ID int,
    @Store_No int,
    @SubTeam_No int,
    @Category_ID int,
    @FamilyCode VARCHAR(13),
    @Vendor_ID int,
    @ReverseOrder tinyint
WITH RECOMPILE
AS

BEGIN
    SET NOCOUNT ON
   
    CREATE TABLE #TopItems (Item_Key int PRIMARY KEY)
    DECLARE @SQL varchar(8000)
    SELECT @SQL = 
    'INSERT INTO #TopItems  
    SELECT TOP ' + CONVERT(varchar(255), @Top) + ' WITH TIES Sales_SumByItem.Item_Key
    FROM Sales_SumByItem (nolock)
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = Sales_SumByItem.Item_Key '
    SELECT @SQL = @SQL + '
    INNER JOIN
        ItemIdentifier (nolock)
        ON ItemIdentifier.Item_Key = Item.Item_Key '
    IF @FamilyCode IS NULL
        SELECT @SQL = @SQL + '
        AND ItemIdentifier.Default_Identifier = 1 '
    IF @Vendor_ID IS NOT NULL
        SELECT @SQL = @SQL + '
    INNER JOIN
        StoreItemVendor (nolock)
        ON StoreItemVendor.Item_Key = Item.Item_Key AND StoreItemVendor.Store_No = Sales_SumByItem.Store_No AND StoreItemVendor.PrimaryVendor = 1 '
    SELECT @SQL = @SQL + '
    INNER JOIN
        Store (nolock)
        ON Store.Store_No = Sales_SumByItem.Store_No
    WHERE Date_Key >= CONVERT(smalldatetime, ''' + @BeginDate + ''') AND Date_Key < DATEADD(d, 1, CONVERT(smalldatetime, ''' + @EndDate + ''')) '
    IF @Zone_ID IS NOT NULL
        SELECT @SQL = @SQL + '
        AND Zone_ID = ' + CONVERT(varchar(255), @Zone_ID) + ' '
    IF @Store_No IS NOT NULL
        SELECT @SQL = @SQL + '
        AND Sales_SumByItem.Store_No = ' + CONVERT(varchar(255), @Store_No) + ' '
    IF @SubTeam_No IS NOT NULL
        SELECT @SQL = @SQL + '
        AND Sales_SumByItem.SubTeam_No = ' + CONVERT(varchar(255), @SubTeam_No) + ' '
    IF @Category_ID IS NOT NULL
        SELECT @SQL = @SQL + '
        AND ISNULL(Item.Category_ID, 0) = ' + CONVERT(varchar(255), @Category_ID) + ' '
    IF @FamilyCode IS NOT NULL
        SELECT @SQL = @SQL + '
        AND ItemIdentifier.Identifier LIKE ''' + @FamilyCode + '%'' '
    IF @Vendor_ID IS NOT NULL
        SELECT @SQL = @SQL + '
        AND StoreItemVendor.Vendor_ID = ' + CONVERT(varchar(255), @Vendor_ID) + ' '
    SELECT @SQL = @SQL + '
    GROUP BY Sales_SumByItem.Item_Key '
    IF @ReverseOrder = 0
      BEGIN
        SELECT @SQL = @SQL + 'ORDER BY SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) DESC'
      END
    ELSE
      BEGIN
        SELECT @SQL = @SQL + 'ORDER BY SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) ASC'
      END
        
    EXEC(@SQL)



    select
        RESULT.*,
        -- get the price
        dbo.fn_Price(P.PriceChgTypeID, P.Multiple, P.Price, P.PricingMethod_ID, P.Sale_Multiple, P.Sale_Price) as Price,
        -- get the NET cost
        --dbo.fn_GetLastCost(RESULT.Item_Key, RESULT.Store_No) as Cost 
        dbo.fn_GetCurrentNetCost(RESULT.Item_Key, RESULT.Store_No) as Cost 
    FROM (
         SELECT 
            Store.Store_No,
            Store_Name,
		    CompanyName,
		    TI.Item_Key,
            Identifier, 
            Item_Description,
            Insert_Date,
            SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) As SalesAmount,
            sum(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As SalesQuantity,
            -- The next field is calculated SalesQuantity above * AvgCost
            SUM(SF.AvgCost * dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As AvgCost                                    
        FROM Store (nolock)
            INNER JOIN
                Price (nolock)
                    ON Price.Store_No = Store.Store_No
            INNER JOIN
                #TopItems TI -- Must use alias for table variables to refer to them
                    ON TI.Item_Key = Price.Item_Key
            LEFT JOIN 
                (SELECT Sales_SumByItem.*, 
	               ISNULL(dbo.fn_GetCurrentNetCost(Sales_SumByItem.Item_Key, Sales_SumByItem.Store_No), 0) AS AvgCost  
                 FROM Sales_SumByItem (nolock)
                    INNER JOIN 
                        Store (nolock)
                            ON Store.Store_No = Sales_SumByItem.Store_No
                    INNER JOIN
                        #TopItems TI
                            ON TI.Item_Key = Sales_SumByItem.Item_Key
                 WHERE Date_Key >= CONVERT(smalldatetime, @BeginDate) AND Date_Key < DATEADD(day, 1, CONVERT(smalldatetime, @EndDate))
                ) SF ON Price.Item_Key = SF.Item_Key AND Price.Store_No = SF.Store_No      
            INNER JOIN
                Item (nolock)
                    ON Item.Item_Key = TI.Item_Key
            INNER JOIN
                ItemIdentifier (nolock)
                    ON (ItemIdentifier.Item_Key = Item.Item_Key
                        AND Default_Identifier = 1)
            LEFT JOIN
                ItemUnit
                    ON Item.Retail_Unit_Id = ItemUnit.Unit_ID
            INNER JOIN 
	           StoreItemVendor (nolock)
	               ON StoreItemVendor.Item_Key = TI.Item_Key AND StoreItemVendor.Store_No = Store.Store_No AND StoreItemVendor.PrimaryVendor = 1
            INNER JOIN 
	           Vendor (nolock)
	        ON Vendor.Vendor_ID = StoreItemVendor.Vendor_ID
        WHERE (Mega_Store = 1 OR WFM_Store = 1)
            AND Zone_ID = ISNULL(@Zone_ID, Zone_ID)
            AND Store.Store_No = ISNULL(@Store_No, Store.Store_No)
            AND Vendor.Vendor_ID = ISNULL(@Vendor_ID, StoreItemVendor.Vendor_ID)
        GROUP BY Store.Store_No, Store_Name, CompanyName, TI.Item_Key, Identifier, Item_Description, Insert_Date
        ) as RESULT
        LEFT JOIN Price P (nolock)
            ON RESULT.Item_Key = P.Item_Key
                AND RESULT.Store_No = P.Store_No
        ORDER BY Store_Name, CompanyName, Identifier

    DROP TABLE #TopItems

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorMovement] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorMovement] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorMovement] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorMovement] TO [IRMAReportsRole]
    AS [dbo];

