--This script is used to pull the national class data from Icon and store it into a CSV file.
--The CSV file will be used in the "Update National Classes in IRMA _4" scripts to load those into IRMA.
use Icon
go
select hc.hierarchyClassID Id, hc.hierarchyParentClassID ParentId, hc.hierarchyLevel Level, hc.hierarchyClassName Name, hct.traitValue Code
from HierarchyClass hc
left join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
left join Trait t on hct.traitID = t.traitID
	and t.traitDesc = 'NCC'
where hc.hierarchyID = 6
order by hc.hierarchyLevel, hc.hierarchyClassID