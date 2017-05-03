IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SaveItemDefaultValue')
	BEGIN
		DROP  Procedure  dbo.SaveItemDefaultValue
	END

GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


CREATE PROCEDURE dbo.SaveItemDefaultValue
    @ItemDefaultAttribute_ID int
   ,@ProdHierarchyLevel4_ID int
   ,@Category_ID int
   ,@Value varchar(100)
AS

BEGIN
    SET NOCOUNT ON
    
    BEGIN TRANSACTION

	BEGIN TRY
	
		-- Check to see if there already is a value set for this position in the hierarchy.
		-- Request a table lock, and hold it for the duration of the transaction,
		-- on the ItemDefaultValue table to keep any other sessions from being able
		-- to update or insert into the ItemDefaultValue table during this transaction.
	   IF (
				(
				SELECT COUNT(*)
				FROM ItemDefaultValue WITH (TABLOCK,HOLDLOCK)
				WHERE ItemDefaultAttribute_ID = @ItemDefaultAttribute_ID
					AND (@ProdHierarchyLevel4_ID is null OR ProdHierarchyLevel4_ID = @ProdHierarchyLevel4_ID)
					AND (@Category_ID is null OR Category_ID = @Category_ID)
				) > 0
			)

		BEGIN
			-- existing value found, so do an update
			UPDATE ItemDefaultValue
			SET
				ItemDefaultAttribute_ID = @ItemDefaultAttribute_ID
				,ProdHierarchyLevel4_ID = @ProdHierarchyLevel4_ID
				,Category_ID = @Category_ID
				,Value = @Value
			WHERE ItemDefaultAttribute_ID = @ItemDefaultAttribute_ID
				AND (@ProdHierarchyLevel4_ID is null OR ProdHierarchyLevel4_ID = @ProdHierarchyLevel4_ID)
				AND (@Category_ID is null OR Category_ID = @Category_ID)
		END

		ELSE
	    BEGIN
			-- no existing value found, so do an insert
			-- this will appropriately throw an informative error if the
			-- corresponding ItemDefaultAttribute row doesn't exist
			INSERT INTO ItemDefaultValue
				(
				ItemDefaultAttribute_ID
				,ProdHierarchyLevel4_ID
				,Category_ID
				,Value
				)
			VALUES
				(
				@ItemDefaultAttribute_ID
				,@ProdHierarchyLevel4_ID
				,@Category_ID
				,@Value
				)
	    
		END
    
		-- this releases the requested table lock
		COMMIT
	
	END TRY
	
	BEGIN CATCH
	
		-- this also releases the requested table lock
		ROLLBACK

		-- report the error to the application
		DECLARE @ErrorMessage varchar(2048)
        DECLARE @ErrorSeverity smallint
        DECLARE @ErrorNumber int
        
		SELECT @ErrorMessage = ERROR_PROCEDURE() + ' failed with ' + ERROR_MESSAGE() + ': %d'
		SELECT @ErrorSeverity = ERROR_SEVERITY()
		SELECT @ErrorNumber = ERROR_NUMBER()

        RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber)

	END CATCH

END

GO

