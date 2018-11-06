
CREATE PROCEDURE  dbo.Replenishment_POSPush_DeAuthorizeItem
	@StoreItemAuthorizationIds INTTYPE READONLY,
	@POSDeAuth bit
AS 

BEGIN

BEGIN TRANSACTION

BEGIN TRY	
	--RESET StoreItem.POSDeAuth FLAG FOR POS PUSH PROCESS
	UPDATE StoreItem 
	SET POSDeAuth = @POSDeAuth
	FROM StoreItem 
	INNER JOIN @StoreItemAuthorizationIds sids on StoreItem.StoreItemAuthorizationId = sids.[Key]
	
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	
	SELECT  
		'Replenishment_POSPush_DeAuthorizeItem failed with error: ' + ERROR_MESSAGE() AS ErrorMessage
		, ERROR_NUMBER() AS ErrorNumber, ERROR_SEVERITY() AS ErrorSeverity, ERROR_STATE() AS ErrorState  
		, ERROR_PROCEDURE() AS ErrorProcedure,ERROR_LINE() AS ErrorLine 
END CATCH

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

