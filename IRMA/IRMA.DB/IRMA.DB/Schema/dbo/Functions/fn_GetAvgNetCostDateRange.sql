CREATE FUNCTION [dbo].[fn_GetAvgNetCostDateRange]
	(@ItemKey int, 
	 @Store_No int, 
	 @StartDate smalldatetime, 
	 @EndDate smalldatetime)

	 RETURNS SMALLMONEY
AS
BEGIN
	DECLARE @AVERAGENETCOST SMALLMONEY
  
	SELECT @AVERAGENETCOST = 
	(	
		SELECT
			dbo.fn_GetAverageCostDateRange(@ItemKey, @Store_No, @StartDate, @EndDate)
				- isnull(dbo.fn_ItemNetDiscountDateRange(@ItemKey, @Store_No, @StartDate, @EndDate),0) 
	)
	RETURN @AVERAGENETCOST
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetAvgNetCostDateRange] TO [IRMAReportsRole]
    AS [dbo];

