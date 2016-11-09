﻿CREATE FUNCTION dbo.fn_ItemSalesQty 
	(@Identifier varchar(13),
     @Weight_UnitID int,
     @Price_Level tinyint,
     @Sales_Quantity int,
     @Return_Quantity int,
     @Package_Desc1 int,
     @Weight decimal(9,2)
)
RETURNS decimal(9,2)
AS
BEGIN  
DECLARE @return decimal(9,2)

--If item is sold by weight (Weight_UnitID is not null) or Identifier is a scale item
--then return the weight as quantity, otherwise return sales_quantity - return.

--If the price level is not 1 and the item is either sold by weight or a scale item
--then also multiply the pack size.
select @Return = CASE isnull(@Weight_UnitID, 0) + cast(dbo.FN_IsScaleItem(@Identifier) as int)
                    WHEN 0 
                       THEN (@Sales_Quantity - @Return_Quantity) * 
							CASE 
								WHEN @Price_Level IS NULL THEN 1
								WHEN @Price_Level = 0 THEN 1
								WHEN @Price_Level = 1 THEN 1
								ELSE @Package_Desc1 
								END
                       ELSE  @Weight 
                    END 
                    
RETURN @return
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemSalesQty] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemSalesQty] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemSalesQty] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemSalesQty] TO [IRMAReportsRole]
    AS [dbo];

