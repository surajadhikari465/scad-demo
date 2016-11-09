--Alter SPROCs and drop UDTs
ALTER PROCEDURE [app].[ItemImport]
AS
GO

DROP TYPE [app].[ItemImportType]
GO

ALTER PROCEDURE [app].[BulkItemAdd]
AS
GO

DROP TYPE [app].[ItemAddType]
GO

--Recreate the UDTs
CREATE TYPE [app].[ItemImportType] AS TABLE(
	[ScanCode] [nvarchar](13) NOT NULL,
	[Product Description] [nvarchar](255) NOT NULL,
	[POS Description] [nvarchar](255) NOT NULL,
	[Package Unit] [nvarchar](255) NOT NULL,
	[Food Stamp Eligible] [nvarchar](255) NOT NULL,
	[POS Scale Tare] [nvarchar](255) NOT NULL,
	[Retail Size] [nvarchar](10) NOT NULL,
	[Retail Uom] [nvarchar](15) NOT NULL,
	[Brand ID] [nvarchar](9) NOT NULL,
	[Browsing Hierarchy ID] [nvarchar](9) NOT NULL,
	[Merchandise Hierarchy ID] [nvarchar](9) NOT NULL,
	[Tax Class ID] [nvarchar](9) NOT NULL,
	[IsValidated] [nvarchar](1) NOT NULL
)
GO

CREATE TYPE [app].[ItemAddType] AS TABLE(
	[ScanCode] [nvarchar](13) NOT NULL,
	[ProductDescription] [nvarchar](255) NOT NULL,
	[PosDescription] [nvarchar](255) NOT NULL,
	[PackageUnit] [nvarchar](255) NOT NULL,
	[FoodStampEligible] [nvarchar](255) NOT NULL,
	[PosScaleTare] [nvarchar](255) NOT NULL,
	[RetailSize] [nvarchar](10) NOT NULL,
	[RetailUom] [nvarchar](15) NOT NULL,
	[BrandId] [nvarchar](9) NOT NULL,
	[BrandName] [nvarchar](255) NOT NULL,
	[BrowsingId] [nvarchar](9) NOT NULL,
	[MerchandiseId] [nvarchar](9) NOT NULL,
	[TaxId] [nvarchar](9) NOT NULL,
	[IsValidated] [nvarchar](1) NOT NULL
)
GO

--Alter the SPROCs
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--Bulk Item Add SPROC
ALTER PROCEDURE [app].[BulkItemAdd]
	@ItemList app.ItemAddType readonly,
	@UserName nvarchar(255)
