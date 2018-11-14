CREATE PROCEDURE [app].[RefreshLocales]
	@ids app.IntList readonly
AS
BEGIN
	DECLARE @taskName nvarchar(20) = 'RefreshLocales'

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding ESB messages for Locales...';

	declare
		@LocaleMessageId int = (select MessageTypeId from app.MessageType where MessageTypeName = 'Locale'),
		@ReadyStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready'),
		@BusinessUnitTraitId int = (select traitID from Trait where traitCode = 'BU'),
		@StoreAbbreviationTraitId int = (select traitID from Trait where traitCode = 'SAB'),
		@PhoneNumberTraitId int = (select traitID from Trait where traitCode = 'PHN')

	insert into 
		app.MessageQueueLocale
		(
		[MessageTypeId],
		[MessageStatusId],
		[MessageHistoryId],
		[InsertDate],
		[LocaleId],
		[OwnerOrgPartyId],
		[StoreAbbreviation],
		[LocaleName],
		[LocaleOpenDate],
		[LocaleCloseDate],
		[LocaleTypeId],
		[ParentLocaleId],
		[BusinessUnitId],
		[AddressId],
		[AddressUsageCode],
		[CountryName],
		[CountryCode],
		[TerritoryName],
		[TerritoryCode],
		[CityName],
		[PostalCode],
		[Latitude],
		[Longitude],
		[AddressLine1],
		[AddressLine2],
		[AddressLine3],
		[TimezoneCode],
		[TimezoneName],
		[PhoneNumber],
		[VenueCode],
		[VenueOccopant],
		[VenueSubType],
		[InProcessBy],
		[ProcessedDate]
		)
	select
		@LocaleMessageId,
		@ReadyStatusId,
		null,
		sysdatetime(),
		l.localeId,
		l.ownerOrgPartyID,
		abbr.traitValue,
		l.localeName,
		l.localeOpenDate,
		l.localeCloseDate,
		l.localeTypeID,
		l.parentLocaleID,
		isnull(lt.traitValue, 0),
		a.addressID,
		au.AddressUsageCode,
		c.countryName,
		c.countryCode,
		t.territoryName,
		t.territoryCode,
		ci.CityName,
		pc.postalCode,
		pa.latitude,
		pa.longitude,
		pa.addressLine1,
		pa.addressLine2,
		pa.addressLine3,
		isnull(tz.timezoneCode, 'CST'),
		isnull(tz.timezoneName, '(UTC-06:00) Central Time (US & Canada)'),
		isnull(ph.traitValue, '512-477-4455'),
		null,
		null,
		null,
		null,
		null
	from
		@ids ids
		join Locale l on ids.I = l.localeID
		left join Locale p on l.parentLocaleID = p.localeID
		left join Locale r on p.parentLocaleID = r.localeID
		left join LocaleTrait lt on l.localeID = lt.localeID and lt.traitID = @BusinessUnitTraitId
		left join LocaleTrait abbr on l.localeID = abbr.localeID and abbr.traitID = @StoreAbbreviationTraitId
		left join LocaleTrait ph on l.localeID = ph.localeID and ph.traitID = @PhoneNumberTraitID
		left join LocaleAddress la on l.localeID = la.localeID
		left join Address a on la.addressID = a.addressID
		left join AddressUsage au on la.addressUsageID = au.addressUsageID
		left join PhysicalAddress pa on a.addressID = pa.addressID
		left join Country c on pa.countryID = c.countryID
		left join Territory t on pa.territoryID = t.territoryID
		left join City ci on pa.cityID = ci.cityID
		left join PostalCode pc on pa.postalCodeID = pc.postalCodeID
		left join Timezone tz on pa.timezoneID = tz.timezoneID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Successfully refreshed Locales...';
END