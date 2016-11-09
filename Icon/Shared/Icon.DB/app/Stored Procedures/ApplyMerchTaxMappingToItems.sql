-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-16
-- Description:	Applies an updated merchandise/tax
--				mapping to all items associated to
--				the specified sub-brick.
-- =============================================

CREATE PROCEDURE app.ApplyMerchTaxMappingToItems
	@MerchandiseClassId int,
	@TaxClassId int
AS
BEGIN
	set nocount on

	declare @TaxHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Tax')
	declare @MerchTaxTraitId int = (select traitID from Trait where traitCode = 'MDT')
	declare @CurrentMappedTaxClass int = (select cast(traitValue as int) from HierarchyClassTrait where traitID = @MerchTaxTraitId and hierarchyClassID = @MerchandiseClassId)

	declare @UpdatedItems app.UpdatedItemIDsType

    ;with AssociatedItems as
	(
		select
			ihc.itemID
		from
			ItemHierarchyClass ihc
		where
			ihc.hierarchyClassID = @MerchandiseClassId			
	),

	NonOverridenItems as
	(
		select
			ihc.itemID
		from
			AssociatedItems i
			join ItemHierarchyClass ihc on i.itemID = ihc.itemID
			join HierarchyClass hc on ihc.hierarchyClassID = hc.hierarchyClassID
		where
			hc.hierarchyID = @TaxHierarchyId and
			ihc.hierarchyClassID = isnull(@CurrentMappedTaxClass, ihc.hierarchyClassID)
	)

	update
		ItemHierarchyClass
	set
		hierarchyClassID = @TaxClassId
	output
		inserted.itemID into @UpdatedItems
	from
		NonOverridenItems i
		join ItemHierarchyClass ihc on i.itemID = ihc.itemID
		join HierarchyClass hc on ihc.hierarchyClassID = hc.hierarchyClassID
	where
		hc.hierarchyID = @TaxHierarchyId

	exec app.GenerateItemUpdateMessages @UpdatedItems
	exec app.GenerateItemUpdateEvents @UpdatedItems
END
