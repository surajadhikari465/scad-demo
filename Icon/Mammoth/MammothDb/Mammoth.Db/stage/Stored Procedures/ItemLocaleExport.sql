CREATE PROCEDURE [stage].[ItemLocaleExport]
	@Region char(2), @GroupSize int = 100, @MaxRows int = 0
	AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE @rowCount int 

	IF IsNull(@MaxRows, 0) = 0 
		SET @MaxRows = 2147483647 -- max int

	DECLARE @ColorAddedId INT = (
			SELECT AttributeID
			FROM Attributes
			WHERE AttributeDesc LIKE '%Color%Add%'
			)
	DECLARE @CountryOfProcessingId INT = (
			SELECT AttributeID
			FROM Attributes
			WHERE AttributeDesc LIKE 'Country of Processing'
			)
	DECLARE @OriginId INT = (
			SELECT AttributeID
			FROM Attributes
			WHERE AttributeDesc LIKE 'Origin'
			)
	DECLARE @EstId INT = (
			SELECT AttributeID
			FROM Attributes
			WHERE AttributeDesc LIKE 'Electronic Shelf Tag'
			)
	DECLARE @ExclusiveId INT = (
			SELECT AttributeID
			FROM Attributes
			WHERE AttributeDesc LIKE 'Exclusive'
			)
	DECLARE @NumDigitsToScaleId INT = (
			SELECT AttributeID
			FROM Attributes
			WHERE AttributeDesc LIKE 'Number of Digits Sent To Scale'
			)
	DECLARE @ChicagoBabyId INT = (
			SELECT AttributeID
			FROM Attributes
			WHERE AttributeDesc LIKE 'Chicago Baby'
			)
	DECLARE @TagUomId INT = (
			SELECT AttributeID
			FROM Attributes
			WHERE AttributeDesc LIKE 'Tag UOM'
			)
	DECLARE @LinkedScanCodeId INT = (
			SELECT AttributeID
			FROM Attributes
			WHERE AttributeDesc LIKE 'Linked Scan Code'
			)
	

	DECLARE @ScaleExtraTextId INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Scale Extra Text' )
	DECLARE @CFSSendtoScale INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'CFS Send to Scale' ) 
	DECLARE @ForceTare INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Force Tare' ) 
	DECLARE @ShelfLife INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Shelf Life' ) 
	DECLARE @UnwrappedTareWeight INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Unwrapped Tare Weight' ) 
	DECLARE @UseByEAB INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Use By EAB' ) 
	DECLARE @WrappedTareWeight INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Wrapped Tare Weight' ) 

	DECLARE @timestamp DATETIME,
		@msg VARCHAR(255)

	TRUNCATE TABLE stage.ItemLocaleExportStaging

	IF EXISTS (
			SELECT *
			FROM sys.indexes
			WHERE name LIKE '%IX_ItemLocaleExportStaging%'
			)
		DROP INDEX IX_ItemLocaleExportStaging ON stage.ItemLocaleExportStaging

	SET @timestamp = GETDATE();
	SET @msg = CONVERT(VARCHAR, @timestamp, 120) + ': begin item locale staging '

	RAISERROR (
			@msg,
			0,
			1
			)
	WITH NOWAIT

	INSERT INTO [stage].[ItemLocaleExportStaging]
	SELECT TOP (@MaxRows) s.Region,
		s.BusinessUnitId,
		l.LocaleID,
		i.ItemID,
		it.ItemTypeCode,
		it.ItemTypeDesc,
		l.StoreName AS LocaleName,
		i.ScanCode,
		s.Discount_Case AS [CaseDiscount],
		s.Discount_TM [TmDiscount],
		s.Restriction_Age [AgeRestriction],
		s.Restriction_Hours [RestrictedHours],
		s.Authorized,
		s.Discontinued,
		s.LabelTypeDesc [LabelTypeDescription],
		s.LocalItem,
		s.Product_Code [ProductCode],
		s.RetailUnit,
		s.Sign_Desc [SignDescription],
		s.Locality,
		s.Sign_RomanceText_Long [SignRomanceLong],
		s.Sign_RomanceText_Short [SignRomanceShort],
		NULL [ColorAdded],
		NULL [CountryOfProcessing],
		NULL [Origin],
		NULL [ElectronicShelfTag],
		NULL [Exclusive],
		NULL [NumberOfDigitsSentToScale],
		NULL [ChicagoBaby],
		NULL [TagUom],
		NULL [LinkedItem],
		NULL [ScaleExtraText],
		NULL [CFS Send to Scale],
		NULL [Force Tare],
		NULL [Shelf Life],
		NULL [Unwrapped Tare Weight],
		NULL [Use By EAB],
		NULL [Wrapped Tare Weight],
		s.Msrp [Msrp],
		NULL [SupplierName],
		NULL [IrmaVendorKey],
		NULL [SupplierItemID],
		NULL [SupplierCaseSize],
		s.OrderedByInfor [OrderedByInfor],
		s.AltRetailSize [AltRetailSize],
		s.AltRetailUOM [AltRetailUOM],
		s.DefaultScanCode [DefaultScanCode],
		s.IrmaItemKey [IrmaItemKey],
		NULL Groupid,
		0 Processed
  FROM dbo.ItemLocaleAttributes s
  INNER JOIN dbo.Items i ON s.ItemID = i.ItemID
  INNER JOIN dbo.ItemTypes it ON i.ItemTypeID = it.ItemTypeID
  INNER JOIN dbo.Locale l ON l.Region = @region AND s.BusinessUnitID = l.BusinessUnitID
  WHERE s.Region = @region
	option (recompile)

	SET @rowCount = @@ROWCOUNT;
	SET @timestamp = GETDATE();
	SET @msg = CONVERT(VARCHAR, @timestamp, 120) + ': items staged'

	RAISERROR (
			@msg,
			0,
			1
			)
	WITH NOWAIT

	CREATE NONCLUSTERED INDEX IX_ItemLocaleExportStaging ON stage.ItemLocaleExportStaging (
		localeid,
		itemid
		)

	-- left joins were removed.
	-- recreated as updates with inner joins.
	UPDATE il
	SET ColorAdded = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @ColorAddedId
	option (recompile)

	UPDATE il
	SET CountryOfProcessing = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @CountryOfProcessingId
	option (recompile)

	UPDATE il
	SET Origin = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @OriginId
	option (recompile)

	UPDATE il
	SET [ElectronicShelfTag] = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @EstId
	option (recompile)

	UPDATE il
	SET Exclusive = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @ExclusiveId
	option (recompile)

	UPDATE il
	SET NumberOfDigitsSentToScale = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @NumDigitsToScaleId
	option (recompile)

	UPDATE il
	SET ChicagoBaby = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @ChicagoBabyId
	option (recompile)

	UPDATE il
	SET TagUom = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @TagUomId
	option (recompile)

	UPDATE il
	SET LinkedItem = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @LinkedScanCodeId
	option (recompile)

	UPDATE il
	SET ScaleExtraText = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @ScaleExtraTextId
	option (recompile)

	
	UPDATE il
	SET [CFS Send to Scale] = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @CFSSendtoScale
	option (recompile)

	
	UPDATE il
	SET [Force Tare] = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @ForceTare
	option (recompile)

	
	UPDATE il
	SET [Shelf Life] = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @ShelfLife
	option (recompile)

	
	UPDATE il
	SET [Unwrapped Tare Weight] = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @UnwrappedTareWeight
	option (recompile)


	
	UPDATE il
	SET [Use By EAB] = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @UseByEAB
	option (recompile)


	
	UPDATE il
	SET [Wrapped Tare Weight] = ext.AttributeValue
	FROM [stage].ItemLocaleExportStaging il
	INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
		AND il.LocaleId = ext.LocaleID
	WHERE ext.AttributeId = @WrappedTareWeight
	option (recompile)

  UPDATE il
  SET [SupplierName] = ils.SupplierName
  FROM [stage].ItemLocaleExportStaging il
  INNER JOIN dbo.ItemLocaleSupplier ils ON ils.Region = @Region AND ils.ItemID = il.ItemId AND ils.BusinessUnitID = il.BusinessUnitId
  option (recompile);

  UPDATE il
  SET [IrmaVendorKey] = ils.IrmaVendorKey
  FROM [stage].ItemLocaleExportStaging il
  INNER JOIN dbo.ItemLocaleSupplier ils ON ils.Region = @Region AND ils.ItemID = il.ItemId AND ils.BusinessUnitID = il.BusinessUnitId
  option (recompile);

  UPDATE il
  SET [SupplierItemID] = ils.SupplierItemID
  FROM [stage].ItemLocaleExportStaging il
  INNER JOIN dbo.ItemLocaleSupplier ils ON ils.Region = @Region AND ils.ItemID = il.ItemId AND ils.BusinessUnitID = il.BusinessUnitId
  option (recompile);

  UPDATE il
  SET [SupplierCaseSize] = ils.SupplierCaseSize
  FROM [stage].ItemLocaleExportStaging il
  INNER JOIN dbo.ItemLocaleSupplier ils ON ils.Region = @Region AND ils.ItemID = il.ItemId AND ils.BusinessUnitID = il.BusinessUnitId
  option (recompile);


	SET @timestamp = GETDATE();
	SET @msg = CONVERT(VARCHAR, @timestamp, 120) + ': attributes updated'

	RAISERROR (
			@msg,
			0,
			1
			)
	WITH NOWAIT;
		
  -- assign batches based on @GroupSize
	WITH cte
	AS (
		SELECT ItemId,
			GroupId,
			(
				RANK() OVER (
					ORDER BY localeid,
						itemid
					) - 1
				) / @GroupSize CalculatedGroupId
		FROM [stage].[ItemLocaleExportStaging]
		)
	UPDATE cte
	SET GroupId = cte.CalculatedGroupId

	SET @timestamp = GETDATE();
	SET @msg = CONVERT(VARCHAR, @timestamp, 120) + ': group ids assigned'

	RAISERROR (
			@msg,
			0,
			1
			)
	WITH NOWAIT

	SELECT @rowCount AS [rowCount]
END
GO

GRANT EXECUTE ON stage.ItemLocaleExport TO [MammothRole];
GO