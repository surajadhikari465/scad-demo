/****** Object:  StoredProcedure [dbo].[TaxHosting_DeleteTaxJurisdiction]    Script Date: 05/19/2006 16:33:50 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[TaxHosting_DeleteTaxJurisdiction]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[TaxHosting_DeleteTaxJurisdiction]
GO

/****** Object:  StoredProcedure [dbo].[TaxHosting_DeleteTaxJurisdiction]    Script Date: 05/19/2006 16:33:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[TaxHosting_DeleteTaxJurisdiction]
	@TaxJurisdictionID int
AS 

BEGIN
SET NOCOUNT ON
-- 20080417 - DaveStacey - Delete Tax Jurisdiction... 
----------------------------------------------
-- Use TRY...CATCH for error handling
----------------------------------------------
BEGIN TRY
        ----------------------------------------------
        -- Wrap the updates in a transaction
        ----------------------------------------------
        BEGIN TRANSACTION
		DECLARE @CodeLocation varchar(50)
        SELECT @CodeLocation = 'Delete TaxJurisdiction...'
	    -- update TaxJurisdiction Description
	BEGIN
	    Delete dbo.TaxJurisdiction 
	    WHERE TaxJurisdictionID = @TaxJurisdictionID
		and (SELECT COUNT(1) 
	 	 FROM dbo.Store 
	 	 WHERE Store.TaxJurisdictionID = @TaxJurisdictionID) = 0
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
SET NOCOUNT OFF


GO

