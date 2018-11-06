CREATE PROCEDURE dbo.GetItemMultipleUPCs
@Item_Key int
AS

SELECT UPC
FROM ItemUPC
WHERE Item_Key = @Item_Key AND UPC <> '0'
ORDER BY UPC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemMultipleUPCs] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemMultipleUPCs] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemMultipleUPCs] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemMultipleUPCs] TO [IRMAReportsRole]
    AS [dbo];

