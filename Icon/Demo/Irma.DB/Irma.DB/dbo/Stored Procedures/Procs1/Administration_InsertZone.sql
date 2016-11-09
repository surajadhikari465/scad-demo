/****** Object:  StoredProcedure [dbo].[Administration_InsertZone]    Script Date: 04/17/2008 14:28:53 ******/
CREATE PROCEDURE [dbo].[Administration_InsertZone] 	
	@NewZoneID Int output,
	@Zone_Name varchar(50),	
	@GLMarketingExpenseAcct int,
	@RegionID int,	
	@LastUpdateUserID Int,
	@newLastUpdate timestamp output

AS
BEGIN

----------------------------------------------
-- Use TRY...CATCH for error handling
----------------------------------------------
BEGIN TRY
        ----------------------------------------------
        -- Wrap the updates in a transaction
        ----------------------------------------------
        BEGIN TRANSACTION

DECLARE
        @CodeLocation varchar(50)--, @NewZoneID Int
-- 20080109 - DaveStacey - Stored Procedure to either set up a new Tax Jurisdiction with or without values
-- Copied and modified from Russell's Store Clone Script after discussion w/Tom Lux and Joe A.  
-- Rollback logic blatantly stolen from Tim Pope's Store Clone Script

        ----------------------------------------------------------------------
        -- add a default new Jurisdiction
        ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [dbo].[Zone]...'

				INSERT INTO dbo.Zone (Zone_Name, GLMarketingExpenseAcct, Region_ID, LastUpdateUserID)
				Select @Zone_Name, @GLMarketingExpenseAcct, @RegionID, @LastUpdateUserID

			Select @NewZoneID = SCOPE_IDENTITY() 


	select @newLastUpdate = LastUpdate  from dbo.Zone (nolock) where Zone_ID = @NewZoneID

	Select @NewZoneID, @newLastUpdate
--         ----------------------------------------------
--         -- Commit the transaction
--         ----------------------------------------------
        IF @@TRANCOUNT > 0
                COMMIT TRANSACTION

        PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Successfully added new zone ''' + @Zone_Name + '''' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-')

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
    ON OBJECT::[dbo].[Administration_InsertZone] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_InsertZone] TO [IRMAClientRole]
    AS [dbo];

