CREATE PROCEDURE dbo.InsertCustomerReturnItem 
	@ReturnID int,
    @Item_Key int,
    @Quantity decimal(18,4),
    @Weight decimal(18,4),
    @Amount decimal(18,4),
    @CustReturnReasonID int,
    @ReturnItemID int OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    INSERT INTO CustomerReturnItem (ReturnID, Item_Key, Quantity, Weight, Amount, CustReturnReasonID)
    VALUES (@ReturnID, @Item_Key, @Quantity, @Weight, @Amount, @CustReturnReasonID)

    SELECT @ReturnItemID = SCOPE_IDENTITY()

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCustomerReturnItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCustomerReturnItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCustomerReturnItem] TO [IRMAReportsRole]
    AS [dbo];

