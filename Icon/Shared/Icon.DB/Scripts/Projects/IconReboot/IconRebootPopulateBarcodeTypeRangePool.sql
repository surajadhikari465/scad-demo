USE Icon
GO

SET NOCOUNT ON;

DECLARE @barcodeTypeId int
DECLARE @beginRange bigint
DECLARE @endRange bigint
DECLARE @scalePLU bit

PRINT 'Begin Updating BarcodeTypeRangePool with all available ranges...';

DECLARE cursorBarCodeType CURSOR
FOR SELECT 
	barCodeTypeId,
	beginRange,
	endRange,
	scalePLU
FROM 
	dbo.BarcodeType
ORDER BY barCodeTypeId ASC

OPEN cursorBarCodeType

FETCH NEXT FROM cursorBarCodeType INTO 
        @barcodeTypeId,
		@beginRange,
		@endRange,
		@scalePLU

WHILE @@FETCH_STATUS = 0
BEGIN
		DECLARE @counter bigint
		SET @counter = @beginRange
		WHILE @counter <= @endRange
			BEGIN
			INSERT INTO [dbo].[BarcodeTypeRangePool] (barCodeTypeId, ScanCode) VALUES(@barcodeTypeId,@counter)

			IF @scalePLU = 1
			BEGIN
				set @counter = @counter + 100000;
			END
			ELSE
			BEGIN
				set @counter = @counter + 1;
			END

		END

	FETCH NEXT FROM cursorBarCodeType INTO 
        @barcodeTypeId,
		@beginRange,
		@endRange,
		@scalePLU
END

CLOSE cursorBarCodeType
DEALLOCATE cursorBarCodeType

PRINT 'Updating all assigned Scan Codes in the BarcodeTypeRangePool...';
UPDATE [dbo].[BarcodeTypeRangePool] SET Assigned=1,AssignedDateTimeUtc=SYSUTCDATETIME()
WHERE scanCode in (SELECT scanCode from dbo.ScanCode s WHERE s.scanCode = [dbo].[BarcodeTypeRangePool].scanCode)

PRINT 'Finished BarcodeTypeRangePool script...';