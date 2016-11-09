CREATE Procedure dbo.RegionSalesCompTrendReport
	(
		@Team_No int,
		@Subteam_No int,
		@CurrDate smalldatetime
	)
AS

-- Make sure incoming date has no time
SELECT @CurrDate = CONVERT(smalldatetime, CONVERT(varchar(255), @CurrDate, 101))

DECLARE @TWY smallint, @TWP tinyint, @TWW tinyint, @TWD tinyint, @LYY smallint, @LYP tinyint, @LYW tinyint
DECLARE @TMON smalldatetime, @TBYMon smalldatetime, @LMON smalldatetime, @LBYMon smalldatetime
DECLARE @LCurrDate smalldatetime

SELECT @TWY = [Year], @TWP = Period, @TWW =  [Week], @TWD = Day_Of_Week
FROM [Date] D (nolock)
WHERE Date_Key = @CurrDate

SELECT @LCurrDate = Date_Key
FROM [Date] D (nolock)
WHERE  [Year] = (@TWY - 1) AND Period = @TWP AND [Week] = @TWW AND Day_Of_Week = @TWD

SELECT @TWY = [Year], @TWP = Period, @TWW =  [Week]
FROM [Date] D
WHERE Date_Key = DATEADD(day, -1, @CurrDate)

SELECT @LYY = [Year], @LYP = Period, @LYW =  [Week]
FROM [Date] D
WHERE Date_Key = DATEADD(day, -1, @LCurrDate)

SELECT @TMON = Date_Key
FROM [Date] D
WHERE Day_Of_Week = 1 AND [Year] = @TWY AND Period = @TWP AND [Week] = @TWW

SELECT @TBYMon = Date_Key
FROM [Date] D
WHERE Day_Of_Week = 1 AND [Year] = @TWY AND Period = 1 AND [Week] = 1

SELECT @LMON = Date_Key
FROM [Date] D
WHERE Day_Of_Week = 1 AND [Year] = @LYY AND Period = @LYP AND [Week] = @LYW

SELECT @LBYMon = Date_Key
FROM [Date] D
WHERE Day_Of_Week = 1 AND [Year] = @LYY AND Period = 1 AND [Week] = 1

DECLARE @Comp TABLE (StoreAbbr varchar(5), T2WSales money, T4WSales money, T8WSales money, T16WSales money, TYTDSales money, L2WSales money, L4WSales money, L8WSales money, L16WSales money, LYTDSales money)

--
-- Fill in this Year's Data
--

INSERT INTO @Comp (StoreAbbr, T2WSales)
SELECT StoreAbbr, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE Sales.Date_Key >= DATEADD(week, -1, @TMON)
    AND Sales.Date_Key < @CurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr

INSERT INTO @Comp (StoreAbbr, T4WSales)
SELECT StoreAbbr, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE Sales.Date_Key >= DATEADD(week, -3, @TMON)
    AND Sales.Date_Key < @CurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr

INSERT INTO @Comp (StoreAbbr, T8WSales)
SELECT StoreAbbr, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE Sales.Date_Key >= DATEADD(week, -7, @TMON)
    AND Sales.Date_Key < @CurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr

INSERT INTO @Comp (StoreAbbr, T16WSales)
SELECT StoreAbbr, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE Sales.Date_Key >= DATEADD(week, -15, @TMON)
    AND Sales.Date_Key < @CurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr

INSERT INTO @Comp (StoreAbbr, TYTDSales)
SELECT StoreAbbr, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE Sales.Date_Key >= @TBYMon
    AND Sales.Date_Key < @CurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr

--
-- Fill in last Year's Data
--

INSERT INTO @Comp (StoreAbbr, L2WSales)
SELECT StoreAbbr, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE Sales.Date_Key >= DATEADD(week, -1, @LMON) 
    AND Sales.Date_Key < @LCurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr

INSERT INTO @Comp (StoreAbbr, L4WSales)
SELECT StoreAbbr, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE Sales.Date_Key >= DATEADD(week, -3, @LMON)
    AND Sales.Date_Key < @LCurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr

INSERT INTO @Comp (StoreAbbr, L8WSales)
SELECT StoreAbbr, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE Sales.Date_Key >= DATEADD(week, -7, @LMON)
    AND Sales.Date_Key < @LCurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr

INSERT INTO @Comp (StoreAbbr, L16WSales)
SELECT StoreAbbr, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE Sales.Date_Key >= DATEADD(week, -15, @LMON)
    AND Sales.Date_Key < @LCurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr

INSERT INTO @Comp (StoreAbbr, LYTDSales)
SELECT StoreAbbr, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE Sales.Date_Key >= @LBYMon
    AND Sales.Date_Key < @LCurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr

SELECT 
    StoreAbbr,
    CASE WHEN L2WSales <> 0 THEN (T2WSales - L2WSales) / L2WSales ELSE 0 END As L2Comp,
    CASE WHEN L4WSales <> 0 THEN (T4WSales - L4WSales) / L4WSales ELSE 0 END As L4Comp,
    CASE WHEN L8WSales <> 0 THEN (T8WSales - L8WSales) / L8WSales ELSE 0 END As L8Comp,
    CASE WHEN L16WSales <> 0 THEN (T16WSales - L16WSales) / L16WSales ELSE 0 END As L16Comp,
    CASE WHEN LYTDSales <> 0 THEN (TYTDSales - LYTDSales) / LYTDSales ELSE 0 END As LYTDComp
FROM 
    (SELECT StoreAbbr, SUM(T2WSales) As T2WSales, SUM(L2WSales) As L2WSales, SUM(T4WSales) As T4WSales, SUM(L4WSales) As L4WSales, SUM(T8WSales) As T8WSales, SUM(L8WSales) As L8WSales, SUM(T16WSales) As T16WSales, SUM(L16WSales) As L16WSales, SUM(TYTDSales) As TYTDSales, SUM(LYTDSales) As LYTDSales
    FROM @Comp C
    GROUP BY StoreAbbr) T
ORDER BY StoreAbbr
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RegionSalesCompTrendReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RegionSalesCompTrendReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RegionSalesCompTrendReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RegionSalesCompTrendReport] TO [IRMAReportsRole]
    AS [dbo];

