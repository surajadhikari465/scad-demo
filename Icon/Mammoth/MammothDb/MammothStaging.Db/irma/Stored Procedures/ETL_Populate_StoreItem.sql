﻿
CREATE PROCEDURE [irma].[ETL_Populate_StoreItem] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	SET NOCOUNT ON
	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;
	DECLARE @Rows INT;

	SET @table_name = N'StoreItem'
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
	, StoreItemAuthorizationID
	, Store_No
	, Item_Key
	, Authorized
	, POSDeAuth
	, ScaleAuth
	, ScaleDeAuth
	, Refresh
	, ECommerce
	)
	SELECT    ' + QUOTENAME(@Region,'''') + '
	, i.StoreItemAuthorizationID
	, i.Store_No
	, i.Item_Key
	, i.Authorized
	, i.POSDeAuth
	, i.ScaleAuth
	, i.ScaleDeAuth
	, i.Refresh
	, i.ECommerce
	FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].' + QUOTENAME(@table_name) + ' i
	INNER JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].Store s ON (i.Store_No = s.Store_No)
	INNER JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].StoreRegionMapping srm ON (s.Store_No = srm.Store_No)
	WHERE (s.WFM_Store = 1 OR s.Mega_Store = 1 )
	AND (srm.Region_Code = ' + QUOTENAME(@Region,'''') + ')
	AND (s.Internal = 1 AND s.BusinessUnit_ID IS NOT NULL)
'
	PRINT @sqlText


	EXECUTE sp_executesql @sqlText, @sqlParams

	SELECT @Rows = @@ROWCOUNT
	PRINT '...' + CONVERT(NVARCHAR(18), @Rows) + ' rows inserted'
	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';


END