
CREATE PROCEDURE dbo.Replenishment_POSPush_DeleteIdentifier
@IdentifierIds INTTYPE READONLY
AS 

BEGIN
/*********************************************************************************************************************************************
CHANGE LOG
DEV				DATE					TASK				Description
----------------------------------------------------------------------------------------------
MJS				20160919				8814				Remove identifier from validated scan code on deletes.
Jamali			11/01/2016				PBI18900			Added the @IdentifierIds parameter to delete the data from the ValidatedScancode 
															and ItemIdentifier in a single call
***********************************************************************************************************************************************/

BEGIN TRANSACTION

BEGIN TRY	
	DELETE ValidatedScanCode
	FROM ValidatedScanCode vsc
	INNER JOIN ItemIdentifier ii ON vsc.ScanCode = ii.Identifier
	INNER JOIN @Identifierids ids ON ii.Identifier_Id = ids.[Key]

	DELETE ItemIdentifier
	FROM ItemIdentifier ii
	INNER JOIN @Identifierids ids ON ii.Identifier_Id = ids.[Key]
	
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	
	SELECT  
		'Replenishment_POSPush_DeleteIdentifier failed with error: ' + ERROR_MESSAGE() AS ErrorMessage
		, ERROR_NUMBER() AS ErrorNumber, ERROR_SEVERITY() AS ErrorSeverity, ERROR_STATE() AS ErrorState  
		, ERROR_PROCEDURE() AS ErrorProcedure,ERROR_LINE() AS ErrorLine 
END CATCH

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteIdentifier] TO [IRMAReportsRole]
    AS [dbo];

