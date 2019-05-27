CREATE PROCEDURE infor.ItemAddOrUpdate 
	@sourceTable infor.ItemAddOrUpdateType READONLY
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @localeID INT = (SELECT localeID FROM dbo.Locale WHERE localeName = 'Whole Foods')
	DECLARE @validationDateTraitId INT = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'VAL')

	SET IDENTITY_INSERT dbo.Item ON

	MERGE dbo.Item AS Target
	USING @sourceTable AS Source 
	ON Target.itemID = Source.ItemId
	WHEN MATCHED THEN
		UPDATE SET itemTypeID = Source.ItemTypeId
	WHEN NOT MATCHED BY Target THEN
		INSERT (itemID, itemTypeID) 
		VALUES (ItemId, ItemTypeId);

	SET IDENTITY_INSERT dbo.Item OFF
		
	MERGE dbo.ScanCode AS Target
	USING @sourceTable AS Source
	ON Target.itemID = Source.ItemId
	WHEN MATCHED THEN
		UPDATE SET scanCodeTypeID = Source.ScanCodeTypeId
	WHEN NOT MATCHED BY Target THEN
		INSERT (itemID, scanCode, scanCodeTypeID, localeID) 
		VALUES (ItemId, ScanCode, ScanCodeTypeId, @localeID);
	
	MERGE dbo.ItemTrait as Target
	USING @sourceTable AS Source
	ON Target.itemID = Source.ItemId AND Target.traitID = @validationDateTraitId
	WHEN NOT MATCHED THEN 
		INSERT (traitID, itemID, uomID, traitValue, localeID) 
		VALUES (@validationDateTraitId, Source.ItemId, null, convert(nvarchar(255), sysdatetime(), 121), @localeID);

	UPDATE seq
	SET SequenceID = i.SequenceID,
		InforMessageId = i.InforMessageId,
		ModifiedDateUtc = SYSUTCDATETIME()
	FROM infor.ItemSequence seq
	JOIN @sourceTable i ON seq.ItemID = i.ItemId
	WHERE i.SequenceId IS NOT NULL

	INSERT INTO infor.ItemSequence(ItemID, InforMessageId, SequenceID)
	SELECT 
		i.ItemId,
		i.InforMessageId,
		i.SequenceId
	FROM @sourceTable i
	WHERE i.SequenceId IS NOT NULL
		AND i.ItemId NOT IN
		(
			SELECT ItemId FROM infor.ItemSequence
		)
END
GO