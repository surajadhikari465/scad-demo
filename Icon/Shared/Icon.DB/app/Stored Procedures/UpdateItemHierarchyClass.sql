CREATE PROCEDURE [app].[UpdateItemHierarchyClass]
	/*
		NOTE: The list we are getting can have item-hier-class entries for many items and any/all hierarchies
		for those items (brand, browsing, merch, tax, etc.).
	*/
	@itemList [app].ItemListByHierarchyClassType READONLY
AS

	declare @taskName varchar(32)
	select @taskName = 'iCon.UpdateItemHierarchyClass'

	/*
		For item-hierarchy-class updates, we do the following:
		1) Delete any existing entry for the target item and parent hierarchy.
		2) Add/Insert item-hier-class entry.
	*/

	declare
		@removeItemHierClassList ItemListByHierarchyClassType,
		@expectedRemoveCount int, 
		@actualRemoveCount int,
		@localeID int

	set @localeID = 1;

	/*
		Identify existing hier-class entries for all items that need to be removed.
	*/

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Identifying existing item-hier-class entries to be removed...';
	insert into @removeItemHierClassList (itemID, hierarchyClassID, localeID)
		select
			il.itemID,
			oldIHC.hierarchyClassID,
			localeID = 1
		from @itemList il  -- We use the passed list of item-hier-class entries to identify existing entries that we need to delete before we can add the new entry.
		join ItemHierarchyClass oldIHC (nolock) -- Get hier class entries for each item.
			on il.itemID = oldIHC.itemID
		join HierarchyClass currentHC (nolock) -- We need parent hier for the hier class, so we have to go through the hier class table to get hier ID.
			on oldIHC.hierarchyClassID = currentHC.hierarchyClassID
		join Hierarchy currentH (nolock) -- This gives us all hiers for hier classes linked to each item (brand, browsing, merch, etc.).
			on currentHC.hierarchyID = currentH.hierarchyID
		join HierarchyClass newHC (nolock) -- We need to lookup the new hier class we are assigning and match it to the same hier for any hier class currently linked to the item.
			on il.hierarchyClassID = newHC.hierarchyClassID
		join Hierarchy newH (nolock) -- This gives us the target parent hier for the hier class being assigned so we can remove existing entries before adding the new assignment entry.
			on newH.hierarchyID = newHC.hierarchyID
		where
			currentH.hierarchyID = newH.hierarchyID -- This links the hier, like "Brand", we are assigning to any existing hier entry for the item so we can remove it.
			and currentHC.hierarchyClassID <> newHC.hierarchyClassID -- This filters out new hierarchy classes that are already the same as the current hierarchy class

	select @expectedRemoveCount = @@rowcount

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Removing existing item-hier-class entries...';
	-- Remove existing item-hier-class entries.
	delete ItemHierarchyClass
	from ItemHierarchyClass ihc
	join @removeItemHierClassList del
		on ihc.itemID = del.itemID
		and ihc.hierarchyClassID = del.hierarchyClassID
		and isnull(ihc.localeID, 1) = isnull(del.localeID, 1)

	-- Determine rows affected to compare to what we thought should have been removed.
	select @actualRemoveCount = @@rowcount

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Verifying expected and actual item-hier-class rows removed...';
	if @actualRemoveCount <> @expectedRemoveCount
	begin
		DECLARE @ErrorMessage NVARCHAR(4000);
		select @ErrorMessage = 'The number of expected ItemHierarchyClass entries identified for removal [' + cast(@expectedRemoveCount as varchar) + ']'
		+ ' did not match the number affected rows from the DELETE attempt [' + cast(@actualRemoveCount as varchar) + ']'
		RAISERROR (
			@ErrorMessage -- Message text.
			,11 -- Severity.
			,0 -- State.
		)

	end

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding new item-hier-class entries...';
	-- Add new item-hier-class entries.
	insert into ItemHierarchyClass (itemID, hierarchyClassID, localeID)
		select il.itemID, il.hierarchyClassID, il.localeID from @itemList il
		where not exists
			(select 1 from ItemHierarchyClass ihc where ihc.itemID = il.itemID and ihc.hierarchyClassID = il.hierarchyClassID)

	-- Return item IDs where the item's hierarchy class has changed.
	declare @updatedItems table(
		itemID int,
		originalHierarchyClassID int,
		newHierarchyClassID int,
		PRIMARY KEY (itemID, originalHierarchyClassID, newHierarchyClassID))
	
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Get newly added hierarchy class IDs...';
	-- Get newly added hierarchy class IDs
	insert into @updatedItems (itemID, originalHierarchyClassID, newHierarchyClassID)
	select il.itemID, 0, il.hierarchyClassID
	from @itemList il
	where not exists 
		(select 1 from @removeItemHierClassList rihcl where rihcl.itemID = il.itemID and rihcl.hierarchyClassID = il.hierarchyClassID)
		
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Get updated hierarchy class IDs...';
	-- Get updated hierarchy class IDs
	insert into @updatedItems (itemID, originalHierarchyClassID, newHierarchyClassID)
	select il.itemID, rihcl.hierarchyClassID, il.hierarchyClassID
	from @itemList il
	join @removeItemHierClassList rihcl 
		on il.itemID = rihcl.itemID
	
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Returning item updates...';
	
	--Return updated item IDs
	select distinct itemID from @updatedItems	

return