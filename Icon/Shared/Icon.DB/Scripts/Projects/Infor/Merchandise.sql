----Merchandise
select 
	hp.hierarchyLevelName 'Level', 
	hc.hierarchyClassID 'Merch Hier ID', 
	hc.hierarchyClassName 'Merchandise Name', 
	case when hc.hierarchyParentClassID is null then '' else convert(nvarchar(255), hc.hierarchyParentClassID) end 'Parent Hier Class ID',
	case when hct.traitValue is null then 'NULL' else hct.traitValue end 'Sub Team',
	case when nonMerch.traitValue is null then 'NULL' else nonMerch.traitValue end 'Non-Merch',
	case when prohDis.traitValue is null then 'NULL' else prohDis.traitValue end 'Prohibit Discount',
	case when taxDef.traitValue is null then 'NULL' else taxDef.traitValue end 'Default Tax Hier ID'
from HierarchyClass hc
join Hierarchy h on hc.hierarchyID = h.hierarchyID
join HierarchyPrototype hp on hc.hierarchyID = hp.hierarchyID and hc.hierarchyLevel = hp.hierarchyLevel
left join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
	and hct.traitID = 49
left join HierarchyClassTrait nonMerch on hc.hierarchyClassID = nonMerch.hierarchyClassID
	and nonMerch.traitID = 58
left join HierarchyClassTrait prohDis on hc.hierarchyClassID = prohDis.hierarchyClassID
	and prohDis.traitID = 11
left join HierarchyClassTrait taxDef on hc.hierarchyClassID = taxDef.hierarchyClassID
	and taxDef.traitID = 68
where h.hierarchyName = 'Merchandise'
order by hc.hierarchyLevel, hc.hierarchyClassID