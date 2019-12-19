CREATE PROCEDURE [dbo].[GetAvailableScanCodesForBarcodeTypeId] 
@BarCodeTypeId INT
AS
BEGIN
		
BEGIN TRY
	BEGIN TRAN
		DECLARE @nextScanCode NVARCHAR(13)

		UPDATE dbo.BarcodeTypeRangePool  
		SET 
		Assigned=1, 
		AssignedDateTimeUtc=SYSUTCDATETIME() 
		OUTPUT inserted.ScanCode
		WHERE
		ScanCode = (SELECT TOP 1 scancode FROM dbo.BarcodeTypeRangePool 
						WHERE 
						BarCodeTypeId = @BarCodeTypeId 
						AND Assigned = 0 
						ORDER BY scanCode ASC)
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		THROW;
	END CATCH
END
