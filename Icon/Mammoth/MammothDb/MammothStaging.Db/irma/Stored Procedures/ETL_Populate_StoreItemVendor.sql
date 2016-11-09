
CREATE PROCEDURE [irma].[ETL_Populate_StoreItemVendor] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	SET NOCOUNT ON
	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;
	DECLARE @Rows INT;

	SET @table_name = N'StoreItemVendor'
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
		, StoreItemVendorID
		, Store_No
		, Item_Key
		, Vendor_ID
		, AverageDelivery
		, PrimaryVendor
		, DeleteDate
		, DeleteWorkStation
		, LastCostAddedDate
		, LastCostRefreshedDate
		, DiscontinueItem
		)
	SELECT    ' + QUOTENAME(@Region,'''') + '
		, i.StoreItemVendorID
		, i.Store_No
		, i.Item_Key
		, i.Vendor_ID
		, i.AverageDelivery
		, i.PrimaryVendor
		, i.DeleteDate
		, i.DeleteWorkStation
		, i.LastCostAddedDate
		, i.LastCostRefreshedDate
		, i.DiscontinueItem
FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].' + QUOTENAME(@table_name) + ' i
	INNER JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].Store s ON (i.Store_No = s.Store_No)
	INNER JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].StoreRegionMapping srm ON (s.Store_No = srm.Store_No)
	WHERE (s.WFM_Store = 1 OR s.Mega_Store = 1 )
	AND (srm.Region_Code = ' + QUOTENAME(@Region,'''') + ')
	AND (s.Internal = 1 AND s.BusinessUnit_ID IS NOT NULL)
	AND (i.PrimaryVendor = 1)
'
--	PRINT @sqlText


	EXECUTE sp_executesql @sqlText, @sqlParams

	SELECT @Rows = @@ROWCOUNT
	PRINT '...' + CONVERT(NVARCHAR(18), @Rows) + ' rows inserted'
	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';


END