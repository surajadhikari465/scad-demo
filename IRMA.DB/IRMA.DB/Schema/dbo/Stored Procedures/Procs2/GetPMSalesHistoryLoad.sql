﻿CREATE PROCEDURE dbo.GetPMSalesHistoryLoad
    @EndDate datetime,
    @Weeks int
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @StartDate datetime

    SELECT @StartDate = DATEADD(day, 1, DATEADD(week, -1 * @Weeks, @EndDate))

    DECLARE @Sales_SumByItem TABLE (Date_Key smalldatetime NOT NULL ,
                                	Store_No int NOT NULL ,
                                	Item_Key int NOT NULL ,
                                    SubTeam_No int NOT NULL ,
                                	Total_Unit_Quantity decimal(18, 2) NULL ,
                                	Total_Sales_Amount money NULL,
                                    PriceHistoryID int NULL)

    INSERT INTO @Sales_SumByItem (Date_Key, Store_No, Item_Key, SubTeam_No, Total_Unit_Quantity, Total_Sales_Amount)
    SELECT Date_Key, Store_No, Sales_SumByItem.Item_Key, Sales_SumByItem.SubTeam_No,
           SUM(dbo.Fn_ItemSalesQty(Identifier, ItemUnit.Weight_Unit, Price_Level, 
                   Sales_Quantity, Return_Quantity, Package_Desc1, Weight)),
           SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount)
    FROM Sales_SumByItem (nolock)
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = Sales_SumByItem.Item_Key
    INNER JOIN 
        ItemIdentifier II
        on Item.Item_key = II.Item_Key
    INNER JOIN
        PMSubTeamInclude SI
        ON SI.SubTeam_No = Item.SubTeam_No
    LEFT JOIN
        ItemUnit
        ON Item.Retail_Unit_ID = ItemUnit.Unit_ID
    WHERE Price_Level = 1
        AND (Date_Key >= @StartDate AND Date_Key < DATEADD(day, 1, @EndDate))
    GROUP BY Date_Key, Store_No, Sales_SumByItem.Item_Key, Sales_SumByItem.SubTeam_No
    HAVING SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) > 0

    SET DATEFIRST 1 -- Set Monday as first day of week because it makes the date functions below easier

    UPDATE @Sales_SumByItem
    SET PriceHistoryID = ISNULL((SELECT MAX(PriceHistoryID)
                                 FROM PriceHistory PH (nolock)
                                 WHERE PH.Item_Key = Sales_SumByItem.Item_Key 
                                     AND PH.Store_No = Sales_SumByItem.Store_No
                                     AND PH.Effective_Date < DATEADD(day, 8 - DATEPART(dw, Sales_SumByItem.Date_Key), Sales_SumByItem.Date_Key)), 0)
    FROM @Sales_SumByItem As Sales_SumByItem
    INNER JOIN
        Store (nolock)
        ON Store.Store_No = Sales_SumByItem.Store_No 

    SET DATEFIRST 7 -- Reset to default of Sunday - just in case next process uses same connection

    SELECT 
        Sales_SumByItem.Store_No, 
        Sales_SumByItem.Item_Key, 
        Year as FiscalYear, 
        Period As FiscalPeriod, 
        Week As FiscalWeek,
        CONVERT(decimal(18,2), SUM(Total_Unit_Quantity)) As Unit_Quantity,
        CONVERT(money, SUM(Total_Sales_Amount)) As RetailSales,
        CONVERT(money, MAX(ISNULL(AH.AvgCost, 0))) As Cost,
        CONVERT(money, MAX(PH.Price / CASE WHEN PH.Multiple <> 0 THEN PH.Multiple ELSE 1 END)) AS Price,
        '' As Pending_Cost
    FROM @Sales_SumByItem As Sales_SumByItem
        LEFT JOIN
            PriceHistory PH (nolock)
            ON Sales_SumByItem.PriceHistoryID = PH.PriceHistoryID
	LEFT OUTER JOIN 
	    AvgCostHistory AH (nolock) 
	    ON Sales_SumByItem.Item_Key = AH.Item_Key AND Sales_SumByItem.Store_No =  AH.Store_No AND Sales_SumByItem.SubTeam_No = AH.SubTeam_No 
        INNER JOIN
            Date (nolock)
            ON Date.Date_Key = Sales_SumByItem.Date_Key
    WHERE ISNULL(AH.AvgCost, 0) > 0
    GROUP BY Sales_SumByItem.Store_No, Sales_SumByItem.Item_Key, Year, Period, Week   

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPMSalesHistoryLoad] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPMSalesHistoryLoad] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPMSalesHistoryLoad] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPMSalesHistoryLoad] TO [IRMAReportsRole]
    AS [dbo];

