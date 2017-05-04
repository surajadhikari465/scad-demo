SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].MovementSingleStore') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].MovementSingleStore
GO
/*

	exec MovementSingleStore '08/01/2007', '08/02/2007', 10, null, 300, null, null, Null, null, 1

	exec MovementSingleStore '08/01/2007', '08/02/2007', 10, null, 300, null, null, Null, null, 0

*/
CREATE PROCEDURE dbo.MovementSingleStore
    @BeginDate varchar(10),
    @EndDate varchar(10),
    @Top int,
    @Zone_ID int,
    @Store_No varchar(8000),
    @SubTeam_No int,
    @Category_ID int,
    @FamilyCode VARCHAR(13),
    @Vendor_ID int,
    @ReverseOrder tinyint
    -- This is a variation of the TopMovers_UseLastCost report stored procedure, 
    -- which was used for the MovementDetail report
WITH RECOMPILE
AS

BEGIN
    SET NOCOUNT ON
   
    CREATE TABLE #TopItems (
		RowID int IDENTITY(1,1) NOT NULL,
		Item_Key int PRIMARY KEY
		)
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
    SELECT @SQL = @SQL + 'ORDER BY SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) '
    IF @ReverseOrder = 0
      BEGIN
        SELECT @SQL = @SQL + 'DESC'
      END
    ELSE
      BEGIN
        SELECT @SQL = @SQL + 'ASC'
      END
    EXEC(@SQL)
	
    SELECT 
		Store_Name, 
		SubTeam_Name, 
		Category_Name, 
		ItemIdentifier.Identifier, 
		Item_Description,
		SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) As SalesAmount,

		sum(dbo.fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
						   Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As SalesQuantity,
		                   
		-- The next field is calculated SalesQuantity above * Cost
		SUM(SF.Cost * dbo.fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
										 Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As Cost,
		Vendor.CompanyName as Vendor,
		Item.Package_Desc2 as Size,
		ItemUnit.Unit_Abbreviation as UOM,
		LItem.Identifier as LinkedPLU,
		TI.RowID
                                                 
    FROM Store (nolock)
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
         WHERE Date_Key >= CONVERT(smalldatetime, @BeginDate) AND Date_Key < DATEADD(day, 1, CONVERT(smalldatetime, @EndDate))
        ) SF ON Price.Item_Key = SF.Item_Key AND Price.Store_No = SF.Store_No      
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = TI.Item_Key
        
	LEFT JOIN 
        (select Item_Key, Identifier from dbo.ItemIdentifier (nolock)
		 where Default_Identifier = 1) LItem 
			ON Price.LinkedItem = LItem.Item_Key
    INNER JOIN StoreItemVendor siv (nolock)
        ON Store.Store_No = siv.Store_No
            AND Item.Item_Key = siv.Item_Key
    INNER JOIN Vendor (nolock)
        ON siv.Vendor_ID = Vendor.Vendor_ID
        
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
          AND 
			1=CASE 	
				WHEN @Store_No IS NULL THEN 1 			
				WHEN Store.Store_No IN (SELECT Key_Value FROM dbo.fn_Parse_List(@Store_No,',')) THEN 1 
				ELSE 0
			END
    GROUP BY Store_Name, SubTeam_Name, Category_Name, ItemIdentifier.Identifier, 
		Item_Description, Vendor.CompanyName, LItem.Identifier, 
		Item.Package_Desc2, ItemUnit.Unit_Abbreviation, TI.RowID
    ORDER BY RowID
    
    DROP TABLE #TopItems

    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


