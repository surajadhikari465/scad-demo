-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-20
-- Description:	Returns all items that do not have
--				the default tax class which is assigned
--				to the item's sub-brick.
-- =============================================

CREATE PROCEDURE app.GetDefaultTaxClassMismatches
AS
BEGIN
	set nocount on

	declare 
		@MerchDefaultMappingTraitId int = (select traitID from Trait where traitCode = 'MDT'),
		@TaxHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Tax'),
		@MerchandiseHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise'),
		@BrandHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Brands'),
		@WholeFoodsLocaleId int = (select localeID from Locale where localeName = 'Whole Foods'),
		@ProductDescriptionTraitId int = (select traitID from Trait where traitCode = 'PRD')
	
	;with ItemsWithDefaultTaxMapping as
	(
		select
			ihc.itemID as ItemId,
			ihc.hierarchyClassID as MerchandiseClass,
			convert(int, hct.traitValue) as DefaultTaxClassId
		from
			ItemHierarchyClass ihc
			join HierarchyClass hc on ihc.hierarchyClassID = hc.hierarchyClassID
			join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
		where
			hc.hierarchyID = @MerchandiseHierarchyId and
			hct.traitID = @MerchDefaultMappingTraitId
	),

	ItemsWithMismatchedTaxClass as
	(
		select
			i.ItemId,
			i.DefaultTaxClassId,
			ihc.hierarchyClassID as ActualTaxClassId
		from
			ItemsWithDefaultTaxMapping i
			join ItemHierarchyClass ihc on i.ItemId = ihc.itemID
			join HierarchyClass hc on ihc.hierarchyClassID = hc.hierarchyClassID
		where
			hc.hierarchyID = @TaxHierarchyId and
			ihc.hierarchyClassID <> i.DefaultTaxClassId
	),

	ItemBrand as
	(
		select
			ihc.itemID,
			hc.hierarchyClassName as Brand
		from
			ItemHierarchyClass ihc
			join HierarchyClass hc on ihc.hierarchyClassID = hc.hierarchyClassID
		where
			hc.hierarchyID = @BrandHierarchyId
	),
	
	ProductDescription as
	(
		select
			it.itemID,
			it.traitValue as ProductDescription
		from
			ItemTrait it
		where
			it.localeID = @WholeFoodsLocaleId and
			it.traitID = @ProductDescriptionTraitId
	),

	ItemMerchandiseLineage as
	(
		select
			ihc.itemID,
			brick.hierarchyClassName + '|' + subbrick.hierarchyClassName as MerchandiseLineage
		from
			ItemHierarchyClass ihc
			join HierarchyClass subbrick on ihc.hierarchyClassID = subbrick.hierarchyClassID
			join HierarchyClass brick on subbrick.hierarchyParentClassID = brick.hierarchyClassID
		where
			subbrick.hierarchyID = @MerchandiseHierarchyId
	),

	TaxClass as
	(
		select
			hc.hierarchyClassID,
			hc.hierarchyClassName
		from
			HierarchyClass hc
		where
			hc.hierarchyID = @TaxHierarchyId
	)
	
	select 
		sc.scanCode as ScanCode,
		b.Brand,
		prd.ProductDescription,
		m.MerchandiseLineage,
		td.hierarchyClassName + '|' + convert(nvarchar(16), td.hierarchyClassId) as DefaultTaxClass,
		ta.hierarchyClassName + '|' + convert(nvarchar(16), ta.hierarchyClassId) as TaxClassOverride
	from 
		ItemsWithMismatchedTaxClass i
		join ScanCode sc on i.ItemId = sc.itemID
		join ItemBrand b on i.ItemId = b.itemID
		join ProductDescription prd on i.ItemId = prd.itemID
		join ItemMerchandiseLineage m on i.ItemId = m.itemID
		join TaxClass td on i.DefaultTaxClassId = td.hierarchyClassID
		join TaxClass ta on i.ActualTaxClassId = ta.hierarchyClassID
END
