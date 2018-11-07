SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TGMToolGetDataCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TGMToolGetDataCategory]
GO


CREATE PROCEDURE dbo.TGMToolGetDataCategory
@SubTeam_No int, 
@StartDate as DateTime, 
@EndDate as DateTime, 
@Discontinue_Item bit, 
@WFM_Item bit,
@Category_ID int
AS
/**********************************************************************************************************************************************************************************************************************************
CHANGE LOG
DEV					DATE					TASK						Description
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DN					2013.01.11				TFS 8755					Using the function dbo.fn_GetDiscontinueStatus instead of the Discontinue_Item field in the Item table.
**********************************************************************************************************************************************************************************************************************************/

BEGIN
    DECLARE @Item TABLE(Item_Key int PRIMARY KEY)

    INSERT INTO @Item
    SELECT Item_Key
    FROM Item (NOLOCK)
    WHERE SubTeam_No = @SubTeam_No
    AND dbo.fn_GetDiscontinueStatus(Item_Key,NULL,NULL) <= @Discontinue_Item AND WFM_Item >= @WFM_Item AND Category_ID = @Category_ID
    AND Retail_Sale = 1 AND Deleted_Item = 0

    DECLARE @ActualSales TABLE(Store_No int, Item_Key int, TotalActualRetail money)

    INSERT INTO @ActualSales
    SELECT Sales_SumByItem.Store_No, Item.Item_Key, SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) AS TotalActualRetail 
    FROM Store (NOLOCK) 
    LEFT JOIN 
        Sales_SumByItem WITH (INDEX(PK_Sales_SumByItem), NOLOCK)  
        ON (Sales_SumByItem.Date_Key >= @StartDate AND Sales_SumByItem.Date_Key < @EndDate)
           AND Store.Store_No = Sales_SumByItem.Store_No
    INNER JOIN 
        Item (NOLOCK) 
        ON Item.Item_key = Sales_SumByItem.Item_Key
    INNER JOIN @Item I ON I.Item_Key = Item.Item_Key
    WHERE ((Mega_Store = 1) OR (WFM_Store = 1)) 
    GROUP BY Sales_SumByItem.Store_No, Item.Item_Key

    SELECT T1.Item_Key, T1.Store_No, 
        Identifier, Item_Description, CONVERT(varchar(8),CAST(Item.Package_Desc1 AS float)) + '/' + CONVERT(varchar(8),CAST(Item.Package_Desc2 AS float)) + ' ' + ISNULL(ItemUnit.Unit_Abbreviation,'') AS Package_Desc,
        Category_ID, Item.SubTeam_No, 
        ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, Store.Store_No, @SubTeam_No, GETDATE()), 0) AS CurrentCost, 
        ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, Store.Store_No, @SubTeam_No, GETDATE()), 0) AS CurrentExtCost, 
        Price.Price / Price.Multiple AS CurrentRetail, 
        CAST(CASE WHEN ISNULL(RU.Weight_Unit, 0) = 1 AND dbo.fn_IsScaleItem(Identifier) = 0 THEN 1 ELSE 0 END AS int) AS Sold_By_Weight, 
        T1.TotalQuantity, T2.TotalActualRetail, T1.TotalRetail, T1.TotalCost, T1.TotalExtCost
    FROM @ActualSales T2 
    LEFT JOIN (
          SELECT ItemHistory.Item_Key, ItemHistory.Store_No,  
                 CAST(SUM(CASE WHEN (ISNULL(ItemUnit.Weight_Unit, 0) = 1) AND (ISNULL(ItemHistory.Weight, 0) <> 0) 
                               THEN Weight 
                               ELSE CASE WHEN ISNULL(Quantity, 0) <> 0 THEN Quantity ELSE ISNULL(Weight, 0) END
                               END) AS DECIMAL(18,4)) AS TotalQuantity,
                 SUM(CASE WHEN (ISNULL(ItemUnit.Weight_Unit, 0) = 1) AND (ISNULL(ItemHistory.Weight, 0) <> 0) 
                          THEN Weight 
                          ELSE CASE WHEN ISNULL(Quantity, 0) <> 0 THEN Quantity ELSE ISNULL(Weight, 0) END
                          END 
                     * Retail) AS TotalRetail,
                 SUM(CASE WHEN (ISNULL(ItemUnit.Weight_Unit, 0) = 1) AND (ISNULL(ItemHistory.Weight, 0) <> 0) 
                          THEN Weight 
                          ELSE CASE WHEN ISNULL(Quantity, 0) <> 0 THEN Quantity ELSE ISNULL(Weight, 0) END
                          END
                     * Cost) AS TotalCost,
                 SUM(CASE WHEN (ISNULL(ItemUnit.Weight_Unit, 0) = 1) AND (ISNULL(ItemHistory.Weight, 0) <> 0) 
                          THEN Weight 
                          ELSE CASE WHEN ISNULL(Quantity, 0) <> 0 THEN Quantity ELSE ISNULL(Weight, 0) END
                          END
                     * ExtCost) AS TotalExtCost
          FROM ItemHistory (NOLOCK)
          INNER JOIN 
              Item (NOLOCK) 
              ON Item.Item_Key = ItemHistory.Item_Key
                 AND ItemHistory.Adjustment_ID = 3 AND (ItemHistory.DateStamp >= @StartDate AND ItemHistory.DateStamp < @EndDate)
          LEFT JOIN
              ItemUnit (NOLOCK)
              ON Item.Retail_Unit_ID = ItemUnit.Unit_ID
          GROUP BY ItemHistory.Item_Key, ItemHistory.Store_No)
          T1 ON (T2.Item_Key = T1.Item_Key AND T2.Store_No = T1.Store_No)
    INNER JOIN 
        Price (NOLOCK) 
        ON T1.Item_Key = Price.Item_Key AND T1.Store_No = Price.Store_No
    INNER JOIN 
        Store (NOLOCK)
        ON Store.Store_No = Price.Store_No 
    INNER JOIN 
        Item (NOLOCK) 
        ON Price.Item_Key = Item.Item_Key
    INNER JOIN
        @Item I ON I.Item_Key = Item.Item_Key
    INNER JOIN 
        ItemIdentifier (NOLOCK) 
        ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
    LEFT JOIN 
        ItemUnit (NOLOCK)
        ON Item.Package_Unit_ID = ItemUnit.Unit_ID 
    LEFT JOIN
        ItemUnit RU (nolock)
        ON Item.Retail_Unit_ID = RU.Unit_ID
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


