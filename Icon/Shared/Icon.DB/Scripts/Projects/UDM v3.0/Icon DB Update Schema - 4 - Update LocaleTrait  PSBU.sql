-- Task 3343
UPDATE localetrait
SET traitid = (
		SELECT traitid
		FROM trait
		WHERE traitdesc = 'Region Abbreviation'
		)
WHERE traitid = 39

UPDATE localetrait
SET traitid = (
		SELECT traitid
		FROM trait
		WHERE traitdesc = 'PS Business Unit ID'
		)
WHERE traitid = 40

UPDATE localetrait
SET traitvalue = RIGHT(traitvalue, 5)
WHERE traitid = (
		SELECT traitid
		FROM trait
		WHERE traitdesc = 'PS Business Unit ID'
		)
	AND traitvalue LIKE 'BU_%'
	-- should be 466 rows affected