
CREATE PROCEDURE [irma].[ETL_Populate_ItemSignAttribute] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	SET NOCOUNT ON
	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;
	DECLARE @Rows INT;

	SET @table_name = N'ItemSignAttribute'
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
		, ItemSignAttributeID
		, Item_Key
		, Locality
		, SignRomanceTextLong
		, SignRomanceTextShort
		, AnimalWelfareRating
		, Biodynamic
		, CheeseMilkType
		, CheeseRaw
		, EcoScaleRating
		, GlutenFree
		, HealthyEatingRating
		, Kosher
		, NonGmo
		, Organic
		, PremiumBodyCare
		, ProductionClaims
		, FreshOrFrozen
		, SeafoodCatchType
		, Vegan
		, Vegetarian
		, WholeTrade
		, UomRegulationChicagoBaby
		, UomRegulationTagUom
		, Msc
		, GrassFed
		, PastureRaised
		, FreeRange
		, DryAged
		, AirChilled
		, MadeInHouse
		, Exclusive
		, ColorAdded
	)
	SELECT    ' + QUOTENAME(@Region,'''') + '
		, ItemSignAttributeID
		, Item_Key
		, Locality
		, SignRomanceTextLong
		, SignRomanceTextShort
		, AnimalWelfareRating
		, Biodynamic
		, CheeseMilkType
		, CheeseRaw
		, EcoScaleRating
		, GlutenFree
		, HealthyEatingRating
		, Kosher
		, NonGmo
		, Organic
		, PremiumBodyCare
		, ProductionClaims
		, FreshOrFrozen
		, SeafoodCatchType
		, Vegan
		, Vegetarian
		, WholeTrade
		, UomRegulationChicagoBaby
		, UomRegulationTagUom
		, Msc
		, GrassFed
		, PastureRaised
		, FreeRange
		, DryAged
		, AirChilled
		, MadeInHouse
		, Exclusive
		, ColorAdded
	FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.dbo.' + QUOTENAME(@table_name) + ' i
'
--	PRINT @sqlText


	EXECUTE sp_executesql @sqlText, @sqlParams

	SELECT @Rows = @@ROWCOUNT
	PRINT '...' + CONVERT(NVARCHAR(18), @Rows) + ' rows inserted'
	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';


END