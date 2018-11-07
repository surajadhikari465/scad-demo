CREATE  PROCEDURE [dbo].[TaxHosting_UpdateTaxJurisdiction]
	@TaxJurisdictionID int,
	@TaxJurisdictionDesc varchar(30),
	@LastUpdate datetime,
	@LastUpdateUserID Int,
	@newLastUpdate datetime output
AS 

BEGIN

-- 20080417 - DaveStacey - Add Update for Tax Jurisdiction Description... 
----------------------------------------------
-- Use TRY...CATCH for error handling
----------------------------------------------
BEGIN TRY
        ----------------------------------------------
        -- Wrap the updates in a transaction
        ----------------------------------------------
        BEGIN TRANSACTION
		DECLARE @CodeLocation varchar(50)
        SELECT @CodeLocation = 'Update TaxJurisdictionDesc...'
	    -- update TaxJurisdiction Description
	BEGIN
	    UPDATE dbo.TaxJurisdiction SET TaxJurisdictionDesc = @TaxJurisdictionDesc,
			LastUpdateUserID = @LastUpdateUserID
	    WHERE TaxJurisdictionID = @TaxJurisdictionID
				and LastUpdate = @LastUpdate
END
IF @@ROWCOUNT = 0     
BEGIN       
	RAISERROR('Row has been edited by another user', 16, 1)            
END   
Else     
BEGIN

	select @newLastUpdate = LastUpdate from dbo.TaxJurisdiction (nolock) where TaxJurisdictionID = @TaxJurisdictionID

	Select @newLastUpdate
    END
         ----------------------------------------------
         -- Commit the transaction
         ----------------------------------------------
        IF @@TRANCOUNT > 0
                COMMIT TRANSACTION

END TRY
--===============================================================================================
BEGIN CATCH
        ----------------------------------------------
        -- Rollback the transaction
        ----------------------------------------------
        IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION

        ----------------------------------------------
        -- Display a detailed error message
        ----------------------------------------------
        PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10)
                + CHAR(9) + ' at statement  ''' + @CodeLocation + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') + 'line ' + CONVERT(varchar, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Database changes were rolled back.' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-')

    SELECT
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() AS ErrorState,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage
END CATCH
--===============================================================================================
--
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_UpdateTaxJurisdiction] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_UpdateTaxJurisdiction] TO [IRMAClientRole]
    AS [dbo];

