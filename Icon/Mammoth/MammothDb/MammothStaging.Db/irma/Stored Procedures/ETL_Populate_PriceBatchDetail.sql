CREATE PROCEDURE [irma].[ETL_Populate_PriceBatchDetail] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;
	
	SET @table_name = N'PriceBatchDetail'
	SET @sqlParams = N''
	SET @runtime_start = getdate();

	SELECT @LinkedServer = LinkedServer 
		, @DB = [Database]
	FROM [etl].DataSources
	WHERE Environment = @Environment 
	AND Region = @region

	PRINT '...Populating [irma]. ' + QUOTENAME(@table_name) + ' (' + @Region + ')';

	SET @sqlText = 'DELETE FROM irma.' + QUOTENAME(@table_name) + ' WHERE Region = @iRegion'
	SET @sqlParams = N'@iRegion NCHAR(2)'

	EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @region

	SET @sqlParams = N''
	SET @sqlText = '
		INSERT INTO [irma].' + QUOTENAME(@table_name) + ' ( 
			Region,
			PriceBatchDetailID,
			Item_Key,
			Store_No,
			Price,
			Multiple,
			StartDate,
			PriceChgTypeID
		)
		SELECT
			' + QUOTENAME(@Region,'''') + ' as Region, 
			sd.PriceBatchDetailID,
			data.Item_Key,
			data.Store_No,
			sd.Price,
			sd.Multiple,
			sd.StartDate,
			sd.PriceChgTypeID
		FROM
			(SELECT    
				' + QUOTENAME(@Region,'''') + ' as Region, 
				MAX(pbd.PriceBatchDetailID) as PriceBatchDetailID,
				pbd.Item_Key,
				pbd.Store_No
			FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].' + QUOTENAME(@table_Name) + 'pbd
				JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].PriceChgType		pct on	pbd.PriceChgTypeID = pct.PriceChgTypeID
				JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].Store s ON (pbd.Store_No = s.Store_No)
				JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].PriceBatchHeader pbh ON (pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID)
				JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].PriceBatchStatus pbs ON (pbh.PriceBatchStatusID = pbs.PriceBatchStatusID)
				JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].StoreRegionMapping srm ON (s.Store_No = srm.Store_No)
			WHERE
				(s.WFM_Store = 1 OR s.Mega_Store = 1 )
				AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
				AND (srm.Region_Code = ' + QUOTENAME(@Region,'''') + ')
				AND pct.On_Sale = 0
				AND pbs.PriceBatchStatusDesc = ''Sent''
			GROUP BY
				pbd.Item_Key,
				pbd.Store_No) data
		JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].[PriceBatchDetail] sd on data.PriceBatchDetailID = sd.PriceBatchDetailID;'


	PRINT @sqlText
	EXECUTE sp_executesql @sqlText, @sqlParams;

	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';

END