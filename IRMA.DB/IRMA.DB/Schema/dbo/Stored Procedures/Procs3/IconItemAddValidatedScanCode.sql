
CREATE PROCEDURE dbo.IconItemAddValidatedScanCode
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
	-- Add Brand and Validated Brand if they don't exist yet
	-- =====================================================
	BEGIN TRY
		INSERT INTO ValidatedScanCode
		SELECT
			sc.ScanCode as ScanCode,
			@now		as InsertDate
		FROM
			(SELECT vi.ScanCode
			FROM @ValidatedItemList vi
			EXCEPT
			SELECT vsc.ScanCode
			FROM ValidatedScanCode vsc) sc
	END TRY
	BEGIN CATCH
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('IconItemAddValidatedScanCode failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconItemAddValidatedScanCode] TO [IConInterface]
    AS [dbo];

