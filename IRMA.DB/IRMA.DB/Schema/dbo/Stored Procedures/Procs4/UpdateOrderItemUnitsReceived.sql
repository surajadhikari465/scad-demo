CREATE PROCEDURE dbo.UpdateOrderItemUnitsReceived
    @OrderItem_ID int
AS

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE
        @Amount decimal(18,4),
        @FromUnit int,
        @ToUnit int,
        @PD1 decimal(9,4), 
        @PD2 decimal(9,4), 
        @PDU int, 
        @Total_Weight decimal(18,4), 
        @QuantityReceived decimal(9,2),
        @UnitsReceived decimal(18,4),
        @Unit int,
        @CostedByWeight bit

    SELECT @Unit = Unit_ID FROM ItemUnit (nolock) WHERE UnitSysCode = 'unit'
    
    SELECT  @Amount = CASE WHEN (CostedByWeight = 1) AND (Total_Weight > 0) THEN Total_Weight ELSE QuantityReceived END,
            @FromUnit = QuantityUnit,
            @ToUnit = CASE WHEN CostedByWeight = 1 
                           THEN Item.Package_Unit_ID 
                           ELSE @Unit END,
            @PD1 = OrderItem.Package_Desc1,
            @PD2 = OrderItem.Package_Desc2,
            @PDU = OrderItem.Package_Unit_ID,
            @Total_Weight = Total_Weight,
            @QuantityReceived = QuantityReceived,
            @CostedByWeight = Item.CostedByWeight
    FROM OrderItem (nolock)
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = OrderItem.Item_Key
    WHERE OrderItem_ID = @OrderItem_ID

    SELECT @Error_No = @@ERROR

    IF @QuantityReceived = 0 OR @QuantityReceived IS NULL
        SET @UnitsReceived = 0
    ELSE
    BEGIN
        IF ((@ToUnit = @Unit) OR (@CostedByWeight = 1 AND @Total_Weight = 0)) AND (@Error_No = 0)
        BEGIN
            -- Reverse the To Unit and the From for the call to this store proc
            -- because quantity is converted opposite from cost.
            EXEC CostConversion @Amount, @ToUnit, @FromUnit, @PD1, @PD2, @PDU, 0, 0, @UnitsReceived OUTPUT
    
            SELECT @Error_No = @@ERROR
        END
        ELSE -- ToUnit is a weight unit - simply use the total weight from the order item
            SELECT @UnitsReceived = @Amount
    END
        
    IF @Error_No = 0
    BEGIN
        UPDATE OrderItem
        SET UnitsReceived = @UnitsReceived
        FROM OrderItem (rowlock)
        WHERE OrderItem_ID = @OrderItem_ID

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No <> 0
    BEGIN
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('UpdateOrderItemUnitsReceived failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemUnitsReceived] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemUnitsReceived] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemUnitsReceived] TO [IRMAReportsRole]
    AS [dbo];

