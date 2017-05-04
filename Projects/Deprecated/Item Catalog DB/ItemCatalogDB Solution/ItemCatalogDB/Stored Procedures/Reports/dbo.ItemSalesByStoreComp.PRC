SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].ItemSalesByStoreComp') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].ItemSalesByStoreComp
GO

CREATE PROCEDURE dbo.ItemSalesByStoreComp
    @BeginDate varchar(10),
    @EndDate varchar(10),
    @Identifier varchar(13)
WITH RECOMPILE
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @Item_Key int

    SELECT @Item_Key = item_Key from itemIdentifier where Identifier = @Identifier and Deleted_identifier = 0

    DECLARE @Results table(SalesAmount money, 
                           SalesQuantity money, 
                           AvgCost money)

    SELECT Store.Store_Name, SubTeam_Name, Category_Name, Item.Item_Description,
           SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) As SalesAmount,
           sum(dbo.Fn_ItemSalesQty(@Identifier, ItemUnit.Weight_Unit, Price_Level, 
                                   Sales_Quantity, Return_Quantity, Package_Desc1, 
                                   Weight * ((ISNULL(Sales_Quantity, 0) / CASE WHEN ISNULL(Sales_Quantity, 0) <> 0 
                                                                                THEN ABS(Sales_Quantity) 
                                                                                ELSE 1 
                                                                               END)  + 
                                            ((ISNULL(Return_Quantity, 0) / CASE WHEN ISNULL(Return_Quantity, 0) <> 0 
                                                                                 THEN ABS(Return_Quantity) 
                                                                                 ELSE 1 
                                                                                END) * -1))
                                  )                            
              ) As SalesQuantity,
           -- The next field is calculated SalesQuantity above * AvgCost
           SUM(SF.AvgCost * (dbo.Fn_ItemSalesQty(@Identifier, ItemUnit.Weight_Unit, Price_Level, 
                                                 Sales_Quantity, Return_Quantity, Package_Desc1, 
                                                 Weight * ((ISNULL(Sales_Quantity, 0) / CASE WHEN ISNULL(Sales_Quantity, 0) <> 0 
                                                                                              THEN ABS(Sales_Quantity) 
                                                                                              ELSE 1 
                                                                                             END)  + 
                                                          ((ISNULL(Return_Quantity, 0) / CASE WHEN ISNULL(Return_Quantity, 0) <> 0 
                                                                                               THEN ABS(Return_Quantity) 
                                                                                               ELSE 1 
                                                                                              END) * -1))
                                                )
                            )
              ) As AvgCost



    FROM Store (nolock) -- Show records for all top items for all stores whether or not there were sales in the reporting period
        INNER JOIN
            Price (nolock)
            ON Price.Store_No = Store.Store_No
        INNER JOIN
            Item (nolock)
            ON Item.Item_Key = Price.Item_Key
        LEFT JOIN 
            (SELECT Sales_Fact.*, 
		    ISNULL(dbo.fn_AvgCostHistory(Sales_Fact.Item_Key, Sales_Fact.Store_No, Sales_Fact.SubTeam_No, Sales_Fact.Time_Key), 0)
			AS AvgCost 
             FROM Sales_Fact (nolock)
             INNER JOIN 
                    Store (nolock) 
                    ON Store.Store_No = Sales_Fact.Store_No
             WHERE Time_Key >= CONVERT(smalldatetime, @BeginDate) 
                   AND Time_Key < DATEADD(day, 1, CONVERT(smalldatetime, @EndDate))
                   AND Item_Key = @Item_Key
            ) SF ON Price.Item_Key = SF.Item_Key 
                     AND Price.Store_No = SF.Store_No
        LEFT JOIN
            ItemUnit (nolock)
            on item.retail_unit_id = itemunit.unit_id
        INNER JOIN 
            itemcategory IC (nolock)    
            on Item.Category_ID = IC.Category_ID
        INNER JOIN
            SubTeam (nolock)
            on SubTeam.SubTeam_no = Item.SubTeam_no
    WHERE (Mega_Store = 1 OR WFM_Store = 1) and item.item_key = @item_key

    GROUP BY Item.Item_Description, Store_Name, SubTeam_Name, Category_Name 
    ORDER BY Item.Item_Description desc, Store_Name, SubTeam_Name, Category_Name 

    SET NOCOUNT OFF

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


