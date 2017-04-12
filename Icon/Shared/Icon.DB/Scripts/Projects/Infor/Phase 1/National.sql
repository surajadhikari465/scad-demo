--National
select 
	hp.hierarchyLevelName 'Level', 
	hc.hierarchyClassID 'National Hier ID', 
	hc.hierarchyClassName 'National Class Name', 
	case when hc.hierarchyParentClassID is null then 'NULL' else convert(nvarchar(255), hc.hierarchyParentClassID) end 'Parent Hier Class ID',
	case when hct.traitValue is null then 'NULL' else hct.traitValue end 'National Class Code'
from HierarchyClass hc
join Hierarchy h on hc.hierarchyID = h.hierarchyID
join HierarchyPrototype hp on hc.hierarchyID = hp.hierarchyID and hc.hierarchyLevel = hp.hierarchyLevel
left join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
where h.hierarchyName = 'National'
order by hc.hierarchyLevel, hc.hierarchyClassID