-- PBI 26623 Flexible Text attribute failing to allow full 300 characters:
-- increase the TraitValue field width of the infor.ItemTraitAddOrUpdateType

DECLARE @scriptKey VARCHAR(128) = 'FixTraitLength'

IF NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	--must drop procedure that uses the type before changing
	IF EXISTS (
		SELECT 1
		FROM   INFORMATION_SCHEMA.ROUTINES
		WHERE  ROUTINE_NAME = 'ItemTraitAddOrUpdate' AND SPECIFIC_SCHEMA = 'infor')
	BEGIN
		DROP PROCEDURE infor.ItemTraitAddOrUpdate;
	END
	
	-- drop the type 
	IF (type_id('infor.ItemTraitAddOrUpdateType') IS NOT NULL)
	BEGIN
		DROP TYPE infor.ItemTraitAddOrUpdateType;
	END

	-- re-add the type with the changes 
	IF (type_id('infor.ItemTraitAddOrUpdateType') IS NULL)
	BEGIN
		CREATE TYPE infor.ItemTraitAddOrUpdateType AS TABLE(
			ItemId int NOT NULL,
			TraitId int NOT NULL,
			TraitValue nvarchar(300) NULL,
			LocaleId int NOT NULL
		);
	END
	
	-- re-add the procedure which uses the type
	IF NOT EXISTS (
		SELECT 1
		FROM   INFORMATION_SCHEMA.ROUTINES
		WHERE  ROUTINE_NAME = 'ItemTraitAddOrUpdate' AND SPECIFIC_SCHEMA = 'infor')
	BEGIN
		EXEC ('CREATE PROCEDURE infor.ItemTraitAddOrUpdate
					@itemTraits infor.ItemTraitAddOrUpdateType READONLY
				AS
			BEGIN
				DELETE itt
					FROM dbo.ItemTrait itt
					JOIN @itemTraits itts on itt.ItemID = itts.ItemID
										AND itt.traitID = itts.TraitID
										AND itt.localeID = itts.localeID
					WHERE itts.TraitValue is null 
					OR rtrim(itts.TraitValue) = ''''

				MERGE INTO dbo.ItemTrait AS it
				USING (Select * from @itemTraits
						WHERE TraitValue is not null 
							AND rtrim(TraitValue) <> '''') AS Source
				ON it.itemID = Source.ItemID
					AND it.traitID = Source.TraitId
					AND it.localeID = Source.LocaleId
				WHEN MATCHED THEN
					UPDATE
					SET traitValue = Source.TraitValue
				WHEN NOT MATCHED THEN
					INSERT (traitID, itemID, traitValue, localeID)
					VALUES (Source.TraitId, Source.ItemId, Source.TraitValue, Source.LocaleId);
			END'
		);
	END	

	-- ensure permissions on type	
	IF (type_id('infor.ItemTraitAddOrUpdateType') IS NOT NULL)
	BEGIN
		GRANT EXEC ON type::infor.ItemTraitAddOrUpdateType TO [IconUser], [WFM\IConInterfaceUser];
	END

	-- ensure permissions on procedure
	IF EXISTS (
		SELECT 1
		FROM   INFORMATION_SCHEMA.ROUTINES
		WHERE  ROUTINE_NAME = 'ItemTraitAddOrUpdate' AND SPECIFIC_SCHEMA = 'infor')
	BEGIN
		GRANT EXEC ON infor.ItemTraitAddOrUpdate TO [IconUser], [WFM\IConInterfaceUser];
	END
	
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END

GO