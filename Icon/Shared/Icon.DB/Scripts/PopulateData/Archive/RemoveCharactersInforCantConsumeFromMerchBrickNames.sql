declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'RemoveCharactersInforCantConsumeFromMerchBrickNames'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	--The following hierarchy classes were determined to contain characters that 
	--Infor would not be able to consume. Since we can't edit the names in Icon 
	--Web we are updating the names through a data surgery.
	
	update HierarchyClass
	set hierarchyClassName = 
		'After Sun Moisturisers'
	where hierarchyClassID = 83031

	update HierarchyClass
	set hierarchyClassName = 
		'Avocados - Pebbled Peel (Hass Type)'
	where hierarchyClassID = 83045

	update HierarchyClass
	set hierarchyClassName = 
		'Apple and Pear Ciders - Sparkling'
	where hierarchyClassID = 87353

	update HierarchyClass
	set hierarchyClassName = 
		'Non Grape Fermented Alcoholic Beverages - Still'
	where hierarchyClassID = 87357

	update HierarchyClass
	set hierarchyClassName = 
		'Vegetable Juice Drinks - Ready to Drink (Shelf Stable)'
	where hierarchyClassID = 87619

	update HierarchyClass
	set hierarchyClassName = 
		'Grains/Cereal - Ready to Eat - (Perishable)'
	where hierarchyClassID = 89183

	update HierarchyClass
	set hierarchyClassName = 
		'Dairy/Egg Based Products - Ready to Eat (Perishable)'
	where hierarchyClassID = 89184

	update HierarchyClass
	set hierarchyClassName = 
		'Hair - Styling (Powered)'
	where hierarchyClassID = 89185

	update HierarchyClass
	set hierarchyClassName = 
		'Dairy/Dairy Substitute Based Drinks - Ready to Drink (Perishable)'
	where hierarchyClassID = 97068

	update HierarchyClass
	set hierarchyClassName = 
		'Fats Edible - Animal (Shelf Stable)'
	where hierarchyClassID = 97070

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO