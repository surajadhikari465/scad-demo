CREATE PROCEDURE [irma].[ETL_Populate_ItemIdentifier] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;

	SET @table_name = N'ItemIdentifier'
	SET @sqlParams = N''
	SET @runtime_start = getdate();

	SELECT @LinkedServer = LinkedServer 
		, @DB = [Database]
	FROM [etl].DataSources
	WHERE Environment = @Environment 
	AND Region = @region

	PRINT '...Populating [irma]. ' + QUOTENAME(@table_name) + ' (' + @region + ')';

	SET @sqlText = 'DELETE FROM irma.' + QUOTENAME(@table_name) + ' WHERE Region = @iRegion'
	SET @sqlParams = N'@iRegion NCHAR(2)'

	EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @region

	SET @sqlParams = N''
	SET @sqlText = 'INSERT INTO [irma].' + QUOTENAME(@table_name) + ' ( 
	Region, Identifier_ID ,Item_Key ,Identifier ,Default_Identifier ,National_Identifier , CheckDigit , IdentifierType , NumPluDigitsSentToScale , Scale_Identifier)
	SELECT ' + QUOTENAME(@region, '''') + ', Identifier_ID, i02.Item_Key, Identifier, Default_Identifier, National_Identifier, CheckDigit, IdentifierType, NumPluDigitsSentToScale, Scale_Identifier
	FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.dbo.Item i01
	INNER JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.dbo.ItemIdentifier i02 ON (i01.Item_Key = i02.Item_Key)  
	INNER JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.dbo.ValidatedScanCode v ON (i02.Identifier = v.ScanCode)
	WHERE i01.Deleted_Item = 0
	AND i01.Remove_Item = 0
	AND i02.Remove_Identifier = 0
	AND i02.Add_Identifier = 0
	AND i02.Deleted_Identifier = 0
'

	EXECUTE sp_executesql @sqlText, @sqlParams

	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';

END