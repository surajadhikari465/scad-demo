CREATE Procedure dbo.CalculateOrderDiscountForLineItem
	 @OrderDiscountType int, 
	 @OrderQuantityDiscount decimal(18,4), 
	 @ItemDiscountType int, 
	 @ItemQuantityDiscount decimal(18,4),
	 @LineCost money OUTPUT,
	 @LineFreight money OUTPUT
AS
--
-- Calculates new line item costs with discount - EXCEPT FOR FREE ITEMS
-- This stored procedure is called by CalculateOrderItemCosts and should not be called directly by anyone else, ever.
--
IF ISNULL(@ItemDiscountType, 0) > 0
BEGIN
    -- 1=Cash Discount; 2=Percent; 3=Free Items (supported in the caller, CalculateOrderItemCosts); 4=Landed Percent
    IF ISNULL(@ItemDiscountType, 0) IN (1,2,4)
    BEGIN
        SELECT @LineCost = CASE @ItemDiscountType WHEN 1 THEN @LineCost - @ItemQuantityDiscount ELSE @LineCost * ((100 - @ItemQuantityDiscount) / 100) END
        
        IF @ItemDiscountType = 4
            SELECT @LineFreight = @LineFreight * ((100 - @ItemQuantityDiscount) / 100)
    END
END
ELSE
BEGIN
    IF ISNULL(@OrderDiscountType, 0) = 2
    BEGIN
        -- The only supported value now is 2, which is "Percent".  We used to have 1=Cash Discount and 3=Landed Percent.
        SELECT @LineCost = @LineCost * ((100 - @OrderQuantityDiscount) / 100)
        
--        IF @OrderDiscountType = 4
--            SELECT @LineFreight = @LineFreight * ((100 - @OrderQuantityDiscount) / 100) 
    END
END

IF @LineCost < 0
    SET @LineCost = 0
IF @LineFreight < 0
    SET @LineFreight = 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CalculateOrderDiscountForLineItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CalculateOrderDiscountForLineItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CalculateOrderDiscountForLineItem] TO [IRMAClientRole]
    AS [dbo];

