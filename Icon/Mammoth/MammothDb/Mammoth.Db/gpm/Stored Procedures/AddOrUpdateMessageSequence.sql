CREATE PROCEDURE [gpm].[AddOrUpdateMessageSequence]
	@ItemID int,
	@BusinessUnitID int,
	@PatchFamilyID nvarchar(50),
	@PatchFamilySequenceID int,
	@MessageID nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [gpm].[MessageSequence]
	SET 
		PatchFamilySequenceID = @PatchFamilySequenceID,
		MessageID = @MessageID,
		ModifiedDateUtc = SYSUTCDATETIME()
	WHERE PatchFamilyID = @PatchFamilyID

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO [gpm].[MessageSequence] (ItemID, BusinessUnitID, PatchFamilyID, PatchFamilySequenceID, MessageID)
		VALUES (@ItemID, @BusinessUnitID, @PatchFamilyID, @PatchFamilySequenceID, @MessageID)
	END
	
END
GO

GRANT EXEC ON [gpm].[AddOrUpdateMessageSequence] TO TibcoRole
GO