AS
BEGIN
	
	set nocount on;

	begin tran
		begin try

			declare @TaskName varchar(32) = 'New Item Import'
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Beginning new item import...';

			-- Chapter 1: Create new Item & ScanCode entries.

			-- Set up variables for the different Item and ScanCode types.
			declare @ItemTypeId int, @RetailItem int, @DepositItem int, @ReturnItem int, @CouponItem int, @MerchandiseHierarchyId int, @NonMerchandiseTraitId int, @Upc int, @PosPlu int, @ScalePlu int
			set @RetailItem = (select itemTypeID from ItemType where itemTypeCode = 'RTL')
			set @DepositItem = (select itemTypeID from ItemType where itemTypeCode = 'DEP')
			set @ReturnItem = (select itemTypeID from ItemType where itemTypeCode = 'RTN')
			set @CouponItem = (select itemTypeID from ItemType where itemTypeCode = 'CPN')
			set @MerchandiseHierarchyId = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise')
			set @NonMerchandiseTraitId = (select traitID from Trait where traitCode = 'NM')
			set @Upc = (select scanCodeTypeId from ScanCodeType where scanCodeTypeDesc = 'UPC')
			set @PosPlu = (select scanCodeTypeId from ScanCodeType where scanCodeTypeDesc = 'POS PLU')
			set @ScalePlu = (select scanCodeTypeId from ScanCodeType where scanCodeTypeDesc = 'Scale PLU')

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Determine ItemType...';

			declare @ItemTypes table (ScanCode nvarchar(13), ItemTypeId int)

			insert into
				@ItemTypes
			select
				il.ScanCode,
				case 
					when hct.traitValue = 'CRV' or hct.traitValue = 'Bottle Deposit' then @DepositItem
					when hct.traitValue = 'CRV Credit' or hct.traitValue = 'Bottle Return' then @ReturnItem
					when hct.traitValue = 'Coupon' then @CouponItem
					else @RetailItem
				end
			from
				@ItemList il
				left join HierarchyClass hc			on il.MerchandiseId = hc.hierarchyClassID and hc.hierarchyID = @MerchandiseHierarchyId
				left join HierarchyClassTrait hct	on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID = @NonMerchandiseTraitId

			
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Create new itemIDs...';

			-- This temp table mirrors the ScanCode table, and ultimately will be used to insert into ScanCode.
			declare @NewScanCodeEntries table (ItemId int, ScanCode nvarchar(13), ScanCodeTypeId int, LocaleId int)
			declare @DefaultScanCodeLocaleId int = 1

			merge into 
				dbo.Item i
			using 
				@ItemList il join @ItemTypes it on il.ScanCode = it.ScanCode on 0 = 1
			when not matched
				then
					insert (itemTypeID)
					values (it.ItemTypeID)
			output
				Inserted.itemID,
				il.ScanCode,
				case 
					when len(il.ScanCode) <= 6 then @PosPlu
					when len(il.ScanCode) = 11 and il.ScanCode like '2%00000' then @ScalePlu
					else @Upc
				end,
				@DefaultScanCodeLocaleId
			into
				@NewScanCodeEntries;
	
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Insert the new scan codes...';

			-- All the information needed to create the ScanCode entries is now available, so we can insert directly from the temp table.
			insert into	ScanCode select	* from @NewScanCodeEntries

			-- If any of the newly inserted scan codes were PLUs, then they also need to be inserted into the PLU mapping table.
			insert into 
				app.PLUMap (itemID)
			select
				nsc.ItemId
			from
				@NewScanCodeEntries	nsc
			where
				nsc.ScanCodeTypeId in (@ScalePlu, @PosPlu)



			-- Chapter 2: Create the item traits.

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Preparing to add item traits...';

			-- Create the canonical item traits for each new item, plus the Insert Date trait.
			declare @ProductDescription int, @PosDescription int, @PackageUnit int, 
					@FoodStampEligible int, @PosScaleTare int, @RetailSize int, 
					@RetailUom int, @InsertDate int, @ModifiedDate int, @ModifiedUser int,
					@ValidationDate int
			set @ProductDescription = (select traitID from Trait where traitCode = 'PRD')
			set @PosDescription = (select traitID from Trait where traitCode = 'POS')
			set @PackageUnit = (select traitID from Trait where traitCode = 'PKG')
			set @FoodStampEligible = (select traitID from Trait where traitCode = 'FSE')
			set @PosScaleTare = (select traitID from Trait where traitCode = 'SCT')
			set @RetailSize = (select traitID from Trait where traitCode = 'RSZ')
			set @RetailUom = (select traitID from Trait where traitCode = 'RUM')
			set @InsertDate = (select traitID from Trait where traitCode = 'INS')
			set @ModifiedDate = (select traitID from Trait where traitCode = 'MOD')
			set @ModifiedUser = (select traitID from Trait where traitCode = 'USR')
			set @ValidationDate = (select traitID from Trait where traitCode = 'VAL')

			declare @NewItemTraitEntries table (TraitId int, ItemId int, UomId nvarchar(5), TraitValue nvarchar(255), LocaleId int)

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Product Description...';

			-- Product Description.
			insert into 
				@NewItemTraitEntries
			select
				@ProductDescription,
				nsc.ItemId,
				null,
				il.ProductDescription,
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add POS Description...';

			-- POS Description.
			insert into 
				@NewItemTraitEntries
			select
				@PosDescription,
				nsc.ItemId,
				null,
				il.PosDescription,
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Package Unit...';

			-- Package Unit.
			insert into 
				@NewItemTraitEntries
			select
				@PackageUnit,
				nsc.ItemId,
				null,
				il.PackageUnit,
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Food Stamp Eligible...';

			-- Food Stamp Eligible.
			insert into 
				@NewItemTraitEntries
			select
				@FoodStampEligible,
				nsc.ItemId,
				null,
				il.FoodStampEligible,
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add POS Scale Tare...';

			-- POS Scale Tare is optional for the user and may be passed in as an empty string, but we still want the item trait to be created.  For those cases, insert zero.
			insert into 
				@NewItemTraitEntries
			select
				@PosScaleTare,
				nsc.ItemId,
				null,
				case
					when il.PosScaleTare = '' then 0 else il.PosScaleTare 
				end,
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Retail Size...';

			-- Retail Size
			insert into 
				@NewItemTraitEntries
			select
				@RetailSize,
				nsc.ItemId,
				null,
				il.RetailSize,
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Retail UOM...';

			-- Retail UOM
			insert into 
				@NewItemTraitEntries
			select
				@RetailUom,
				nsc.ItemId,
				null,
				il.RetailUom,
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Insert Date...';

			-- Insert Date.
			insert into 
				@NewItemTraitEntries
			select
				@InsertDate,
				nsc.ItemId,
				null,
				convert(nvarchar(255), sysdatetime(), 121),
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Modified Date...';

			-- Modified Date.
			insert into 
				@NewItemTraitEntries
			select
				@ModifiedDate,
				nsc.ItemId,
				null,
				null,
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Modified User...';
			
			-- Modified User.
			insert into 
				@NewItemTraitEntries
			select 
				@ModifiedUser,
				nsc.ItemId,
				null,
				@UserName,
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode
				
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Validation Date...';
			
			-- Validation Date.
			insert into 
				@NewItemTraitEntries
			select
				@ValidationDate,
				nsc.ItemId,
				null,
				convert(nvarchar(255), sysdatetime(), 121),
				1
			from
				@NewScanCodeEntries nsc
				join @ItemList il on nsc.ScanCode = il.ScanCode
				where il.IsValidated = '1'

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Insert all new item trait entries...';

			insert into ItemTrait select * from @NewItemTraitEntries



			-- Chapter 3: Create new ItemHierarchyClass entries.

			-- It may happen that the user unknowingly enters an existing brand name, but appends |0 as if it were a new brand.  We'll try to catch those
			-- cases here.
			declare @NotReallyNewBrands table (BrandId int, BrandName nvarchar(35))
			insert into
				@NotReallyNewBrands
			select
				hc.hierarchyClassId,
				il.BrandName
			from
				@ItemList il
				join HierarchyClass hc on il.BrandName = hc.hierarchyClassName
				join Hierarchy h on hc.hierarchyID = h.hierarchyID and h.hierarchyName = 'Brands'
			where
				il.BrandId = 0

			-- It's possible that the input data contains new brands, so we need to figure out if that's the case for this set.
			declare @NewBrands table (BrandId int, BrandName nvarchar(255))
	
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Check for new brands...';

			insert into
				@NewBrands (BrandName)
			select distinct
				il.BrandName		
			from
				@ItemList il
				join @NewScanCodeEntries nsc on il.ScanCode = nsc.ScanCode
				left join HierarchyClass hc on il.BrandName = hc.hierarchyClassName
				left join Hierarchy h on hc.hierarchyID = h.hierarchyID and h.hierarchyName = 'Brands'
			where
				hc.hierarchyClassName is null
				and il.BrandName <> ''
		
			-- Now create new HierarchyClass entries for these new brands.
			if exists (select BrandName from @NewBrands)
				begin

					print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Insert new brands...';

					declare @BrandHierarchyLevel int = 1, @BrandHierarchyParentClassId int = null, @BrandHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Brands')

					insert into 
						HierarchyClass
					select
						@BrandHierarchyLevel,
						@BrandHierarchyId,
						@BrandHierarchyParentClassId,
						BrandName
					from
						@NewBrands

					-- Associate the new hierarchyClassIds with the new items.
					update
						@NewBrands
					set
						BrandId = hierarchyClassId
					from
						@NewBrands nb
						join HierarchyClass hc on nb.BrandName = hc.hierarchyClassName
						join Hierarchy h on hc.hierarchyID = h.hierarchyID and h.hierarchyName = 'Brands'

					-- All new hierarchy classes need to have the "Sent to ESB" hierarchy class trait.  Initial state is null.
					declare @SentToEsbTraitId int = (select traitID from Trait where traitCode = 'ESB')
					insert into
						HierarchyClassTrait
					select
						@SentToEsbTraitId,
						nb.BrandId,
						null,
						null
					from
						@NewBrands nb
				end	
		
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Insert all item-brand associations...';

			-- This should get us to a point where we can insert all new item/brand relationships.
			declare @ItemHierarchyClassDefaultLocaleId int = 1
			insert into
				ItemHierarchyClass
			select
				nsc.ItemId,
				case
					when il.BrandId = 0 and nnb.BrandName is null then nb.BrandId 
					when il.BrandId = 0 and nnb.BrandName is not null then nnb.BrandId
					else il.BrandId
				end,
				@ItemHierarchyClassDefaultLocaleId
			from
				@ItemList il
				join @NewScanCodeEntries nsc on il.ScanCode = nsc.ScanCode
				left join @NewBrands nb on il.BrandName = nb.BrandName
				left join @NotReallyNewBrands nnb on il.BrandName = nnb.BrandName

			-- Next, process the merchandise, tax, and browsing associations.  These are optional, so we'll need to check for empty strings (in which case no record will
			-- be created in ItemHierarchyClass).  Additionally, for this data, the user must choose values that already exist, so we can assume the HierarchyClass entries
			-- will be there.

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Insert all item-merchandise associations...';

			-- Merchandise.
			insert into
				ItemHierarchyClass
			select
				nsc.ItemId,
				il.MerchandiseId,
				@ItemHierarchyClassDefaultLocaleId
			from
				@ItemList il
				join @NewScanCodeEntries nsc on il.ScanCode = nsc.ScanCode
			where
				il.MerchandiseId <> ''

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Insert all item-tax associations...';

			-- Tax.
			insert into
				ItemHierarchyClass
			select
				nsc.ItemId,
				il.TaxId,
				@ItemHierarchyClassDefaultLocaleId
			from
				@ItemList il
				join @NewScanCodeEntries nsc on il.ScanCode = nsc.ScanCode
			where
				il.TaxId <> ''

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Insert all item-browsing associations...';

			-- Browsing.
			insert into
				ItemHierarchyClass
			select
				nsc.ItemId,
				il.BrowsingId,
				@ItemHierarchyClassDefaultLocaleId
			from
				@ItemList il
				join @NewScanCodeEntries nsc on il.ScanCode = nsc.ScanCode
			where
				il.BrowsingId <> ''



			-- Chapter 4: Generate messages to ESB.

			-- If any new brands were created as a result of the import, then we have to create Hierarchy messages to the ESB.

			if exists (select BrandName from @NewBrands)
				begin

					print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Generate new brand messages to ESB...';

					declare @HierarchyMessageTypeId int, @ReadyStatusId int, @MessageActionId int, @BrandsHierarchyId int,
							@BrandsHierarchyLevelName nvarchar(16), @BrandsItemsAttached bit, @BrandsHierarchyLevel int, @BrandsParentClassId int

					set @HierarchyMessageTypeId = (select MessageTypeId from app.MessageType where MessageTypeName = 'Hierarchy')
					set @ReadyStatusId = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')
					set @MessageActionId = (select MessageActionId from app.MessageAction where MessageActionName = 'AddOrUpdate')
					set @BrandsHierarchyId = (select hierarchyID from Hierarchy where hierarchyName = 'Brands')
					set @BrandsHierarchyLevelName = (select hierarchyLevelName from HierarchyPrototype where hierarchyID = @BrandsHierarchyId)
					set @BrandsItemsAttached = (select itemsAttached from HierarchyPrototype where hierarchyID = @BrandsHierarchyId)
					set @BrandsHierarchyLevel = (select hierarchyLevel from HierarchyPrototype where hierarchyID = @BrandsHierarchyId)
					set @BrandsParentClassId = null
			
					insert into
						app.MessageQueueHierarchy
					select
						MessageTypeId			= @HierarchyMessageTypeId,
						MessageStatusId			= @ReadyStatusId,
						MessageHistoryId		= null,
						MessageActionId			= @MessageActionId,
						InsertDate				= sysdatetime(),
						HierarchyId				= @BrandsHierarchyId,
						HierarchyName			= 'Brands',
						HierarchyLevelName		= @BrandsHierarchyLevelName,
						ItemsAttached			= @BrandsItemsAttached,
						HierarchyClassId		= nb.BrandId,
						HierarchyClassName		= nb.BrandName,
						HierarchyLevel			= @BrandsHierarchyLevel,
						HierarchyParentClassId	= @BrandsParentClassId,
						null,
						null
					from
						@NewBrands nb		
				end

			
			-- Epilogue: If any of these newly added scan codes match a scan code in New Item (app.IRMAItem), remove them.

			delete
				IrmaItem
			from
				app.IRMAItem	IrmaItem
				join @ItemList	il			on	IrmaItem.Identifier = il.ScanCode

			if @@trancount > 0
				begin
					print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Committing ' + cast(@@trancount as varchar) + ' transaction(s)...';
					commit transaction
					print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Transaction committed successfully.';
				end
			else
				begin
					print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'No active transaction found.';
				end
		end try
		begin catch
			if @@trancount > 0
				begin
					print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Performing transaction rollback...';
					rollback transaction
					print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Rollback complete.';
				end
			else
				begin
					print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'No active transaction found.';
				end;

			throw;
			
		end catch

