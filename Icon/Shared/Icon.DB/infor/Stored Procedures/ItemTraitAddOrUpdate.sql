CREATE PROCEDURE [infor].[ItemTraitAddOrUpdate]
	@itemTraits infor.ItemTraitAddOrUpdateType READONLY
AS
BEGIN
	DELETE itt
	  FROM dbo.ItemTrait itt
	  JOIN @itemTraits itts on itt.ItemID = itts.ItemID
	                       AND itt.traitID = itts.TraitID
						   AND itt.localeID = itts.localeID
     WHERE itts.TraitValue is null 
	    OR rtrim(itts.TraitValue) = ''

	MERGE INTO dbo.ItemTrait AS it
	USING (Select * from @itemTraits
	       WHERE TraitValue is not null 
	         AND rtrim(TraitValue) <> '') AS Source
	ON it.itemID = Source.ItemID
		AND it.traitID = Source.TraitId
		AND it.localeID = Source.LocaleId
	WHEN MATCHED THEN
		UPDATE
		SET traitValue = Source.TraitValue
	WHEN NOT MATCHED THEN
		INSERT (traitID, itemID, traitValue, localeID)
		VALUES (Source.TraitId, Source.ItemId, Source.TraitValue, Source.LocaleId);
END