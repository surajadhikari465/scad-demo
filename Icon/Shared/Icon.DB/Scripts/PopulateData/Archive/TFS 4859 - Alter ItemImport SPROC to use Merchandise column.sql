ALTER PROCEDURE [app].[ItemImport]
	@itemList app.ItemImportType READONLY
	,@userName nvarchar(255)
AS
/*

	We receive a list of items and their traits in a single row.
	We build one list containing all the different traits for an item on separate rows.
	This way, we only call the update once, but the list could be larger than doing one trait at a time.
	We only include entries (in the to-be-updated list) where the current and new trait values differ.
	
*/

	declare @taskName varchar(32)
	select @taskName = 'Icon.ItemImport'

	declare @itemListByTrait ItemListByTraitType
	declare @targetTraitID int
	declare @localeID int set @localeID = 1

	declare @modifiedDateTraitId int
	set @modifiedDateTraitId = (select traitID from Trait where traitCode = 'MOD')
	declare @modifiedUserTraitId int 
	set @modifiedUserTraitId = (select traitID from Trait where traitCode = 'USR')

	declare @ItemTypeId int, @RetailItem int, @DepositItem int, @ReturnItem int, @CouponItem int, @MerchandiseHierarchyId int, @NonMerchandiseTraitId int
	set @RetailItem = (select itemTypeID from ItemType where itemTypeCode = 'RTL')
	set @DepositItem = (select itemTypeID from ItemType where itemTypeCode = 'DEP')
	set @ReturnItem = (select itemTypeID from ItemType where itemTypeCode = 'RTN')
	set @CouponItem = (select itemTypeID from ItemType where itemTypeCode = 'CPN')
	set @MerchandiseHierarchyId = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise')
	set @NonMerchandiseTraitId = (select traitID from Trait where traitCode = 'NM')


	----------------------------------------------------------------------------
	----------------------------------------------------------------------------

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Determine ItemType...';
	/*
		Determines the ItemType for each scancode based on the Merchandise Hierarchy ID
	*/

	declare @ItemTypes table (ItemId int, ScanCode nvarchar(13), ItemTypeId int)

	insert into
		@ItemTypes
	select
		sc.itemID,
		il.ScanCode,
		case 
			when hct.traitValue = 'CRV' or hct.traitValue = 'Bottle Deposit' then @DepositItem
			when hct.traitValue = 'CRV Credit' or hct.traitValue = 'Bottle Return' then @ReturnItem
			when hct.traitValue = 'Coupon' then @CouponItem
			else @RetailItem
		end
	from
		@itemList						il
		inner join ScanCode				sc	on	il.ScanCode						= sc.scanCode
		left join HierarchyClass		hc	on	il.[Merchandise Hierarchy ID] = hc.hierarchyClassID 
												and hc.hierarchyID				= @MerchandiseHierarchyId
		left join HierarchyClassTrait	hct	on	hc.hierarchyClassID				= hct.hierarchyClassID 
												and hct.traitID					= @NonMerchandiseTraitId


	----------------------------------------------------------------------------
	----------------------------------------------------------------------------

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Product Description changes to item-update list...';
	/*
		Update [Product Description]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Product Description'
	insert into @itemListByTrait
		select
			sc.itemID,
			@targetTraitID,
			'Product Description',
			il.[Product Description]
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		join ItemTrait it (nolock)
			on sc.itemID = it.itemID -- Link to item trait instance.
			and it.traitID = @targetTraitID -- Link to specific trait name.
			and il.[Product Description] <> '' -- Ignore no-update entries.
			and isnull(it.traitValue,'') <> il.[Product Description] -- Only process entries where the current and new trait values differ.


	----------------------------------------------------------------------------
	----------------------------------------------------------------------------


	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding POS Description changes to item-update list...';
	/*
		Update [POS Description]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'POS Description'
	insert into @itemListByTrait
		select
			sc.itemID,
			@targetTraitID,
			'POS Description',
			il.[POS Description]
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		join ItemTrait it (nolock)
			on sc.itemID = it.itemID -- Link to item trait instance.
			and it.traitID = @targetTraitID -- Link to specific trait name.
			and il.[POS Description] <> '' -- Ignore no-update entries.
			and isnull(it.traitValue,'') <> il.[POS Description] -- Only process entries where the current and new trait values differ.


	----------------------------------------------------------------------------
	----------------------------------------------------------------------------


	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Package Unit changes to item-update list...';
	/*
		Update [Package Unit]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Package Unit'
	insert into @itemListByTrait
		select
			sc.itemID,
			@targetTraitID,
			'Package Unit',
			il.[Package Unit]
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		join ItemTrait it (nolock)
			on sc.itemID = it.itemID -- Link to item trait instance.
			and it.traitID = @targetTraitID -- Link to specific trait name.
			and il.[Package Unit] <> '' -- Ignore no-update entries.
			and isnull(it.traitValue,'') <> il.[Package Unit] -- Only process entries where the current and new trait values differ.


	----------------------------------------------------------------------------
	----------------------------------------------------------------------------


	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Food Stamp Eligible changes to item-update list...';
	/*
		Update [Food Stamp Eligible]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Food Stamp Eligible'
	insert into @itemListByTrait
		select
			sc.itemID,
			@targetTraitID,
			'Food Stamp Eligible',
			il.[Food Stamp Eligible]
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		join ItemTrait it (nolock)
			on sc.itemID = it.itemID -- Link to item trait instance.
			and it.traitID = @targetTraitID -- Link to specific trait name.
			and il.[Food Stamp Eligible] <> '' -- Ignore no-update entries.
			and isnull(it.traitValue,'') <> il.[Food Stamp Eligible] -- Only process entries where the current and new trait values differ.


	----------------------------------------------------------------------------
	----------------------------------------------------------------------------

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding POS Scale Tare changes to item-update list...';
	/*
		Update [POS Scale Tare]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'POS Scale Tare'
	insert into @itemListByTrait
		select
			sc.itemID,
			@targetTraitID,
			'POS Scale Tare',
			il.[POS Scale Tare]
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		join ItemTrait it (nolock)
			on sc.itemID = it.itemID -- Link to item trait instance.
			and it.traitID = @targetTraitID -- Link to specific trait name.
			and il.[POS Scale Tare] <> '' -- Ignore no-update entries.
			and isnull(it.traitValue,'') <> il.[POS Scale Tare] -- Only process entries where the current and new trait values differ.

	----------------------------------------------------------------------------
	----------------------------------------------------------------------------


	/*
		For item-hierarchy-class updates, we build a full list of target entries and pass that to an update procedure.
	*/

	declare
		@updateItemHierClassList ItemListByHierarchyClassType

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding brand changes to item-update list...';
	/*
		Add Brand Hier Entries
	*/
	insert into @updateItemHierClassList
		select
			sc.itemID,
			il.[Brand ID],
			localeID = @localeID -- Not yet handling locale for items.
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		where
			il.[Brand ID] <> '' -- Ignore no-update entries.

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding browsing hierarchy changes to item-update list...';
	/*
		Add Browsing Hier Entries
	*/
	insert into @updateItemHierClassList
		select
			sc.itemID,
			il.[Browsing Hierarchy ID],
			localeID = @localeID -- Not yet handling locale for items.
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		where
			il.[Browsing Hierarchy ID] <> '' -- Ignore no-update entries.

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding merchandise hierarchy changes to item-update list...';
	/*
		Add Merchandise Hier Entries
	*/
	insert into @updateItemHierClassList
		select
			sc.itemID,
			il.[Merchandise Hierarchy ID],
			localeID = @localeID -- Not yet handling locale for items.
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		where
			il.[Merchandise Hierarchy ID] <> '' -- Ignore no-update entries.

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding tax hierarchy changes to item-update list...';
	/*
		Add Tax Hier Entries
	*/
	insert into @updateItemHierClassList
		select
			sc.itemID,
			il.[Tax Class ID],
			localeID = @localeID -- Not yet handling locale for items.
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		where
			il.[Tax Class ID] <> '' -- Ignore no-update entries.


	----------------------------------------------------------------------------
	----------------------------------------------------------------------------


