
CREATE PROCEDURE [dbo].[TaxHosting_InsertDefaultTaxFlags]
	@TaxClassDesc varchar(50)
AS
BEGIN
	DECLARE @TaxClassID int = (SELECT TaxClassID FROM TaxClass WHERE TaxClassDesc = @TaxClassDesc);

	IF @TaxClassID IS NULL
		RETURN

	-- Insert Default Tax Flags if they don't already exist
	-- Add a TaxFlag row for the new TaxClass for every row that exists in the TaxDefinition table
	-- TaxDefinition Primary Key is TaxJurisdictionID and TaxFlagKey combined.
	INSERT INTO TaxFlag
	SELECT
		@TaxClassID				as TaxClassID,
		td.TaxJurisdictionID	as TaxJurisdictionID,
		td.TaxFlagKey			as TaxFlagKey,
		0						as TaxFlagValue
	FROM
		TaxDefinition td
	WHERE NOT EXISTS
	(
		SELECT tf.TaxFlagKey, tf.TaxClassID, tf.TaxJurisdictionID
		FROM TaxFlag tf
		WHERE tf.TaxFlagKey = td.TaxFlagKey
			AND tf.TaxJurisdictionID = td.TaxJurisdictionID
			AND tf.TaxClassID = @TaxClassID		
	)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_InsertDefaultTaxFlags] TO [IRMAClientRole]
    AS [dbo];

