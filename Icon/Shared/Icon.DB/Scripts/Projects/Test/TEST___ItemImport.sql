/*
Tom Lux
2014.02.19

This is a simple test script to exercise the user-defined table type for the item import process
and the proc that performs that import.

This script:
1) Adds a star to the beginning of the Product Description trait
2) Saves the first hier class from each hier to all items
...for a select subset of items.

Overall Process:
1) Load target items into local table var, including the new POS Desc that will be applied (add star if not there, remove if star found).
2) Display the updates to be applied (local table var).
3) Call import procedure.
4) Display the targeted items/traits/hiers, hopefully using a slightly different query method to help confirm the updates were applied correctly.

** NOTE **
No changes are saved.

*/

-- We'll roll everything back at the end.
begin transaction

-- We need a username to pass into import proc.
declare @UserName nvarchar(255)
select @UserName = 'import.test.luxury'
-- Create special-typed instance of table to pass to import proc.
declare @ItemList ItemImportType

declare @prodDescFilter varchar(128)
select @prodDescFilter = '%LUXURY%'


-- Load local import table with some data.
insert into @ItemList
	select
		ScanCode = prd.scanCode, 
		[Product Description] = prd.traitValue, 
		[POS Description] = pod.traitValue, 
		[Package Unit] = pu.traitValue, 
		[Food Stamp Eligible] = '0', 
		[Tare] = '1',
		[Brand ID] = ihc.hierarchyClassID,
		-- We need to populate these remaining hiers with additional visits to hier tables.
		[Browsing Hierarchy ID] = '',
		[Merchandising Hierarchy ID] = '',
		[Tax Class ID] = ''
	from vItemsAndTraits prd
	join vItemsAndTraits pod
		on prd.itemID = pod.itemID
		and prd.traitDesc = 'Product Description'
		and pod.traitDesc = 'POS Description'
	join vItemsAndTraits pu
		on pod.itemID = pu.itemID
		and pu.traitDesc = 'Package Unit'
	join ItemHierarchyClass ihc (nolock)
		on prd.itemID = ihc.itemID
	join HierarchyClass hc (nolock)
		on ihc.hierarchyClassID = hc.hierarchyClassID
	join Hierarchy h (nolock)
		on hc.hierarchyID = h.hierarchyID
		and h.hierarchyName like 'brand'
	where
		prd.traitValue like @prodDescFilter

-- Find browsing hier class IDs.
update @ItemList
set [Browsing Hierarchy ID] = ihc.hierarchyClassID
from @ItemList il
join ScanCode sc
on il.ScanCode = sc.scanCode
join ItemHierarchyClass ihc (nolock)
	on sc.itemID = ihc.itemID
join HierarchyClass hc (nolock)
	on ihc.hierarchyClassID = hc.hierarchyClassID
join Hierarchy h (nolock)
	on hc.hierarchyID = h.hierarchyID
	and h.hierarchyName like 'browsing'

-- Find merch hier class IDs.
update @ItemList
set [Merchandising Hierarchy ID] = ihc.hierarchyClassID
from @ItemList il
join ScanCode sc
on il.ScanCode = sc.scanCode
join ItemHierarchyClass ihc (nolock)
	on sc.itemID = ihc.itemID
join HierarchyClass hc (nolock)
	on ihc.hierarchyClassID = hc.hierarchyClassID
join Hierarchy h (nolock)
	on hc.hierarchyID = h.hierarchyID
	and h.hierarchyName like 'merchandising'

-- Find tax hier class IDs.
update @ItemList
set [Tax Class ID] = ihc.hierarchyClassID
from @ItemList il
join ScanCode sc
on il.ScanCode = sc.scanCode
join ItemHierarchyClass ihc (nolock)
	on sc.itemID = ihc.itemID
join HierarchyClass hc (nolock)
	on ihc.hierarchyClassID = hc.hierarchyClassID
join Hierarchy h (nolock)
	on hc.hierarchyID = h.hierarchyID
	and h.hierarchyName like 'tax'


-- Show current data values.
select
	note = 'Current data:', 
	username = @UserName, 
	sc.itemID,
	il.* 
