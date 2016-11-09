select *
from ItemTrait it
join Trait t
on it.traitID = t.traitID
where t.traitDesc = 'POS Description'
and LEN(it.traitValue) > 25

UPDATE ItemTrait 
SET traitValue = SUBSTRING(traitValue, 0, 26)
FROM ItemTrait it
JOIN Trait t
	ON it.traitID = t.traitID
	AND t.traitDesc = 'POS Description'
WHERE LEN(it.traitValue) > 25