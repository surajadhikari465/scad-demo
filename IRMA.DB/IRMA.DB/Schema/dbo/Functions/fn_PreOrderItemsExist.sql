CREATE FUNCTION [dbo].[fn_PreOrderItemsExist]
(
    @OrderHeader_ID int
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @count int
	DECLARE @result bit

    SELECT @count = COUNT(*)
    FROM OrderItem OI
    JOIN Item I ON I.Item_Key = OI.Item_Key
    WHERE OI.OrderHeader_ID = @OrderHeader_ID AND I.Pre_Order = 1
    
    IF @count > 0
		SELECT @result = 1
	ELSE
		SELECT @result = 0

	RETURN @result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PreOrderItemsExist] TO [IRMAClientRole]
    AS [dbo];

