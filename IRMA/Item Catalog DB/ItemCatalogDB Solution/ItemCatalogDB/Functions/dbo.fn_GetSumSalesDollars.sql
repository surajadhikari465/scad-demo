if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_GetSumSalesDollars]'))
drop Function [dbo].[fn_GetSumSalesDollars]
GO

Create  FUNCTION dbo.fn_GetSumSalesDollars
(

        @Item_Key int,
        @Store_No int,
        @StartDate datetime,
        @EndDate datetime

)
RETURNS decimal(9,2)
AS
BEGIN
Declare @SumDollars as Decimal(9,2)


        select 
			@SumDollars = sum(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) 
		from 
			Sales_SumByItem
		where 
			Item_Key = @Item_Key
        and Store_No = @Store_No
        and Date_Key between @StartDate and @EndDate

        RETURN @SumDollars
END


GO