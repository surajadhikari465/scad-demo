CREATE PROCEDURE dbo.PopulateDefaultTaxFlags 
     @TaxClassDesc VARCHAR(50)
	,@ExternalTaxGroupCode VARCHAR(7)
AS
BEGIN
	DECLARE @TaxClassID INT

	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO TaxClass (
			TaxClassDesc
			,ExternalTaxGroupCode
			)
		VALUES (
			@TaxClassDesc
			,@ExternalTaxGroupCode
			)

		SET @TaxClassID = SCOPE_IDENTITY();

		MERGE TaxFlag AS tf
		USING (
			SELECT TaxFlagKey
				,TaxJurisdictionID
				,TaxClassID
			FROM (
				SELECT DISTINCT taxflagkey
				FROM TaxDefinition
				WHERE taxpercent IS NOT NULL
				) td
			CROSS JOIN (
				SELECT TaxJurisdictionid
				FROM TaxJurisdiction
				) tj
			CROSS JOIN (
				SELECT @TaxClassID AS taxclassid
				) tc
			) dtf
			ON tf.TaxClassID = dtf.TaxClassID
				AND tf.TaxFlagKey = dtf.TaxFlagKey
				AND tf.TaxJurisdictionID = dtf.TaxJurisdictionID
		WHEN NOT MATCHED
			THEN
				INSERT (
					TaxClassID
					,TaxJurisdictionID
					,TaxFlagKey
					,TaxFlagValue
					)
				VALUES (
					dtf.TaxClassID
					,dtf.TaxJurisdictionID
					,dtf.TaxFlagKey
					,0
					);

		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		IF XACT_STATE() <> 0
			ROLLBACK TRANSACTION
	END CATCH
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PopulateDefaultTaxFlags] TO [IConInterface]
    AS [dbo];