CREATE PROCEDURE [app].[PDX_LocationHierarchyFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

DECLARE @localeIds TABLE
(
  localeId int
)

DECLARE @chainName nvarchar(255) = '365'
DECLARE @RegionAbbrTraitId int

select @RegionAbbrTraitId = traitId from Trait where traitDesc = 'Region Abbreviation'

insert into @localeIds
select store.localeID
from Locale store
join Locale metro on store.parentLocaleID = metro.localeID
join Locale region on metro.parentLocaleID = region.localeID
join Locale chain on region.parentLocaleID = chain.localeID
where store.localeTypeID = 4
  and chain.localeName = @chainName;

--insert into @localeIds
--select localeID from Locale
--where localeName = 'BelMar';

WITH Chain
AS (
       select localeID, localeName from Locale
       where parentLocaleID is null
       ),
BuzUnit
AS (
       select lt.localeID, traitValue, parentLocaleID
         from LocaleTrait lt
         join Trait t on lt.traitId = t.traitId
		 join Locale l on lt.localeID = l.localeID
       where t.traitDesc = 'PS Business Unit ID'
          and lt.localeID in (select localeId from @localeIds)
       ),
Metro
AS (
       select localeId, localeName, parentLocaleID
         from Locale l
         join LocaleType lt on l.localeTypeID = lt.localeTypeID
       where lt.localeTypeDesc = 'Metro'
       ),
Region
AS (
       select l.localeId, parentLocaleID
         from Locale l
         join LocaleType lt on l.localeTypeID = lt.localeTypeID
       where lt.localeTypeDesc = 'Region'
       ),
StoreAddress
AS (
       select la.localeID, pa.addressLine1, pa.addressLine2, cityName, t.territoryCode, pc.postalCode
         from LocaleAddress la
         join PhysicalAddress pa on la.addressID = pa.addressID
         join City c on pa.cityID = c.cityID
         join Territory t on t.territoryID = pa.territoryID
         join PostalCode pc on pc.postalCodeID = pa.postalCodeID
       where la.localeID in (select localeId from @localeIds)
       )
select bu.traitValue as STORE_NUMBER, l.localeName as LOCATION_LABEL, m.localeID as METRO_ID, m.localeName as METRO_LABEL, r.localeID as REGION_ID, IsNull(ltt.traitValue, 'TS')  as REGION_ABBR,
       sa.addressLine1 as LOCATION_ADDR, sa.addressLine2 as LOCATION_ADDR2, sa.cityName as LOCATION_CITY, sa.territoryCode as STATE_PROVINCE,
       sa.postalCode as POSTAL_CODE, c.localeID as CHAIN_ID, c.localeName as CHAIN_LABEL
  from Locale l
  join BuzUnit bu on l.localeID = bu.localeId
  join Metro m on l.parentLocaleID = m.localeID
  join StoreAddress sa on sa.localeID = l.localeID
  join Region r on m.parentLocaleID = r.localeID
  join Chain c on c.localeID = r.parentLocaleID
  left join LocaleTrait ltt on ltt.localeID = r.localeID and ltt.traitID = @RegionAbbrTraitId
END

