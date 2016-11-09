
begin
	
	if not exists (select itemTypeID from ItemType where itemTypeCode = 'FEE')
		insert into ItemType values ('FEE', 'Fee')

	if exists (select itemTypeID from ItemType where itemTypeDesc = 'Non Retail')
		update ItemType set itemTypeDesc = 'Non-Retail' where itemTypeDesc = 'Non Retail'

end
go
