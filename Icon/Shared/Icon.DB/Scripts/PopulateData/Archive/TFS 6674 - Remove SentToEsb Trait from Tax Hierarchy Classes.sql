
begin
	
	declare 
		@taxHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Tax'),
		@sentToEsbTraitId int = (select traitID from Trait where traitCode = 'ESB')

	delete
		hct
	from
		HierarchyClassTrait hct
		join HierarchyClass hc	on hct.hierarchyClassID = hc.hierarchyClassID
	where
		hc.hierarchyID = @taxHierarchyId
		and hct.traitID = @sentToEsbTraitId

end
