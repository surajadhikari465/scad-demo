CREATE PROCEDURE gpm.GetCheckPointMessageData
	@Region nvarchar(2),
	@BusinessUnitIds gpm.BusinessUnitIdsType READONLY,
	@ScanCodes gpm.ScanCodesType READONLY
AS
BEGIN	
	IF OBJECT_ID('tempdb..#CheckPointBusinessUnits') IS NOT NULL DROP TABLE #CheckPointBusinessUnits
	IF OBJECT_ID('tempdb..#CheckPointScanCodes') IS NOT NULL DROP TABLE #CheckPointScanCodes
	
	SELECT BusinessUnitId INTO #CheckPointBusinessUnits FROM @BusinessUnitIds
	SELECT ScanCode INTO #CheckPointScanCodes FROM @ScanCodes
	
	SELECT 
		i.ItemID,
		i.ScanCode,
		l.BusinessUnitID AS BusinessUnitId,
		CONVERT(INT,ISNULL(ms.PatchFamilySequenceID,1)) AS SequenceId,
		ISNULL(ms.PatchFamilyID,CONCAT(i.ItemID,'-',l.BusinessUnitID)) AS PatchFamilyId
	FROM Items i 
		JOIN dbo.Locale l ON l.Region = @Region
		JOIN #CheckPointBusinessUnits bu on bu.BusinessUnitId = l.BusinessUnitID
		JOIN #CheckPointScanCodes sc on sc.ScanCode = i.ScanCode
		LEFT JOIN gpm.MessageSequence ms 
				  ON ms.BusinessUnitID = bu.BusinessUnitId
				  AND ms.ItemID = i.ItemID
	 
	IF OBJECT_ID('tempdb..#CheckPointBusinessUnits') IS NOT NULL DROP TABLE #CheckPointBusinessUnits
	IF OBJECT_ID('tempdb..#CheckPointScanCodes') IS NOT NULL DROP TABLE #CheckPointScanCodes
END

GO
GRANT EXEC ON [gpm].[GetCheckPointMessageData] TO TibcoRole, MammothRole
GO