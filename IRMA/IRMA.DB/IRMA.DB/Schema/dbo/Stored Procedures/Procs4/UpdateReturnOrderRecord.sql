CREATE PROCEDURE dbo.UpdateReturnOrderRecord (
    @Instance        INT,
    @User_ID         INT,
    @OrderItem_ID    INT,
    @Quantity        MONEY,
    @Total_Weight    MONEY,
    @CreditReason_ID INT)
AS
BEGIN
    IF @CreditReason_ID = -1
        SET @CreditReason_ID = (SELECT CreditReason_ID 
                                FROM   ReturnOrder 
                                WHERE  [Instance]   = @Instance AND
                                       [User_ID]    = @User_ID  AND
                                       OrderItem_ID = @OrderItem_ID)
    
    UPDATE ReturnOrder
    SET    Quantity        = @Quantity,
           Total_Weight    = @Total_Weight,
           CreditReason_ID = @CreditReason_ID
    WHERE  Instance        = @Instance AND
           [User_ID]       = @User_ID  AND
           OrderItem_ID    = @OrderItem_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateReturnOrderRecord] TO [IRMAClientRole]
    AS [dbo];

