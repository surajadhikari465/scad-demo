CREATE Procedure dbo.RegionSalesCompByWeekReport
	(
		@Team_No int,
		@Subteam_No int,
		@CurrDate smalldatetime
	)
AS

-- Make sure incoming date has no time
SELECT @CurrDate = CONVERT(smalldatetime, CONVERT(varchar(255), @CurrDate, 101))

DECLARE @LYCurrDate smalldatetime, @TWY smallint, @TWP tinyint, @TWW tinyint, @TWD tinyint

SELECT @TWY = [Year], @TWP = Period, @TWW =  [Week], @TWD = Day_Of_Week
FROM [Date] D (nolock)
WHERE Date_Key = @CurrDate

SELECT @LYCurrDate = Date_Key
FROM [Date] D (nolock)
WHERE  [Year] = (@TWY - 1) AND Period = @TWP AND [Week] = @TWW AND Day_Of_Week = @TWD

SELECT @TWY = [Year], @TWP = Period
FROM [Date] D (nolock)
WHERE Date_Key = DATEADD(day, -1, @CurrDate)

DECLARE @Comp TABLE (StoreAbbr varchar(5), [Week] tinyint, TYSales money, LYSales money)

INSERT INTO @Comp (StoreAbbr, [Week], TYSales)
SELECT StoreAbbr, [Week], SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    [Date] D (nolock)
    ON D.Date_Key = Sales.Date_Key
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE [Year] = @TWY AND Period = @TWP
    AND Sales.Date_Key < @CurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr, [Week]

INSERT INTO @Comp (StoreAbbr, [Week], LYSales)
SELECT StoreAbbr, [Week], SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
FROM Sales_SumByItem Sales (nolock)
INNER JOIN
    [Date] D (nolock)
    ON D.Date_Key = Sales.Date_Key
INNER JOIN
    StoreSubTeam (nolock)
    ON StoreSubTeam.Store_No = Sales.Store_No AND StoreSubTeam.SubTeam_No = Sales.SubTeam_No
INNER JOIN
    Store (nolock)
    ON Store.Store_No = Sales.Store_No
WHERE [Year] = @TWY - 1 AND Period = @TWP
    AND Sales.Date_Key < @LYCurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr, [Week]

    
SELECT @TWP As Period, StoreAbbr, [Week], SUM(TYSales) As TYSales, SUM(LYSales) As LYSales
FROM @Comp C
GROUP BY StoreAbbr, [Week] 
ORDER BY StoreAbbr, [Week]
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RegionSalesCompByWeekReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RegionSalesCompByWeekReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RegionSalesCompByWeekReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RegionSalesCompByWeekReport] TO [IRMAReportsRole]
    AS [dbo];

