CREATE PROCEDURE dbo.TopMoversList
    @BeginDate varchar(10),
    @EndDate varchar(10),
    @Top int,
    @Zone_ID int,
    @Store_No int,
    @SubTeam_No int,
    @Category_ID int,
    @FamilyCode varchar(13),
    @Vendor_ID int,
    @ReverseOrder tinyint
WITH RECOMPILE
AS
/*DECLARE   @BeginDate varchar(10),
          @EndDate varchar(10),
          @Top int,
          @Zone_ID int,
          @Store_No int,
          @SubTeam_No int,
          @Category_ID int,
          @FamilyCode varchar(13),
          @Vendor_ID int

SELECT   @BeginDate ='04/20/2004',
         @EndDate = '04/20/2004',
         @Top = 10,
         --@Zone_ID ,
         @Store_No = 101,
         --@SubTeam_No int,
         --@Category_ID int,
         --@FamilyCode varchar(13),
         @Vendor_ID = 5764
--*/
BEGIN
    SET NOCOUNT ON

    CREATE TABLE #TopItems (Item_Key int PRIMARY KEY, Identifier varchar (15))
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
        ON ItemIdentifier.Item_Key = Item.Item_Key '
    IF @FamilyCode IS NULL
        SELECT @SQL = @SQL + '
        AND ItemIdentifier.Default_Identifier = 1 '
    IF @Vendor_ID IS NOT NULL
        SELECT @SQL = @SQL + '
    INNER JOIN
        ItemVendor (nolock)
        ON ItemVendor.Item_Key = Item.Item_Key '
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
        AND ItemVendor.Vendor_ID = ' + CONVERT(varchar(255), @Vendor_ID)  + ' '
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

    SELECT SubTeam_Name, 
           Category_Name, 
           Identifier, 
           Item_Description,
           SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) As SalesAmount,
           SUM(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                                   Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As SalesQuantity,             
           
           -- The next field is calculated SalesQuantity above * AvgCost
           SUM(SF.AvgCost * dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                                                 Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As AvgCost 
                                                 
    FROM (SELECT Sales_SumByItem.*, 
	   ISNULL(dbo.fn_AvgCostHistory(Sales_SumByItem.Item_Key, Sales_SumByItem.Store_No, Sales_SumByItem.SubTeam_No, Sales_SumByItem.Date_Key), 0) As AvgCost
	     FROM Sales_SumByItem (nolock)
             INNER JOIN
                #TopItems TI
                ON TI.Item_Key = Sales_SumByItem.Item_Key
             INNER JOIN
                Item (nolock)
                ON Item.Item_Key = TI.Item_Key
             INNER JOIN
                Store (nolock)
                ON Store.Store_No = Sales_SumByItem.Store_No
            INNER JOIN
                ItemIdentifier (nolock)
                ON (ItemIdentifier.Item_Key = Item.Item_Key 
                    and ItemIdentifier.Default_Identifier = 1)
         WHERE Zone_ID = ISNULL(@Zone_ID, Zone_ID)
               AND Sales_SumByItem.Store_No = ISNULL(@Store_No, Sales_SumByItem.Store_No)
               AND Date_Key >= CONVERT(smalldatetime, @BeginDate) 
               AND Date_Key < DATEADD(day, 1, CONVERT(smalldatetime, @EndDate))
         ) SF        
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = SF.Item_Key
    INNER JOIN
        ItemIdentifier (nolock)
        ON (ItemIdentifier.Item_Key = Item.Item_Key 
            AND Default_Identifier = 1)
    LEFT JOIN
        ItemUnit
        ON Item.Retail_Unit_ID = ItemUnit.Unit_ID
    LEFT JOIN
        ItemCategory (nolock)
        ON Item.Category_ID = ItemCategory.Category_ID
    INNER JOIN
        SubTeam (nolock)
        ON SubTeam.SubTeam_No = SF.SubTeam_No
    GROUP BY SubTeam_Name, Category_Name, Identifier, Item_Description
    ORDER BY SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) DESC

    DROP TABLE #TopItems

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TopMoversList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TopMoversList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TopMoversList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TopMoversList] TO [IRMAReportsRole]
    AS [dbo];

