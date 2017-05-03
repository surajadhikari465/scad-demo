
CREATE PROCEDURE dbo.IconItemAddTax
	-- Add the parameters for the stored procedure here
	@ValidatedItemList dbo.IconUpdateItemType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @now datetime;
	SET @now = (SELECT GETDATE());

	-- =====================================================
	-- Add Tax Class if it does not exist
	-- =====================================================
	BEGIN TRY
		INSERT INTO TaxClass
		SELECT
			vi.TaxClassName						as TaxClassDesc,
			SUBSTRING(vi.TaxClassName, 1, 7)	as ExternalTaxGroupCode
		FROM
			@ValidatedItemList vi
		WHERE 
			NOT EXISTS (SELECT * 
						FROM TaxClass tc 
						WHERE 
							tc.TaxClassDesc = vi.TaxClassName
							OR SUBSTRING(tc.TaxClassDesc, 1, 7) = SUBSTRING(vi.TaxClassName, 1, 7))

	END TRY
	BEGIN CATCH
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('IconItemAddTax failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconItemAddTax] TO [IConInterface]
    AS [dbo];

