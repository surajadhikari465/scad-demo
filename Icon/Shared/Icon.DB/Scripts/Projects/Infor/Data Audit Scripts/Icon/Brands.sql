--Brand
--Returns Brand Names, Brand Abbreviations, and the Brand ID
select 
	hc.hierarchyClassName 'Brand Name', 
	case when hct.traitValue is null then 'NULL' else hct.traitValue end 'Brand Abbreviation',
	hc.hierarchyClassID 'Brand ID'
from HierarchyClass hc
join Hierarchy h on hc.hierarchyID = h.hierarchyID
left join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
	and hct.traitID = 66 --trait ID for brand abbreviation
where h.hierarchyName = 'Brands'
order by hc.hierarchyClassID