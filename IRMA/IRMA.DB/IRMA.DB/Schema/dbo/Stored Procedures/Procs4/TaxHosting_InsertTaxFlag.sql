CREATE PROCEDURE dbo.TaxHosting_InsertTaxFlag
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

    DECLARE @TaxDefCount int
    SELECT @TaxDefCount = 0 

    -- validate that TaxDefinition exists before insert into TaxFlag table
    DECLARE csTaxDef CURSOR
    READ_ONLY
    FOR     
        SELECT TaxJurisdictionID, TaxFlagKey
        FROM TaxDefinition
	WHERE TaxJurisdictionID = @TaxJurisdictionID
		AND TaxFlagKey = @TaxFlagKey
    OPEN csTaxDef

    FETCH NEXT FROM csTaxDef
    BEGIN
    	IF (@@fetch_status = 0)
	BEGIN
    		SELECT @TaxDefCount = 1 
	END
    END
    CLOSE csTaxDef
    DEALLOCATE csTaxDef

    SELECT @error_no = @@ERROR

    -- insert new TaxDefinition before inserting new TaxFlag
    IF (@error_no = 0) AND (@taxDefCount <= 0)
    BEGIN
	INSERT INTO TaxDefinition (
		TaxJurisdictionID
		,TaxFlagKey
		,TaxPercent
		,POSID
	)
	VALUES (
		@TaxJurisdictionID
		,@TaxFlagKey
		,@TaxPercent
		,@POSID
	)

	SELECT @error_no = @@ERROR
    END

	-- This block is being commented out per TFS 1024
    -- update ExternalTaxGroupCode (CCH Group Item Code) in TaxClass table
    --IF (@error_no = 0) 
    --BEGIN
	--UPDATE TaxClass 
	--Set ExternalTaxGroupCode = @ExternalTaxGroupCode
	--WHERE TaxClassID = @TaxClassID

	--SELECT @error_no = @@ERROR
    --END

    --CHECK @ResetActiveTaxFlags VALUE; IF TRUE THEN TURN ALL EXISTING TRUE FLAG VALUES TO FALSE
    IF (@error_no = 0) AND (@ResetActiveTaxFlags = 1)
    BEGIN
	UPDATE TaxFlag SET TaxFlagValue = 0
	WHERE TaxJurisdictionID = @TaxJurisdictionID
		AND TaxClassID = @TaxClassID

	SELECT @error_no = @@ERROR
    END

    -- insert new TaxFlag value
    IF (@error_no = 0)
    BEGIN
	INSERT INTO TaxFlag (TaxClassID, TaxJurisdictionID, TaxFlagKey, TaxFlagValue)
	VALUES (@TaxClassID, @TaxJurisdictionID, @TaxFlagKey, @TaxFlagValue)

	SELECT @error_no = @@ERROR
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
        RAISERROR ('InsertTaxFlag failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_InsertTaxFlag] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_InsertTaxFlag] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_InsertTaxFlag] TO [IRMAReportsRole]
    AS [dbo];

