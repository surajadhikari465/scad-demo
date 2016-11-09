
CREATE PROCEDURE [irma].[ETL_Populate_ScaleExtraText] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	SET NOCOUNT ON
	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;
	DECLARE @Rows INT;

	SET @table_name = N'Scale_ExtraText'
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
	, Scale_ExtraText_ID
	, Scale_LabelType_ID
	, Description
	, ExtraText
	)
	SELECT    ' + QUOTENAME(@Region,'''') + '
	, Scale_ExtraText_ID
	, Scale_LabelType_ID
	, Description
	, ExtraText
	FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.dbo.' + QUOTENAME(@table_name) + ' i
'
--	PRINT @sqlText


	EXECUTE sp_executesql @sqlText, @sqlParams

	SELECT @Rows = @@ROWCOUNT
	PRINT '...' + CONVERT(NVARCHAR(18), @Rows) + ' rows inserted'
	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';


END