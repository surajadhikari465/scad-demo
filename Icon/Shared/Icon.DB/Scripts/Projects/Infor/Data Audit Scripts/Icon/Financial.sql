--Financial
--Returns the Icon HierarchyClass ID, Subteam Number, and Subteam Name
select 
	hc.hierarchyClassID 'Financial Hierarchy ID', 
	SUBSTRING(hc.hierarchyClassName, CHARINDEX('(', hc.hierarchyClassName) + 1, 4) 'Subteam',
	hc.hierarchyClassName 'Subteam Name'
from HierarchyClass hc
join Hierarchy h on hc.hierarchyID = h.hierarchyID
where h.hierarchyName = 'Financial'
order by hc.hierarchyClassID