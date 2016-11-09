CREATE PROCEDURE [app].[BulkItemAdd]
	@ItemList app.ItemAddType readonly,
	@UserName nvarchar(255)
AS
BEGIN
	
	set nocount on;
			
	declare @TaskName varchar(32) = 'New Item Import'
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Beginning new item import...';

	-- Chapter 1: Create new Item & ScanCode entries.

	-- Set up variables for the different Item and ScanCode types.
	declare 
		@RetailItemTypeId int = (select itemTypeID from ItemType where itemTypeCode = 'RTL'), 
		@DepositItemTypeId int = (select itemTypeID from ItemType where itemTypeCode = 'DEP'), 
		@ReturnItemTypeId int = (select itemTypeID from ItemType where itemTypeCode = 'RTN'), 
		@CouponItemTypeId int = (select itemTypeID from ItemType where itemTypeCode = 'CPN'), 
		@NonRetailItemTypeId int = (select itemTypeID from ItemType where itemTypeCode = 'NRT'), 
		@FeeItemTypeId int = (select itemTypeID from ItemType where itemTypeCode = 'FEE'), 
		@MerchandiseHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise'), 
		@NonMerchandiseTraitId int = (select traitID from Trait where traitCode = 'NM'), 
		@Upc int = (select scanCodeTypeId from ScanCodeType where scanCodeTypeDesc = 'UPC'), 
		@PosPlu int = (select scanCodeTypeId from ScanCodeType where scanCodeTypeDesc = 'POS PLU'), 
		@ScalePlu int = (select scanCodeTypeId from ScanCodeType where scanCodeTypeDesc = 'Scale PLU')

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Determine ItemType...';

	declare @ItemTypes table (ScanCode nvarchar(13), ItemTypeId int)

	insert into
		@ItemTypes
	select
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
	declare 
		@ProductDescription int = (select traitID from Trait where traitCode = 'PRD'),
		@PosDescription int = (select traitID from Trait where traitCode = 'POS'), 
		@PackageUnit int = (select traitID from Trait where traitCode = 'PKG'), 
		@FoodStampEligible int = (select traitID from Trait where traitCode = 'FSE'), 
		@PosScaleTare int = (select traitID from Trait where traitCode = 'SCT'), 
		@RetailSize int = (select traitID from Trait where traitCode = 'RSZ'), 
		@RetailUom int = (select traitID from Trait where traitCode = 'RUM'), 
		@DeliverySystem int = (select traitID from Trait where traitCode = 'DS'), 
		@InsertDate int = (select traitID from Trait where traitCode = 'INS'), 
		@ModifiedDate int = (select traitID from Trait where traitCode = 'MOD'), 
		@ModifiedUser int = (select traitID from Trait where traitCode = 'USR'),
		@ValidationDate int = (select traitID from Trait where traitCode = 'VAL'),
		@AlcoholByVolume int = (select traitID from Trait where traitCode = 'ABV'),
		@CaseinFree int = (select traitID from Trait where traitCode = 'CF'),
		@DrainedWeight int = (select traitID from Trait where traitCode = 'DW'),
		@DrainedWeightUom int = (select traitID from Trait where traitCode = 'DWU'),
		@FairTradeCertified int = (select traitID from Trait where traitCode = 'FTC'),
		@Hemp int = (select traitID from Trait where traitCode = 'HEM'),
		@LocaleLoanProducer int = (select traitID from Trait where traitCode = 'LLP'),
		@MainProductName int = (select traitID from Trait where traitCode = 'MPN'),
		@NutritionRequired int = (select traitID from Trait where traitCode = 'NR'),
		@OrganicPersonalCare int = (select traitID from Trait where traitCode = 'OPC'),
		@Paleo int = (select traitID from Trait where traitCode = 'PLO'),
		@ProductFlavorType int = (select traitID from Trait where traitCode = 'PFT')

	declare @NewItemTraitEntries table 
	(
		TraitId int, 
		ItemId int, 
		UomId nvarchar(5), 
		TraitValue nvarchar(255), 
		LocaleId int
	)

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

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Delivery System...';

	-- Delivery System
	insert into 
		@NewItemTraitEntries
	select
		@DeliverySystem,
		nsc.ItemId,
		null,
		il.DeliverySystem,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Alcohol By Volume...';

	-- Alcohol By Volume
	insert into 
		@NewItemTraitEntries
	select
		@AlcoholByVolume,
		nsc.ItemId,
		null,
		il.AlcoholByVolume,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.AlcoholByVolume is not null and il.AlcoholByVolume <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Casein Free...';

	-- Casein Free
	insert into 
		@NewItemTraitEntries
	select
		@CaseinFree,
		nsc.ItemId,
		null,
		il.CaseinFree,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.CaseinFree is not null and il.CaseinFree <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Drained Weight...';

	-- Drained Weight
	insert into 
		@NewItemTraitEntries
	select
		@DrainedWeight,
		nsc.ItemId,
		null,
		il.DrainedWeight,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.DrainedWeight is not null and il.DrainedWeight <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Drained Weight UOM...';

	-- Drained Weight UOM
	insert into 
		@NewItemTraitEntries
	select
		@DrainedWeightUom,
		nsc.ItemId,
		null,
		il.DrainedWeightUom,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.DrainedWeightUom is not null and il.DrainedWeightUom <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Fair Trade Certified...';

	-- Fair Trade Certified
	insert into 
		@NewItemTraitEntries
	select
		@FairTradeCertified,
		nsc.ItemId,
		null,
		il.FairTradeCertified,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.FairTradeCertified is not null and il.FairTradeCertified <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Hemp...';

	-- Hemp
	insert into 
		@NewItemTraitEntries
	select
		@Hemp,
		nsc.ItemId,
		null,
		il.Hemp,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.Hemp is not null and il.Hemp <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Local Loan Producer...';

	-- Local Loan Producer
	insert into 
		@NewItemTraitEntries
	select
		@LocaleLoanProducer,
		nsc.ItemId,
		null,
		il.LocalLoanProducer,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.LocalLoanProducer is not null and il.LocalLoanProducer <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Main Product Name...';

	-- Main Product Name
	insert into 
		@NewItemTraitEntries
	select
		@MainProductName,
		nsc.ItemId,
		null,
		il.MainProductName,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.MainProductName is not null and il.MainProductName <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Nutrition Required...';

	-- Nutrition Required
	insert into 
		@NewItemTraitEntries
	select
		@NutritionRequired,
		nsc.ItemId,
		null,
		il.NutritionRequired,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.NutritionRequired is not null and il.NutritionRequired <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Organic Personal Care...';

	-- Organic Personal Care
	insert into 
		@NewItemTraitEntries
	select
		@OrganicPersonalCare,
		nsc.ItemId,
		null,
		il.OrganicPersonalCare,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.OrganicPersonalCare is not null and il.OrganicPersonalCare <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Paleo...';

	-- Paleo
	insert into 
		@NewItemTraitEntries
	select
		@Paleo,
		nsc.ItemId,
		null,
		il.Paleo,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.Paleo is not null and il.Paleo <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Add Product Flavor Type...';

	-- Product Flavor Type
	insert into 
		@NewItemTraitEntries
	select
		@ProductFlavorType,
		nsc.ItemId,
		null,
		il.ProductFlavorType,
		1
	from
		@NewScanCodeEntries nsc
		join @ItemList il on nsc.ScanCode = il.ScanCode
	where
		il.ProductFlavorType is not null and il.ProductFlavorType <> ''

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
	where 
		il.IsValidated = '1'

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Insert all new item trait entries...';

	insert into ItemTrait 
	select * from @NewItemTraitEntries

			
	-- Save the ones that were validated so that we can generate product messages later on.
	declare @updatedItems app.UpdatedItemIDsType

	insert into 
		@updatedItems
	select 
		distinct ItemId 
	from 
		@NewItemTraitEntries


	-- Chapter 3: Create ItemSignAttribute values.

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Inserting into ItemSignAttribute...';

	insert into
		ItemSignAttribute ([ItemID]
           ,[AnimalWelfareRatingId]
           ,[Biodynamic]
           ,[CheeseMilkTypeId]
           ,[CheeseRaw]
           ,[EcoScaleRatingId]
           ,[GlutenFreeAgencyId]
           ,[HealthyEatingRatingId]
           ,[KosherAgencyId]
           ,[Msc]
           ,[NonGmoAgencyId]
           ,[OrganicAgencyId]
           ,[PremiumBodyCare]
           ,[SeafoodFreshOrFrozenId]
           ,[SeafoodCatchTypeId]
           ,[VeganAgencyId]
           ,[Vegetarian]
           ,[WholeTrade]
           ,[GrassFed]
           ,[PastureRaised]
           ,[FreeRange]
           ,[DryAged]
           ,[AirChilled]
           ,[MadeInHouse])
	select
		nsc.ItemId,
		il.AnimalWelfareRatingId,
		COALESCE(il.Biodynamic, 0),
		il.CheeseMilkTypeId,
		COALESCE(il.CheeseRaw, 0),
		il.EcoScaleRatingId,
		il.GlutenFreeAgencyId,
		null,
		il.KosherAgencyId,
		COALESCE(il.Msc, 0),
		il.NonGmoAgencyId,
		il.OrganicAgencyId,
		COALESCE(il.PremiumBodyCare, 0),
		il.SeafoodFreshOrFrozenId,
		il.SeafoodCatchTypeId,
		il.VeganAgencyId,
		COALESCE(il.Vegetarian, 0),
		COALESCE(il.WholeTrade, 0),
		COALESCE(il.GrassFed, 0),
		COALESCE(il.PastureRaised, 0),
		COALESCE(il.FreeRange, 0),
		COALESCE(il.DryAged, 0),
		COALESCE(il.AirChilled, 0),
		COALESCE(il.MadeInHouse, 0)
	from
		@ItemList il
		join @NewScanCodeEntries nsc on il.ScanCode = nsc.ScanCode


	-- Chapter 4: Create new ItemHierarchyClass entries.

	-- It may happen that the user unknowingly enters an existing brand name, but appends |0 as if it were a new brand.  We'll try to catch those
	-- cases here.
	declare @brandHierarchyId int = (select hierarchyID from dbo.Hierarchy where hierarchyName = 'Brands')
	declare @NotReallyNewBrands table (BrandId int, BrandName nvarchar(35))

	insert into
		@NotReallyNewBrands
	select distinct
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
			and hc.hierarchyID = @brandHierarchyId
	where
		hc.hierarchyClassName is null
		and il.BrandName <> ''
		
	-- Now create new HierarchyClass entries for these new brands.
	if exists (select BrandName from @NewBrands)
		begin

			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Insert new brands...';

			declare @BrandHierarchyLevel int = 1, @BrandHierarchyParentClassId int = null

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
			
			declare @mammothEventTypeId int, @hierarchyClassList mammoth.BulkImportedHierarchyClassType

			select @mammothEventTypeId = EventTypeId from mammoth.EventType where name = 'HierarchyClassAdd'
			insert into @hierarchyClassList (HierarchyClassId, HierarchyClassName, MammothEventTypeId)
				 select BrandId, BrandName, @mammothEventTypeId from @NewBrands

			exec mammoth.BulkImportHierarchyClassMammothEvents @hierarchyClassList, @BrandHierarchyId, @BrandHierarchyLevel, 'Brand'

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

	-- Next, process the merchandise, tax, browsing and national class associations.  These are optional, so we'll need to check for empty strings (in which case no record will
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

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @TaskName + '] ' + 'Insert all item-national class associations...';

	-- National.
	insert into
		ItemHierarchyClass
	select
		nsc.ItemId,
		il.NationalClassId,
		@ItemHierarchyClassDefaultLocaleId
	from
		@ItemList il
		join @NewScanCodeEntries nsc on il.ScanCode = nsc.ScanCode
	where
		il.NationalClassId <> ''

	-- Chapter 5: Generate messages to ESB.

	-- Generate events and product messages. The GenerateItemUpdateMessages and GenerateItemUpdateEvents stored procedure will take care of the validation status
			
	if exists (select itemID from @updatedItems)
		begin
			exec app.GenerateItemUpdateMessages @updatedItems
			exec app.GenerateItemUpdateEvents @updatedItems, 'Item Validation'
			exec app.GenerateItemSubTeamEvents @updatedItems
		end
			

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
END