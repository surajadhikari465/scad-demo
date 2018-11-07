IF EXISTS ( SELECT  *
            FROM    SYSOBJECTS
            WHERE   NAME = 'fn_ReceivedOrderedCost' ) 
    DROP FUNCTION dbo.fn_ReceivedOrderedCost
    GO

CREATE FUNCTION dbo.fn_ReceivedOrderedCost
    (
      @LineItemCost MONEY ,
      @QuantityOrdered DECIMAL(18, 4) ,
      @CostedByWeight BIT ,
      @QuantityUnit INT ,
      @Package_Desc1 DECIMAL(9, 4) ,
      @Package_Desc2 DECIMAL(9, 4) ,
      @Package_Unit_ID INT ,
      @UnitsReceived DECIMAL(18, 4) ,
      @unit INT ,
      @pound INT

    )
RETURNS DECIMAL(38, 28)
AS 
    BEGIN

    -- ReceivedOrderedCost = ([Ordered Line Item Total] (which includes discounts, if any, applied) / [units ordered]) * [units received]

    -- (This can be different from SUM(ReceivedItemCost) because if OrderItem.INVCost < OrderItem.Cost, INVCost is used to calculate ReceivedItemCost.  See SP UpdateOrderRefreshCosts for how this works.)

        RETURN (ISNULL(@LineItemCost, 0) / CASE WHEN dbo.fn_CostConversion(ISNULL(@QuantityOrdered, 0), CASE WHEN @CostedByWeight = 1 THEN @pound ELSE @unit END, @QuantityUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID) > 0 THEN dbo.fn_CostConversion(ISNULL(@QuantityOrdered, 0), CASE WHEN @CostedByWeight = 1 THEN @pound ELSE @unit END, @QuantityUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID) ELSE 1 END) * @UnitsReceived

    END
GO