END

GO

--Item Import SPROC
ALTER PROCEDURE [app].[ItemImport]
	@itemList app.ItemImportType readonly,
	@userName nvarchar(255)
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

	declare @ModifiedDateTraitId int, @ModifiedUserTraitId int, @ValidationDateTraitId int
	set @ModifiedDateTraitId = (select traitID from Trait where traitCode = 'MOD')
	set @ModifiedUserTraitId = (select traitID from Trait where traitCode = 'USR')
	set @ValidationDateTraitId = (select traitID from Trait where traitCode = 'VAL')

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
			and il.[Retail Size] <> '' -- Ignore no-update entries.
			and isnull(it.traitValue,'') <> il.[Retail Size] -- Only process entries where the current and new trait values differ.
			
	----------------------------------------------------------------------------
	----------------------------------------------------------------------------
	
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
			and il.[Retail Uom] <> '' -- Ignore no-update entries.
			and isnull(it.traitValue,'') <> il.[Retail Uom] -- Only process entries where the current and new trait values differ.
			
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

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Validating Items...';
	/*
	
		****** VALIDATE ITEMS ******

	*/

	--Only adding Validation Date traits to items that have not been validated
	insert ItemTrait(traitID, itemID, uomID, traitValue, localeID)
		output inserted.itemID
			into @updatedItemIDs
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
	where it.traitID = @ModifiedDateTraitId
		
	update ItemTrait 
	set traitValue = @userName
	from ItemTrait it
		join @updatedItemIDs i 
		on it.itemID = i.itemID
	where it.traitID = @ModifiedUserTraitId

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

GO