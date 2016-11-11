declare @scriptKey varchar(128)

-- Product Backlog Item 17488: Purge AppLog on ItemCatalog
set @scriptKey = 'Add Financial Hierarchy Code to Sub Teams'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	insert into HierarchyClassTrait(hierarchyClassID, traitID, traitValue)
	select
		hc.hierarchyClassID,
		53,
		SUBSTRING(hc.hierarchyClassName, CHARINDEX('(', hc.hierarchyClassName) + 1, 4)
	from HierarchyClass hc
	left join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
		and hct.traitID = 53 -- 53 is the trait ID for the Financial Hierarchy Code
	where hc.hierarchyID = 5 -- 5 is the hierarchy ID for the Financial Hierarchy
		and hct.hierarchyClassID is null

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO