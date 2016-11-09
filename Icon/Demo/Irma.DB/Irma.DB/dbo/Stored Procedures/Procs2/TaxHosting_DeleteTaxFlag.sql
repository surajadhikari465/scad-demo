CREATE PROCEDURE dbo.TaxHosting_DeleteTaxFlag
	@TaxClassID int,	
	@TaxJurisdictionID int,
	@TaxFlagKey varchar(1)	
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0
    
    DECLARE @TaxFlagCount int
    SELECT @TaxFlagCount = 0 

	-- delete associated TaxOverride data
	DELETE TaxOverride
	FROM TaxOverride
	INNER JOIN
		Store
		ON Store.Store_No = TaxOverride.Store_No
	INNER JOIN
		Item
		ON Item.Item_Key = TaxOverride.Item_Key
	INNER JOIN
		TaxClass
		ON TaxClass.TaxClassID = Item.TaxClassID
	INNER JOIN
		TaxJurisdiction
		ON TaxJurisdiction.TaxJurisdictionID = Store.TaxJurisdictionID
	INNER JOIN
		TaxFlag
		ON TaxFlag.TaxJurisdictionID = TaxJurisdiction.TaxJurisdictionID
		AND TaxFlag.TaxClassID = TaxClass.TaxClassID
		AND TaxFlag.TaxFlagKey = TaxOverride.TaxFlagKey
	WHERE TaxOverride.TaxFlagKey = @TaxFlagKey
		AND TaxFlag.TaxJurisdictionID = @TaxJurisdictionID
		AND TaxFlag.TaxClassID = @TaxClassID
		
	SELECT @error_no = @@ERROR
	
    -- delete TaxFlag data
    IF (@error_no = 0)
    BEGIN
		DELETE FROM TaxFlag
		WHERE TaxClassID = @TaxClassID
		AND TaxJurisdictionID = @TaxJurisdictionID
		AND TaxFlagKey = @TaxFlagKey

		SELECT @error_no = @@ERROR
	END

    IF (@error_no = 0)
    BEGIN
        -- determine if any TaxClass values are associated to this TaxDefinition
	DECLARE csTaxFlag CURSOR
	READ_ONLY
	FOR     
	    SELECT *
	    FROM TaxFlag
	    WHERE TaxJurisdictionID = @TaxJurisdictionID
		AND TaxFlagKey = @TaxFlagKey
	OPEN csTaxFlag
    
        FETCH NEXT FROM csTaxFlag
	BEGIN
    		IF (@@fetch_status = 0)
		BEGIN
    			SELECT @TaxFlagCount = 1 
		END
	END
        CLOSE csTaxFlag
        DEALLOCATE csTaxFlag

        SELECT @error_no = @@ERROR
    END   

    --SELECT ''@TaxFlagCount: '' + CAST(@TaxFlagCount  AS VARCHAR)

    -- delete TaxDefinition data
    IF (@error_no = 0) AND (@TaxFlagCount <= 0)
    BEGIN    
	DELETE FROM TaxDefinition
    	WHERE TaxJurisdictionID = @TaxJurisdictionID 
		AND TaxFlagKey = @TaxFlagKey

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
        RAISERROR ('DeleteTaxFlag failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_DeleteTaxFlag] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_DeleteTaxFlag] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_DeleteTaxFlag] TO [IRMAReportsRole]
    AS [dbo];

