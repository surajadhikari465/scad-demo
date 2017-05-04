CREATE PROCEDURE dbo.TaxHosting_UpdateTaxFlag
	@TaxJurisdictionID int,
	@TaxFlagKey varchar(1),
	@TaxClassID int,
	@TaxFlagValue bit,
	@TaxPercent decimal(5,2),
	@POSID int,
	@ResetActiveTaxFlags bit
AS 


/*********************************************************************************************************************************************
CHANGE LOG
DEV					DATE					TASK						Description
----------------------------------------------------------------------------------------------------------------------------------------------
MYounes         Jan 21, 2011                1024                   Commented out Update to Tax Class Table of ExternalTaxGroupCode field
**********************************************************************************************************************************************/

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

    --CHECK @ResetActiveTaxFlags VALUE; IF TRUE THEN TURN ALL EXISTING TRUE FLAG VALUES TO FALSE
    IF @ResetActiveTaxFlags = 1
    BEGIN
	UPDATE
		TaxFlag
	SET
		TaxFlagValue = 0
	WHERE
		TaxJurisdictionID = @TaxJurisdictionID
		AND TaxClassID = @TaxClassID

	SELECT @error_no = @@ERROR
    END

    IF (@error_no = 0)
    BEGIN
	-- update TaxDefinition data
    	UPDATE
    		TaxDefinition
    	SET
    		TaxPercent = @TaxPercent
    		,POSID = @POSID

			-- commented out per TFS 1024
--    		--,ExternalTaxGroupCode = @ExternalTaxGroupCode

    	WHERE
    		TaxJurisdictionID = @TaxJurisdictionID 
			AND TaxFlagKey = @TaxFlagKey

    	SELECT @error_no = @@ERROR
    END

  IF (@error_no = 0)

	--This block is being commented out per tFS 1024
    --BEGIN
	-- update TaxClass data
    	--UPDATE
    	--	TaxClass
    	--SET
    	--	ExternalTaxGroupCode = @ExternalTaxGroupCode
    	--WHERE
    	--	TaxClassID = @TaxClassID 

    	--SELECT @error_no = @@ERROR
    --END

    IF (@error_no = 0)
    BEGIN
	    -- update TaxFlag data
	    UPDATE
			TaxFlag
		SET
			TaxFlagValue = @TaxFlagValue
	    WHERE
			TaxClassID = @TaxClassID
			AND TaxJurisdictionID = @TaxJurisdictionID
			AND TaxFlagKey = @TaxFlagKey
    END

    SET NOCOUNT OFF

    IF @error_no = 0
	COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('UpdateTaxFlag failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_UpdateTaxFlag] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_UpdateTaxFlag] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_UpdateTaxFlag] TO [IRMAReportsRole]
    AS [dbo];

