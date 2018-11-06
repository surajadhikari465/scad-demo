CREATE PROCEDURE dbo.ThirteenWeekMovementReport
    @Zone_ID int,
    @Store_No int,
    @SubTeam_No int,
    @Category_ID int,
    @FamilyCode varchar(13),
    @Identifier varchar(13),
    @EndDateStr varchar(255),
    @NumWeeks int,
    @IncludeSaleAmt int
WITH RECOMPILE
AS
/*
DECLARE @Zone_ID int,
        @Store_No int,
        @SubTeam_No int,
        @Category_ID int,
        @FamilyCode varchar(255),
        @Identifier varchar(255),
        @EndDateStr varchar(255),
        @NumWeeks int,
        @IncludeSaleAmt int
SELECT  --@Zone_ID int,
        @Store_No = 10094,
        @SubTeam_No = 2200,
        --@Category_ID int,
        --@FamilyCode = '76280',
        @Identifier = '76331247937',
        @EndDateStr = '20051029',
        @NumWeeks = 2,
        @IncludeSaleAmt = 0

*/



BEGIN
    SET NOCOUNT ON

    DECLARE @StartDate datetime, @EndDate datetime

    SELECT @EndDate = DATEADD(day, 1, CONVERT(datetime, @EndDateStr, 101)),
           @StartDate = DATEADD(week, -1 * @NumWeeks, @EndDate)

    SELECT 
        Identifier,
        Item_Description,
        ISNULL(Brand_Name, '') As Brand,
        SUM(CASE WHEN Date_Key >= @StartDate AND Date_Key < DATEADD(week, 1, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 1, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week1_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 1, @StartDate) AND Date_Key < DATEADD(week, 2, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 2, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week2_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 2, @StartDate) AND Date_Key < DATEADD(week, 3, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 3, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week3_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 3, @StartDate) AND Date_Key < DATEADD(week, 4, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 4, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week4_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 4, @StartDate) AND Date_Key < DATEADD(week, 5, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 5, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week5_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 5, @StartDate) AND Date_Key < DATEADD(week, 6, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 6, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week6_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 6, @StartDate) AND Date_Key < DATEADD(week, 7, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 7, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week7_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 7, @StartDate) AND Date_Key < DATEADD(week, 8, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 8, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week8_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 8, @StartDate) AND Date_Key < DATEADD(week, 9, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 9, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week9_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 9, @StartDate) AND Date_Key < DATEADD(week, 10, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 10, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week10_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 10, @StartDate) AND Date_Key < DATEADD(week, 11, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 11, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week11_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 11, @StartDate) AND Date_Key < DATEADD(week, 12, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 12, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week12_Qty,
        SUM(CASE WHEN Date_Key >= DATEADD(week, 12, @StartDate) AND Date_Key < DATEADD(week, 13, @StartDate) 
                 THEN dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)
                 ELSE CASE WHEN DATEADD(week, 13, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week13_Qty,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= @StartDate AND Date_Key < DATEADD(week, 1, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 1, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week1_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 1, @StartDate) AND Date_Key < DATEADD(week, 2, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 2, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week2_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 2, @StartDate) AND Date_Key < DATEADD(week, 3, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 3, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week3_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 3, @StartDate) AND Date_Key < DATEADD(week, 4, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 4, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week4_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 4, @StartDate) AND Date_Key < DATEADD(week, 5, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 5, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week5_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 5, @StartDate) AND Date_Key < DATEADD(week, 6, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 6, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week6_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 6, @StartDate) AND Date_Key < DATEADD(week, 7, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 7, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week7_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 7, @StartDate) AND Date_Key < DATEADD(week, 8, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 8, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week8_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 8, @StartDate) AND Date_Key < DATEADD(week, 9, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 9, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week9_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 9, @StartDate) AND Date_Key < DATEADD(week, 10, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 10, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week10_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 10, @StartDate) AND Date_Key < DATEADD(week, 11, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 11, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week11_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 11, @StartDate) AND Date_Key < DATEADD(week, 12, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 12, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week12_Sales,
        SUM(CASE WHEN @IncludeSaleAmt = 1 AND Date_Key >= DATEADD(week, 12, @StartDate) AND Date_Key < DATEADD(week, 13, @StartDate) 
                 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount 
                 ELSE CASE WHEN @IncludeSaleAmt = 1 AND DATEADD(week, 13, @StartDate) <= @EndDate THEN 0 ELSE NULL END END) As Week13_Sales
    FROM
        Sales_SumByItem (nolock)
        INNER JOIN
            Store (nolock)
            ON Store.Store_No = Sales_SumByItem.Store_No
            AND ISNULL(@Store_No, Store.Store_No) = Store.Store_No
            AND ISNULL(Store.Zone_ID, 0) = ISNULL(@Zone_ID, ISNULL(Store.Zone_ID, 0))
        INNER JOIN
            Item (nolock)
            ON Item.Item_Key = Sales_SumByItem.Item_Key
            AND ISNULL(Category_ID, 0) = ISNULL(@Category_ID, ISNULL(Category_ID, 0))            
        INNER JOIN
            ItemIdentifier (nolock)
            ON ItemIdentifier.Item_Key = Sales_SumByItem.Item_Key and Default_Identifier = case when @Identifier is null then 1 else default_identifier end
        LEFT JOIN
            ItemUnit
            on Item.Retail_unit_id = ItemUnit.Unit_ID
        LEFT JOIN
            ItemBrand (nolock)
            ON ItemBrand.Brand_ID = Item.Brand_ID
    WHERE 
        Date_Key >= @StartDate AND Date_Key < @EndDate
        AND Sales_SumByItem.SubTeam_No = @SubTeam_No
        AND itemidentifier.identifier LIKE CASE WHEN @Identifier IS NULL THEN ISNULL(@FamilyCode,'') + '%' ELSE @Identifier END
    GROUP BY Identifier, Item_Description, ISNULL(Brand_Name, '')

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThirteenWeekMovementReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThirteenWeekMovementReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThirteenWeekMovementReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThirteenWeekMovementReport] TO [IRMAReportsRole]
    AS [dbo];

