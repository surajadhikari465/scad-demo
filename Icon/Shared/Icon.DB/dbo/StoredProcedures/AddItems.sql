CREATE PROCEDURE dbo.AddItems
	@Items dbo.AddItemsType readonly
AS
BEGIN
	DECLARE @UpcPluCategoryId INT = (
			SELECT PluCategoryID
			FROM app.PluCategory
			WHERE PluCategoryName = 'UPC'
			)
		,@barcodeTypeId INT
		,@upcScanCodeTypeId INT = (
			SELECT TOP 1ScanCodeTypeId
			FROM ScanCodeType
			WHERE scanCodeTypeDesc = 'UPC'
		)
		,@posPluScanCodeTypeId INT = (
			SELECT TOP 1ScanCodeTypeId
			FROM ScanCodeType
			WHERE scanCodeTypeDesc = 'POS PLU'
			
		)
		,@scalePluScanCodeTypeId INT =(
			SELECT TOP 1 ScanCodeTypeId
			FROM ScanCodeType
			WHERE scanCodeTypeDesc = 'SCALE PLU'
		)
		,@wfmLocaleId INT = (
			SELECT TOP 1 localeID
			FROM Locale
			WHERE localeName = 'Whole Foods'
		)
	CREATE TABLE #Items
	(
		ItemId INT NOT NULL,
		ItemTypeId INT NOT NULL,
		ScanCode NVARCHAR(13) NOT NULL,
		BarCodeTypeId INT NOT NULL,
		ItemAttributesJson NVARCHAR(MAX) NOT NULL,
		BrandsHierarchyClassId INT NOT NULL,
		FinancialHierarchyClassId INT NOT NULL,
		MerchandiseHierarchyClassId INT NOT NULL,
		NationalHierarchyClassId INT NOT NULL,
		TaxHierarchyClassId INT NOT NULL,
		ManufacturerHierarchyClassId INT NULL,
		ScanCodeTypeId INT NOT NULL
	)

	--Merge insert into items so that we can output the ItemId and ScanCode relationship to insert into ScanCode
	MERGE dbo.Item i
	USING @Items t
	ON 0 = 1
	WHEN NOT MATCHED THEN
	INSERT (itemTypeID, ItemAttributesJson)
	VALUES (t.ItemTypeId, ItemAttributesJson)
	OUTPUT 
		inserted.ItemId AS ItemId,
		t.ItemTypeId,
		t.ScanCode,
		t.BarCodeTypeId,
		t.ItemAttributesJson,
		t.BrandsHierarchyClassId,
		t.FinancialHierarchyClassId,
		t.MerchandiseHierarchyClassId,
		t.NationalHierarchyClassId,
		t.TaxHierarchyClassId,
		t.ManufacturerHierarchyClassId,
		CASE 
			WHEN LEN(t.ScanCode) < 7 THEN @posPluScanCodeTypeId
			WHEN LEN(t.ScanCode) = 11 
				AND (t.ScanCode like '2[0-9][0-9][0-9][0-9][0-9]00000'
					OR t.ScanCode like '460000[0-9][0-9][0-9][0-9][0-9]' 
					OR t.ScanCode like '480000[0-9][0-9][0-9][0-9][0-9]') THEN @scalePluScanCodeTypeId
			ELSE @upcScanCodeTypeId
		END AS ScanCodeTypeId
	INTO #Items;

	--Insert ScanCodes
	INSERT INTO [dbo].[ScanCode] (
		[itemID]
		,[scanCode]
		,[scanCodeTypeID]
		,[localeID]
		,barcodeTypeID
		)
	SELECT i.ItemId,
		i.ScanCode,
		i.ScanCodeTypeId,
		@wfmLocaleId,
		i.BarcodeTypeId
	FROM #Items i

	--Insert HierarchyClasses
	CREATE TABLE #tempHierarchyClassIds
	(
		ItemId INT NOT NULL,
		HierarchyClassId INT NOT NULL
	)
	INSERT #tempHierarchyClassIds(ItemId, HierarchyClassId)
	SELECT i.ItemId,
		i.MerchandiseHierarchyClassId
	FROM #Items i
	UNION
	SELECT i.ItemId,
		i.BrandsHierarchyClassId
	FROM #Items i
	UNION
	SELECT i.ItemId,
		i.TaxHierarchyClassId
	FROM #Items i
	UNION
	SELECT i.ItemId,
		i.NationalHierarchyClassId
	FROM #Items i
	UNION
	SELECT i.ItemId,
		i.FinancialHierarchyClassId
	FROM #Items i
	UNION
	SELECT i.ItemId,
		i.ManufacturerHierarchyClassId
	FROM #Items i
	WHERE i.ManufacturerHierarchyClassId IS NOT NULL
		AND i.ManufacturerHierarchyClassId > 0	

	INSERT INTO ItemHierarchyClass (
		ItemId
		,hierarchyClassID
		,localeID
		)
	SELECT ihc.ItemId,
		   ihc.HierarchyClassId,
		   @wfmLocaleId
	FROM #tempHierarchyClassIds ihc

	UPDATE dbo.BarcodeTypeRangePool
	SET Assigned = 1
	WHERE Assigned = 0
		AND ScanCode IN
		(
			SELECT ScanCode
			FROM #Items
		)

	--Return Item Ids
	SELECT ItemId,
		ScanCode
	FROM #Items
END