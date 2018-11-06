
CREATE PROCEDURE dbo.Replenishment_POSPush_AddIdentifier
(
	@IdentifierIds INTTYPE READONLY
)
AS 

BEGIN

BEGIN TRANSACTION

BEGIN TRY	
	UPDATE ItemIdentifier 
	SET Add_Identifier = 0
	FROM ItemIdentifier 
	INNER JOIN @IdentifierIds ids ON ItemIdentifier.Identifier_Id = ids.[Key]
	
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	
	SELECT  
		'Replenishment_POSPush_AddIdentifier failed with error: ' + ERROR_MESSAGE() AS ErrorMessage
		, ERROR_NUMBER() AS ErrorNumber, ERROR_SEVERITY() AS ErrorSeverity, ERROR_STATE() AS ErrorState  
		, ERROR_PROCEDURE() AS ErrorProcedure,ERROR_LINE() AS ErrorLine 
    	
END CATCH

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_AddIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_AddIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_AddIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_AddIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_AddIdentifier] TO [IRMAReportsRole]
    AS [dbo];

