CREATE PROCEDURE [app].[UpdateItemListByTrait]
	@itemList app.ItemListByTraitType READONLY
AS

/*

We get a list of items and their traits (each different trait is a separate row),
so we update by linking the item and trait code coming in to the same fields in the ItemTrait table.

*/

declare	@updatedItems table
(
	itemID int,
	traitID int,
	primary key (itemID, traitID)
)	

declare @localeId int = (select localeID from Locale where localeName = 'Whole Foods')	

merge 
	dbo.ItemTrait it
using 
	(select * from @itemList where isNull(traitValue, '') <> '') il
on it.itemID = il.itemID
	and it.traitID = il.traitID
	and it.localeID = @localeID
when matched then
	update set 
		traitValue = il.traitValue
when not matched then
	insert (itemID, traitID, localeID, traitValue)
	values (il.itemID, il.traitID, @localeID, il.traitValue)
output inserted.itemID, inserted.traitID
into @updatedItems;

-- Return item IDs where the item's traits have changed.
select distinct itemID from @updatedItems ui

return