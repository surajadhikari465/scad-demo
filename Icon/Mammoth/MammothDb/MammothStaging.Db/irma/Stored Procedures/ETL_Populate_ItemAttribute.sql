

CREATE PROCEDURE [irma].[ETL_Populate_ItemAttribute] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	SET NOCOUNT ON
	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;
	DECLARE @Rows INT;

	SET @table_name = N'ItemAttribute'
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
	, ItemAttribute_ID
	, Item_Key
	, Check_Box_1
	, Check_Box_2
	, Check_Box_3
	, Check_Box_4, 
	Check_Box_5, 
	Check_Box_6, 
	Check_Box_7, 
	Check_Box_8
	, Check_Box_9
	, Check_Box_10
	, Check_Box_11
	, Check_Box_12
	, Check_Box_13
	, Check_Box_14
	, Check_Box_15
	, Check_Box_16
	, Check_Box_17
	, Check_Box_18
	, Check_Box_19
	, Check_Box_20
	, Text_1
	, Text_2
	, Text_3
	, Text_4
	, Text_5
	, Text_6
	, Text_7
	, Text_8
	, Text_9
	, Text_10
	, Date_Time_1
	, Date_Time_2
	, Date_Time_3
	, Date_Time_4
	, Date_Time_5
	, Date_Time_6
	, Date_Time_7
	, Date_Time_8
	, Date_Time_9
	, Date_Time_10
	, LastUpdateTimestamp
		)
	SELECT    ' + QUOTENAME(@Region,'''') + '
	, ItemAttribute_ID
	, Item_Key
	, Check_Box_1
	, Check_Box_2
	, Check_Box_3
	, Check_Box_4, 
	Check_Box_5, 
	Check_Box_6, 
	Check_Box_7, 
	Check_Box_8
	, Check_Box_9
	, Check_Box_10
	, Check_Box_11
	, Check_Box_12
	, Check_Box_13
	, Check_Box_14
	, Check_Box_15
	, Check_Box_16
	, Check_Box_17
	, Check_Box_18
	, Check_Box_19
	, Check_Box_20
	, Text_1
	, Text_2
	, Text_3
	, Text_4
	, Text_5
	, Text_6
	, Text_7
	, Text_8
	, Text_9
	, Text_10
	, Date_Time_1
	, Date_Time_2
	, Date_Time_3
	, Date_Time_4
	, Date_Time_5
	, Date_Time_6
	, Date_Time_7
	, Date_Time_8
	, Date_Time_9
	, Date_Time_10
	, LastUpdateTimestamp
FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.dbo.' + QUOTENAME(@table_name) + ' i
'
--	PRINT @sqlText


	EXECUTE sp_executesql @sqlText, @sqlParams

	SELECT @Rows = @@ROWCOUNT
	PRINT '...' + CONVERT(NVARCHAR(18), @Rows) + ' rows inserted'
	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';


END