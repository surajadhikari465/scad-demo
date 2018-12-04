-- PBI 29201: As GDT I want hardcoding removed for several attributes in Icon 
--  - Remove old columns from ItemSignAttribute table (must be last PBI in series)
-- (PBI 29184 - As GDT I want enumeration removed for several attributes in Icon - Add New Columns to Database)
-- Pull Requests related to this: 1397, 1399, 1400, 1402, 1405, 1422

DECLARE @scriptKey VARCHAR(128)= 'PopulateNewItemSignAttributeColumnsWithExistingData'

IF (not exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	-- Populate the new columns in the ItemSignAttribute table from values stored 
	--   in the temporary staging table during pre-deploy	
	UPDATE dbo.ItemSignAttribute SET AnimalWelfareRating = t.AnimalWelfareRating
	FROM stage.tempItemSignAttribute t
	WHERE dbo.ItemSignAttribute.ItemSignAttributeID = t.ItemSignAttributeID and t.AnimalWelfareRating is not null

	UPDATE dbo.ItemSignAttribute SET MilkType = t.MilkType
	FROM stage.tempItemSignAttribute t
	WHERE dbo.ItemSignAttribute.ItemSignAttributeID = t.ItemSignAttributeID and t.MilkType is not null

	UPDATE dbo.ItemSignAttribute SET EcoScaleRating = t.EcoScaleRating
	FROM stage.tempItemSignAttribute t
	WHERE dbo.ItemSignAttribute.ItemSignAttributeID = t.ItemSignAttributeID and t.EcoScaleRating is not null

	UPDATE dbo.ItemSignAttribute SET FreshOrFrozen = t.FreshOrFrozen
	FROM stage.tempItemSignAttribute t
	WHERE dbo.ItemSignAttribute.ItemSignAttributeID = t.ItemSignAttributeID and t.FreshOrFrozen is not null

	UPDATE dbo.ItemSignAttribute SET SeafoodCatchType = t.SeafoodCatchType
	FROM stage.tempItemSignAttribute t
	WHERE dbo.ItemSignAttribute.ItemSignAttributeID = t.ItemSignAttributeID and t.SeafoodCatchType is not null
	
	-- dispose of the table used for temporarily storing data between the pre- and post-deploy
	IF EXISTS(
		SELECT *
		FROM sys.tables t join sys.schemas s ON (t.schema_id = s.schema_id)
		WHERE s.name = 'stage' and t.name = 'tempItemSignAttribute')
	BEGIN
		DROP TABLE stage.tempItemSignAttribute
	END

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO