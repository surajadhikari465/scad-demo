CREATE PROCEDURE dbo.DeleteItemDefaultValue
    @ItemDefaultAttribute_ID int
   ,@ProdHierarchyLevel4_ID int
   ,@Category_ID int
   ,@Value varchar(100)
AS

BEGIN
    SET NOCOUNT ON
    
	DECLARE @ErrorMessage varchar(2048)
	DECLARE @ErrorSeverity smallint
	DECLARE @ErrorNumber int
    DECLARE @RowCount int

	BEGIN TRANSACTION

	BEGIN TRY

		-- Delete the row only if it hasn't changed
		DELETE
		FROM ItemDefaultValue
		WHERE ItemDefaultAttribute_ID = @ItemDefaultAttribute_ID
			AND ((ProdHierarchyLevel4_ID is null AND @ProdHierarchyLevel4_ID is null)  OR ProdHierarchyLevel4_ID = @ProdHierarchyLevel4_ID)
			AND ((Category_ID is null AND @Category_ID is null) OR Category_ID = @Category_ID)
			AND ((Value is null AND @Value is null) OR Value = @Value)
    
		SELECT @RowCount = @@ROWCOUNT

		IF @RowCount = 0
		BEGIN
			
			SELECT @ErrorMessage = 'No rows were deleted because the row values for ItemDefaultAttribute ID ' + CAST(@ItemDefaultAttribute_ID as VARCHAR(50)) + ' are different than the provided values.'
			RAISERROR (@ErrorMessage, 10, 1, 50001)

		END
	
		COMMIT

		-- return this row count value
		Return @RowCount
		
	END TRY
	
	BEGIN CATCH
	
		ROLLBACK

		-- report the error to the application
		SELECT @ErrorMessage = ERROR_PROCEDURE() + ' failed with ' + ERROR_MESSAGE() + ': %d'
		SELECT @ErrorSeverity = ERROR_SEVERITY()
		SELECT @ErrorNumber = ERROR_NUMBER()

        RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber)

	END CATCH

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemDefaultValue] TO [IRMAClientRole]
    AS [dbo];

