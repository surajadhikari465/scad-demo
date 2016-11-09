
begin

	if not exists (select traitCode from Trait where traitPattern like '%Blackhawk%')
		update Trait set traitPattern = 'Bottle Deposit|CRV|Coupon|Bottle Return|CRV Credit|Legacy POS Only|Blackhawk Fee' where traitCode = 'NM'

	if exists (select traitCode from Trait where traitDesc like 'Non Merchandise')
		update Trait set traitDesc = 'Non-Merchandise' where traitCode = 'NM'

end
go
