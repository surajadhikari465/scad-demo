/*
Options:
Put our global-setup values (that don't exist in IRMA) in a temp table and insert the values that are not there.
Thinking this could help for rerunning script, but if there are already some values in the DB, it makes the solution more fuzzy.


use [icon.db]
select * from partytype
select * from party
select * from organizationtype -- optional
select * from organizationname -- can generate many names for an org
select * from organization -- optional type, parent id, and desc
select * from localetype
select * from locale
select * from localetrait
select * from store

go
:setvar DBLink "[idd-mw\mwd].[itemcatalog_test].[dbo].[store]"
select top 10 * from $(DBLink)


select top 3 * from [idd-mw\mwd].[itemcatalog_test].[dbo].[store]


select store_name from [idd-mw\mwd].[itemcatalog_test].[dbo].[store]


*/

use [Icon.DB]


declare @targetPartyType varchar(100) = 'LuxOrg'
declare @targetPartyID int
exec [Init.PopData.SetupParty] @targetPartyType, @targetPartyID output

select '@targetPartyID' = @targetPartyID

exec [Init.PopData.SetupOrganization] 'Lux Foods', @targetPartyID

-- TODO: ERROR CHECKING?

-- check org
select [desc] = 'New org entry', * from Organization where orgPartyID = @targetPartyID

declare @targetOrgPartyID int
select @targetOrgPartyID = orgPartyID from Organization where orgPartyID = @targetPartyID

exec [Init.PopData.SetupLocale] @targetOrgPartyID

-- view data
select
	'Tables In This View' = 'Party::PartyType::Org::Locale'
	,Party = 'Party ->'
	,p.*
	,PartyType = 'PartyType ->'
	,pt.*
	,Organization = 'Organization ->'
	,o.*
	,Locale = 'Locale ->'
	,l.*
from Party p
join PartyType pt
	on p.partyTypeCode = pt.partyTypeCode
join Organization o
	on p.partyID = o.orgPartyID
join Locale l
	on o.orgPartyID = l.ownerOrgPartyID


