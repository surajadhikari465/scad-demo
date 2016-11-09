CREATE PROCEDURE dbo.Replenishment_POSPush_DeAuthorizeItem
	@StoreItemAuthorizationId int,
	@POSDeAuth bit
AS 

BEGIN
	--RESET StoreItem.POSDeAuth FLAG FOR POS PUSH PROCESS
	UPDATE StoreItem 
	SET POSDeAuth = @POSDeAuth
	WHERE StoreItemAuthorizationId = @StoreItemAuthorizationId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeAuthorizeItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeAuthorizeItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeAuthorizeItem] TO [IRMASchedJobsRole]
    AS [dbo];

