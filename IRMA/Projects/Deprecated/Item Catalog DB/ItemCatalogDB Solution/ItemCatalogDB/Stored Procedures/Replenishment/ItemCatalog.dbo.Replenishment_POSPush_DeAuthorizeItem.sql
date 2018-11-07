/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_DeAuthorizeItem]    Script Date: 6/26/2007 16:32:49 ******/
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Replenishment_POSPush_DeAuthorizeItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	EXEC ('create PROCEDURE [dbo].[Replenishment_POSPush_DeAuthorizeItem] (@foo int) as select 1')
GO

ALTER PROCEDURE  dbo.Replenishment_POSPush_DeAuthorizeItem
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


 