from @ItemList il
join ScanCode sc on il.ScanCode = sc.scanCode
order by [Product Description]


if not exists (
	select [Product Description]
	from @ItemList 
	where [Product Description] like '*' + @prodDescFilter
)
begin
	-- Insert * in Product Desc.
	update @ItemList
	set [Product Description] = '*' + [Product Description]
end
else
begin
	-- Remove * from Product Desc.
	update @ItemList
	set [Product Description] = substring([Product Description], 2, len([Product Description]))
	where [Product Description] like '*' + @prodDescFilter
end


declare
	@targetBrandID int, @targetBrandName nvarchar(255),
	@targetBrowsingID int, @targetBrowsingName nvarchar(255),
	@targetMerchID int, @targetMerchName nvarchar(255),
	@targetTaxID int, @targetTaxName nvarchar(255)

select top 1 @targetBrandID = hc.hierarchyClassID, @targetBrandName = hc.hierarchyClassName
from Hierarchy h (nolock)
join HierarchyClass hc (nolock)
	on hc.hierarchyID = h.hierarchyID
	and h.hierarchyName like 'brand'

select top 1 @targetBrowsingID = hc.hierarchyClassID, @targetBrowsingName = hc.hierarchyClassName
from Hierarchy h (nolock)
join HierarchyClass hc (nolock)
	on hc.hierarchyID = h.hierarchyID
	and h.hierarchyName like 'browsing'

select top 1 @targetMerchID = hc.hierarchyClassID, @targetMerchName = hc.hierarchyClassName
from Hierarchy h (nolock)
join HierarchyClass hc (nolock)
	on hc.hierarchyID = h.hierarchyID
	and h.hierarchyName like 'merchandising'

select top 1 @targetTaxID = hc.hierarchyClassID, @targetTaxName = hc.hierarchyClassName
from Hierarchy h (nolock)
join HierarchyClass hc (nolock)
	on hc.hierarchyID = h.hierarchyID
	and h.hierarchyName like 'tax'

select
	targetBrandID = @targetBrandID, name = @targetBrandName,
	targetBrandID = @targetBrowsingID, name = @targetBrowsingName,
	targetMerchID = @targetMerchID, name = @targetMerchName,
	targetTaxID = @targetTaxID, name = @targetTaxName

update @ItemList
set
	[Brand ID] = @targetBrandID,
	[Browsing Hierarchy ID] = @targetBrowsingID,
	[Merchandising Hierarchy ID] = @targetMerchID,
	[Tax Class ID] = @targetTaxID

-- Show data to be imported.
select
	note = 'Data To Be Applied:', 
	username = @UserName, 
	* 
from @ItemList
order by [Product Description]


-- Call import.
exec ItemImport @ItemList, @username


-- Attempt to retrieve the same set of data that was pulled into our local import table above, but access it a different way (in case View or update have issues).
select
	'desc' = 'updated item traits',
	*
FROM
	item i
JOIN scancode scn
	ON i.itemid = scn.itemid
JOIN itemtrait it
	ON i.itemid = it.itemid
JOIN trait t
	ON it.traitID = t.traitID
where t.traitDesc like 'product description'
and it.traitValue like @prodDescFilter
order by it.traitValue


/*
	Show all hierarchies for target items (one item-hier per row, so could be multiple rows per item).
	All items should have the same value for each hier.
*/
select
	'desc' = 'item hier',
	sc.itemid, 
	sc.scancode, 
	productDesc = it.traitValue,
	h.hierarchyid,
	h.hierarchyname,
	hc.hierarchyLevel,
	hc.hierarchyclassid,
	hc.hierarchyclassname,
	ihc.localeid
from @ItemList il
join scancode sc on il.ScanCode = sc.scanCode
join itemtrait it on sc.itemid = it.itemid
join trait t on it.traitID = t.traitID and t.traitDesc like 'product description'
join itemhierarchyclass ihc on sc.itemid = ihc.itemid
join hierarchyclass hc on ihc.hierarchyclassid = hc.hierarchyclassid
join hierarchy h on hc.hierarchyid = h.hierarchyid
order by it.traitValue, sc.scanCode, h.hierarchyName


-- Don't really save anything...
rollback
