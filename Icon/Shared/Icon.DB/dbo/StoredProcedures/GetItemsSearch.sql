CREATE PROCEDURE dbo.GetItemsSearch
	@ItemId INT = NULL,
	@ItemTypeId INT = NULL,
	@ScanCode NVARCHAR(MAX) = NULL,
	@UseScanCodePartialSearch BIT = NULL,
	@BarcodeTypeId INT = NULL,
	@BrandsHierarchyClassId INT = NULL, 
	@MerchandiseHierarchyClassId INT = NULL,
	@TaxHierarchyClassId INT = NULL,
	@FinancialHierarchyClassId INT = NULL,
	@NationalHierarchyClassId INT = NULL,
	@ManufacturerHierarchyClassId INT = NULL,
	@ItemAttributeJsonParameters NVARCHAR(MAX) = NULL,
	@Top INT = 0,
	@Skip INT = 0,
	@OrderByValue NVARCHAR(100) = NULL,
	@OrderByOrder NVARCHAR(5) = NULL

	-- ====================================
	-- NOTE:
	-- @extendedParams parameter is a json object that should be an array of an object with AttributeName and AttributeValue as the properties.
	-- Example:   [ { "AttributeName"="Kosher", "AttributeValue":"Yes" }, { "AttributeName"="EStoreEligible", "AttributeValue":"Yes" }, { "AttributeName"="Raw", "AttributeValue":"Yes" }]
	-- ====================================
