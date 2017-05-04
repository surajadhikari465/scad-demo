CREATE PROCEDURE [dbo].[InsertCloneTaxJurisdiction] 
	@OldTaxJurisdictionID	int,  			-- The Tax Jurisdiction to Be Copied from (If needed)
	@NewTaxJurisdictionID Int output,
	@TaxJurisdictionDesc	varchar(50),	-- The New Tax Jurisdiction to be added
	@LastUpdateUserID Int,
	@newLastUpdate datetime output

AS
BEGIN
/*
	<<< Change History >>>
	
	20080109 - DaveStacey - Stored Procedure to either set up a new Tax Jurisdiction with or without values
	Copied and modified from Russell's Store Clone Script after discussion w/Tom Lux and Joe A.  
	Rollback logic blatantly stolen from Tim Pope's Store Clone Script


	2/22/10, TFS 11988 (Fields to support external tax hosting data), v3.5.9, Tom Lux
	Added explicit column list for work against TaxDefinition table, since it has a new column.
	
*/

----------------------------------------------
-- Use TRY...CATCH for error handling
----------------------------------------------
BEGIN TRY
        ----------------------------------------------
        -- Wrap the updates in a transaction
        ----------------------------------------------
        BEGIN TRANSACTION

DECLARE
        @CodeLocation varchar(50)--, @NewTaxJurisdictionID Int

        ----------------------------------------------------------------------
        -- add a default new Jurisdiction
        ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [dbo].[TaxJurisdiction]...'

				INSERT INTO dbo.TaxJurisdiction (TaxJurisdictionDesc, LastUpdateUserID)
				Select @TaxJurisdictionDesc, @LastUpdateUserID

			Select @NewTaxJurisdictionID = SCOPE_IDENTITY() 

IF @OldTaxJurisdictionID > 1
	BEGIN
        ----------------------------------------------------------------------
        -- add Clone with values
        ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [dbo].TaxDefinition/TaxFlag with values...'


		INSERT INTO dbo.TaxDefinition (
			[TaxJurisdictionID]
			,[TaxFlagKey]
			,[TaxPercent]
			,[POSID])
		SELECT DISTINCT 
			@NewTaxJurisdictionID
			,D.[TaxFlagKey]
			,[TaxPercent]
			,[POSID]
		FROM dbo.TaxJurisdiction J (NOLOCK)
			JOIN dbo.TaxDefinition D (NOLOCK) ON J.TaxJurisdictionId = D.TaxJurisdictionID
			WHERE D.TaxJurisdictionID = @OldTaxJurisdictionID

		INSERT INTO dbo.TaxFlag
		SELECT DISTINCT C.TaxClassId, @NewTaxJurisdictionID, D.TaxFlagKey, F.TaxFlagValue
		FROM dbo.TaxJurisdiction J (NOLOCK)
			JOIN dbo.TaxDefinition D (NOLOCK) ON J.TaxJurisdictionId = D.TaxJurisdictionId
			JOIN dbo.TaxFlag F (NOLOCK) ON F.TaxJurisdictionId = D.TaxJurisdictionId AND F.TaxFlagKey = D.TaxFlagKey 
			JOIN dbo.TaxClass C (NOLOCK) ON C.TaxClassID = F.TaxClassID
		WHERE J.TaxJurisdictionId = @OldTaxJurisdictionID
	END
ELSE
	BEGIN

        ----------------------------------------------------------------------
        -- add Clone without values
        ----------------------------------------------------------------------
        SELECT @CodeLocation = 'INSERT INTO [dbo].TaxDefinition/TaxFlag without values...'

		INSERT INTO dbo.TaxDefinition (
			[TaxJurisdictionID]
			,[TaxFlagKey]
			,[TaxPercent]
			,[POSID])
		SELECT DISTINCT
			J.TaxJurisdictionId
			,D.TaxFlagKey
			,0.0000
			,NULL
		FROM dbo.TaxJurisdiction J (NOLOCK)
			CROSS JOIN dbo.TaxDefinition D (NOLOCK)
		WHERE J.TaxJurisdictionId = @NewTaxJurisdictionID

		INSERT INTO dbo.TaxFlag
		SELECT DISTINCT C.TaxClassId, J.TaxJurisdictionId, D.TaxFlagKey, 0
		FROM dbo.TaxJurisdiction J (NOLOCK)
			CROSS JOIN dbo.taxdefinition D (NOLOCK)
			CROSS JOIN dbo.TaxClass C (NOLOCK)
		where J.TaxJurisdictionId = @NewTaxJurisdictionID
	END


	select @newLastUpdate = LastUpdate  from dbo.TaxJurisdiction (nolock) where TaxJurisdictionID = @NewTaxJurisdictionID

	Select @NewTaxJurisdictionID, @newLastUpdate
--         ----------------------------------------------
--         -- Commit the transaction
--         ----------------------------------------------
        IF @@TRANCOUNT > 0
                COMMIT TRANSACTION

        PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Successfully added new tax jurisdiction ''' + @TaxJurisdictionDesc + '''' + CHAR(13) + CHAR(10)
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
    ON OBJECT::[dbo].[InsertCloneTaxJurisdiction] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCloneTaxJurisdiction] TO [IRMAClientRole]
    AS [dbo];

