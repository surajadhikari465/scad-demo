CREATE PROCEDURE dbo.Replenishment_POSPush_UpdateRefreshSent
@Identifier_ID int,
@StoreNo int
AS 

BEGIN
UPDATE StoreItem 
SET Refresh = 0
WHERE Item_Key = (SELECT Item_Key FROM ItemIdentifier WHERE Identifier_ID = @Identifier_ID)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdateRefreshSent] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdateRefreshSent] TO [IRMAClientRole]
    AS [dbo];

