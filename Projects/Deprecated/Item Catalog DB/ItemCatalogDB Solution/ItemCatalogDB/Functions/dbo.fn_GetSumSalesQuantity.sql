if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_GetSumSalesQuantity]'))
drop function [dbo].[fn_GetSumSalesQuantity]
GO

Create  FUNCTION dbo.fn_GetSumSalesQuantity
(

        @Item_Key int,
        @Store_No int,
        @StartDate datetime,
        @EndDate datetime

)
RETURNS integer
AS
BEGIN
Declare @SumQuantity as Integer


        select 
			@SumQuantity = SUM(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity))
        from 
			Sales_SumByItem
        where 
			Item_Key = @Item_Key
        and Store_No = @Store_No
        and Date_Key between @StartDate and @EndDate

        RETURN @SumQuantity
END


GO
