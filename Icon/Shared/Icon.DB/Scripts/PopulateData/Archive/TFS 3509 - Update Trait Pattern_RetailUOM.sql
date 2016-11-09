--================================================
-- Date:		10/12/14
-- Author:		Ben Sims
-- Description: Update trait pattern and traitGroup
--				for Retail UOM and traitGroup for
--				Retail Size Traits.
--================================================
UPDATE t
SET 
	t.traitPattern = '^[a-zA-z ]+$',
	t.traitGroupID = 1
FROM
	Trait t
WHERE
	t.traitDesc = 'Retail UOM'
GO

UPDATE t
SET 
	t.traitGroupID = 1
FROM
	Trait t
WHERE
	t.traitDesc = 'Retail Size'
GO