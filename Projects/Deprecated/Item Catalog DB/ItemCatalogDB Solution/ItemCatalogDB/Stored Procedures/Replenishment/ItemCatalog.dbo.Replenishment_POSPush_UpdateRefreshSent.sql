IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Replenishment_POSPush_UpdateRefreshSent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	EXEC ('create PROCEDURE [dbo].[Replenishment_POSPush_UpdateRefreshSent] (@foo int) as select 1')
GO

ALTER PROCEDURE dbo.Replenishment_POSPush_UpdateRefreshSent
(
	@IdentifierIds INTTYPE READONLY--this is a user table type that contains just one int type column
)

AS

BEGIN
-- ****************************************************************************************************************************************************
-- Procedure: Replenishment_POSPush_UpdateRefreshSent
-- Description: Sets the Refresh column value to false for the specified Identifiers
--
-- Modification History:
-- Date       	Init  			TFS/PBI   			Comment
-- 10/26/2016	Jamali			PBI18900		Added the parameter #IdentifierIds and wrote the code to update multiple StoreItem records in a single 
--												call to the procedure
-- ****************************************************************************************************************************************************

IF OBJECT_ID('tempdb..#IdentifierIds') IS NOT NULL
BEGIN
	DROP TABLE #IdentifierIds
END 

CREATE TABLE #IdentifierIds
(
	IdentifierId INT
)

--Put the incoming data into the temp table for better performance
INSERT INTO #IdentifierIds
	SELECT [Key] FROM @IdentifierIds 

BEGIN TRANSACTION

BEGIN TRY	
	UPDATE StoreItem 
	SET Refresh = 0
	FROM StoreItem si
	INNER JOIN ItemIdentifier ii ON ii.Item_Key = si.Item_Key
	INNER JOIN #IdentifierIds iids on iids.IdentifierId = ii.Identifier_ID
	
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	
	SELECT  
		'Replenishment_POSPush_UpdateRefreshSent failed with error: ' + ERROR_MESSAGE() AS ErrorMessage
		, ERROR_NUMBER() AS ErrorNumber, ERROR_SEVERITY() AS ErrorSeverity, ERROR_STATE() AS ErrorState  
		, ERROR_PROCEDURE() AS ErrorProcedure,ERROR_LINE() AS ErrorLine 
    	
END CATCH

END
GO

 


 