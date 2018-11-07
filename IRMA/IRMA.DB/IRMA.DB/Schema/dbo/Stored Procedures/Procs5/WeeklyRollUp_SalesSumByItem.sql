CREATE PROCEDURE dbo.WeeklyRollUp_SalesSumByItem
@StartDate datetime,
@EndDate datetime
AS 

declare @FixStartDate datetime,
	@FixEndDate datetime

select @FixStartDate = RTRIM(CAST(DATEPART(mm, @StartDate) AS CHAR(2))) + '/' + RTRIM(CAST(DATEPART(dd, @StartDate) AS CHAR(2))) + '/' + CAST(DATEPART(yy, @StartDate) AS CHAR(4))
select @FixEndDate = RTRIM(CAST(DATEPART(mm, @EndDate) AS CHAR(2))) + '/' + RTRIM(CAST(DATEPART(dd, @EndDate) AS CHAR(2))) + '/' + CAST(DATEPART(yy, @EndDate) AS CHAR(4))


INSERT INTO dbo.Sales_SumByItemWkly
select @FixEndDate as date_key,
store_no,
item_key, 
subteam_no,
0 as price_level,
sum(sales_quantity) as sales_quantity,
0 as return_quantity,
sum(weight) as weight,
sum(sales_amount) as sales_amount,
0 as return_amount,
0 as markdown_amount,
0 as promotion_amount,
0 as store_coupon_amount,
getdate()
from dbo.Sales_SumByItem
where date_key between @FixStartDate
    and @FixEndDate
group by store_no,item_key,subteam_no
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WeeklyRollUp_SalesSumByItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WeeklyRollUp_SalesSumByItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WeeklyRollUp_SalesSumByItem] TO [IRMAReports]
    AS [dbo];

