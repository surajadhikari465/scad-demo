CREATE PROCEDURE dbo.TopMoversSummary_UseLastCost
    @BeginDate varchar(10),
    @EndDate varchar(10),
    @Top int,
    @Zone_ID int,
    @SubTeam_No int,
    @Category_ID int,
    @FamilyCode varchar(13),
    @Vendor_ID int,
    @ReverseOrder tinyint
WITH RECOMPILE
AS

BEGIN
    SET NOCOUNT ON

    CREATE TABLE #TopItems (Item_Key int PRIMARY KEY, Identifier varchar(13))
    DECLARE @SQL varchar(8000)
    SELECT @SQL = 
    'INSERT INTO #TopItems  
    SELECT TOP ' + CONVERT(varchar(255), @Top) + ' WITH TIES Sales_SumByItem.Item_Key, ItemIdentifier.Identifier
    FROM Sales_SumByItem (nolock)
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = Sales_SumByItem.Item_Key '
    SELECT @SQL = @SQL + '
    INNER JOIN
        ItemIdentifier (nolock)
        ON ItemIdentifier.Item_Key = Item.Item_Key'
    IF @FamilyCode IS NULL
        SELECT @SQL = @SQL + '
        AND ItemIdentifier.Default_Identifier = 1 '
    IF @Vendor_ID IS NOT NULL
        SELECT @SQL = @SQL + '
    INNER JOIN
        ItemVendor (nolock)
        ON ItemVendor.Item_Key = Item.Item_Key  '
    SELECT @SQL = @SQL + '
    INNER JOIN
        Store (nolock)
        ON Store.Store_No = Sales_SumByItem.Store_No
    WHERE Date_Key >= CONVERT(smalldatetime, ''' + @BeginDate + ''') AND Date_Key < DATEADD("day", 1, CONVERT(smalldatetime, ''' + @EndDate + ''')) '
    IF @Zone_ID IS NOT NULL
        SELECT @SQL = @SQL + '
        AND Zone_ID = ' + CONVERT(varchar(255), @Zone_ID) + ' '
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
        AND ItemVendor.Vendor_ID = ' + CONVERT(varchar(255), @Vendor_ID) + ' '
    SELECT @SQL = @SQL + '
    GROUP BY Sales_SumByItem.Item_Key, ItemIdentifier.Identifier '
    IF @ReverseOrder = 0 
      BEGIN
        SELECT @SQL = @SQL + ' ORDER BY SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) DESC'
      END
    ELSE
      BEGIN
        SELECT @SQL = @SQL + ' ORDER BY SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) ASC'
      END

    EXEC(@SQL)

    DECLARE @Results table(Store_Name varchar(255), 
                           SubTeam_Name varchar(255), 
                           Category_Name varchar(255), 
                           SalesAmount money, 
                           SalesQuantity money, 
                           Cost money)

    INSERT INTO @Results
    SELECT Store_Name, 
           SubTeam_Name, 
           Category_Name,
           SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) As SalesAmount,
           sum(dbo.Fn_ItemSalesQty(TI.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                                   Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As SalesQuantity,
                                   
           -- The next field is calculated SalesQuantity above * Cost
           SUM(SF.Cost * dbo.Fn_ItemSalesQty(TI.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                                                 Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As Cost                                                 
    FROM Store (nolock) -- Show records for all top items for all stores whether or not there were sales in the reporting period
        INNER JOIN
            Price (nolock)
            ON Price.Store_No = Store.Store_No
        INNER JOIN
            #TopItems TI -- Must use alias for table variables to refer to them
            ON TI.Item_Key = Price.Item_Key
        LEFT JOIN 
            (SELECT Sales_SumByItem.*, 
	   	ISNULL(dbo.fn_GetCurrentNetCost(Sales_SumByItem.Item_Key, Sales_SumByItem.Store_No), 0) AS Cost  
             FROM Sales_SumByItem (nolock)
             INNER JOIN 
                    Store (nolock) 
                    ON Store.Store_No = Sales_SumByItem.Store_No
             INNER JOIN
                    #TopItems TI
                    ON TI.Item_Key = Sales_SumByItem.Item_Key
             WHERE Date_Key >= CONVERT(smalldatetime, @BeginDate) AND Sales_SumByItem.Date_Key < DATEADD(day, 1, CONVERT(smalldatetime, @EndDate))
            ) SF ON (Price.Item_Key = SF.Item_Key 
                     AND Price.Store_No = SF.Store_No)
        INNER JOIN
            Item (nolock)
            ON Item.Item_Key = TI.Item_Key
        LEFT JOIN
            ItemUnit (nolock)
            on item.retail_unit_id = ItemUnit.unit_id
        LEFT JOIN
            ItemCategory (nolock)
            ON Item.Category_ID = ItemCategory.Category_ID
        INNER JOIN
            SubTeam (nolock)
            ON SubTeam.SubTeam_No = SF.SubTeam_No
    WHERE (Mega_Store = 1 OR WFM_Store = 1)
           AND Zone_ID = ISNULL(@Zone_ID, Zone_ID)
    GROUP BY Store_Name, SubTeam_Name, Category_Name
    ORDER BY Store_Name, SubTeam_Name, Category_Name

    SELECT R.Store_Name, 
           SubTeam_Name, 
           Category_Name, 
           SalesAmount, 
           SalesQuantity, 
           Cost, 
           TotalSalesAmount
    FROM @Results R
    INNER JOIN
        (SELECT Store_Name, 
                SUM(SalesAmount) As TotalSalesAmount
         FROM @Results
         GROUP BY Store_Name
        ) TS ON TS.Store_Name = R.Store_Name
--    UNION -- Add in the totals as if they are a separate category - keeps from having to use a subreport, which would run this query again.  'ZZZZ...Z' is replaced in the report with the word 'Total' - 'ZZZZ...Z' sorts it to the bottom
--        SELECT Store_Name, 
--               SubTeam_Name, 
--               REPLICATE('Z', 25) As Category_Name, 
--               SUM(SalesAmount) As SalesAmount, 
--               SUM(SalesQuantity) As SalesQuantity,
--               SUM(Cost) As Cost,
--               NULL As TotalSalesAmount
--        FROM @Results R
--        GROUP BY Store_Name, SubTeam_Name

    DROP TABLE #TopItems

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TopMoversSummary_UseLastCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TopMoversSummary_UseLastCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TopMoversSummary_UseLastCost] TO [IRMAReportsRole]
    AS [dbo];

