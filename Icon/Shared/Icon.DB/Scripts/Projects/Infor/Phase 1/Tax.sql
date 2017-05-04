--Tax
--Returns Icon HierarchyClass ID, Tax Code, Tax Name, Tax Abbreviation, and Tax Romance
select 
	hc.hierarchyClassID 'Tax Hier ID', 
	substring(hc.hierarchyClassName, 0, 8) 'Tax Class ID',
	hc.hierarchyClassName 'Tax Class',
	case when abr.traitValue is null then 'NULL' else abr.traitValue end 'Tax Abbreviation',
	case when rom.traitValue is null then 'NULL' else rom.traitValue end 'Tax Romance'
from HierarchyClass hc
join Hierarchy h on hc.hierarchyID = h.hierarchyID
left join HierarchyClassTrait abr on hc.hierarchyClassID = abr.hierarchyClassID
	and abr.traitID = 51
left join HierarchyClassTrait rom on hc.hierarchyClassID = rom.hierarchyClassID
	and rom.traitID = 67
where h.hierarchyName = 'Tax'
order by hc.hierarchyLevel, hc.hierarchyClassID