CREATE PROCEDURE [dbo].[GetItems] @ScanCodes gpm.ScanCodesType READONLY
AS
BEGIN
	DECLARE @BrandHierarchyId INT
	DECLARE @ExtraTextAttributeId INT
	DECLARE @NationalHierarchyId INT
	DECLARE @AuthorizedAttributeId INT

	SELECT ScanCode
	INTO #GetItemScanCodes
	FROM @ScanCodes

	SET @AuthorizedAttributeId = (
			SELECT attributeId
			FROM Attributes
			WHERE AttributeCode = 'NA'
			)
	SET @NationalHierarchyId = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'National'
			)
	SET @BrandHierarchyId = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Brands'
			)
	SET @ExtraTextAttributeId = (
			SELECT AttributeID
			FROM attributes
			WHERE AttributeCode = 'SET'
			)


	create table #GetItemItemIds 
	(
		ItemId int not null
	)

	insert into #GetItemItemIds
	select ItemID from dbo.Items i inner join #GetItemScanCodes gisc on i.ScanCode = gisc.ScanCode

	create nonclustered index ix_GetItems_ItemIds on #GetItemItemIds (ItemID)

	
	create table #RegionalItemInformation
	(
		Region varchar(2) not null, 
		BusinessUnitId int not null,
		ItemId int not null,
		Authorized bit null, 
		Sign_Desc nvarchar(255) null,
		SupplierName nvarchar(255) null, 
		HasData bit null
	)

	insert into #RegionalItemInformation (Region ,BusinessUnitId, ItemId)
	select l.Region, l.BusinessUnitID, i.ItemId from dbo.Locale l cross apply #GetItemItemIds i

	update #RegionalItemInformation
	set Sign_Desc = ila.Sign_Desc, 
		Authorized = ila.Authorized
	FROM dbo.ItemLocaleAttributes ila inner join #GetItemItemIds i on ila.ItemID = i.ItemId 
	where i.ItemId = #RegionalItemInformation.ItemId and ila.BusinessUnitID = #RegionalItemInformation.BusinessUnitId
	
	update #RegionalItemInformation
	set SupplierName = ils.SupplierName
	from dbo.ItemLocaleSupplier ils inner join #GetItemItemIds i on ils.ItemID = i.ItemId
	where i.ItemId = #RegionalItemInformation.ItemId and ils.BusinessUnitID = #RegionalItemInformation.BusinessUnitId

	update #RegionalItemInformation
	set HasData = case when Authorized is null and Sign_Desc is null and SupplierName is null then 0 else 1 end

	
	create nonclustered index ix_GetItems_HasData on #RegionalItemInformation (hasdata)
	

		
	SELECT I.ItemID
		,I.BrandHCID
		,hc_brand.HierarchyClassName BrandName
		,i.ScanCode
		,i.RetailSize
		,i.PackageUnit
		,i.RetailUOM
		,st.PSNumber SubTeamNumber
		,st.Name SubTeamName
		,hc_fam.HierarchyClassName FamilyName
		,hc_fam.HierarchyClassID FamilyId
		,hc_cat.HierarchyClassName CategoryName
		,hc_cat.HierarchyClassID CategoryId
		,hc_subcat.HierarchyClassName SubCatName
		,hc_subcat.HierarchyClassID SubCatId
		,hc_cls.HierarchyClassName ClassName
		,hc_cls.HierarchyClassID ClassId
		,nutrition.Allergens
		,nutrition.Ingredients
		,hc_tax.HierarchyClassID TaxClassId
		,hc_tax.HierarchyClassName TaxClassDesc,
		i.HierarchyNationalClassID
	FROM #GetItemItemIds iid
	INNER JOIN dbo.Items i ON i.ItemID = iid.ItemId
	INNER JOIN dbo.HierarchyClass hc_brand ON i.BrandHCID = hc_brand.HierarchyClassID
	INNER JOIN dbo.Financial_SubTeam st ON i.PSNumber = st.PSNumber
	LEFT JOIN dbo.Hierarchy_NationalClass hnc ON i.HierarchyNationalClassID = hnc.HierarchyNationalClassID
	LEFT JOIN dbo.HierarchyClass hc_fam ON hnc.FamilyHCID = hc_fam.HierarchyClassID
		AND hc_fam.HIERARCHYID = @NationalHierarchyId
	LEFT JOIN dbo.HierarchyClass hc_cat ON hnc.CategoryHCID = hc_cat.HierarchyClassID
		AND hc_cat.HIERARCHYID = @NationalHierarchyId
	LEFT JOIN dbo.HierarchyClass hc_subcat ON hnc.SubcategoryHCID = hc_subcat.HierarchyClassID
		AND hc_subcat.HIERARCHYID = @NationalHierarchyId
	LEFT JOIN dbo.HierarchyClass hc_cls ON hnc.ClassHCID = hc_cls.HierarchyClassID
		AND hc_cls.HIERARCHYID = @NationalHierarchyId
	LEFT JOIN dbo.HierarchyClass hc_tax ON hc_tax.HierarchyClassID = i.TaxClassHCID
	LEFT JOIN dbo.ItemAttributes_Nutrition nutrition ON nutrition.ItemID = i.ItemID
	WHERE hc_brand.HIERARCHYID = @BrandHierarchyId



	select Region, BusinessUnitId, ItemId,Authorized, Sign_Desc, SupplierName from #RegionalItemInformation where hasdata = 1 order by Region, BusinessUnitId

	IF OBJECT_ID('tempdb..#GetItemScanCodes') IS NOT NULL
		DROP TABLE #GetItemScanCodes
	IF OBJECT_ID('tempdb..#GetItemItemIds') IS NOT NULL
		DROP TABLE #GetItemItemIds
	IF OBJECT_ID('tempdb..#RegionalItemInformation') IS NOT NULL
		DROP TABLE #RegionalItemInformation

END
GO

GRANT EXECUTE on [dbo].[GetItems] to [MammothRole]
GO

