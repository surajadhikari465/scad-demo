/*
All master pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.
*/
-- TFS 12162: Add a new Default Agency trait.
begin
	if not exists (select traitID from Trait where traitCode = 'DFC')
		begin
			print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] TFS 12162: Add a new Default Agency trait -- Adding trait'
			declare @HierarchyClassTraitGroup int = (select traitGroupID from TraitGroup where traitGroupCode = 'HYT')
			insert into Trait values ('DFC','1','Default Certification Agency',@HierarchyClassTraitGroup)
		end
	else
		begin
			print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] **SKIPPED** TFS 12162: Add a new Default Agency trait -- Trait already exists.'
		end
end
go
