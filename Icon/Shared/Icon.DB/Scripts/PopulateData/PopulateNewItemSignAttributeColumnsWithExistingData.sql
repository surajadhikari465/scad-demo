declare @scriptKey varchar(128)

-- PBI 29184 - As GDT I want enumeration removed for several attributes in Icon - Add New Columns to Database
set @scriptKey = 'PopulateNewItemSignAttributeColumnsWithExistingData'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN

	-- Populate the new columns with values from old columns using a post deploy script
	UPDATE dbo.ItemSignAttribute  
	SET AnimalWelfareRating = awr.[Description]
	FROM dbo.AnimalWelfareRating awr
		INNER JOIN dbo.ItemSignAttribute isa on isa.AnimalWelfareRatingId = awr.AnimalWelfareRatingId		

	UPDATE dbo.ItemSignAttribute  
	SET MilkType = mt.[Description]
	FROM dbo.MilkType mt
		INNER JOIN dbo.ItemSignAttribute isa on isa.CheeseMilkTypeId = mt.MilkTypeId

	UPDATE dbo.ItemSignAttribute  
	SET EcoScaleRating = esr.[Description]
	FROM dbo.EcoScaleRating esr
		INNER JOIN dbo.ItemSignAttribute isa on isa.EcoScaleRatingId = esr.EcoScaleRatingId

	UPDATE dbo.ItemSignAttribute  
	SET FreshOrFrozen = sff.[Description]
	FROM dbo.SeafoodFreshOrFrozen sff
		INNER JOIN dbo.ItemSignAttribute isa on isa.SeafoodFreshOrFrozenId = sff.SeafoodFreshOrFrozenId

	UPDATE dbo.ItemSignAttribute  
	SET SeafoodCatchType = sct.[Description]
	FROM dbo.SeafoodCatchType sct
		INNER JOIN dbo.ItemSignAttribute isa on isa.SeafoodCatchTypeId = sct.SeafoodCatchTypeId

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO