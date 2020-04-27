CREATE PROCEDURE [dbo].[UpdateBrand]
	@Brand AddUpdateBrandType readonly 
AS
BEGIN
	select * into #BrandData from @Brand 
	declare @BrandHierarchyId int 
	declare @HierarchyLevel int = 1
	declare @HierarchyParentClassId int = null
	declare @AddedBrandId int 
	declare @BrandAbbreviationTraitId int
	declare @DesignationTraitId int
	declare @ParentCompanyTraitId int
	declare @ZipCodeTraitId int 
	declare @LocalityTraitId int

	declare @BrandId int
	declare @BrandName nvarchar(255)
	declare @BrandAbbreviation nvarchar(255)
	declare @Designation nvarchar(255)
	declare @ZipCode nvarchar(255)
	declare @Locality nvarchar(255)
	declare @ParentCompany nvarchar(255)

	select @BrandAbbreviationTraitId = TraitId from Trait t where t.traitCode = 'BA'
	select @DesignationTraitId = TraitId from Trait t where t.traitCode = 'GRD'
	select @ParentCompanyTraitId = TraitId from Trait t where t.traitCode = 'PCO'
	select @ZipCodeTraitId = TraitId from Trait t where t.traitCode = 'ZIP'
	select @LocalityTraitId = TraitId from Trait t where t.traitCode = 'LCL'
	select @BrandHierarchyId = Hierarchyid from Hierarchy where hierarchyname = 'Brands'

	select @BrandId = BrandId,
		@BrandName = BrandName,
		@BrandAbbreviation = BrandAbbreviation,
		@Designation = Designation, 
		@Locality = Locality, 
		@ParentCompany = ParentCompany,
		@ZipCode = ZipCode
	from #BrandData
	
	
	Update HierarchyClass
	set hierarchyClassName = @BrandName
	where hierarchyClassID = @BrandId

	Update HierarchyClassTrait
	set traitvalue = @BrandAbbreviation
	where traitID = @BrandAbbreviationTraitId
	and HierarchyClassID = @BrandId

	if (@Designation is not null)
		if lower(@Designation) = 'remove'
			delete from HierarchyClassTrait where HierarchyClassID = @BrandId and traitid = @DesignationTraitId
		else
			if exists (select 1 from HierarchyClassTrait where HierarchyClassID = @BrandId and traitid = @DesignationTraitId)
				Update HierarchyClassTrait
				set traitvalue = @Designation
				where traitID = @DesignationTraitId
				and HierarchyClassID = @BrandId
			else 
				insert into HierarchyClassTrait (HierarchyClassID, traitID, traitValue) values (@BrandId, @DesignationTraitId, @Designation)

	if (@Locality is not null)
		if lower(@Locality) = 'remove'
			delete from HierarchyClassTrait where HierarchyClassID = @BrandId and traitid = @LocalityTraitId
		else
			if exists (select 1 from HierarchyClassTrait where HierarchyClassID = @BrandId and traitid = @LocalityTraitId)
				Update HierarchyClassTrait
				set traitvalue = @Locality
				where traitID =  @LocalityTraitId
				and HierarchyClassID = @BrandId
			else 
				insert into HierarchyClassTrait (HierarchyClassID, traitID, traitValue) values (@BrandId, @LocalityTraitId, @Locality)

	if (@ZipCode is not null)
		if lower(@ZipCode) = 'remove'
			delete from HierarchyClassTrait where HierarchyClassID = @BrandId and traitid = @ZipCodeTraitId
		else
			if exists (select 1 from HierarchyClassTrait where HierarchyClassID = @BrandId and traitid = @ZipCodeTraitId)
				Update HierarchyClassTrait
				set traitvalue = @ZipCode
				where traitID =  @ZipCodeTraitId
				and HierarchyClassID = @BrandId
			else 
				insert into HierarchyClassTrait (HierarchyClassID, traitID, traitValue) values (@BrandId, @ZipCodeTraitId, @ZipCode)

	if (@ParentCompany is not null)
		if lower(@ParentCompany) = 'remove'
			delete from HierarchyClassTrait where HierarchyClassID = @BrandId and traitid = @ParentCompanyTraitId
		else
			if exists (select 1 from HierarchyClassTrait where HierarchyClassID = @BrandId and traitid = @ParentCompanyTraitId)
				Update HierarchyClassTrait
				set traitvalue = @ParentCompany
				where traitID =  @ParentCompanyTraitId
				and HierarchyClassID = @BrandId
			else 
				insert into HierarchyClassTrait (HierarchyClassID, traitID, traitValue) values (@BrandId, @ParentCompanyTraitId, @ParentCompany)


	select @BrandId				

END
