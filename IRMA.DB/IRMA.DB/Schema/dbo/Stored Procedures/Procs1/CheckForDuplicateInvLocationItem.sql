CREATE PROCEDURE dbo.CheckForDuplicateInvLocationItem
@InvLocID int,
@ItemID int

AS

SET NOCOUNT ON

SELECT 
	Count(InvLocID) as Found
FROM 
	InventoryLocationItems (nolock)
WHERE 
	InvLocID = @InvLocID and Item_Key = @ItemID

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateInvLocationItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateInvLocationItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateInvLocationItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateInvLocationItem] TO [IRMAReportsRole]
    AS [dbo];

