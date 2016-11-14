--Sub team
select 
	hc5.hierarchyClassName 'Level 1',
	hc4.hierarchyClassName 'Level 2',
	hc3.hierarchyClassName 'Level 3',
	hc2.hierarchyClassName 'Level 4',
	hc.hierarchyClassName 'Level 5',
	'Subteam' 'Select an Attribute',
	hct.traitValue 'Attribute Value',
	'TRUE' 'Force this value to be used on all items',
	hc.hierarchyLevel 'Attribute Level'
from HierarchyClass hc
join HierarchyClass hc2 on hc.hierarchyParentClassID = hc2.hierarchyClassID
join HierarchyClass hc3 on hc2.hierarchyParentClassID = hc3.hierarchyClassID
join HierarchyClass hc4 on hc3.hierarchyParentClassID = hc4.hierarchyClassID
join HierarchyClass hc5 on hc4.hierarchyParentClassID = hc5.hierarchyClassID
join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
where hc.hierarchyID = 1
	and hct.traitID = 49
union --Item Type
select 
	hc5.hierarchyClassName 'Level 1',
	hc4.hierarchyClassName 'Level 2',
	hc3.hierarchyClassName 'Level 3',
	hc2.hierarchyClassName 'Level 4',
	hc.hierarchyClassName 'Level 5',
	'Item Type' 'Select an Attribute',
	case 
			when hct.traitValue is null then 'N/A'
			else hct.traitValue
		end 'Attribute Value',
	'TRUE' 'Force this value to be used on all items',
	hc.hierarchyLevel 'Attribute Level'
from HierarchyClass hc
join HierarchyClass hc2 on hc.hierarchyParentClassID = hc2.hierarchyClassID
join HierarchyClass hc3 on hc2.hierarchyParentClassID = hc3.hierarchyClassID
join HierarchyClass hc4 on hc3.hierarchyParentClassID = hc4.hierarchyClassID
join HierarchyClass hc5 on hc4.hierarchyParentClassID = hc5.hierarchyClassID
left join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
	and hct.traitID = 58
where hc.hierarchyID = 1
union --Prohibit Discount level 4
select 
	hc5.hierarchyClassName 'Level 1',
	hc4.hierarchyClassName 'Level 2',
	hc3.hierarchyClassName 'Level 3',
	hc2.hierarchyClassName 'Level 4',
	hc.hierarchyClassName 'Level 5',
	'Prohibit Discount' 'Select an Attribute',
	case 
		when hct.traitValue = 1 then 'TRUE'
		else 'FALSE'
	end 'Attribute Value',
	'TRUE' 'Force this value to be used on all items',
	hc2.hierarchyLevel 'Attribute Level'
from HierarchyClass hc
join HierarchyClass hc2 on hc.hierarchyParentClassID = hc2.hierarchyClassID
join HierarchyClass hc3 on hc2.hierarchyParentClassID = hc3.hierarchyClassID
join HierarchyClass hc4 on hc3.hierarchyParentClassID = hc4.hierarchyClassID
join HierarchyClassTrait hct on hc2.hierarchyClassID = hct.hierarchyClassID
join HierarchyClass hc5 on hc4.hierarchyParentClassID = hc5.hierarchyClassID
where hc.hierarchyID = 1
	and hct.traitID = 11
union --Prohibit Discount level 5
select 
	hc5.hierarchyClassName 'Level 1',
	hc4.hierarchyClassName 'Level 2',
	hc3.hierarchyClassName 'Level 3',
	hc2.hierarchyClassName 'Level 4',
	hc.hierarchyClassName 'Level 5',
	'Prohibit Discount' 'Select an Attribute',
	case 
		when hct.traitValue = 1 then 'TRUE'
		else 'FALSE'
	end 'Attribute Value',
	'TRUE' 'Force this value to be used on all items',
	hc.hierarchyLevel 'Attribute Level'
from HierarchyClass hc
join HierarchyClass hc2 on hc.hierarchyParentClassID = hc2.hierarchyClassID
join HierarchyClass hc3 on hc2.hierarchyParentClassID = hc3.hierarchyClassID
join HierarchyClass hc4 on hc3.hierarchyParentClassID = hc4.hierarchyClassID
join HierarchyClass hc5 on hc4.hierarchyParentClassID = hc5.hierarchyClassID
join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
where hc.hierarchyID = 1
	and hct.traitID = 11
union --Tax Class
select 
	hc5.hierarchyClassName 'Level 1',
	hc4.hierarchyClassName 'Level 2',
	hc3.hierarchyClassName 'Level 3',
	hc2.hierarchyClassName 'Level 4',
	hc.hierarchyClassName 'Level 5',
	'Tax Class' 'Select an Attribute',
	tax.hierarchyClassName 'Attribute Value',
	'FALSE' 'Force this value to be used on all items',
	hc.hierarchyLevel 'Attribute Level'
from HierarchyClass hc
join HierarchyClass hc2 on hc.hierarchyParentClassID = hc2.hierarchyClassID
join HierarchyClass hc3 on hc2.hierarchyParentClassID = hc3.hierarchyClassID
join HierarchyClass hc4 on hc3.hierarchyParentClassID = hc4.hierarchyClassID
join HierarchyClass hc5 on hc4.hierarchyParentClassID = hc5.hierarchyClassID
join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
	and hct.traitID = 68
join HierarchyClass tax on convert(int, hct.traitValue) = tax.hierarchyClassID
where hc.hierarchyID = 1
order by [Select an Attribute], [Attribute Value], [Attribute Level], [Level 1], [Level 2], [Level 3], [Level 4], [Level 5]