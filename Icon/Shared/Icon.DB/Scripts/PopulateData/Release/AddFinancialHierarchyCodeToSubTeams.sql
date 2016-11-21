declare @scriptKey varchar(128)

-- Product Backlog Item 17488: Purge AppLog on ItemCatalog
set @scriptKey = 'Add Financial Hierarchy Code to Sub Teams'

if(not exists(select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
begin
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @finHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Financial'),
			@finHierarchyCodeTraitId int = (select traitID from Trait where traitDesc = 'Financial Hierarchy Code')

	insert into HierarchyClassTrait(hierarchyClassID, traitID, traitValue)
	select
		hc.hierarchyClassID,
		53,
		SUBSTRING(hc.hierarchyClassName, CHARINDEX('(', hc.hierarchyClassName) + 1, 4)
	from HierarchyClass hc
	left join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
		and hct.traitID = @finHierarchyCodeTraitId
	where hc.hierarchyID = @finHierarchyId
		and hct.hierarchyClassID is null

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

end
else
begin
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
end
go