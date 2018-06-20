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
	-- Add Validated Scan Code if they don't exist yet
	-- =====================================================
	SELECT * INTO #ValidatedItems FROM @ValidatedItemList;
	CREATE NONCLUSTERED INDEX IX_ScanCode_#ValidatedItems ON #ValidatedItems (ScanCode);

	BEGIN TRY

		INSERT INTO ValidatedScanCode (ScanCode, InsertDate, InforItemId, ItemTypeCode)
		SELECT
			vi.ScanCode as ScanCode,
			@now		as InsertDate,
			vi.ItemID	as InforItemId,
			vi.ItemTypeCode as ItemTypeCode
		FROM
			#ValidatedItems vi
		WHERE NOT EXISTS (SELECT 1 FROM ValidatedScanCode vsc WHERE vsc.ScanCode = vi.ScanCode)

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