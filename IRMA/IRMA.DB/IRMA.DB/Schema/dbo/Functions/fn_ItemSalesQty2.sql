CREATE FUNCTION dbo.fn_ItemSalesQty2 
(
	 @Item_Key int,
	 @Weight_Unit bit,  -- Based on the Item.Retail_Unit_Id
     @Price_Level tinyint,
     @Sales_Quantity int,
     @Return_Quantity int,
     @Package_Desc1 int,
     @Package_Desc2 Decimal(9, 4),
     @Weight decimal(9,2),
     @SalesAmount money,
     @UnitPrice money
)
RETURNS decimal(9,2)
AS
BEGIN  
    DECLARE @return decimal(9,2), @HasScaleIdentifier bit

    SELECT @HasScaleIdentifier = dbo.fn_HasScaleIdentifier(@Item_Key)

    IF @HasScaleIdentifier = 0 OR @Weight_Unit = 1
    BEGIN
        SET @Return = CASE WHEN @Weight_Unit = 0 OR @Price_Level = 3  -- Price_Level = 3 => case sale
                           THEN (@Sales_Quantity - @Return_Quantity) * CASE WHEN @Price_Level = 1 
                                                                            THEN 1 
                                                                            ELSE dbo.FN_GetExePack(@Package_Desc1, @Package_Desc2, @Weight_Unit)
                                                                            END
                           ELSE  @Weight 
                           END
    END
    ELSE -- @HasScaleIdentifier = 1 AND @Weight_Unit = 0
    BEGIN
        SET @Return = CASE WHEN (ISNULL(@UnitPrice, 0) = 0) OR (@SalesAmount = 0)
                           THEN (@Sales_Quantity - @Return_Quantity) * CASE WHEN @Price_Level = 1 
                                                                            THEN 1 
                                                                            ELSE dbo.FN_GetExePack(@Package_Desc1, @Package_Desc2, @Weight_Unit)
                                                                            END 
                           ELSE ROUND(@SalesAmount / @UnitPrice, 2) 
                           END
    END
                        
    RETURN @return
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemSalesQty2] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemSalesQty2] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemSalesQty2] TO [IRMASchedJobsRole]
    AS [dbo];

