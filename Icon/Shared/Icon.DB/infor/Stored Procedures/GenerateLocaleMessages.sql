CREATE PROCEDURE infor.GenerateLocaleMessages @locales infor.GenerateLocaleMessageType READONLY
	,@stores infor.GenerateStoreMessageType READONLY
AS
BEGIN
	DECLARE @LocaleMessageId INT = (
			SELECT MessageTypeId
			FROM app.MessageType
			WHERE MessageTypeName = 'Locale'
			)
		,@ReadyStatusId INT = (
			SELECT MessageStatusId
			FROM app.MessageStatus
			WHERE MessageStatusName = 'Ready'
			)
		,@BusinessUnitTraitId INT = (
			SELECT traitID
			FROM Trait
			WHERE traitCode = 'BU'
			)
		,@StoreAbbreviationTraitId INT = (
			SELECT traitID
			FROM Trait
			WHERE traitCode = 'SAB'
			)
		,@PhoneNumberTraitId INT = (
			SELECT traitID
			FROM Trait
			WHERE traitCode = 'PHN'
			)

	INSERT INTO app.MessageQueueLocale
	SELECT @LocaleMessageId
		,@ReadyStatusId
		,NULL
		,sysdatetime()
		,l.localeId
		,l.ownerOrgPartyID
		,abbr.traitValue
		,l.localeName
		,l.localeOpenDate
		,l.localeCloseDate
		,ltp.localeTypeID
		,l.parentLocaleID
		,isnull(lt.traitValue, 0)
		,a.addressID
		,au.AddressUsageCode
		,c.countryName
		,c.countryCode
		,t.territoryName
		,t.territoryCode
		,ci.CityName
		,pc.postalCode
		,pa.latitude
		,pa.longitude
		,pa.addressLine1
		,pa.addressLine2
		,pa.addressLine3
		,isnull(tz.timezoneCode, 'CST')
		,isnull(tz.timezoneName, '(UTC-06:00) Central Time (US & Canada)')
		,isnull(ph.traitValue, '512-477-4455')
		,NULL
		,NULL
	FROM @locales tempLocales
	JOIN Locale l ON tempLocales.LocaleId = l.localeID
	JOIN LocaleType ltp ON l.localeTypeID = ltp.localeTypeID
	LEFT JOIN Locale p ON l.parentLocaleID = p.localeID
	LEFT JOIN Locale r ON p.parentLocaleID = r.localeID
	LEFT JOIN LocaleTrait lt ON l.localeID = lt.localeID
		AND lt.traitID = @BusinessUnitTraitId
	LEFT JOIN LocaleTrait abbr ON l.localeID = abbr.localeID
		AND abbr.traitID = @StoreAbbreviationTraitId
	LEFT JOIN LocaleTrait ph ON l.localeID = ph.localeID
		AND ph.traitID = @PhoneNumberTraitID
	LEFT JOIN LocaleAddress la ON l.localeID = la.localeID
	LEFT JOIN Address a ON la.addressID = a.addressID
	LEFT JOIN AddressUsage au ON la.addressUsageID = au.addressUsageID
	LEFT JOIN PhysicalAddress pa ON a.addressID = pa.addressID
	LEFT JOIN Country c ON pa.countryID = c.countryID
	LEFT JOIN Territory t ON pa.territoryID = t.territoryID
	LEFT JOIN City ci ON pa.cityID = ci.cityID
	LEFT JOIN PostalCode pc ON pa.postalCodeID = pc.postalCodeID
	LEFT JOIN Timezone tz ON pa.timezoneID = tz.timezoneID

	INSERT INTO app.MessageQueueLocale
	SELECT @LocaleMessageId
		,@ReadyStatusId
		,NULL
		,sysdatetime()
		,l.localeId
		,l.ownerOrgPartyID
		,abbr.traitValue
		,l.localeName
		,l.localeOpenDate
		,l.localeCloseDate
		,ltp.localeTypeID
		,l.parentLocaleID
		,isnull(lt.traitValue, 0)
		,a.addressID
		,au.AddressUsageCode
		,c.countryName
		,c.countryCode
		,t.territoryName
		,t.territoryCode
		,ci.CityName
		,pc.postalCode
		,pa.latitude
		,pa.longitude
		,pa.addressLine1
		,pa.addressLine2
		,pa.addressLine3
		,isnull(tz.timezoneCode, 'CST')
		,isnull(tz.timezoneName, '(UTC-06:00) Central Time (US & Canada)')
		,isnull(ph.traitValue, '512-477-4455')
		,NULL
		,NULL
	FROM @stores tempStores
	JOIN LocaleTrait lt ON convert(nvarchar(255), tempStores.BusinessUnitId) = lt.traitValue
		AND lt.traitID = @BusinessUnitTraitId
	JOIN Locale l ON lt.localeID = l.localeID
	JOIN LocaleType ltp ON l.localeTypeID = ltp.localeTypeID
	LEFT JOIN Locale p ON l.parentLocaleID = p.localeID
	LEFT JOIN Locale r ON p.parentLocaleID = r.localeID
	LEFT JOIN LocaleTrait abbr ON l.localeID = abbr.localeID
		AND abbr.traitID = @StoreAbbreviationTraitId
	LEFT JOIN LocaleTrait ph ON l.localeID = ph.localeID
		AND ph.traitID = @PhoneNumberTraitID
	LEFT JOIN LocaleAddress la ON l.localeID = la.localeID
	LEFT JOIN Address a ON la.addressID = a.addressID
	LEFT JOIN AddressUsage au ON la.addressUsageID = au.addressUsageID
	LEFT JOIN PhysicalAddress pa ON a.addressID = pa.addressID
	LEFT JOIN Country c ON pa.countryID = c.countryID
	LEFT JOIN Territory t ON pa.territoryID = t.territoryID
	LEFT JOIN City ci ON pa.cityID = ci.cityID
	LEFT JOIN PostalCode pc ON pa.postalCodeID = pc.postalCodeID
	LEFT JOIN Timezone tz ON pa.timezoneID = tz.timezoneID
END
