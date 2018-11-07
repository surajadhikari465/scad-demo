/****** Object:  StoredProcedure [dbo].[Administration_UpdateZone]    Script Date: 04/17/2008 14:28:53 ******/
CREATE PROCEDURE [dbo].[Administration_UpdateZone]
	@ZoneID int,
	@Zone_Name varchar(100),
	@GLMarketingExpenseAcct int,
	@RegionID int,
	@LastUpdate datetime,
	@LastUpdateUserID Int,
	@newLastUpdate timestamp output
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
        SELECT @CodeLocation = 'Update ZoneDesc...'
	    -- update Zone Description
	BEGIN
	    UPDATE dbo.Zone SET Zone_Name = @Zone_Name,
			 GLMarketingExpenseAcct = @GLMarketingExpenseAcct,
			 Region_ID = @RegionID,
			LastUpdateUserID = @LastUpdateUserID
	    WHERE Zone_ID = @ZoneID

END
IF @@ROWCOUNT = 0     
BEGIN       
	RAISERROR('Row has been edited by another user', 16, 1)            
END   
Else     
BEGIN

	select @newLastUpdate = LastUpdate from dbo.Zone (nolock) where Zone_ID = @ZoneID

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
    ON OBJECT::[dbo].[Administration_UpdateZone] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UpdateZone] TO [IRMAClientRole]
    AS [dbo];

