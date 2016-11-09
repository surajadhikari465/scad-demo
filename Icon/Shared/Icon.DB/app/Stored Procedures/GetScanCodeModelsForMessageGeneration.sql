
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-10-30
-- Description:	Called from the POS Push Controller
--				to cache a group of IRMA identifiers
--				which will be used to generated ItemLocale
--				and Price messages for ESB.
-- =============================================

CREATE PROCEDURE [app].[GetScanCodeModelsForMessageGeneration]
	@Identifiers app.ScanCodeListType readonly
AS
BEGIN
	
	set nocount on;
	
	declare
		@ValidationDateTraitId int = (select traitID from Trait where traitCode = 'VAL'),
		@MerchandiseHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise'),
		@NonMerchandiseTraitId int = (select traitID from Trait where traitCode = 'NM'),
		@DepartmentSaleTraitId int = (select traitID from Trait where traitCode = 'DPT')

	select
		ScanCode = sc.scanCode,
		ScanCodeId = sc.scanCodeID,
		ScanCodeTypeId = sct.scanCodeTypeID,
		ScanCodeTypeDesc = sct.scanCodeTypeDesc,
		ItemId = i.itemID,
		ItemTypeCode = ity.itemTypeCode,
		ItemTypeDesc = ity.itemTypeDesc,
		ValidationDate = val.traitValue,
		DepartmentSaleTrait = dpt.traitValue,
		NonMerchandiseTrait = hct.traitValue
	from
		@Identifiers					ii
		join ScanCode					sc	on	ii.ScanCode				= sc.scanCode
		join ScanCodeType				sct on	sc.scanCodeTypeID		= sct.scanCodeTypeID
		join Item						i	on	sc.itemID				= i.itemID
		join ItemType					ity	on	i.itemTypeID			= ity.itemTypeID
		left join ItemTrait				val	on	i.itemID				= val.itemID and val.traitID = @ValidationDateTraitId
		left join ItemTrait				dpt	on	i.itemID				= dpt.itemID and dpt.traitID = @DepartmentSaleTraitId
		join ItemHierarchyClass			ihc	on	i.itemID				= ihc.itemID
		join HierarchyClass				hc	on	ihc.hierarchyClassID	= hc.hierarchyClassID and hc.hierarchyID = @MerchandiseHierarchyId
		left join HierarchyClassTrait	hct	on	hc.hierarchyClassID		= hct.hierarchyClassID and hct.traitID = @NonMerchandiseTraitId

END
GO
