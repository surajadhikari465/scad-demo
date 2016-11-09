if not exists(select 1 from Trait where traitCode = 'MDT')
begin
	set identity_insert dbo.Trait on
	insert into Trait( traitID, traitCode, traitPattern, traitDesc, traitGroupID)
	values (68, 'MDT', '^[0-9]*$', 'Merch Default Tax Associatation', 7) 
	set identity_insert dbo.Trait off
end