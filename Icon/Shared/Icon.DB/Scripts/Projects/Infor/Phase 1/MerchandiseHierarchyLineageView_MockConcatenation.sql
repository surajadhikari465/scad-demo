﻿--Merchandise
--Returns the full lineage of every leaf node in the Merchandise Hierarchy. Each node has the ID and Name. Also returns the Sub Brick Code and concatenates the subbrick and subteam name
select 
	hc5.hierarchyClassID 'Segment ID',
	hc5.hierarchyClassName 'Segment Name',
	hc4.hierarchyClassID 'Family ID',
	hc4.hierarchyClassName 'Family Name',
	hc3.hierarchyClassID 'Class ID',
	hc3.hierarchyClassName 'Class Name',
	hc2.hierarchyClassID 'GS1 Brick ID',
	hc2.hierarchyClassName 'GS1 Brick Name',
	hc.hierarchyClassID 'Sub Brick ID',
	hc.hierarchyClassName + ':' + subteam.traitValue 'Sub Brick Name',
	hct.traitValue 'Sub Brick Code'
from HierarchyClass hc
join HierarchyClass hc2 on hc.hierarchyParentClassID = hc2.hierarchyClassID
join HierarchyClass hc3 on hc2.hierarchyParentClassID = hc3.hierarchyClassID
join HierarchyClass hc4 on hc3.hierarchyParentClassID = hc4.hierarchyClassID
join HierarchyClass hc5 on hc4.hierarchyParentClassID = hc5.hierarchyClassID
join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
join HierarchyClassTrait subteam on hc.hierarchyClassID = subteam.HierarchyClassID
where hc.hierarchyID = 1
	and hct.traitID = 52 --traitID for Sub Brick Code
	and subteam.traitID = 49 --traitID for MerchFinMapping which has the name of the subteam the subbrick is associated to
order by hc5.hierarchyClassID, hc4.hierarchyClassID, hc3.hierarchyClassID, hc2.hierarchyClassID, hc.hierarchyClassID