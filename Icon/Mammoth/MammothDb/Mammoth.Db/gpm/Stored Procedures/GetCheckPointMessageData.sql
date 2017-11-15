CREATE PROCEDURE gpm.GetCheckPointMessageData
	@Region nvarchar(2),
	@BusinessUnitId int,
	@ScanCode varchar(13)
AS
BEGIN
	SELECT 
		i.ItemID,
		@BusinessUnitId AS BusinessUnitId,
		CONVERT(INT,ISNULL(ms.PatchFamilySequenceID,1)) AS SequenceId,
		ISNULL(ms.PatchFamilyID,CONCAT(i.ItemID,'-',@BusinessUnitId)) AS PatchFamilyId
	FROM Items i 
	LEFT JOIN gpm.MessageSequence ms 
			  ON ms.BusinessUnitID =@BusinessUnitId
			  AND ms.ItemID = i.ItemID
     WHERE i.ScanCode = @ScanCode
END