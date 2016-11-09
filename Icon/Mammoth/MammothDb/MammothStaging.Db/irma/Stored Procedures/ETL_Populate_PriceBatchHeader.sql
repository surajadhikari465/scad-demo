
CREATE PROCEDURE [irma].[ETL_Populate_PriceBatchHeader] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;
	
	SET @table_name = N'PriceBatchHeader'
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
		Region
		, PriceBatchHeaderID
		, PriceBatchStatusID
		, ItemChgTypeID
		, PriceChgTypeID
		, StartDate
		, PrintedDate
		, SentDate
		, ProcessedDate
		, POSBatchID
		, BatchDescription
		, AutoApplyFlag
		, ApplyDate)
		SELECT ' + QUOTENAME(@Region,'''') + ' as Region 
		, pbh.PriceBatchHeaderID
		, pbh.PriceBatchStatusID
		, pbh.ItemChgTypeID
		, pbh.PriceChgTypeID
		, pbh.StartDate
		, pbh.PrintedDate
		, pbh.SentDate
		, pbh.ProcessedDate
		, pbh.POSBatchID
		, pbh.BatchDescription
		, pbh.AutoApplyFlag
		, pbh.ApplyDate
		FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].' + QUOTENAME(@table_Name) + ' pbh ' + CHAR(13) +
		'JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].PriceChgType pct on (pbh.PriceChgTypeID = pct.PriceChgTypeID)' + CHAR(13) +
		'WHERE pct.On_Sale = 0
'


	PRINT @sqlText
	EXECUTE sp_executesql @sqlText, @sqlParams;

	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';

END