-- All or nothing update.
begin tran
begin try


	----------------------------------------------------------------------------
	----------------------------------------------------------------------------


	declare @updatedItemIDs app.UpdatedItemIDsType


	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Applying item-trait updates...';
	/*
	
		****** APPLY TRAIT UPDATES ******

	*/
	insert @updatedItemIDs
	exec app.UpdateItemListByTrait @itemListByTrait


	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Applying item-hierarchy updates...';
	/*
	
		****** APPLY HIERARCHY UPDATES ******

	*/
	insert @updatedItemIDs
	exec app.[UpdateItemHierarchyClass] @updateItemHierClassList

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Applying itemTypeID updates...';
	/*

		****** APPLY ITEM TYPE UPDATES ******

	*/
	update Item
	set itemTypeID = it.ItemTypeId
	from
		Item					i
		JOIN	@updatedItemIDs ui on i.itemID = ui.itemID
		JOIN	@ItemTypes		it on ui.itemID = it.ItemId

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Generating item update events...';
	/*

		****** GENERATING ITEM UPDATE EVENTS ******

	*/	
	exec app.GenerateItemUpdateEvents @updatedItemIDs


	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Generating messages for ESB...';
	/*

		****** GENERATING MESSAGES FOR ESB ******

	*/	
	exec app.GenerateItemUpdateMessages @updatedItemIDs

	print '[' + convert(nvarchar, getdate(), 121) + ']' + '[' + @taskName + ']' + 'Updating modified date and modified user...';
	/*
		
		****** UPDATING MODIFIED DATE AND MODIFIED USER ******

	*/
	update ItemTrait 
	set traitValue = convert(nvarchar(255), sysdatetime(), 121)
	from ItemTrait it
		join @updatedItemIDs i 
		on it.itemID = i.itemID
	where it.traitID = @modifiedDateTraitId
		
	update ItemTrait 
	set traitValue = @userName
	from ItemTrait it
		join @updatedItemIDs i 
		on it.itemID = i.itemID
	where it.traitID = @modifiedUserTraitId

	if @@TRANCOUNT > 0
	begin
		print '-------------------------------------------------';
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Committing ' + cast(@@TRANCOUNT as varchar) + ' transaction(s)...';
		COMMIT TRANSACTION
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Updates committed successfully.';
		print '-------------------------------------------------';
	end
	else
	begin
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'No updates to commit.';
	end
end try
begin catch
	IF @@TRANCOUNT > 0
	begin
		print '-------------------------------------------------';
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Performing transaction rollback...';
		ROLLBACK TRANSACTION
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Rollback complete.';
		print '-------------------------------------------------';
	end
	else
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Nothing to rollback.';
	end;

	throw;

end catch

