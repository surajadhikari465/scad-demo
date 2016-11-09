 update [HierarchyPrototype] set hierarchyLevelName = 'National Family' where hierarchyID = 6 and hierarchyLevel = 1
 update [HierarchyPrototype] set hierarchyLevelName = 'National Category' where hierarchyID = 6 and hierarchyLevel = 2
 update [HierarchyPrototype] set hierarchyLevelName = 'National Sub Category' where hierarchyID = 6 and hierarchyLevel = 3
 update [HierarchyPrototype] set hierarchyLevelName = 'National Class' where hierarchyID = 6 and hierarchyLevel = 4

 
update Hierarchy set hierarchyName = 'National' where hierarchyID = 6