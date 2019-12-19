CREATE VIEW dbo.ItemView AS 
	SELECT 
		i.ItemId AS ItemId,
		i.ItemTypeId AS ItemTypeId,
		it.ItemTypeCode as ItemTypeCode,
		it.ItemTypeDesc as ItemTypeDescription,
		sc.scanCode AS ScanCode,
		sc.scanCodeTypeID AS ScanCodeTypeId,
		sct.scanCodeTypeDesc as ScanCodeTypeDescription,
		sc.BarcodeTypeId AS barcodeTypeId,
		bt.BarcodeType as BarcodeType,
		merchandiseHierarchy.hierarchyClassID AS MerchandiseHierarchyClassId,
		brandsHierarchy.hierarchyClassID AS BrandsHierarchyClassId,
		taxHierarchy.hierarchyClassID AS TaxHierarchyClassId,
		financialHierarchy.hierarchyClassID AS FinancialHierarchyClassId,
		nationalHierarchy.hierarchyClassID AS NationalHierarchyClassId,
		manufacturerHierarchy.hierarchyClassID AS ManufacturerHierarchyClassId,
		i.ItemAttributesJson AS ItemAttributesJson,
		i.Inactive as Inactive,
		brandsHierarchy.hierarchyLineage as Brand,
		merchandiseHierarchy.HierarchyLineage as Merchandise,
		taxHierarchy.HierarchyLineage as Tax,
		nationalHierarchy.HierarchyLineage as NationalClass,
		financialHierarchy.HierarchyLineage as Financial,
		manufacturerHierarchy.HierarchyLineage as Manufacturer

	FROM Item i
	JOIN ScanCode sc on i.ItemId = sc.itemID
	JOIN ScanCodeType sct on sct.scanCodeTypeID = sc.scanCodeTypeID
	JOIN BarcodeType bt on bt.BarcodeTypeId = sc.BarcodeTypeId
	JOIN ItemType it on it.itemTypeID = i.ItemTypeId
	CROSS APPLY (
			SELECT 
				ihc.itemID
				,hclv5.HIERARCHYID AS HIERARCHYID
				,hclv5.hierarchyClassID AS HierarchyClassId
				,hclv5.hierarchyClassName AS HierarchyClassName
				,hclv5.hierarchyLevel AS HierarchyLevel
				,hclv1.hierarchyClassName + ' | ' + hclv2.hierarchyClassName + ' | ' + hclv3.hierarchyClassName + ' | ' + hclv4.hierarchyClassName + ' | ' + hclv5.hierarchyClassName + ': ' + nch.hierarchyClassName AS HierarchyLineage
				,hclv2.hierarchyParentClassID AS HierarchyParentClassId
			FROM ItemHierarchyClass ihc 
			JOIN HierarchyClass hclv5 on hclv5.hierarchyClassID = ihc.hierarchyClassID
			JOIN HierarchyClass hclv4 ON hclv5.hierarchyParentClassID = hclv4.hierarchyClassID
			JOIN HierarchyClass hclv3 ON hclv4.hierarchyParentClassID = hclv3.hierarchyClassID
			JOIN HierarchyClass hclv2 ON hclv3.hierarchyParentClassID = hclv2.hierarchyClassID
			JOIN HierarchyClass hclv1 ON hclv2.hierarchyParentClassID = hclv1.hierarchyClassID
			JOIN HierarchyClassTrait hct ON hclv5.hierarchyClassID = hct.hierarchyClassID
				AND hct.traitID = (
					SELECT traitID
					FROM Trait
					WHERE TraitCode = 'MFM'
					)
			JOIN dbo.HierarchyClass nch ON nch.hierarchyClassID = hct.traitValue
			JOIN Hierarchy h ON hclv5.hierarchyID = h.hierarchyID
			WHERE h.hierarchyName = 'Merchandise' AND ihc.itemId = i.ItemId 
		) merchandiseHierarchy
	CROSS APPLY  (
			SELECT 
				ihc.itemID,
				ihc.hierarchyClassId,
				hc.hierarchyClassName,
				hc.hierarchyClassName AS HierarchyLineage
			FROM ItemHierarchyClass ihc 
			JOIN HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
			JOIN Hierarchy h ON hc.hierarchyID = h.hierarchyID
			WHERE h.hierarchyName = 'Brands' AND ihc.itemId = i.ItemId 
		) brandsHierarchy 
	CROSS APPLY  (
			SELECT 
				ihc.itemID,
				ihc.hierarchyClassId,
				hc.hierarchyClassName,
				hc.hierarchyClassName AS HierarchyLineage
			FROM ItemHierarchyClass ihc 
			JOIN HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
			JOIN Hierarchy h ON hc.hierarchyID = h.hierarchyID
			WHERE h.hierarchyName = 'Tax' AND ihc.itemId = i.ItemId 
		) taxHierarchy
	CROSS APPLY  (
			SELECT 
				ihc.itemID,
				ihc.hierarchyClassId,
				hc.hierarchyClassName,
				hc.hierarchyClassName AS HierarchyLineage
			FROM ItemHierarchyClass ihc 
			JOIN HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
			JOIN Hierarchy h ON hc.hierarchyID = h.hierarchyID
			WHERE h.hierarchyName = 'Financial' AND ihc.itemId = i.ItemId 
		) financialHierarchy 
	CROSS APPLY  (
			SELECT 
				ihc.itemID
				,ihc.hierarchyClassId
				,hclv4.HIERARCHYID AS HIERARCHYID
				,hclv4.hierarchyClassName AS HierarchyClassName
				,hclv4.hierarchyLevel AS HierarchyLevel
				,hclv1.hierarchyClassName + ' | ' + hclv2.hierarchyClassName + ' | ' + hclv3.hierarchyClassName + ' | ' + hclv4.hierarchyClassName + ': ' + hct.traitValue AS HierarchyLineage
				,hclv4.hierarchyParentClassID AS HierarchyParentClassId
			FROM ItemHierarchyClass ihc 
			JOIN HierarchyClass hclv4 ON hclv4.hierarchyClassID = ihc.hierarchyClassID
			JOIN HierarchyClass hclv3 ON hclv4.hierarchyParentClassID = hclv3.hierarchyClassID
			JOIN HierarchyClass hclv2 ON hclv3.hierarchyParentClassID = hclv2.hierarchyClassID
			JOIN HierarchyClass hclv1 ON hclv2.hierarchyParentClassID = hclv1.hierarchyClassID
			JOIN HierarchyClassTrait hct ON hclv4.hierarchyClassID = hct.hierarchyClassID
				AND hct.traitID = (
					SELECT TRAITID
					FROM Trait
					WHERE traitCode = 'NCC'
					)
			JOIN Hierarchy h ON hclv4.HIERARCHYID = h.HIERARCHYID
			WHERE h.hierarchyName = 'National' AND ihc.itemId = i.ItemId 
		) nationalHierarchy 
	OUTER APPLY (
			SELECT 
				ihc.itemID,
				ihc.hierarchyClassId,
				hc.hierarchyClassName,
				hc.hierarchyClassName as HierarchyLineage
			FROM ItemHierarchyClass ihc 
			JOIN HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
			JOIN Hierarchy h ON hc.hierarchyID = h.hierarchyID
			WHERE h.hierarchyName = 'Manufacturer' AND ihc.itemId = i.ItemId 
		) manufacturerHierarchy 