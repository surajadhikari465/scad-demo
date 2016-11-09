IF NOT EXISTS (SELECT * FROM Trait WHERE traitDesc = 'Hidden Item' AND traitCode = 'HID')
begin
	set identity_insert dbo.Trait on
	INSERT INTO Trait (traitID, traitCode, traitPattern, traitDesc, traitGroupID) 
	VALUES (70, 'HID', '0|1', 'Hidden Item', 1)
	set identity_insert dbo.Trait off
end
