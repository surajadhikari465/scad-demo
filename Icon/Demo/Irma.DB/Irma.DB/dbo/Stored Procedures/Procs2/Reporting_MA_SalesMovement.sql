CREATE Procedure dbo.Reporting_MA_SalesMovement
/*
@Top                    INT           = NULL,
@ReverseOrder           TINYINT       = 0,
@VendorID               INT           = NULL,
@Team_No                INT           = NULL,    -- JDA equivalent = Department
@Team_No_List           VARCHAR(1000) = NULL,    -- JDA equivalent = Department
@ClassID                INT           = NULL     -- JDA equivalent = SubDepartment 
@ProdHierarchyLevel3_ID INT           = NULL,    -- JDA equivalent = Class
@ProdHierarchyLevel4_ID INT           = NULL,    -- JDA equivalent = SubClass
@PriceChgTypeID         INT           = NULL,    -- JDA equivalent = Event #
@ItemIdentifierID       INT           = NULL,    -- JDA equivalent = SKU
@ItemStatus             VARCHAR(3)    = NULL,
@Unit_ID                INT           = NULL,    -- To get unit abbreviation
@Brand_ID               INT           = NULL,    -- To get Brand_Name
@SupplierType           VARCHAR(3)    = NULL,    -- 'WFM', 'CUS'=Customer, 'INT'=InternalCustomer
@StoreList              VARCHAR(1000) = NULL,    -- IF NULL, show no individual store columns
@BeginDate              VARCHAR(10),
@EndDate                VARCHAR(10),
@PrintMovement          BIT,
@PrintSales             BIT,
@PrintGrossProfit       BIT,
@PrintZeroMovement      BIT,
@SortOrder              CHAR(1),                 -- 'M'=Movement, 'S'=Sales, 'G'=Gross Profit
@ReportFormat           CHAR(3)                  -- 'CSV', 'EXC'=Excel, 'HTM'=HTML, 'PDF', 'XML'       
*/
WITH RECOMPILE
AS
BEGIN
    SET NOCOUNT ON
/*
    CREATE TABLE #TopItems (Item_Key int PRIMARY KEY)
    
    DECLARE @SQL varchar(8000)
    
    IF @Top IS NOT NULL
        BEGIN
            SELECT @SQL = 
            'INSERT INTO #TopItems  
             SELECT TOP ' + CONVERT(varchar(255), @Top) + ' WITH TIES Sales_SumByItem.Item_Key
             FROM Sales_SumByItem (nolock)
             INNER JOIN
                Item (nolock)
                ON Item.Item_Key = Sales_SumByItem.Item_Key '
        END
    ELSE
        BEGIN
            SELECT @SQL = 
            'INSERT INTO #TopItems  
             SELECT Sales_SumByItem.Item_Key
             FROM Sales_SumByItem (nolock)
             INNER JOIN
                Item (nolock)
                ON Item.Item_Key = Sales_SumByItem.Item_Key '
        END
    
-- remainder of this proc has not been touched or verified.
-- Assignment was shelved at this point.    
    
    
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
	IF @Store_No IS NOT NULL
        SELECT @SQL = @SQL + '
	INNER JOIN
        dbo.fn_Parse_List(''' + @Store_No + ''','','') i
        ON Sales_SumByItem.Store_No = i.Key_Value  '
    SELECT @SQL = @SQL + '
    INNER JOIN
        Store (nolock)
        ON Store.Store_No = Sales_SumByItem.Store_No
    WHERE Date_Key >= CONVERT(smalldatetime, ''' + @BeginDate + ''') AND Date_Key < DATEADD(d, 1, CONVERT(smalldatetime, ''' + @EndDate + ''')) '
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
    
*/
END