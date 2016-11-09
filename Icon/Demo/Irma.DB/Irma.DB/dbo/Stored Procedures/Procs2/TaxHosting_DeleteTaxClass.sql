CREATE PROCEDURE dbo.TaxHosting_DeleteTaxClass
	@TaxClassID int
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

	-- delete associated Item data
	UPDATE 
		Item
	SET 
		TaxClassID = NULL
	WHERE
		TaxClassID = @TaxClassID
		
	SELECT @error_no = @@ERROR
	
	-- delete associated ItemChangeHistory data
	UPDATE 
		ItemChangeHistory
	SET 
		TaxClassID = NULL
	WHERE
		TaxClassID = @TaxClassID
		
	SELECT @error_no = @@ERROR
	
	-- delete Tax Class
	DELETE FROM
		TaxClass
	WHERE
		TaxClassID = @TaxClassID		
	
	SELECT @error_no = @@ERROR	

    SET NOCOUNT OFF

    IF @error_no = 0
	COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('DeleteTaxClass failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_DeleteTaxClass] TO [IRMAClientRole]
    AS [dbo];

