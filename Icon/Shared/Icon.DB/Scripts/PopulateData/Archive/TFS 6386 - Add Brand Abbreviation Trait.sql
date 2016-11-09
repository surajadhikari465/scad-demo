-- This script will add the Brand Abbreviation trait to the Trait table if it does not exist yet
IF NOT EXISTS (SELECT * FROM Trait t WHERE t.traitDesc = 'Brand Abbreviation')
BEGIN
	INSERT INTO Trait (traitCode, traitPattern, traitDesc, traitGroupID)
	VALUES ('BA', '^[a-zA-Z0-9 ]{1,10}$', 'Brand Abbreviation', 7)
END