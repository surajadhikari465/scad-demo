IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[RegionSalesCompByDayReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP  Procedure  dbo.RegionSalesCompByDayReport
GO

CREATE Procedure dbo.RegionSalesCompByDayReport
	(
		@Team_No int,
		@Subteam_No int,
		@CompLastYear bit,
		@CurrDate smalldatetime
	)
AS

-- Make sure incoming date has no time
SELECT @CurrDate = CONVERT(smalldatetime, CONVERT(varchar(255), @CurrDate, 101))

DECLARE @LWCurrDate smalldatetime, @TWY smallint, @TWP tinyint, @TWW tinyint, @TWD tinyint, @LWY smallint, @LWP tinyint, @LWW tinyint

IF @CompLastYear = 0
    SELECT @LWCurrDate = DATEADD(week, -1, @CurrDate)
ELSE
BEGIN
    SELECT @TWY = [Year], @TWP = Period, @TWW =  [Week], @TWD = Day_Of_Week
    FROM [Date] D (nolock)
    WHERE Date_Key = @CurrDate

    SELECT @LWCurrDate = Date_Key
    FROM [Date] D (nolock)
    WHERE  [Year] = (@TWY - 1) AND Period = @TWP AND [Week] = @TWW AND Day_Of_Week = @TWD
END

SELECT @LWY = [Year], @LWP = Period, @LWW =  [Week]
FROM [Date] D (nolock)
WHERE Date_Key = DATEADD(day, -1, @LWCurrDate)

SELECT @TWY = [Year], @TWP = Period, @TWW =  [Week]
FROM [Date] D (nolock)
WHERE Date_Key = DATEADD(day, -1, @CurrDate)

DECLARE @Comp TABLE (StoreAbbr varchar(5), Day_Of_Week tinyint, TWSales money, LWSales money)

INSERT INTO @Comp (StoreAbbr, Day_Of_Week, TWSales)
SELECT StoreAbbr, Day_Of_Week, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
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
WHERE [Year] = @TWY AND Period = @TWP AND [Week] = @TWW
    AND Sales.Date_Key < @CurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr, Day_Of_Week

INSERT INTO @Comp (StoreAbbr, Day_Of_Week, LWSales)
SELECT StoreAbbr, Day_Of_Week, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount - Store_Coupon_Amount)
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
WHERE [Year] = @LWY AND Period = @LWP AND [Week] = @LWW
    AND Sales.Date_Key < @LWCurrDate
    AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, Sales.SubTeam_No) = Sales.SubTeam_No
GROUP BY  StoreAbbr, Day_Of_Week
    
SELECT @TWW As [Week], StoreAbbr, Day_Of_Week, SUM(TWSales) As TWSales, SUM(LWSales) As LWSales
FROM @Comp C
GROUP BY StoreAbbr, Day_Of_Week 
ORDER BY StoreAbbr, Day_Of_Week

GO
