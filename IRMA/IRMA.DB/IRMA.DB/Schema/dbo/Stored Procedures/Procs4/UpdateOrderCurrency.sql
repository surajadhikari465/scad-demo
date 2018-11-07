CREATE PROCEDURE dbo.UpdateOrderCurrency 
	@OrderHeader_ID int,
	@CurrencyID int
AS

UPDATE OrderHeader
SET CurrencyID = @CurrencyID
FROM OrderHeader (rowlock)
WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderCurrency] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderCurrency] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderCurrency] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderCurrency] TO [IRMASchedJobsRole]
    AS [dbo];

