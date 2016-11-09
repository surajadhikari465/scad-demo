CREATE PROCEDURE [irma].[ETL_Populate_Store] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start datetime;

	SET @table_name = N'Store'
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
	SET @sqlText = 'INSERT INTO [irma].' + QUOTENAME(@table_name) + ' ( 
			  [Region]  
			, [Store_No]
			, [Store_Name]
			, [BusinessUnit_ID]
			, [StoreJurisdictionID]
			, [WFM_Store]
			, [Mega_Store]
			, [Internal]
			)
		SELECT    ' + QUOTENAME(@Region,'''') + ',
				  s.Store_No
				  , Store_Name
				  , BusinessUnit_ID
				, [StoreJurisdictionID]
				, [WFM_Store]
				, [Mega_Store]
				, [Internal]
			FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].' + QUOTENAME(@table_name) + ' s
			JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].StoreRegionMapping srm ON (s.Store_No = srm.Store_No)
		WHERE
				(s.WFM_Store = 1 OR s.Mega_Store = 1 )
				AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
				AND (srm.Region_Code = ' + QUOTENAME(@Region,'''') + ')';

	EXECUTE sp_executesql @sqlText, @sqlParams;

	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';

END