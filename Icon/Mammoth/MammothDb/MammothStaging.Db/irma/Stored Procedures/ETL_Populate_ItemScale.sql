
CREATE PROCEDURE [irma].[ETL_Populate_ItemScale] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	SET NOCOUNT ON
	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;

	SET @table_name = N'ItemScale'
	SET @sqlParams = N''
	SET @runtime_start = getdate();

	SELECT @LinkedServer = LinkedServer
		, @DB = [Database]
	FROM [etl].DataSources	WHERE Environment = @Environment AND Region = @region

	PRINT '...Populating [irma]. ' + QUOTENAME(@table_name) + ' (' + @Region + ')';

	SET @sqlText = 'DELETE FROM irma.' + QUOTENAME(@table_name) + ' WHERE Region = @iRegion'
	SET @sqlParams = N'@iRegion NCHAR(2)'

	EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region

	SET @sqlParams = N''
	SET @sqlText = 'INSERT INTO [irma].' + QUOTENAME(@table_name) + ' ( 
		  Region
		, ItemScale_ID
		, Item_Key
		, Nutrifact_ID
		, Scale_ExtraText_ID
		, Scale_Tare_ID
		, Scale_Alternate_Tare_ID
		, Scale_LabelStyle_ID
		, Scale_EatBy_ID
		, Scale_Grade_ID
		, Scale_RandomWeightType_ID
		, Scale_ScaleUOMUnit_ID
		, Scale_FixedWeight
		, Scale_ByCount
		, ForceTare
		, PrintBlankShelfLife
		, PrintBlankEatBy
		, PrintBlankPackDate
		, PrintBlankWeight
		, PrintBlankUnitPrice
		, PrintBlankTotalPrice
		, Scale_Description1
		, Scale_Description2
		, Scale_Description3
		, Scale_Description4
		, ShelfLife_Length
		, Scale_Ingredient_ID
		, Scale_Allergen_ID		
		)
	SELECT    ' + QUOTENAME(@Region,'''') + '
		, ItemScale_ID
		, Item_Key
		, Nutrifact_ID
		, Scale_ExtraText_ID
		, Scale_Tare_ID
		, Scale_Alternate_Tare_ID
		, Scale_LabelStyle_ID
		, Scale_EatBy_ID
		, Scale_Grade_ID
		, Scale_RandomWeightType_ID
		, Scale_ScaleUOMUnit_ID
		, Scale_FixedWeight
		, Scale_ByCount
		, ForceTare
		, PrintBlankShelfLife
		, PrintBlankEatBy
		, PrintBlankPackDate
		, PrintBlankWeight
		, PrintBlankUnitPrice
		, PrintBlankTotalPrice
		, Scale_Description1
		, Scale_Description2
		, Scale_Description3
		, Scale_Description4
		, ShelfLife_Length
		, Scale_Ingredient_ID
		, Scale_Allergen_ID		
	FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.dbo.' + QUOTENAME(@table_name) + ' i
'
	PRINT @sqlText

	EXECUTE sp_executesql @sqlText, @sqlParams
	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';

END