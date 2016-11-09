
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-07-07
-- Description:	Used for initial load of the locale 
--				hierarchy.  Parameter @Level accepts
--				the name of locale hierarchy level (Chain,
--				Region, Metro, Store).
-- =============================================

CREATE PROCEDURE [app].[InitialLoadLocale]
	@Level	nvarchar(16),
	@Region nvarchar(50) = NULL
AS
BEGIN
	
	declare
		@LocaleMessageId int = (select MessageTypeId from app.MessageType where MessageTypeName = 'Locale'),
		@ReadyStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready'),
		@LocaleTypeId int = (select localeTypeId from LocaleType where localeTypeDesc = @Level),
		@BusinessUnitTraitId int = (select traitID from Trait where traitCode = 'BU'),
		@StoreAbbreviationTraitId int = (select traitID from Trait where traitCode = 'SAB'),
		@PhoneNumberTraitId int = (select traitID from Trait where traitCode = 'PHN')

	insert into 
		app.MessageQueueLocale
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
		@LocaleTypeId,
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
		null
	from
		Locale l
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
	where
		l.localeTypeID = @LocaleTypeId
		AND ((@Level = 'Chain')
			OR (@Level = 'Region' AND l.localeName = @Region)
			OR (@Level = 'Metro' AND p.localeName = @Region)
			OR (@Level = 'Store' AND r.localeName = @Region)
			OR (@Region IS NULL))
END
GO
