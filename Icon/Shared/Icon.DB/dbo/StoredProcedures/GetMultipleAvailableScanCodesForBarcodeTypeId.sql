CREATE PROCEDURE [dbo].[GetMultipleAvailableScanCodesForBarcodeTypeId] 
	@BarcodeTypeId INT,
	@Count INT,
	@ExcludedScanCodes app.ScanCodeListType READONLY
AS
BEGIN
	SELECT ScanCode
	INTO #excludedScanCodes
	FROM @ExcludedScanCodes

	UPDATE dbo.BarcodeTypeRangePool
	SET Assigned = 1
		,AssignedDateTimeUtc = SYSUTCDATETIME()
	OUTPUT inserted.ScanCode
	WHERE ScanCode IN (
			SELECT TOP (@Count) ScanCode
			FROM dbo.BarcodeTypeRangePool btrp
			WHERE btrp.BarCodeTypeId = @BarCodeTypeId
				AND btrp.Assigned = 0
				AND btrp.ScanCode NOT IN (
					SELECT ScanCode
					FROM #excludedScanCodes
					)
			ORDER BY ScanCode ASC
			)
END