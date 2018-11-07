CREATE FUNCTION [dbo].[fn_ItemSalesQty_NotSO] 
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

select @Return = CASE isnull(@Weight_UnitID, 0) + cast(dbo.FN_IsScaleItem(@Identifier) as int)
                    WHEN 0 
                       THEN (@Sales_Quantity - @Return_Quantity)
                       ELSE  @Weight 
                    END 
                    
RETURN @return
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemSalesQty_NotSO] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemSalesQty_NotSO] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemSalesQty_NotSO] TO [IRMAReportsRole]
    AS [dbo];

