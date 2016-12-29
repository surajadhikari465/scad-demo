--National
--Returns the full lineage of every leaf node in the National Hierarchy. Each node has the ID and Name. Also returns the National Class Code
select 
	hc4.hierarchyClassID 'Family ID',
	hc4.hierarchyClassName 'Family Name',
	hc3.hierarchyClassID 'Category ID',
	hc3.hierarchyClassName 'Category Name',
	hc2.hierarchyClassID 'Sub Category ID',
	hc2.hierarchyClassName 'Sub Category Name',
	hc.hierarchyClassID 'Class ID',
	hc.hierarchyClassName 'Class Name',
	hct.traitValue 'National Class Code'
from HierarchyClass hc
join HierarchyClass hc2 on hc.hierarchyParentClassID = hc2.hierarchyClassID
join HierarchyClass hc3 on hc2.hierarchyParentClassID = hc3.hierarchyClassID
join HierarchyClass hc4 on hc3.hierarchyParentClassID = hc4.hierarchyClassID
join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
where hc.hierarchyID = 6
	and hct.traitID = 69
order by hc4.hierarchyClassID, hc3.hierarchyClassID, hc2.hierarchyClassID, hc.hierarchyClassID