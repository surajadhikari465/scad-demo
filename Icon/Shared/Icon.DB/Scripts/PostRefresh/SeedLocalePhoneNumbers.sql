declare
	@PhoneNumberTraitId int = (select traitID from Trait where traitCode = 'PHN'),
	@StoreLocaleTypeId int = (select localeTypeID from LocaleType where localeTypeCode = 'ST')

merge
	LocaleTrait as lt
using
	(select localeID from locale where localeTypeID = @StoreLocaleTypeId) as loc
on
	loc.localeid = lt.localeid and
	@PhoneNumberTraitId = lt.traitID
when not matched then
	insert 
		(traitID, localeID, uomID, traitValue)
	values
		(@PhoneNumberTraitId, loc.localeID, null, '512-477-4455');
