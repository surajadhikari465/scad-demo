CREATE PROCEDURE dbo.Replenishment_ScalePush_AuthorizeItem
	@StoreItemAuthorizationId int
AS 

BEGIN
	--RESET StoreItem.ScaleAuth FLAG FOR SCALE PUSH PROCESS
	UPDATE StoreItem 
	SET ScaleAuth = 0
	WHERE StoreItemAuthorizationId = @StoreItemAuthorizationId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_AuthorizeItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_AuthorizeItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_AuthorizeItem] TO [IRMASchedJobsRole]
    AS [dbo];

