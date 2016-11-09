--Brand
select 
	hc.hierarchyClassID 'Brand ID', 
	hc.hierarchyClassName 'Brand Name', 
	case when hct.traitValue is null then 'NULL' else hct.traitValue end 'Brand Abbreviation'
from HierarchyClass hc
join Hierarchy h on hc.hierarchyID = h.hierarchyID
left join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
	and hct.traitID = 66
where h.hierarchyName = 'Brands'
order by hc.hierarchyClassID