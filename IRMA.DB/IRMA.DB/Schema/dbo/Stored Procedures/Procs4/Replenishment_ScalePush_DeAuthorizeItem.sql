CREATE PROCEDURE dbo.Replenishment_ScalePush_DeAuthorizeItem
	@StoreItemAuthorizationId int
AS 

BEGIN
	--RESET StoreItem.POSDeAuth FLAG FOR SCALE PUSH PROCESS
	UPDATE StoreItem 
	SET ScaleDeAuth = 0
	WHERE StoreItemAuthorizationId = @StoreItemAuthorizationId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_DeAuthorizeItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_DeAuthorizeItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_DeAuthorizeItem] TO [IRMASchedJobsRole]
    AS [dbo];

