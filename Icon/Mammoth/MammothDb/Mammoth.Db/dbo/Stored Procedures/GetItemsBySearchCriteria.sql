CREATE PROCEDURE [dbo].[GetItemsBySearchCriteria] 
@BrandName varchar(255),
@Subteam   varchar(255),
@Supplier  varchar(255),
@ItemDescription varchar(255),
@Region nvarchar(2) = null,
@IncludeLocales bit = 0,
@IncludedStores gpm.BusinessUnitIdsType READONLY
AS
BEGIN
	DECLARE @BrandHierarchyId INT
	DECLARE @ExtraTextAttributeId INT
	DECLARE @NationalHierarchyId INT
	DECLARE @AuthorizedAttributeId INT

	CREATE TABLE #StoreIds 
	(
		BusinessUnitId int not null,
		localeId int not null
	)

	IF( (SELECT COUNT(*) FROM @IncludedStores) > 0)
	INSERT INTO #StoreIds
		SELECT a.BusinessUnitId, localeId
		FROM @IncludedStores a
		INNER JOIN locale l on l.BusinessUnitID = a.BusinessUnitId
		AND REGION = @Region

	ELSE
	BEGIN
		INSERT INTO #StoreIds
		SELECT BusinessUnitId, localeId
		FROM Locale
		WHERE REGION = @Region
	END


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

   SET @ExtraTextAttributeId =(
			SELECT AttributeID
			FROM dbo.Attributes
			WHERE AttributeCode = 'SET'
			)

--- return global information

	SELECT   I.ItemID as InforItemID
	         ,i.Desc_Product as ItemDescription
			,nutrition.Allergens as Allergens
			,hc_brand.HierarchyClassName as Brand
			,i.ScanCode as UPC
			,nutrition.Ingredients as Ingredients
			,i.RetailSize as PackageDesc1
			,i.PackageUnit as PackageDesc2
			,i.RetailUOM as PackageUnitAbbr
			,hc_cls.HierarchyClassName NationalItemClass
			,hc_cls.HierarchyClassID NationalItemClassID
			,st.Name SubTeamName
			,st.PSNumber SubTeamNumber
			,null as VendorName
			,hc_fam.HierarchyClassName as NationalFamily
			,hc_fam.HierarchyClassID as NationalFamilyId
			,hc_cat.HierarchyClassName as NationalCategory
			,hc_cat.HierarchyClassID as NationalCategoryId
			,hc_subcat.HierarchyClassName  as NationalSubCategory
			,hc_subcat.HierarchyClassID as NationalSubCategoryId
			,i.TaxClassHCID as TaxClassHCID
			,hc_tax.HierarchyClassName as TaxClassDesc
     
	    INTO #globalData 
		FROM dbo.Items i 
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
		AND hc_brand.HierarchyClassName = ISNULL(@BrandName, hc_brand.HierarchyClassName)
		AND i.Desc_Product like '%' + ISNULL(@ItemDescription, i.Desc_Product) +'%'
		AND st.Name  =   ISNULL(@Subteam, st.Name  )
	    ORDER BY InforItemID

	

	IF( @IncludeLocales = 1)
	BEGIN

		CREATE TABLE #LocaleData 
		(
			Region nchar(2),
			BusinessUnitId int,
			ItemId int ,
			Authorized bit,
			ExtraText nvarchar(max) ,
			Sign_Desc nvarchar(255),
			SupplierName nvarchar(255),
			LocaleId int 
		)

		 INSERT INTO #LocaleData(Region, BusinessUnitId, ItemId, Authorized, ExtraText, Sign_Desc, SupplierName, LocaleId)
			SELECT 
				 ila.Region as Region
				,ila.BusinessUnitId as BusinessUnitId
				,ila.ItemId as ItemId
				,ila.Authorized as Authorized
				,NULL
				,ila.Sign_Desc as SignDescription
				,ils.SupplierName as VendorName
				, si.localeId
			FROM #globalData 
			INNER JOIN [ItemLocaleAttributes] ila on  #globalData.InforItemID = ila.itemid 
			INNER JOIN #StoreIds si on si.BusinessUnitID  = ila.BusinessUnitID
				INNER JOIN [dbo].[ItemLocaleSupplier] ils on #globalData.InforItemID = ils.itemid	
			WHERE ila.Region = @Region
	
			AND ils.Region = @Region
			AND ils.SupplierName  = ISNULL(@Supplier, ils.SupplierName)

			UPDATE ld
			SET ExtraText = ilax.attributeValue
			FROM #LocaleData ld inner join [dbo].[ItemLocaleAttributesExt] ilax 
			on ld.ItemID = ilax.ItemID  AND ilax.AttributeID = @ExtraTextAttributeId AND ilax.LocaleID = ld.localeId
			WHERE ilax.Region = @Region

			SELECT   InforItemID
					,ItemDescription
					,Allergens
					,Brand
					,UPC
					,Ingredients
					,PackageDesc1
					,PackageDesc2
					,PackageUnitAbbr
					,NationalItemClass
					,NationalItemClassID
					,SubTeamName
					,SubTeamNumber
					,VendorName
					,NationalFamily
					,NationalFamilyId
					,NationalCategory
					,NationalCategoryId
					,NationalSubCategory
					,NationalSubCategoryId 
					,TaxClassHCID
					,TaxClassDesc
			FROM #globalData
			WHERE InforItemID IN (SELECT ItemID from #LocaleData)

			SELECT Region,
				   BusinessUnitId,
				   ItemId,
				   Authorized,
				   ExtraText,
				   Sign_Desc AS SignDescription,
				   SupplierName AS VendorName
			FROM #LocaleData

	OPTION (RECOMPILE)

END
ELSE
	BEGIN
	SELECT   InforItemID
			,ItemDescription
			,Allergens
			,Brand
			,UPC
			,Ingredients
			,PackageDesc1
			,PackageDesc2
			,PackageUnitAbbr
			,NationalItemClass
			,NationalItemClassID
			,SubTeamName
			,SubTeamNumber
			,VendorName
			,NationalFamily
			,NationalFamilyId
			,NationalCategory
			,NationalCategoryId
			,NationalSubCategory
			,NationalSubCategoryId 
			,TaxClassHCID
			,TaxClassDesc
	FROM #globalData

	END
	IF OBJECT_ID('tempdb..#LocaleData') IS NOT NULL
		DROP TABLE #LocaleData
	
	IF OBJECT_ID('tempdb..#StoreIds') IS NOT NULL
		DROP TABLE #StoreIds

END
GO

GRANT EXECUTE on [dbo].[GetItemsBySearchCriteria] to [MammothRole]
GO