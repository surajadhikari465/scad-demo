/*
Tom Lux
2014.02.24
Updated:
2014.03.02 - Lux - Tested update of proc to apply NULL to regional map value, if '0' is passed.  Fixed to ensure scancode doesn't overflow.  Set dummy empty values to NULL instead of ''.

How to run this test:
1. Edit the @posDescFilter value so that it returns a few items, such as below:
		declare @posDescFilter varchar(128)
		select @posDescFilter = 'TEST PLU 600%'
		select * from vitemimport where [pos description] like @posDescFilter
This gives you some "real" scan codes to work with in the test and map to dummy regional values, but
this does not guarantee the scan codes are PLUs, so be careful.

--------------------------------------------------------------------------------------
*** WARNING *** --> If you set your filter to return real PLUs that are already mapped, they will be overwritten in this script.
--------------------------------------------------------------------------------------

2. Run the script.

What does it do?
It adds a prefix to the target scan codes and uses the dummy value as regional PLU mappings and updates or inserts into
the mapping table.

*/

-- We need a username to pass into import proc.
declare @UserName nvarchar(255)
select @UserName = 'import.test.luxury'
-- Create special-typed instance of table to pass to import proc.
declare @ItemList PLUMapImportType

declare @posDescFilter varchar(128)
select @posDescFilter = '%luxury%'


-- Load local import table with some data.
insert into @ItemList
	select
		nationalPLU = pod.scanCode
		,flPLU = left('1' + pod.scanCode, 5)
		,maPLU = left('2' + pod.scanCode, 5)
		,mwPLU = left('3' + pod.scanCode, 5)
		,naPLU = left('4' + pod.scanCode, 5)
		,ncPLU = '0' -- This should insert NULL if nat plu doesn't exist in mapping table.
		,nePLU = '' -- This should insert NULL if nat plu doesn't exist in mapping table.
		,pnPLU = null
		,rmPLU = left('5' + pod.scanCode, 5)
		,soPLU = left('6' + pod.scanCode, 5)
		,spPLU = left('7' + pod.scanCode, 5)
		,swPLU = left('8' + pod.scanCode, 5)
		,ukPLU = left('9' + pod.scanCode, 5)
		from vItemsAndTraits pod
	where
		pod.traitDesc = 'POS Description'
		and
		pod.traitValue like @posDescFilter
		and
		len(pod.scanCode) < 12 -- Since we're just grabbing any item, not just PLUs, we must make sure they are 11 digits or less, so overflow into PLUMapImportType doesn't occur.


-- Show data to be imported.
select note = 'Data To Be Applied:', username = @UserName, * from @ItemList

-- Call import.
exec PLUMapImport @ItemList, @username

-- Attempt to retrieve the same set of data that was pulled into our local import table above, but access it a different way (in case View or update have issues).
select
	note = 'First Import:'
	,sc.scancode
	,pm.*
from @ItemList il
join ScanCode sc on il.nationalPLU = sc.scanCode
join vNatPLUMap pm on sc.itemID = pm.itemID


-- Update passed data to test behavior when a '0' or '' is passed for an existing entry.
update @ItemList
set
	mwPLU = '0',
	naPLU = '',
	spPLU = '0',
	swPLU = ''

-- Call import.
exec PLUMapImport @ItemList, @username

-- Show data again.  Regional PLUs should be cleared if '0' was passed.
select
	note = 'Import of ''0'' and '''':'
	,sc.scancode
	,pm.*
from @ItemList il
join ScanCode sc on il.nationalPLU = sc.scanCode
join vNatPLUMap pm on sc.itemID = pm.itemID


------------------------------------------------------------

/*

-- Delete Helper
declare @posDescFilter varchar(128)
select @posDescFilter = '%luxury%'

delete plumap
--select *
from plumap p
join vItemsAndTraits pod
	on p.itemid = pod.itemid
where
	pod.traitDesc = 'POS Description'
	and
	pod.traitValue like @posDescFilter
	and
	len(pod.scanCode) < 12 



*/

/*
~~EXAMPLE RESULTS~~
note			scancode	scanCode	itemID	flPLU	maPLU	mwPLU	naPLU	ncPLU	nePLU	pnPLU	rmPLU	soPLU	spPLU	swPLU	ukPLU
First Import:	68222301501	68222301501	106156	16822	26822	36822	46822	NULL	NULL	NULL	56822	66822	76822	86822	96822
First Import:	8453282107	8453282107	369111	18453	28453	38453	48453	NULL	NULL	NULL	58453	68453	78453	88453	98453
First Import:	89951900164	89951900164	888515	18995	28995	38995	48995	NULL	NULL	NULL	58995	68995	78995	88995	98995

note					scancode	scanCode	itemID	flPLU	maPLU	mwPLU	naPLU	ncPLU	nePLU	pnPLU	rmPLU	soPLU	spPLU	swPLU	ukPLU
Import of '0' and '':	8453282107	8453282107	369111	18453	28453	NULL	48453	NULL	NULL	NULL	58453	68453	NULL	88453	98453
Import of '0' and '':	89951900164	89951900164	888515	18995	28995	NULL	48995	NULL	NULL	NULL	58995	68995	NULL	88995	98995
Import of '0' and '':	68222301501	68222301501	106156	16822	26822	NULL	46822	NULL	NULL	NULL	56822	66822	NULL	86822	96822

*/