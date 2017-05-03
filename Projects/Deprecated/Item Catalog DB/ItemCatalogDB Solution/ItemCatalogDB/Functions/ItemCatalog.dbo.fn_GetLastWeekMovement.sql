
/****** Object:  UserDefinedFunction [dbo].[fn_GetLastWeekMovement]    Script Date: 04/17/2007 10:50:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetLastWeekMovement]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetLastWeekMovement]
GO

CREATE FUNCTION dbo.fn_GetLastWeekMovement (
	@Store_No int, 
	@Item_Key int,
	@Identifier varchar(13)
)

RETURNS int
AS
BEGIN

	-- Declare all variables
	DECLARE @BeginDate datetime, @EndDate datetime, @Units int

	-- Assign the @BeginDate and @EndDate variables
	SELECT 
		@BeginDate = DATEADD(week, -1, getdate()),
		@EndDate = DATEADD(day, -1, CONVERT(datetime, getdate(), 101)) 

	-- Debug the begin and end date values. ? Is this how "last week" was requested?

	-- Assign @Units
	SET @Units = (
		SELECT 
			SUM(dbo.fn_ItemSalesQty(
				ItemIdentifier.Identifier,
				ItemUnit.Weight_Unit,
				Sales_SumByItem.Price_Level,
				Sales_SumByItem.Sales_Quantity,
				Sales_SumByItem.Return_Quantity,
				Item.Package_Desc1,
				Sales_SumByItem.Weight)
				) AS Units
		FROM
			Item (nolock)
			INNER JOIN Sales_SumByItem (nolock)
				ON Item.Item_Key = Sales_SumByItem.Item_Key
			INNER JOIN ItemIdentifier (nolock)
				ON Item.Item_Key = ItemIdentifier.Item_Key
			INNER JOIN ItemUnit (nolock) 
				ON Item.Retail_Unit_ID = ItemUnit.Unit_ID
		WHERE 
			Sales_SumByItem.Store_No = ISNULL(@Store_No, 0)
			AND Sales_SumByItem.Item_Key = ISNULL(@Item_Key, 0)
			AND ItemIdentifier.Identifier = ISNULL(@Identifier, '')
			AND (Sales_SumByItem.Date_Key >= CONVERT(smalldatetime, @BeginDate) 
				AND Sales_SumByItem.Date_Key < DATEADD(day, 1, CONVERT(smalldatetime, @EndDate))) 
		GROUP BY Item.Item_Key			
		)
		
	-- Return the result of the function
	RETURN @Units

END
GO
