﻿-- Items that currently have the Modified Date trait.
select 
	'Num Items With Mod-User Trait' = count(it.itemID)
from 
	ItemTrait it 
	join Trait t on it.traitID = t.traitID 
where 
	t.traitCode = 'USR'

-- Give the trait to all items that don't currently have it.
declare @traitID int = (select traitID from Trait where traitCode = 'USR')

insert into 
	ItemTrait
select
	@traitID,
	i.itemID,
	null, -- UOM
	null, -- traitValue
	1
from
	Item i
where
	i.itemID not in (
						select 
							it.itemID 
						from 
							ItemTrait it 
							join Trait t on it.traitID = t.traitID 
						where t.traitCode = 'USR')