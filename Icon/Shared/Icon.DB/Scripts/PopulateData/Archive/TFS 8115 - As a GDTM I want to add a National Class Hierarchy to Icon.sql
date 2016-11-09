if not exists(select 1 from Hierarchy where hierarchyName = 'National Class')
begin
	set identity_insert dbo.Hierarchy on
	insert into Hierarchy(hierarchyID, hierarchyName) values(6, 'National Class')
	set identity_insert dbo.Hierarchy off
end

if not exists(select 1 from HierarchyPrototype where [hierarchyID] = 6)
begin
  insert into [HierarchyPrototype]([hierarchyID], [hierarchyLevel], [hierarchyLevelName], [itemsAttached])
  select 6, 1, 'Family', 1

  
  insert into [HierarchyPrototype] ([hierarchyID], [hierarchyLevel], [hierarchyLevelName], [itemsAttached])
  select 6, 2, 'Category', 1

  
  insert into [HierarchyPrototype] ([hierarchyID], [hierarchyLevel], [hierarchyLevelName], [itemsAttached])
  select 6, 3, 'Sub Category', 1
  
  insert into [HierarchyPrototype]([hierarchyID] , [hierarchyLevel], [hierarchyLevelName], [itemsAttached])
  select 6, 4, 'Class', 1
 end

set Identity_Insert dbo.Trait on 
insert into Trait (traitID, traitCode, traitPattern, traitDesc, traitGroupID) 
select 69, 'NCC', '^[0-9]*$', 'Nationcal Class Code', 7
set Identity_Insert dbo.Trait off
