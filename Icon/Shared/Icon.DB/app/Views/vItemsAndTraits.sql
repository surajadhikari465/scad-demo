CREATE VIEW [app].[vItemsAndTraits]
AS
/*
	This is a simple view to get the scancode and trait details (code, name, value) for all items.
*/
select
	i.itemID
	,sc.scanCode
	,t.traitDesc
	,it.traitValue
from item i
join ScanCode sc
on i.itemID = sc.itemID
join ItemTrait it
on i.itemID = it.itemID
join Trait t
on it.traitID = t.traitID