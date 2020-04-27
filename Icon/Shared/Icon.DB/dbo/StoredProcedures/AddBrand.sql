CREATE PROCEDURE [dbo].[AddBrand]
	@Brand AddUpdateBrandType readonly 
as
begin

	select * into #BrandData from @Brand 
	
	declare @BrandHierarchyId int 
	declare @HierarchyLevel int = 1
	declare @HierarchyParentClassId int = null
	declare @AddedBrandId int 
	declare @BrandAbbreviationTraitId int
	declare @DesignationTraitId int
	declare @ParentCompanyTraitId int
	declare @ZipCodeTraitId int 
	declare @Locality int

	select @BrandAbbreviationTraitId = TraitId from Trait t where t.traitCode = 'BA'
	select @DesignationTraitId = TraitId from Trait t where t.traitCode = 'GRD'
	select @ParentCompanyTraitId = TraitId from Trait t where t.traitCode = 'PCO'
	select @ZipCodeTraitId = TraitId from Trait t where t.traitCode = 'ZIP'
	select @Locality = TraitId from Trait t where t.traitCode = 'LCL'

	set @BrandHierarchyId = (select hierarchyid from Hierarchy where hierarchyname = 'Brands')

	insert into HierarchyClass (hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
	select bd.BrandName, @BrandHierarchyId, @HierarchyLevel, @HierarchyParentClassId from #BrandData bd

	select @AddedBrandId=SCOPE_IDENTITY()
	
	insert into HierarchyClassTrait (HierarchyClassID, TraitId, TraitValue)
	select @AddedBrandId, @BrandAbbreviationTraitId, bd.BrandAbbreviation from #BrandData bd

	insert into HierarchyClassTrait (HierarchyClassID, TraitId, TraitValue)
	select @AddedBrandId, @DesignationTraitId, bd.Designation from #BrandData bd
	where Designation is not null and lower(Designation) <> 'remove'

	insert into HierarchyClassTrait (HierarchyClassID, TraitId, TraitValue)
	select @AddedBrandId, @ZipCodeTraitId, bd.ZipCode from #BrandData bd
	where ZipCode is not null and lower(ZipCode) <> 'remove'

	insert into HierarchyClassTrait (HierarchyClassID, TraitId, TraitValue)
	select @AddedBrandId, @ParentCompanyTraitId, bd.ParentCompany from #BrandData bd
	where ParentCompany is not null and lower(ParentCompany) <> 'remove'

	insert into HierarchyClassTrait (HierarchyClassID, TraitId, TraitValue)
	select @AddedBrandId, @Locality, bd.Locality from #BrandData bd
	where Locality is not null and lower(Locality) <> 'remove'

	select @AddedBrandId

end