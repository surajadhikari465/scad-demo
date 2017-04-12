--National
--Returns the full lineage of every leaf node in the National Hierarchy. Each node has the ID and Name. Also returns the National Class Code
select 
	null 'Family ID',
	fam.NatFamilyName 'Family Name',
	null 'Category ID',
	null 'Category Name',
	null 'Sub Category ID',
	cat.NatCatName 'Sub Category Name',
	null 'Class ID',
	nc.ClassName 'Class Name',
	nc.ClassID 'National Class Code'
from NatItemClass nc
join NatItemCat cat on nc.NatCatID = cat.NatCatID
join NatItemFamily fam on cat.NatFamilyID = fam.NatFamilyID
order by fam.NatFamilyID, cat.NatCatID, nc.ClassID