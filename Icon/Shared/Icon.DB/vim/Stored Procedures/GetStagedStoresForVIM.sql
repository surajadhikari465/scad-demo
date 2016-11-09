CREATE PROCEDURE [vim].[GetStagedStoresForVIM]
	@Instance int,
	@FirstAttemptWaitTimeInMinute int,
	@SecondAttemptWaitTimeInMinute int,
	@ThirdAttemptWaitTimeInMinute int
AS
BEGIN
    DECLARE @RegStoreNumTraitId int;
	DECLARE @regionAbbreviationTraitId int;
	DECLARE @psBusinessUnitTraitId int;
	DECLARE @storeAbbreviationTraitId int;
	DECLARE @posTypeTraitId int;
	DECLARE @PhoneTraitId int;
	DECLARE @FaxTraitId int;
	DECLARE @LastUserTraitId int;
	DECLARE @TimeStampTraitId int;
	DECLARE @Chain365Name nvarchar(3) = '365';
	DECLARE @Chain365RegionName nvarchar(2) = 'TS'

	select @RegStoreNumTraitId = TraitId from Trait where traitDesc = 'IRMA Store ID'
	Select @regionAbbreviationTraitId = TraitId from Trait where traitDesc = 'Region Abbreviation'
	select @psBusinessUnitTraitId = TraitId from Trait where traitDesc = 'PS Business Unit ID'
	select @storeAbbreviationTraitId = TraitId from Trait where traitDesc = 'Store Abbreviation'
	select @posTypeTraitId = TraitId from Trait where traitDesc = 'Store POS Type'
	select @PhoneTraitId = TraitId from Trait where traitDesc = 'Phone Number'
	select @FaxTraitId = TraitId from Trait where traitDesc = 'Fax'
	select @LastUserTraitId = TraitId from Trait where traitDesc = 'Modified User'
	select @TimeStampTraitId = TraitId from Trait where traitDesc = 'Modified Date'

	select 
		q.QueueId,
	    q.EventTypeId,
	    q.EventMessage,
	    q.EventReferenceId,
	    q.NumberOfRetry,
		bult.traitValue as PSBU,
		stlt.traitValue as RegStoreNum,
		CASE 
			WHEN chain.localeName = @Chain365Name THEN @Chain365RegionName
			ELSE mlt.traitValue
		END as Region,
		store.LocaleName as StoreName,
		abrlt.traitValue as StoreAbbreviation,
		ptlt.traitValue as PosType,
		addr.addressLine1 as Addr1,
		addr.addressLine2 as Addr2,
		city.cityName as City,
		state.territoryCode as StateProvince,
		postalCode as PostalCode,
		country.countryCode as Country,
		phlt.traitValue as Phone,
		faxlt.traitValue as Fax,
		uslt.traitValue as LastUser,
		ISNULL(dtlt.traitValue, getdate()) as TimeStamp,
		zone.timezoneCode as TimeZone,
	    (CASE  
			WHEN store.localeCloseDate IS NOT NULL 
				THEN 'Closed'
	        ELSE (CASE 
					WHEN CONVERT(date,store.localeOpenDate) > GETDATE()
						THEN 'Pending'
		            ELSE 'Open' END)
	              END) as Status,
		(CASE et.Name 
			WHEN 'Locale Add' 
				THEN 'CREATE'
			WHEN 'Locale Update'
				THEN 'UPDATE'
		 END) as Action
    from 
		vim.EventQueue q
		join vim.EventType et on q.EventTypeId = et.EventTypeId
		left join dbo.Locale store on q.EventReferenceId = store.localeID
		left join dbo.LocaleType lt on lt.localeTypeID = store.localeTypeID
		left join Locale metro on store.parentLocaleID = metro.localeID
		left join Locale region on metro.parentLocaleID = region.localeID
		left join Locale chain on region.parentLocaleID = chain.localeID
		left join LocaleTrait stlt  on stlt.localeID = store.localeID and stlt.traitID = @RegStoreNumTraitId
		left join LocaleTrait mlt   on mlt.localeID = region.localeID and mlt.traitID = @regionAbbreviationTraitId
		left join LocaleTrait bult  on bult.localeID = store.localeID and bult.traitID = @psBusinessUnitTraitId
		left join LocaleTrait abrlt on abrlt.localeID = store.localeID and abrlt.traitID = @storeAbbreviationTraitId
		left join LocaleTrait ptlt  on ptlt.localeID = store.localeID and ptlt.traitID = @posTypeTraitId
		left join LocaleTrait phlt  on phlt.LocaleID = store.localeID and phlt.traitID = @PhoneTraitId
		left join LocaleTrait faxlt on faxlt.LocaleID = store.localeID and faxlt.traitID = @FaxTraitId
		left join LocaleTrait uslt  on uslt.LocaleID = store.localeID and uslt.traitID = @LastUserTraitId
		left join LocaleTrait dtlt  on dtlt.LocaleID = store.localeID and dtlt.traitID = @TimeStampTraitId
		left join LocaleAddress laddr on laddr.localeID = store.localeID 
		left join PhysicalAddress addr on addr.addressID = laddr.addressID
		left join City city on city.cityID = addr.cityID
		left join Territory state on state.territoryID = addr.territoryID
		left join PostalCode post on post.postalCodeID = addr.postalCodeID
		left join Country country on country.countryID = post.countryID
		left join Timezone zone on zone.timezoneID = addr.timezoneID
	where lt.localeTypeDesc = 'Store'
		and q.InProcessBy = @Instance
		and ((q.ProcessedFailedDate is Null)
			or ((q.NumberOfRetry is NULL or q.NumberOfRetry = 0) and DATEDIFF(minute, q.ProcessedFailedDate, GETDATE()) > @FirstAttemptWaitTimeInMinute)
			or (q.NumberOfRetry = 1 and DATEDIFF(minute, q.ProcessedFailedDate, GETDATE()) > @SecondAttemptWaitTimeInMinute)
			or (q.NumberOfRetry = 2 and DATEDIFF(minute, q.ProcessedFailedDate, GETDATE()) > @ThirdAttemptWaitTimeInMinute))
END