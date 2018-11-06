CREATE PROCEDURE dbo.BuggySalesSelectItem
@mskUPC bigint
AS

SELECT Item_Key 
FROM ItemIdentifier (NOLOCK) 
WHERE Deleted_Identifier = 0 AND Identifier = @mskUPC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuggySalesSelectItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuggySalesSelectItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuggySalesSelectItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuggySalesSelectItem] TO [IRMAReportsRole]
    AS [dbo];

