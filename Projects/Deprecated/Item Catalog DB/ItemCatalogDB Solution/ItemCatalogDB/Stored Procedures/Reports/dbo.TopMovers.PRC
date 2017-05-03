SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].TopMovers') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].TopMovers
GO
--exec TopMovers '05/05/2005', '05/05/2005', 10, null, 101, null, null, Null, null, 1
--exec TopMovers '05/05/2005', '05/05/2005', 10, null, 101, null, null, Null, null, 0
CREATE PROCEDURE dbo.TopMovers
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
        ItemVendor (nolock)
        ON ItemVendor.Item_Key = Item.Item_Key  '
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
        AND ItemVendor.Vendor_ID = ' + CONVERT(varchar(255), @Vendor_ID) + ' '
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

    SELECT Store_Name, 
           SubTeam_Name, 
           Category_Name, 
           Identifier, 
           Item_Description,
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
	   ISNULL(dbo.fn_AvgCostHistory(Sales_SumByItem.Item_Key, Sales_SumByItem.Store_No, Sales_SumByItem.SubTeam_No, DATEADD(day, 2, Sales_SumByItem.Date_Key)), 0) AS AvgCost  
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
    LEFT JOIN
        ItemCategory (nolock)
        ON Item.Category_ID = ItemCategory.Category_ID
    INNER JOIN
        SubTeam (nolock)
        ON SubTeam.SubTeam_No = SF.SubTeam_No
    WHERE (Mega_Store = 1 OR WFM_Store = 1)
          AND Zone_ID = ISNULL(@Zone_ID, Zone_ID)
          AND Store.Store_No = ISNULL(@Store_No, Store.Store_No)	
    GROUP BY Store_Name, SubTeam_Name, Category_Name, Identifier, Item_Description
    ORDER BY Store_Name, SubTeam_Name, Category_Name, Identifier

    DROP TABLE #TopItems

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


