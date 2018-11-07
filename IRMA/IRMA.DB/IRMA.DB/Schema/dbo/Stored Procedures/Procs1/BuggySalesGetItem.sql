CREATE PROCEDURE dbo.BuggySalesGetItem
@mskUPC int
AS

SELECT Item_Key 
FROM ItemIdentifier 
WHERE Deleted_Identifier = 0 AND Identifier = @mskUPC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuggySalesGetItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuggySalesGetItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuggySalesGetItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuggySalesGetItem] TO [IRMAReportsRole]
    AS [dbo];

