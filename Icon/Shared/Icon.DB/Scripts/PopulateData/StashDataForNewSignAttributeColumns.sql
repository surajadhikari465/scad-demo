-- PBI 29201: As GDT I want hardcoding removed for several attributes in Icon 
--  - Remove old columns from ItemSignAttribute table (must be last PBI in series)
-- (PBI 29184 - As GDT I want enumeration removed for several attributes in Icon - Add New Columns to Database)
-- Pull Requests related to this: 1397, 1399, 1400, 1402, 1405, 1422

DECLARE @scriptKey varchar(128)= 'StashItemSignAttributeEnumDataInTempTable'

IF (not exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	-- since we are removing several ...Id columns from ItemSignAttribute and replacing them with text fields,
	--  we need to copy the data to a "temporary" table in the pre-deployment in order to preserve the data
	--  Then in the Post-Deploy, the stashed data will be copied back to the new columns in the table	
	
	-- create the temporary (persisted for now) table
	IF NOT EXISTS(
		SELECT *
		FROM sys.tables t
		JOIN sys.schemas s ON (t.schema_id = s.schema_id)
		WHERE s.name = 'stage' AND t.name = 'tempItemSignAttribute')
	BEGIN
		CREATE TABLE stage.tempItemSignAttribute (
			ItemSignAttributeID INT NOT NULL IDENTITY, 
			AnimalWelfareRating NVARCHAR(255) NULL,
			MilkType NVARCHAR(255) NULL,
			EcoScaleRating NVARCHAR(255) NULL,
			FreshOrFrozen NVARCHAR(255) NULL,
			SeafoodCatchType NVARCHAR(255) NULL,
		)
	END
	
	-- insert data into the staging table using the joined id values from the existing ItemSignAttribute table
	INSERT INTO stage.tempItemSignAttribute (ItemSignAttributeID, AnimalWelfareRating)
	SELECT isa.ItemSignAttributeID, awr.[Description]
	FROM dbo.AnimalWelfareRating awr
		INNER JOIN dbo.ItemSignAttribute isa ON isa.AnimalWelfareRatingId = awr.AnimalWelfareRatingId

	INSERT INTO stage.tempItemSignAttribute (ItemSignAttributeID, MilkType)
	SELECT isa.ItemSignAttributeID, mt.[Description]
	FROM dbo.MilkType mt
		INNER JOIN dbo.ItemSignAttribute isa ON isa.CheeseMilkTypeId = mt.MilkTypeId

	INSERT INTO stage.tempItemSignAttribute (ItemSignAttributeID, EcoScaleRating)
	SELECT isa.ItemSignAttributeID, esr.[Description]
	FROM dbo.EcoScaleRating esr
		INNER JOIN dbo.ItemSignAttribute isa ON isa.EcoScaleRatingId = esr.EcoScaleRatingId

	INSERT INTO stage.tempItemSignAttribute (ItemSignAttributeID, FreshOrFrozen)
	SELECT isa.ItemSignAttributeID, sff.[Description]
	FROM dbo.SeafoodFreshOrFrozen sff
		INNER JOIN dbo.ItemSignAttribute isa ON isa.SeafoodFreshOrFrozenId = sff.SeafoodFreshOrFrozenId

	INSERT INTO stage.tempItemSignAttribute (ItemSignAttributeID, SeafoodCatchType)
	SELECT isa.ItemSignAttributeID, sct.[Description]
	FROM dbo.SeafoodCatchType sct
		INNER JOIN dbo.ItemSignAttribute isa ON isa.SeafoodCatchTypeId = sct.SeafoodCatchTypeId

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO