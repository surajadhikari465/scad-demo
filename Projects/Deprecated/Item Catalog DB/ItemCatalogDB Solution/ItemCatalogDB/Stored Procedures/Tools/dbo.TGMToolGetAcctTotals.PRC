SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TGMToolGetAcctTotals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TGMToolGetAcctTotals]
GO


CREATE PROCEDURE dbo.TGMToolGetAcctTotals
    @SubTeam_No int, 
    @StartDate DateTime, 
    @EndDate DateTime
AS

SELECT T1.Store_No, ActualRetail, TotalRetail, TotalCost, TotalExtCost
FROM (SELECT Sales_SumByItem.Store_No, SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) AS ActualRetail 
      FROM Store (NOLOCK) LEFT JOIN (
             Sales_SumByItem (NOLOCK) INNER JOIN Item (NOLOCK) ON (Item.Item_key = Sales_SumByItem.Item_Key)
           ) ON (Store.Store_No = Sales_SumByItem.Store_No)
      WHERE Sales_SumByItem.Date_Key >= @StartDate AND Sales_SumByItem.Date_Key < @EndDate AND 
            Item.SubTeam_No = @SubTeam_No
      GROUP BY Sales_SumByItem.Store_No) 
     T1 
INNER JOIN 
     (SELECT ItemHistory.Store_No,
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
      FROM Store (NOLOCK) 
      LEFT JOIN 
          ItemHistory (NOLOCK) 
          ON Store.Store_No = ItemHistory.Store_No 
      LEFT JOIN 
          Item (NOLOCK) 
          ON Item.Item_key = ItemHistory.Item_Key AND @SubTeam_No = Item.SubTeam_No
      LEFT JOIN
          ItemUnit (NOLOCK)
          ON Item.Retail_Unit_ID = ItemUnit.Unit_ID
      WHERE (ItemHistory.DateStamp >= @StartDate AND ItemHistory.DateStamp < @EndDate )
      AND ItemHistory.Adjustment_ID = 3 
      GROUP BY ItemHistory.Store_No) 
     T2 ON (T1.Store_No = T2.Store_No)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

