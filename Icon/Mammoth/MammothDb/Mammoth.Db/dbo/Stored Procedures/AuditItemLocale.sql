CREATE PROCEDURE [dbo].[AuditItemLocale]
  @action VARCHAR(25),
  @region VARCHAR(2),
	@groupSize INT = 250000,
	@groupId INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

  IF(Not Exists(SELECT 1 FROM Regions WHERE Region = @region))
  BEGIN
    DECLARE @msg varchar(100) = 'Invalid region specified ' + @region + '.';
    RAISERROR (@msg, 16, 1);
    RETURN;
  END

  IF @action = 'Initilize'
  BEGIN
    SELECT Count(*) FROM dbo.ItemLocaleAttributes WHERE Region = @region option(recompile);
    RETURN;
  END

  IF @action = 'Get'
	BEGIN
    IF IsNull(@groupSize, 0) <= 0
			SET @groupSize = 250000;

    DECLARE @minId INT = (@groupId * @groupSize) + (CASE WHEN @groupID = 0 THEN 0 ELSE 1 END);

	  DECLARE @ColorAddedId INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE '%Color%Add%');
	  DECLARE @CountryOfProcessingId INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Country of Processing');
	  DECLARE @OriginId INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Origin');
	  DECLARE @EstId INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Electronic Shelf Tag');
	  DECLARE @ExclusiveId INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Exclusive');
	  DECLARE @NumDigitsToScaleId INT = (SELECT AttributeID FROM Attributes	WHERE AttributeDesc = 'Number of Digits Sent To Scale');
	  DECLARE @ChicagoBabyId INT = (SELECT AttributeID FROM Attributes	WHERE AttributeDesc = 'Chicago Baby');
	  DECLARE @TagUomId INT = (SELECT AttributeID FROM Attributes	WHERE AttributeDesc = 'Tag UOM');
	  DECLARE @LinkedScanCodeId INT = (SELECT AttributeID	FROM Attributes	WHERE AttributeDesc = 'Linked Scan Code');
	  DECLARE @ScaleExtraTextId INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Scale Extra Text');
	  DECLARE @CFSSendtoScale INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'CFS Send to Scale'); 
	  DECLARE @ForceTare INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Force Tare'); 
	  DECLARE @ShelfLife INT = (SELECT AttributeID FROM Attributes WHERE AttributeCode = 'SHL'); -- This is the ItemLocale Shelf Life attribute as opposed to the Global Item Shelf Life attribute
	  DECLARE @UnwrappedTareWeight INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Unwrapped Tare Weight');
	  DECLARE @UseByEAB INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Use By EAB');
	  DECLARE @WrappedTareWeight INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Wrapped Tare Weight'); 
    
	  IF (object_id('tempdb..#group') IS NOT NULL) DROP TABLE #group;
    IF (object_id('tempdb..#items') IS NOT NULL) DROP TABLE #items;
    CREATE TABLE #group (ItemID INT, BU INT, INDEX ix_ID NONCLUSTERED(ItemID, BU));
    
    ;WITH cte AS(SELECT TOP 100 PERCENT B.ItemID, B.BusinessUnitID, Row_Number() OVER (ORDER BY B.ItemID ASC, B.BusinessUnitID ASC) rowID
	  		FROM dbo.Items A
        join dbo.ItemLocaleAttributes B on B.ItemID = A.ItemID
        WHERE B.Region = @region
        ORDER BY B.ItemID, B.BusinessUnitID)
	  	INSERT INTO #group (ItemID, BU)
	  	SELECT TOP (@groupSize) ItemID, BusinessUnitID
	  	FROM cte
	  	WHERE rowID >= @minId option(recompile);
    
    CREATE TABLE #items(
	    [BusinessUnitId] [int] NOT NULL,
	    [LocaleId] [int] NOT NULL,
	    [ItemId] [int] NOT NULL,
	    [ItemTypeCode] [nvarchar](3) NOT NULL,
	    [ItemTypeDesc] [nvarchar](255) NULL,
	    [LocaleName] [nvarchar](255) NOT NULL,
	    [ScanCode] [nvarchar](13) NULL,
	    [CaseDiscount] [bit] NOT NULL,
	    [TmDiscount] [bit] NOT NULL,
	    [AgeRestriction] [tinyint] NULL,
	    [RestrictedHours] [bit] NOT NULL,
	    [Authorized] [bit] NOT NULL,
	    [Discontinued] [bit] NOT NULL,
	    [LabelTypeDescription] [nvarchar](255) NULL,
	    [LocalItem] [bit] NOT NULL,
	    [ProductCode] [nvarchar](255) NULL,
	    [RetailUnit] [nvarchar](255) NULL,
	    [SignDescription] [nvarchar](255) NULL,
	    [Locality] [nvarchar](255) NULL,
	    [SignRomanceLong] [nvarchar](max) NULL,
	    [SignRomanceShort] [nvarchar](255) NULL,
	    [ColorAdded] [nvarchar](max) NULL,
	    [CountryOfProcessing] [nvarchar](max) NULL,
	    [Origin] [nvarchar](max) NULL,
	    [ElectronicShelfTag] [nvarchar](max) NULL,
	    [Exclusive] [nvarchar](max) NULL,
	    [NumberOfDigitsSentToScale] [nvarchar](max) NULL,
	    [ChicagoBaby] [nvarchar](max) NULL,
	    [TagUom] [nvarchar](max) NULL,
	    [LinkedItem] [nvarchar](max) NULL,
	    [ScaleExtraText] [nvarchar](max) NULL,
	    [CFS Send to Scale] [nvarchar](max) NULL,
	    [Force Tare] [nvarchar](max) NULL,
	    [Shelf Life] [nvarchar](max) NULL,
	    [Unwrapped Tare Weight] [nvarchar](max) NULL,
	    [Use By EAB] [nvarchar](max) NULL,
	    [Wrapped Tare Weight] [nvarchar](max) NULL,
	    [Msrp] [smallmoney] NOT NULL,
	    [SupplierName] [nvarchar](255) NULL,
	    [IrmaVendorKey] [nvarchar](10) NULL,
	    [SupplierItemID] [nvarchar](20) NULL,
	    [SupplierCaseSize] [decimal](9, 4) NULL,
	    [OrderedByInfor] [bit] NOT NULL,
	    [AltRetailSize] [numeric](9, 4) NULL,
	    [AltRetailUOM] [nvarchar](25) NULL,
	    [DefaultScanCode] [bit] NOT NULL,
	    [IrmaItemKey] [int] NULL,
	    [CreatedDate] [varchar](25) NULL,
	    [ModifiedDate] [varchar](25) NULL,
	    [ScaleItem] [bit] NOT NULL
      INDEX ix_itemId_LocaleId nonclustered (ItemID, LocaleId));
    
	  INSERT INTO #items
	  SELECT
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
		Convert(varchar, s.AddedDate, 120),
		convert(varchar, s.ModifiedDate, 120),
		s.ScaleItem
    FROM #group A
    INNER JOIN dbo.ItemLocaleAttributes s ON s.ItemID = A.ItemID and s.BusinessUnitID = A.BU
    INNER JOIN dbo.Items i ON s.ItemID = i.ItemID
    INNER JOIN dbo.ItemTypes it ON i.ItemTypeID = it.ItemTypeID
    INNER JOIN dbo.Locale l ON l.Region = @region AND s.BusinessUnitID = l.BusinessUnitID
    WHERE s.Region = @region
	  option (recompile)
    
	  -- recreated as updates with inner joins.
	  UPDATE il
	  SET ColorAdded = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @ColorAddedId
	  option (recompile)
    
	  UPDATE il
	  SET CountryOfProcessing = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @CountryOfProcessingId
	  option (recompile)
    
	  UPDATE il
	  SET Origin = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @OriginId
	  option (recompile)
    
	  UPDATE il
	  SET [ElectronicShelfTag] = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @EstId
	  option (recompile)
    
	  UPDATE il
	  SET Exclusive = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @ExclusiveId
	  option (recompile)
    
	  UPDATE il
	  SET NumberOfDigitsSentToScale = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @NumDigitsToScaleId
	  option (recompile)
    
	  UPDATE il
	  SET ChicagoBaby = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @ChicagoBabyId
	  option (recompile)
    
	  UPDATE il
	  SET TagUom = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @TagUomId
	  option (recompile)
    
	  UPDATE il
	  SET LinkedItem = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @LinkedScanCodeId
	  option (recompile)
    
	  UPDATE il
	  SET ScaleExtraText = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @ScaleExtraTextId
	  option (recompile)
	  
	  UPDATE il
	  SET [CFS Send to Scale] = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @CFSSendtoScale
	  option (recompile)
	  
	  UPDATE il
	  SET [Force Tare] = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @ForceTare
	  option (recompile)
	  
	  UPDATE il
	  SET [Shelf Life] = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @ShelfLife
	  option (recompile)
	  
	  UPDATE il
	  SET [Unwrapped Tare Weight] = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @UnwrappedTareWeight
	  option (recompile)
	  
	  UPDATE il
	  SET [Use By EAB] = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @UseByEAB
	  option (recompile)
	  
	  UPDATE il
	  SET [Wrapped Tare Weight] = ext.AttributeValue
	  FROM #items il
	  INNER JOIN dbo.ItemLocaleAttributesExt ext ON ext.region = @region and il.ItemId = ext.ItemID
	  	AND il.LocaleId = ext.LocaleID
	  WHERE ext.AttributeId = @WrappedTareWeight
	  option (recompile)
    
    UPDATE il
    SET [SupplierName] = ils.SupplierName
    FROM #items il
    INNER JOIN dbo.ItemLocaleSupplier ils ON ils.Region = @Region AND ils.ItemID = il.ItemId AND ils.BusinessUnitID = il.BusinessUnitId
    option (recompile);
    
    UPDATE il
    SET [IrmaVendorKey] = ils.IrmaVendorKey
    FROM #items il
    INNER JOIN dbo.ItemLocaleSupplier ils ON ils.Region = @Region AND ils.ItemID = il.ItemId AND ils.BusinessUnitID = il.BusinessUnitId
    option (recompile);
    
    UPDATE il
    SET [SupplierItemID] = ils.SupplierItemID
    FROM #items il
    INNER JOIN dbo.ItemLocaleSupplier ils ON ils.Region = @Region AND ils.ItemID = il.ItemId AND ils.BusinessUnitID = il.BusinessUnitId
    option (recompile);
    
    UPDATE il
    SET [SupplierCaseSize] = ils.SupplierCaseSize
    FROM #items il
    INNER JOIN dbo.ItemLocaleSupplier ils ON ils.Region = @Region AND ils.ItemID = il.ItemId AND ils.BusinessUnitID = il.BusinessUnitId
    option (recompile);
    
    select * from #items;
    
    IF (object_id('tempdb..#group') IS NOT NULL) DROP TABLE #group;
    IF (object_id('tempdb..#items') IS NOT NULL) DROP TABLE #items;
    RETURN;
  END

  SET NOCOUNT OFF;
  SET @msg = 'Unsupported action (' + @action + ').';
  RAISERROR (@msg, 16, 1);
END
GO

GRANT EXECUTE ON dbo.AuditItemLocale TO [MammothRole]
GO