AS
BEGIN
	
	-- Sql String Variables
	DECLARE @selectItemsSql NVARCHAR(max) = N'';
	DECLARE @fromSql NVARCHAR(max) = N'';
	DECLARE @whereSql NVARCHAR(max) = N'';
	DECLARE @jsonWhereSql NVARCHAR(max) = N'';
	DECLARE @orderBySql NVARCHAR(max) = N'';
	DECLARE @ShouldOrderByJsonAttribute BIT = 0;

	--======================================
	-- Set SELECT Items Statement
	--======================================
	SET @selectItemsSql = N'
	SELECT
		i.ItemId,
		i.ItemTypeId,
		i.ItemTypeCode,
		i.ItemTypeDescription,
		i.ScanCode,
		i.BarcodeTypeId as BarcodeTypeId,
		i.BarcodeType as BarcodeType,
		i.MerchandiseHierarchyClassId,
		i.BrandsHierarchyClassId,
		i.TaxHierarchyClassId,
		i.FinancialHierarchyClassId,
		i.NationalHierarchyClassId,
		i.ManufacturerHierarchyClassId,
		i.ItemAttributesJson as ItemAttributesJson,
		i.Brand as Brands,
		i.Merchandise,
		i.Tax,
		i.NationalClass as [National],
		i.Financial,
		i.Manufacturer';

	SET @fromSql = N'
	FROM ItemView i ';

	SET @whereSql = N'
	WHERE 1 =1' 
	
	IF (@ItemId IS NOT NULL)
	BEGIN 
		SET @whereSql = @whereSql + 'AND i.itemId = @ItemId '
	END

	IF (@ItemTypeId IS NOT NULL)
	BEGIN
		SET @whereSql = @whereSql + 'AND i.ItemTypeId = @itemTypeId '
	END

	IF (@ScanCode IS NOT NULL)
	BEGIN
		IF @UseScanCodePartialSearch = 1 
		BEGIN
			SET @whereSql = @whereSql + 'AND i.ScanCode LIKE ''%' + @ScanCode + '%'' '
		END
		ELSE
		BEGIN
			IF CHARINDEX(' ', @ScanCode) > 0
			BEGIN
				IF (object_id('tempdb..#scanCodes') IS NOT NULL)
					DROP TABLE #scanCodes;

				SELECT value AS ScanCode
				INTO #scanCodes
				FROM string_split(@ScanCode, ' ')

				SET @whereSql = @whereSql + 'AND i.ScanCode in (select scancode from #scanCodes) '
			END
			ELSE
			BEGIN
				SET @whereSql = @whereSql + 'AND i.ScanCode = @ScanCode '
			END
		END
	END

	IF (@BarcodeTypeId IS NOT NULL)
	BEGIN
		SET @whereSql = @whereSql + 'AND i.BarcodeTypeId = @BarcodeTypeId '
	END

	IF (@BrandsHierarchyClassId IS NOT NULL)
	BEGIN
		SET @whereSql = @whereSql + 'AND BrandsHierarchyClassId = @BrandsHierarchyClassId '
	END

	IF (@MerchandiseHierarchyClassId IS NOT NULL)
	BEGIN
		SET @whereSql = @whereSql + 'AND MerchandiseHierarchyClassId = @MerchandiseHierarchyClassId '
	END

	IF (@TaxHierarchyClassId IS NOT NULL)
	BEGIN
		SET @whereSql = @whereSql + 'AND TaxHierarchyClassId = @TaxHierarchyClassId '
	END

	IF (@FinancialHierarchyClassId IS NOT NULL)
	BEGIN
		SET @whereSql = @whereSql + 'AND FinancialHierarchyClassId = @FinancialHierarchyClassId '
	END

	IF (@NationalHierarchyClassId IS NOT NULL)
	BEGIN
		SET @whereSql = @whereSql + 'AND NationalHierarchyClassId = @NationalHierarchyClassId '
	END

	IF (@ManufacturerHierarchyClassId IS NOT NULL)
	BEGIN
		SET @whereSql = @whereSql + 'AND ManufacturerHierarchyClassId = @ManufacturerHierarchyClassId '
	END

	IF(@ItemAttributeJsonParameters IS NOT NULL AND @ItemAttributeJsonParameters <> '[]')
	BEGIN
		IF OBJECT_ID('tempdb..#attributes') IS NOT NULL
		DROP TABLE #attributes

		CREATE TABLE #attributes (
			AttributeName NVARCHAR(255) NOT NULL,
			AttributeValue NVARCHAR(max) NOT NULL,
			UsePartialSearch BIT NOT NULL
			)

		INSERT INTO #attributes
		SELECT AttributeName,
			AttributeValue,
			UsePartialSearch
		FROM OPENJSON(@ItemAttributeJsonParameters) WITH (
				AttributeName NVARCHAR(255) '$."AttributeName"',
				AttributeValue NVARCHAR(max) '$."AttributeValue"',
				UsePartialSearch BIT '$."UsePartialSearch"'
				)
        
		IF NOT EXISTS (
				SELECT 1
				FROM #attributes
				WHERE AttributeName = 'Inactive'
				)
		BEGIN
			SET @whereSql = @whereSql + ' AND Inactive = ''false'' '
		END

		-- using alias 'iae' in FROM clause for ItemAttributesJson table
		-- If "CategoryManagement" and "KosherClaim" were the only two extended attributes passed in, the following results in a string like:
		-- JSON_VALUE(ie.ItemAttributesJson, '$.CategoryManagement') = 'In Assortment' AND JSON_VALUE(ie.ItemAttributesJson, '$.KosherClaim') = 'Yes'
		SELECT @jsonWhereSql = STRING_AGG(CONCAT (
					'JSON_VALUE(i.ItemAttributesJson, ''$."',
					a.AttributeName,
					CASE 
						WHEN a.UsePartialSearch = 1 THEN CONCAT('"'') like ''%', a.AttributeValue, '%''')
						ELSE CONCAT('"'') = ''', a.AttributeValue, '''')
					END
					), ' AND ')
		FROM #attributes a

		SET @whereSql = @whereSql + ' AND ' + @jsonWhereSql
	END
	ELSE
	BEGIN
	   SET @whereSql = @whereSql + ' AND Inactive = ''false'' ' 
	END

	IF @OrderByValue IS NOT NULL
    BEGIN
        IF EXISTS (SELECT 1 FROM dbo.Attributes WHERE AttributeName = @OrderByValue)
        BEGIN
            SET @ShouldOrderByJsonAttribute = 1
        END
    END

	IF @OrderByValue IS NULL
	BEGIN
		SET @orderBySql = N' ORDER BY i.ItemId ASC OFFSET @skip ROWS FETCH NEXT @top ROWS ONLY'
	END
	ELSE IF @ShouldOrderByJsonAttribute = 0
	BEGIN
		IF @OrderByValue = 'ScanCode'
		BEGIN
			SET @orderBySql = N' ORDER BY ' + @OrderByValue + ' ' + @OrderByOrder + ' OFFSET @skip ROWS FETCH NEXT @top ROWS ONLY'
		END
		ELSE
		BEGIN
			SET @orderBySql = N' ORDER BY ' + @OrderByValue + ' ' + @OrderByOrder + ', i.ScanCode ASC OFFSET @skip ROWS FETCH NEXT @top ROWS ONLY'
	END END
	ELSE
	BEGIN
		SET @orderBySql = N' ORDER BY JSON_VALUE(i.ItemAttributesJson, ''$."' + @OrderByValue + '"'') ' + @OrderByOrder + ', i.ScanCode ASC OFFSET @skip ROWS FETCH NEXT @top ROWS ONLY'
	END

	DECLARE @getCountSql NVARCHAR(max) = CONCAT (
			N'SELECT COUNT(*) as TotalRecordsCount',
			@fromSql,
			@whereSql
			);
	DECLARE @sql NVARCHAR(max) = CONCAT (
			@selectItemsSql,
			@fromSql,
			@whereSql,
			@orderBySql
			);

	SET @sql += N' OPTION(RECOMPILE)';

	DECLARE @paramDef NVARCHAR(max) = N'
	@ItemId INT,
	@ItemTypeId INT,
	@ScanCode NVARCHAR(13),
	@BarcodeTypeId INT,
	@BrandsHierarchyClassId INT,
	@MerchandiseHierarchyClassId INT,
	@TaxHierarchyClassId INT,
	@FinancialHierarchyClassId INT,
	@NationalHierarchyClassId INT,
    @ManufacturerHierarchyClassId INT,
	@Top INT,
	@Skip INT'

	--Get Count of total items
	EXEC sp_executesql @getCountSql,
		@paramDef,
		@ItemId,
		@ItemTypeId,
		@ScanCode,
		@BarcodeTypeId,
		@BrandsHierarchyClassId,
		@MerchandiseHierarchyClassId,
		@TaxHierarchyClassId,
		@FinancialHierarchyClassId,
		@NationalHierarchyClassId,
		@ManufacturerHierarchyClassId,
		@Top,
		@Skip

	--Get Items
	EXEC sp_executesql @sql,
		@paramDef,
		@ItemId,
		@ItemTypeId,
		@ScanCode,
		@BarcodeTypeId,
		@BrandsHierarchyClassId,
		@MerchandiseHierarchyClassId,
		@TaxHierarchyClassId,
		@FinancialHierarchyClassId,
		@NationalHierarchyClassId,
		@ManufacturerHierarchyClassId,
		@Top,
		@Skip
END