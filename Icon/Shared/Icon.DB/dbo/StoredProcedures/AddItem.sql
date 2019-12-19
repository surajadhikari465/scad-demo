CREATE PROCEDURE dbo.AddItem @BrandsHierarchyClassId INT
	,@FinancialHierarchyClassId INT
	,@MerchandiseHierarchyClassId INT
	,@NationalHierarchyClassId INT
	,@TaxHierarchyClassId INT
	,@ManufacturerHierarchyClassId INT
	,@ItemAttributesJson NVARCHAR(MAX)
	,@ItemTypeCode NVARCHAR(100)
	,@SelectedBarCodeTypeId INT
	,@ScanCode NVARCHAR(13)
	,@ItemScanCode NVARCHAR(13) OUTPUT
AS
BEGIN
	DECLARE @scopeIdentity INT = NULL;
	DECLARE @merchandiseHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Merchandise'
			)
		,@brandsHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Brands'
			)
		,@taxHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Tax'
			)
		,@financialHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Financial'
			)
		,@nationalHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'National'
			)
		,@manufacturerHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Manufacturer'
			)
		,@itemTypeId INT = (
			SELECT itemTypeId
			FROM dbo.ItemType
			WHERE ItemTypeCode = @ItemTypeCode
			)
		,@UpcPluCategoryId INT = (
			SELECT PluCategoryID
			FROM app.PluCategory
			WHERE PluCategoryName = 'UPC'
			)
		,@barcodeTypeId INT
		,@scanCodeTypeId INT

	CREATE TABLE #tempHierarchyClassIds (
		HierarchyClassId INT
		,HIERARCHYID INT
		)

	IF @ScanCode IS NOT NULL
		AND EXISTS (
			SELECT 1
			FROM ScanCode
			WHERE scanCode = @ItemScanCode
			)
	BEGIN
		RAISERROR (
				'Scan Code already exists.'
				,16
				,1
				)
	END

	SET @barcodeTypeId = @SelectedBarCodeTypeId

	IF(SELECT BarcodeType
				FROM dbo.BarcodeType
				WHERE BarCodeTypeId = @barcodeTypeId) = 'UPC'
      SET @scanCodeTypeId = (
					SELECT ScanCodeTypeId
					FROM ScanCodeType
					WHERE scanCodeTypeDesc = 'UPC'
					)
    ELSE
	BEGIN
		DECLARE @EndRange NVARCHAR(13) = (
				SELECT EndRange
				FROM dbo.BarcodeType
				WHERE BarCodeTypeId = @barcodeTypeId
				)

		IF (Len(@EndRange)) < 7
			SET @scanCodeTypeId = (
					SELECT ScanCodeTypeId
					FROM ScanCodeType
					WHERE scanCodeTypeDesc = 'POS PLU'
					)
		ELSE
			SET @scanCodeTypeId = (
					SELECT ScanCodeTypeId
					FROM ScanCodeType
					WHERE scanCodeTypeDesc = 'Scale PLU'
					)
    END

    IF @ScanCode IS  NULL
	BEGIN
		CREATE TABLE #tmpScanCode (ScanCode NVARCHAR(12))

		INSERT INTO #tmpScanCode (ScanCode)
		EXEC GetAvailableScanCodesForBarcodeTypeId @barcodeTypeId

		IF EXISTS (
				SELECT 1
				FROM #tmpScanCode
				)
		BEGIN
			SET @ItemScanCode = (
					SELECT TOP 1 ScanCode
					FROM #tmpScanCode
					)
		END
		ELSE
		BEGIN
			RAISERROR (
					'Error in generating Scan Code. There are no scan codes available in the Barcode type selected.'
					,16
					,1
					)
		    RETURN;
		END
  END
  ELSE
  BEGIN
	 SET @ItemScanCode = @ScanCode
	 UPDATE dbo.BarcodeTypeRangePool  
	 SET 
	 Assigned=1, 
	 AssignedDateTimeUtc=SYSUTCDATETIME() 
	 WHERE ScanCode = @ScanCode
  END

	INSERT INTO #tempHierarchyClassIds
	VALUES (
		@MerchandiseHierarchyClassId
		,@merchandiseHierarchyId
		)
		,(
		@BrandsHierarchyClassId
		,@brandsHierarchyId
		)
		,(
		@TaxHierarchyClassId
		,@taxHierarchyId
		)
		,(
		@FinancialHierarchyClassId
		,@financialHierarchyId
		)
		,(
		@NationalHierarchyClassId
		,@nationalHierarchyId
		)


	-- manufacturer is optional.
	IF (@ManufacturerHierarchyClassId > 0)
		INSERT INTO #tempHierarchyClassIds
		VALUES (
			@ManufacturerHierarchyClassId,
			@manufacturerHierarchyId
		)	

	INSERT INTO [dbo].[Item] (
		[ItemTypeId]
		,[ItemAttributesJson]
		)
	VALUES (
		@itemTypeId
		,@ItemAttributesJson
		)

	SET @scopeIdentity = SCOPE_IDENTITY()

	INSERT INTO [dbo].[ScanCode] (
		[itemID]
		,[scanCode]
		,[scanCodeTypeID]
		,[localeID]
		,barcodeTypeID
		)
	VALUES (
		@scopeIdentity
		,@ItemScanCode
		,@scanCodeTypeId
		,1
		,@SelectedBarCodeTypeId
		)

	INSERT INTO ItemHierarchyClass (
		ItemId
		,hierarchyClassID
		,localeID
		)
	SELECT  @scopeIdentity
		   ,ihc.HierarchyClassId
		   ,1
	FROM #tempHierarchyClassIds ihc
	WHERE ihc.HierarchyClassId > 0

	SELECT @ItemScanCode
END