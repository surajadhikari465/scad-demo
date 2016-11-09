IF NOT EXISTS (SELECT * FROM Trait WHERE traitCode = 'DEG')
BEGIN
	insert into Trait
	values ('DEG', '1', 'Disable Event Generation', 7)
END