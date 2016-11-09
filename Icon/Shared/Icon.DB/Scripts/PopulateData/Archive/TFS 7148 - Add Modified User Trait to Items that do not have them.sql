-------------------------------------
--INSERT MODIFIED USER TRAITS
-------------------------------------
declare @modifiedUserTraitID int = (select traitID from Trait where traitDesc = 'Modified User')

--Insert modified user traits for items that don't have them
insert into ItemTrait(traitID, itemID, traitValue, uomID, localeID)
select	@modifiedUserTraitID,
		i.itemID,
		null,
		null,
		1
from Item i
	join ScanCode sc on i.itemID = sc.itemID --Joining on ScanCode to restrict Items to only Items that currently exist
where not exists 
	(select 1 from ItemTrait it where it.traitID = @modifiedUserTraitID	and it.itemID = i.itemID)