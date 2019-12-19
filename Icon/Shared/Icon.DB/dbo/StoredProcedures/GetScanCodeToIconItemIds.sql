CREATE PROCEDURE infor.GetScanCodeToIconItemIds
	@scanCodes app.ScanCodeListType READONLY
AS
BEGIN
	CREATE TABLE #tempScanCodes 
	(
		ScanCode nvarchar(13)
	)

	INSERT INTO #tempScanCodes (ScanCode)
	SELECT ScanCode 
	FROM @scanCodes

	SELECT 
		sc.scanCode AS ScanCode,
		sc.itemID AS ItemId
	FROM ScanCode sc
	JOIN #tempScanCodes temp on sc.scanCode = temp.ScanCode
END