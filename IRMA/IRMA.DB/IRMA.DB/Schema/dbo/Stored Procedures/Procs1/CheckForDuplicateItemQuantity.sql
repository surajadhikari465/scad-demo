CREATE PROCEDURE dbo.CheckForDuplicateItemQuantity 
@Store_No int, 
@Item_Key int 
AS 

SELECT COUNT(*) AS ItemQuantityCount 
FROM ItemQuantity
WHERE Store_No = @Store_No AND Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemQuantity] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemQuantity] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemQuantity] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemQuantity] TO [IRMAReportsRole]
    AS [dbo];

