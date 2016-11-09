/*
All pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.
*/

go

-- TFS 12162: Add a new Default Organic Agency.
if not exists (select hc.hierarchyClassID from HierarchyClass hc join Hierarchy h on h.hierarchyID = hc.hierarchyID where hc.hierarchyClassName = 'Whole Foods Market' and h.hierarchyName = 'Certification Agency Management')
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] TFS 12162: Add a new Default Organic Agency'
		declare @hierarchyID int = (select hierarchyID from Hierarchy where hierarchyName = 'Certification Agency Management')
		INSERT INTO [dbo].[HierarchyClass]([hierarchyLevel],[hierarchyID],[hierarchyParentClassID],[hierarchyClassName])
			VALUES(1,@hierarchyID,NULL,'Whole Foods Market')
	end
else
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] **SKIPPED** TFS 12162: Add a new Default Organic Agency -- Agency already exists.'
	end
go

-- TFS 12162: Add a new Default Organic Agency.
declare @traitID int = (select traitID from Trait where traitDesc = 'Default Certification Agency')
declare @hierarchyClassID int = (select hierarchyClassID from HierarchyClass hc join Hierarchy h on h.hierarchyID = hc.hierarchyID where hc.hierarchyClassName = 'Whole Foods Market' and h.hierarchyName = 'Certification Agency Management')

if not exists (select hct.hierarchyClassID from HierarchyClassTrait hct where hct.traitID = @traitID and hct.traitValue = 'Organic')
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] TFS 12162: Add a new Default Organic Agency Organic Trait'

		insert into HierarchyClassTrait (traitID, hierarchyClassID, uomID, traitValue)
		values (@traitID,@hierarchyClassID,NULL,'Organic')
	end
else
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] TFS 12162: Add a new Default Organic Agency Organic Trait'
		print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] **SKIPPED** TFS 12162: Add a new Default Organic Agency -- Hierarchy Trait already exists.'
	end
go

-- TFS 11760: Populate the new productKey column on the Item table.
print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] TFS 11760: Populate the new productKey column on the Item table'
begin
	declare @ProductKeyStart int = 200000
	update Item set @ProductKeyStart = productKey = @ProductKeyStart + 1
end
go

