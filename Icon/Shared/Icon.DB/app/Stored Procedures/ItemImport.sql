CREATE PROCEDURE [app].[ItemImport]
	@itemList app.ItemImportType readonly,
	@userName nvarchar(255),
	@updateAllItemFields bit
AS
BEGIN

	set nocount on;

	declare @taskName varchar(32)
	select @taskName = 'Icon.ItemImport'

	declare @itemListByTrait ItemListByTraitType
	declare @targetTraitID int
	declare @localeID int = (select localeID from Locale where localeName = 'Whole Foods')
	
	declare 
		@ModifiedDateTraitId int, 
		@ModifiedUserTraitId int, 
		@ValidationDateTraitId int,
		@DepartmentSaleTraitId int,
		@HiddenItemTraitId int,
		@TaxHierarchyId int,
		@BrowsingHierarchyId int,
		@NationalHierarchyId int,
		@NotesTraitId int;

	set @ModifiedDateTraitId = (select traitID from Trait where traitCode = 'MOD')
	set @ModifiedUserTraitId = (select traitID from Trait where traitCode = 'USR')
	set @ValidationDateTraitId = (select traitID from Trait where traitCode = 'VAL')
	set @DepartmentSaleTraitId = (select traitID from Trait where traitCode = 'DPT')
	set @HiddenItemTraitId = (select traitID from Trait where traitCode = 'HID')
	set @NotesTraitId = (select traitID from Trait where traitCode = 'NTS')
	set @TaxHierarchyId = (select hierarchyID from Hierarchy where hierarchyName = 'Tax')
	set @BrowsingHierarchyId = (select hierarchyID from Hierarchy where hierarchyName = 'Browsing')
	set @NationalHierarchyId = (select hierarchyID from Hierarchy where hierarchyName = 'National')

	declare 
		@ItemTypeId int, 
		@RetailItemTypeId int, 
		@DepositItemTypeId int, 
		@ReturnItemTypeId int, 
		@CouponItemTypeId int, 
		@NonRetailItemTypeId int,
		@FeeItemTypeId int,
		@MerchandiseHierarchyId int, 
		@NonMerchandiseTraitId int

	set @RetailItemTypeId = (select itemTypeID from ItemType where itemTypeCode = 'RTL')
	set @DepositItemTypeId = (select itemTypeID from ItemType where itemTypeCode = 'DEP')
	set @ReturnItemTypeId = (select itemTypeID from ItemType where itemTypeCode = 'RTN')
	set @CouponItemTypeId = (select itemTypeID from ItemType where itemTypeCode = 'CPN')
	set @NonRetailItemTypeId = (select itemTypeID from ItemType where itemTypeCode = 'NRT')
	set @FeeItemTypeId = (select itemTypeID from ItemType where itemTypeCode = 'FEE')
	set @MerchandiseHierarchyId = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise')
	set @NonMerchandiseTraitId = (select traitID from Trait where traitCode = 'NM')
	

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
			when hct.traitValue = 'CRV' or hct.traitValue = 'Bottle Deposit' then @DepositItemTypeId
			when hct.traitValue = 'CRV Credit' or hct.traitValue = 'Bottle Return' then @ReturnItemTypeId
			when hct.traitValue = 'Coupon' then @CouponItemTypeId
			when hct.traitValue = 'Legacy POS Only' then @NonRetailItemTypeId
			when hct.traitValue = 'Non-Retail' then @NonRetailItemTypeId
			when hct.traitValue = 'Blackhawk Fee' then @FeeItemTypeId
			when hct.traitValue is null then @RetailItemTypeId
		end
	from
		@itemList						il
		inner join ScanCode				sc	on	il.ScanCode						= sc.scanCode
		inner join Item					i	on	sc.itemID						= i.itemID
		inner join HierarchyClass		hc	on	il.[Merchandise Hierarchy ID]	= hc.hierarchyClassID 
												and hc.hierarchyID				= @MerchandiseHierarchyId
		left join HierarchyClassTrait	hct	on	hc.hierarchyClassID				= hct.hierarchyClassID 
												and hct.traitID					= @NonMerchandiseTraitId


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
			and it.localeID = @localeID -- Only get Global POS Scale Tare Trait
			and il.[POS Scale Tare] <> '' -- Ignore no-update entries.
			and isnull(it.traitValue,'') <> il.[POS Scale Tare] -- Only process entries where the current and new trait values differ.

	
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Retail Size changes to item-update list...';
	/*
		Update [Retail Size]
	*/

	select @targetTraitID = traitID from Trait where traitDesc = 'Retail Size'

	insert into @itemListByTrait
		select
			sc.itemID,
			@targetTraitID,
			'Retail Size',
			il.[Retail Size]
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		join ItemTrait it (nolock)
			on sc.itemID = it.itemID -- Link to item trait instance.
			and it.traitID = @targetTraitID -- Link to specific trait name.
			and ((il.[Retail Size] is not null and il.[Retail Size] <> '') -- Ignore no-update entries.
				or (il.[Retail Size] is null)) -- Retail Size & UOM are allowed to have a null traitValue (for now), unlike every other canonical trait.
			and isnull(it.traitValue,'') <> isnull(il.[Retail Size],'') -- Only process entries where the current and new trait values differ.
			
	
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Retail UOM changes to item-update list...';
	/*
		Update [Retail UOM]
	*/

	select @targetTraitID = traitID from Trait where traitDesc = 'Retail UOM'

	insert into @itemListByTrait
		select
			sc.itemID,
			@targetTraitID,
			'Retail UOM',
			il.[Retail Uom]
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		join ItemTrait it (nolock)
			on sc.itemID = it.itemID -- Link to item trait instance.
			and it.traitID = @targetTraitID -- Link to specific trait name.
			and ((il.[Retail Uom] is not null and il.[Retail Uom] <> '') -- Ignore no-update entries.
				or (il.[Retail Uom] is null)) -- Retail Size & UOM are allowed to have a null traitValue (for now), unlike every other canonical trait.
			and isnull(it.traitValue,'') <> isnull(il.[Retail Uom],'') -- Only process entries where the current and new trait values differ.

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Alcohol By Volume changes to item-update list...';
	/*
		Update [Alcohol By Volume]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Alcohol By Volume'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Alcohol By Volume',
		il.AlcoholByVolume
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Casein Free changes to item-update list...';
	/*
		Update [Casein Free]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Casein Free'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Casein Free',
		il.CaseinFree
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Drained Weight changes to item-update list...';
	/*
		Update [Drained Weight]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Drained Weight'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Drained Weight',
		il.DrainedWeight
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Drained Weight UOM changes to item-update list...';
	/*
		Update [DrainedWeightUom]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Drained Weight UOM'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Drained Weight UOM',
		il.DrainedWeightUom
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Fair Trade Certified changes to item-update list...';
	/*
		Update [FairTradeCertified]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Fair Trade Certified'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Fair Trade Certified',
		il.FairTradeCertified
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Hemp changes to item-update list...';
	/*
		Update [Hemp]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Hemp'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Hemp',
		il.Hemp
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Local Loan Producer changes to item-update list...';
	/*
		Update [LocalLoanProducer]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Local Loan Producer'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Local Loan Producer',
		il.LocalLoanProducer
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Main Product Name changes to item-update list...';
	/*
		Update [MainProductName]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Main Product Name'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Main Product Name',
		il.MainProductName
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Nutrition Required changes to item-update list...';
	/*
		Update [NutritionRequired]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Nutrition Required'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Nutrition Required',
		il.NutritionRequired
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Organic Personal Care changes to item-update list...';
	/*
		Update [OrganicPersonalCare]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Organic Personal Care'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Organic Personal Care',
		il.OrganicPersonalCare
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Paleo changes to item-update list...';
	/*
		Update [Paleo]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Paleo'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Paleo',
		il.Paleo
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Product Flavor Type changes to item-update list...';
	/*
		Update [ProductFlavorType]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Product Flavor/Type'
	insert into @itemListByTrait
	select
		sc.itemID,
		@targetTraitID,
		'Product Flavor Type',
		il.ProductFlavorType
	from @itemList il
	join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
		on il.ScanCode = sc.scanCode
	left join ItemTrait it (nolock)
		on sc.itemID = it.itemID -- Link to item trait instance.
		and it.traitID = @targetTraitID -- Link to specific trait name.
		and it.localeID = @localeID

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Delivery System changes to item-update list...';
	/*
		Update [Delivery System]
	*/
	select @targetTraitID = traitID from Trait where traitDesc = 'Delivery System'
	insert into @itemListByTrait
		select
			sc.itemID,
			@targetTraitID,
			'Delivery System',
			il.[Delivery System]
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		left join ItemTrait it (nolock)
			on sc.itemID = it.itemID -- Link to item trait instance.
			and it.traitID = @targetTraitID -- Link to specific trait name.
			and it.localeID = @localeID

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

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding national hierarchy changes to item-update list...';
	/*
		Add National Hier Entries
	*/

	insert into @updateItemHierClassList
		select
			sc.itemID,
			il.[National Hierarchy ID],
			localeID = @localeID -- Not yet handling locale for items.
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		where
			il.[National Hierarchy ID] <> '' -- Ignore no-update entries.
			
	declare @updatedItemIDs table 
	(
		itemID int
	)
			
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Applying item-trait updates...';
	/*
		****** APPLY TRAIT UPDATES ******
	*/
	insert @updatedItemIDs
	exec app.UpdateItemListByTrait @itemList = @itemListByTrait
	
	-- Remove the traits when the value is blank or null, but only if @updateAllItemFields is true.
	delete it
	from dbo.ItemTrait it
	join @itemListByTrait ilt on it.itemID = ilt.itemID
		and it.traitID = ilt.traitID
		and it.localeID = @localeID
	where ilt.traitValue is null

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Applying item-hierarchy updates...';
	/*
		****** APPLY HIERARCHY UPDATES ******
	*/
	insert @updatedItemIDs
	exec app.[UpdateItemHierarchyClass] @itemList = @updateItemHierClassList

	delete dbo.ItemHierarchyClass
		output deleted.itemID
			into @updatedItemIDs
	from ItemHierarchyClass ihc
	join HierarchyClass hc
		on ihc.hierarchyClassID = hc.hierarchyClassID
	join Hierarchy h
		on hc.hierarchyID = h.hierarchyID
	join ScanCode sc
		on ihc.itemID = sc.itemID
	join @itemList il
		on il.ScanCode = sc.scanCode
	where (il.[Merchandise Hierarchy ID] is null and h.hierarchyID = @MerchandiseHierarchyId)
		or (il.[Tax Class ID] is null and h.hierarchyID = @TaxHierarchyId)
		or (il.[Browsing Hierarchy ID] is null and h.hierarchyID = @BrowsingHierarchyId) 
		or (il.[National Hierarchy ID] is null and h.hierarchyID = @NationalHierarchyId) 

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Validating Items...';
	/*
		****** VALIDATE ITEMS ******
	*/
		
	declare @validatedItemIDs table 
	(
		itemID int
	)
	--Only adding Validation Date traits to items that have not been validated
	insert dbo.ItemTrait(traitID, itemID, uomID, traitValue, localeID)
		output inserted.itemID
			into @validatedItemIDs
	select	@ValidationDateTraitId,
			sc.itemID,
			NULL,
			convert(nvarchar(255), sysdatetime(), 121),
			@localeID
	from	@itemList il
			join ScanCode sc
			on il.ScanCode = sc.scanCode
				and il.IsValidated = '1'
			left join ItemTrait it
			on sc.itemID = it.itemID
				and it.traitID = @ValidationDateTraitId
	where	it.traitID is null

	print '[' + convert(nvarchar, getdate(), 121) + ']' + '[' +@taskName + '] ' + 'Applying Department Sale updates...';
	/*
		****** DEPARTMENT SALE ITEMS ******
	*/

	--Add the DepartmentSale trait when the value is 1
	insert dbo.ItemTrait(traitID, itemID, uomID, traitValue, localeID)
		output inserted.itemID
			into @updatedItemIDs
	select	@DepartmentSaleTraitId,
			sc.itemID,
			NULL,
			'1',
			@localeID
	from	@itemList il
			join ScanCode sc
				on il.ScanCode = sc.scanCode
				and il.DepartmentSale = '1'
			left join ItemTrait it
				on sc.itemID = it.itemID
				and it.traitID = @DepartmentSaleTraitId
	where	it.traitID is null

	-- Remove the DepartmentSale trait when the value is 0.
	delete dbo.ItemTrait
		output deleted.itemID
			into @updatedItemIDs
	where traitID = @DepartmentSaleTraitId
			and itemID in 
			(select sc.itemID 
				from @itemList il
				join ScanCode sc
					on sc.scanCode = il.ScanCode
				where il.DepartmentSale = '0')

	print '[' + convert(nvarchar, getdate(), 121) + ']' + '[' +@taskName + '] ' + 'Applying Hidden Item updates...';
	/*
		****** HIDDEN ITEMS ******
	*/

	-- Add the HiddenItem trait when the value is 1.
	insert dbo.ItemTrait(traitID, itemID, uomID, traitValue, localeID)
	select	@HiddenItemTraitId,
			sc.itemID,
			NULL,
			'1',
			@localeID
	from	@itemList il
			join ScanCode sc
				on il.ScanCode = sc.scanCode
				and il.HiddenItem = '1'
			left join ItemTrait it
				on sc.itemID = it.itemID
				and it.traitID = @HiddenItemTraitId
	where	it.traitID is null

	-- Remove the HiddenItem trait when the value is 0.
	delete dbo.ItemTrait
	where traitID = @HiddenItemTraitId
			and itemID in 
			(select sc.itemID 
				from @itemList il
				join ScanCode sc
					on sc.scanCode = il.ScanCode
				where il.HiddenItem = '0')

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding Note changes to item-update list...';
	/*
		Update [Notes]
	*/

	update  it set it.traitValue = il.Notes
		from @itemList il
		join ScanCode sc (nolock) -- So we can pass itemID and save this lookup inside the update procedure.
			on il.ScanCode = sc.scanCode
		join ItemTrait it (nolock)
			on sc.itemID = it.itemID -- Link to item trait instance.
			and it.traitID = @NotesTraitId -- Link to specific trait name.
			and il.[Notes] is not null and il.[Notes] <> ''-- Ignore no-update entries.
			and isnull(it.traitValue,'') <> il.[Notes] -- Only process entries where the current and new trait values differ.

	-- Add the Notes if not exists.
	insert dbo.ItemTrait(traitID, itemID, uomID, traitValue, localeID)
	select	@NotesTraitId,
			sc.itemID,
			NULL,
			il.Notes,
			@localeID
	from	@itemList il
			join ScanCode sc
				on il.ScanCode = sc.scanCode
				and il.Notes <> ''
			left join ItemTrait it
				on sc.itemID = it.itemID
				and it.traitID = @NotesTraitId
	where	it.traitID is null

	-- Remove the Notes trait when the value is blank or null, but only if @updateAllItemFields is true.
	if @updateAllItemFields = 1
	begin
		delete dbo.ItemTrait
		where traitID = @NotesTraitId
				and itemID in 
				(select sc.itemID 
					from @itemList il
					join ScanCode sc
						on sc.scanCode = il.ScanCode
					where il.Notes = '' or il.Notes is null)
	end

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Update sign attributes...';
	/*
		Update [Sign attributes]
	*/

	-- For sign attribute updates, a null value indicates that no update/insert should be performed for that field.  
	-- The string '0' indicates that an existing value should be become NULL.
	
	merge
		ItemSignAttribute with (updlock, rowlock) isa
	using
		@itemList il
		join ScanCode sc on il.ScanCode = sc.scanCode
	on
		isa.itemID = sc.itemID
	when matched then
		update set
			AnimalWelfareRatingId =		case 
											when il.AnimalWelfareRatingId = '' then isa.AnimalWelfareRatingId 
											else il.AnimalWelfareRatingId 
										end,

			Biodynamic =	case 
								when il.Biodynamic = '' then isa.Biodynamic 
								when il.Biodynamic is null then 0
								else il.Biodynamic 
							end,

			CheeseMilkTypeId =	case 
									when il.CheeseMilkTypeId = '' then isa.CheeseMilkTypeId 
									else il.CheeseMilkTypeId 
								end,

			CheeseRaw =		case 
								when il.CheeseRaw = '' then isa.CheeseRaw 
								when il.CheeseRaw is null then 0
								else il.CheeseRaw 
							end,

			EcoScaleRatingId =	case 
									when il.EcoScaleRatingId = '' then isa.EcoScaleRatingId 
									else il.EcoScaleRatingId 
								end,

			GlutenFreeAgencyId =	case 
										when il.GlutenFreeAgencyId = '' then isa.GlutenFreeAgencyId 
										else il.GlutenFreeAgencyId 
									end,

			KosherAgencyId =	case 
									when il.KosherAgencyId = '' then isa.KosherAgencyId 
									else il.KosherAgencyId 
								end,

			NonGmoAgencyId =	case
									when il.NonGmoAgencyId = '' then isa.NonGmoAgencyId 
									else il.NonGmoAgencyId 
								end,

			Msc =				case 
									when il.Msc = '' then isa.Msc 
									when il.Msc is null then 0
									else il.Msc 
								end,

			OrganicAgencyId =	case 
									when il.OrganicAgencyId = '' then isa.OrganicAgencyId 
									else il.OrganicAgencyId 
								end,

			PremiumBodyCare =	case 
									when il.PremiumBodyCare = '' then isa.PremiumBodyCare 
									when il.PremiumBodyCare is null then 0
									else il.PremiumBodyCare 
								end,
			

			SeafoodFreshOrFrozenId =	case 
											when il.SeafoodFreshOrFrozenId = '' then isa.SeafoodFreshOrFrozenId 
											else il.SeafoodFreshOrFrozenId 
										end,

			SeafoodCatchTypeId =	case 
										when il.SeafoodCatchTypeId = '' then isa.SeafoodCatchTypeId 
										else il.SeafoodCatchTypeId 
									end,

			VeganAgencyId =		case 
									when il.VeganAgencyId = '' then isa.VeganAgencyId 
									else il.VeganAgencyId 
								end,

			Vegetarian =		case 
									when il.Vegetarian = '' then isa.Vegetarian 
									when il.Vegetarian is null then 0
									else il.Vegetarian 
								end,

			WholeTrade =		case 
									when il.WholeTrade = '' then isa.WholeTrade 
									when il.WholeTrade is null then 0
									else il.WholeTrade 
								end,

			GrassFed =			case 
									when il.GrassFed = '' then isa.GrassFed 
									when il.GrassFed is null then 0
									else il.GrassFed 
								end,			

			PastureRaised =		case 
									when il.PastureRaised = '' then isa.PastureRaised 
									when il.PastureRaised is null then 0
									else il.PastureRaised 
								end,

			FreeRange =			case 
									when il.FreeRange = '' then isa.FreeRange 
									when il.FreeRange is null then 0
									else il.FreeRange 
								end,

			DryAged =			case 
									when il.DryAged = '' then isa.DryAged 
									when il.DryAged is null then 0
									else il.DryAged 
								end,

			AirChilled =		case 
									when il.AirChilled = '' then isa.AirChilled 
									when il.AirChilled is null then 0
									else il.AirChilled 
								end,

			MadeInHouse =		case 
									when il.MadeInHouse = '' then isa.MadeInHouse 
									when il.MadeInHouse is null then 0
									else il.MadeInHouse 
								end
	when not matched then
		insert
			(
				itemID, 
				AnimalWelfareRatingId,
				Biodynamic,
				CheeseMilkTypeId, 
				CheeseRaw, 
				EcoScaleRatingId, 
				GlutenFreeAgencyId, 
				HealthyEatingRatingId, 
				KosherAgencyId,
				NonGmoAgencyId,
				OrganicAgencyId,
				PremiumBodyCare,
				SeafoodFreshOrFrozenId,
				SeafoodCatchTypeId,
				VeganAgencyId,
				Vegetarian,
				WholeTrade,
				GrassFed,
				PastureRaised,
				FreeRange,
				DryAged,
				AirChilled,
				MadeInHouse,
				Msc
			)
		values
			(
				itemID,
				case when il.AnimalWelfareRatingId = '' then null else il.AnimalWelfareRatingId end,
				COALESCE(Biodynamic,0),
				case when il.CheeseMilkTypeId = '' then null else il.CheeseMilkTypeId end,
				COALESCE(CheeseRaw, 0),
				case when il.EcoScaleRatingId = '' then null else il.EcoScaleRatingId end,
				case when il.GlutenFreeAgencyId = '' then null else il.GlutenFreeAgencyId end,
				null,
				case when il.KosherAgencyId = '' then null else il.KosherAgencyId end,
				case when il.NonGmoAgencyId = '' then null else il.NonGmoAgencyId end,
				case when il.OrganicAgencyId = '' then null else il.OrganicAgencyId end,
				COALESCE(PremiumBodyCare, 0),				
				case when il.SeafoodFreshOrFrozenId = '' then null else il.SeafoodFreshOrFrozenId end,
				case when il.SeafoodCatchTypeId = '' then null else il.SeafoodCatchTypeId end,
				case when il.VeganAgencyId = '' then null else il.VeganAgencyId end,
				COALESCE(Vegetarian, 0),
				COALESCE(WholeTrade, 0),
				COALESCE(GrassFed, 0),
				COALESCE(PastureRaised, 0),
				COALESCE(FreeRange, 0),
				COALESCE(DryAged, 0),
				COALESCE(AirChilled, 0),
				COALESCE(MadeInHouse, 0),
				COALESCE(Msc, 0)
			);

		
		declare @distinctUpdatedItemIDs app.UpdatedItemIDsType
		insert into @distinctUpdatedItemIDs select distinct itemID from @updatedItemIDs

		declare @distinctValidatedItemIDs app.UpdatedItemIDsType
		insert into @distinctValidatedItemIDs select distinct itemID from @validatedItemIDs

		print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Applying itemTypeID updates...';
		/*
			****** APPLY ITEM TYPE UPDATES ******
		*/
		update Item
		set itemTypeID = it.ItemTypeId
		from
			Item					i
			JOIN	@distinctUpdatedItemIDs ui on i.itemID = ui.itemID
			JOIN	@ItemTypes		it on ui.itemID = it.ItemId

		print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Generating item update events...';
		/*
			****** GENERATING ITEM UPDATE EVENTS ******
		*/	
		exec app.GenerateItemUpdateEvents @distinctUpdatedItemIDs
		exec app.GenerateItemUpdateEvents @distinctValidatedItemIDs, 'Item Validation'

		print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Generating messages for ESB...';
		/*
			****** GENERATING MESSAGES FOR ESB ******
		*/	
		exec app.GenerateItemUpdateMessages @distinctUpdatedItemIDs
		exec app.GenerateItemUpdateMessages @distinctValidatedItemIDs

		print '[' + convert(nvarchar, getdate(), 121) + ']' + '[' + @taskName + ']' + 'Updating modified date and modified user...';
		/*
			****** UPDATING MODIFIED DATE AND MODIFIED USER ******
		*/

		update ItemTrait 
		set traitValue = convert(nvarchar(255), sysdatetime(), 121)
		from ItemTrait it
			join @updatedItemIDs i 
			on it.itemID = i.itemID
		where it.traitID = @ModifiedDateTraitId
		
		update ItemTrait 
		set traitValue = @userName
		from ItemTrait it
			join @updatedItemIDs i 
			on it.itemID = i.itemID
		where it.traitID = @ModifiedUserTraitId
END