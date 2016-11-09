CREATE PROCEDURE [irma].[ETL_Populate_Item] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;

	SET @table_name = N'Item'
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
		, Item_Key
		, Item_Description
		, Sign_Description
		, Ingredients
		, SubTeam_No
		, Package_Desc1
		, Package_Desc2
		, Package_Unit_ID
		, Deleted_Item
		, WFM_Item
		, Not_Available
		, Remove_Item
		, POS_Description
		, Retail_Sale
		, Food_Stamps
		, Discountable
		, Product_Code
		, StoreJurisdictionID
		, FSA_Eligible
		, Retail_Unit_ID
		, Origin_ID
		, CountryProc_ID
		, LabelType_ID
		)
	SELECT    ' + QUOTENAME(@Region,'''') + ',
		i.Item_Key ,
		i.Item_Description ,
		i.Sign_Description ,
		i.Ingredients ,
		i.SubTeam_No ,
		i.Package_Desc1 ,
		i.Package_Desc2 ,
		i.Package_Unit_ID ,
		i.Deleted_Item ,
		i.WFM_Item ,
		i.Not_Available ,
		i.Remove_Item ,
		i.POS_Description ,
		i.Retail_Sale ,
		i.Food_Stamps ,
		i.Discountable ,
		i.Product_Code ,
		i.StoreJurisdictionID ,
		i.FSA_Eligible ,
		i.Retail_Unit_ID ,
		i.Origin_ID,
		i.CountryProc_ID ,
		i.LabelType_ID
	FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.dbo.' + QUOTENAME(@table_name) + ' i
	WHERE i.Deleted_Item = 0
	AND i.Remove_Item = 0
'
	PRINT @sqlText

	EXECUTE sp_executesql @sqlText, @sqlParams
	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';

END